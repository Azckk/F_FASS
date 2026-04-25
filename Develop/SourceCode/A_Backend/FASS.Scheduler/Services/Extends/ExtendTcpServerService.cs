using Common.AspNetCore.Extensions;
using Common.NETCore;
using Common.NETCore.Extensions;
using Common.NETCore.Helpers;
using Common.NETCore.Models;
using FASS.Data.Consts.Flow;
using FASS.Data.Consts.Record;
using FASS.Data.Consts.Warehouse;
using FASS.Data.Dtos.Record;
using FASS.Data.Dtos.Warehouse;
using FASS.Data.Services.Record.Interfaces;
using FASS.Scheduler.Controllers.Extensions;
using FASS.Scheduler.Utility;
using FASS.Service.Consts.FlowExtend;
using FASS.Service.Consts.Record;
using FASS.Service.Dtos.Warehouse;
using FASS.Service.Services.FlowExtend.Interfaces;
using FASS.Service.Services.Warehouse.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;
using TcpServer = Common.Net.Tcp.TcpServer;

namespace FASS.Scheduler.Services.Extends
{
    public class ExtendTcpServerService
    {
        public ExtendService ExtendService { get; }

        public bool IsStarted { get; private set; }

        public TcpServer TcpServer { get; private set; } = null!;

        private readonly IWorkService _workService;
        private readonly IStorageService _storageService;
        private readonly IContainerService _containerService;
        private readonly IMaterialService _materialService;
        private readonly ITaskRecordService _taskRecordService;
        private readonly IDiaryService _diaryService;
        private readonly IPreMaterialService _preMaterialService;
        private readonly IPreWorkService _preWorkService;

        public ExtendTcpServerService(
            ExtendService eventService)
        {
            ExtendService = eventService;

            _workService = ExtendService.ServiceProvider.GetScopeService<IWorkService>();
            _storageService = ExtendService.ServiceProvider.GetScopeService<IStorageService>();
            _containerService = ExtendService.ServiceProvider.GetScopeService<IContainerService>();
            _materialService = ExtendService.ServiceProvider.GetScopeService<IMaterialService>();
            _taskRecordService = ExtendService.ServiceProvider.GetScopeService<ITaskRecordService>();
            _diaryService = ExtendService.ServiceProvider.GetScopeService<IDiaryService>();
            _preMaterialService = ExtendService.ServiceProvider.GetScopeService<IPreMaterialService>();
            _preWorkService = ExtendService.ServiceProvider.GetScopeService<IPreWorkService>();

            Init();
        }

        public void Init()
        {
            var tcpServerLocalIP = Guard.NotNull(ExtendService.AppSettings.Extend.TcpServerLocalIP);
            TcpServer = new TcpServer() { LocalEndPoint = IPEndPoint.Parse(tcpServerLocalIP) };
            TcpServer.Started += (server) =>
            {
                ExtendService.Logger.LogInformation($"Started LocalEndPoint:[{server.Server.LocalEndPoint}]");
            };
            TcpServer.Stopped += (server) =>
            {
                ExtendService.Logger.LogInformation($"Stopped");
            };
            TcpServer.Accepted += (server, client) =>
            {
                ExtendService.Logger.LogInformation($"Accepted LocalEndPoint:[{server.Server.LocalEndPoint}] RemoteEndPoint:[{client.Client.LocalEndPoint}]");

                client.Connected += (c) =>
                {
                    ExtendService.Logger.LogInformation($"Connected LocalEndPoint:[{c.Client.LocalEndPoint}] RemoteEndPoint:[{c.Client.RemoteEndPoint}]");
                };
                client.Disconnected += (c) =>
                {
                    ExtendService.Logger.LogInformation($"Disconnected");
                };
                client.Sent += (c, sendByteArray) =>
                {
                    ExtendService.Logger.LogInformation($"Sent LocalEndPoint:[{c.Client.LocalEndPoint}] RemoteEndPoint:[{c.Client.RemoteEndPoint}] SendByteArray:[{ByteHelper.ByteArrayToHexString(sendByteArray)}]");
                };
                client.Received += async (c, receiveByteArray) =>
                {
                    ExtendService.Logger.LogInformation($"Received LocalEndPoint:[{c.Client.LocalEndPoint}] RemoteEndPoint:[{c.Client.RemoteEndPoint}] ReceiveByteArray:[{ByteHelper.ByteArrayToHexString(receiveByteArray)}]");
                    try
                    {
                        //写入日志
                        DiaryDto diaryDto = new DiaryDto()
                        {
                            Level = DiaryConst.Level.Information,
                            Type = DiaryExtendConst.Type.PlcMessage,
                            Message = ByteHelper.ByteArrayToHexString(receiveByteArray)
                        };
                        await _diaryService.AddAsync(diaryDto);
                        var call = new Models.ReceiveCallMessage().GetMessage(receiveByteArray);
                        var state = 0x01;
                        switch (call.CallMode)
                        {
                            case 1:
                                //下满桶 叫料库位有容器，目标库位没容器
                                state = await PlcFullOffline(call.StorageNo, call.MaterialNo!);
                                break;
                            case 2:
                                //上空桶 叫料库位无容器，目标库位有空容器
                                state = await PlcEmptyOnline(call.StorageNo, call.MaterialNo!);
                                break;
                            case 3:
                                //下空桶 叫料库位有容器，目标库位有无容器【煅烧、烘干】
                                state = await PlcEmptyOffline(call.StorageNo);
                                break;
                            default:
                                state = 0x01;
                                break;
                        }
                        if (state > 0)
                        {
                            state = 0x01;
                        }
                        var sendMessage = new Models.SendCallResponseMessage().SetMessage(call.Command, call.Car, call.StorageNo, call.CallMode, call.MaterialNo ?? "", (byte)state);
                        var responseByteArray = sendMessage.GetByteArray();
                        c.Send(responseByteArray);
                    }
                    catch (Exception ex)
                    {
                        ExtendService.Logger.LogError($"Message Handle Exception =>{ex.Message}");
                    }
                };
            };
        }

        public void Start()
        {
            try
            {
                if (IsStarted)
                {
                    return;
                }
                IsStarted = true;
                TcpServer.StartAndAccept();
            }
            catch (Exception ex)
            {
                ExtendService.Logger.LogError(ex.ToString());
            }
        }

        public void Stop()
        {
            try
            {
                if (IsStarted)
                {
                    return;
                }
                IsStarted = true;
                TcpServer.Stop();
            }
            catch (Exception ex)
            {
                ExtendService.Logger.LogError(ex.ToString());
            }
        }

        public async Task<int> PlcFullOffline(int storageCode, string materialNo)
        {
            /*
             * 1、叫料工位是有桶，且库位状态为未锁定
             * 2、是否存在满桶中转位
             */
            //1、判定叫料工位状态 
            var callStorage = await _storageService.Set().FirstOrDefaultAsync(e => e.IsEnable && e.Code == storageCode.ToString() && e.IsLock == false && (e.State == StorageConst.State.EmptyContainer || e.State == StorageConst.State.FullContainer));//有容器[空容器或满容器]，没锁定
            if (callStorage == null)
            {
                return 1;//呼叫库位无容器
            }
            //2、获取叫料工位容器
            var containers = await _storageService.GetContainersAsync(callStorage);
            if (containers == null || containers.Count() == 0)
            {
                return 2;//叫料工位不存在容器
            }
            //3、判定物料信息是否已存在，不存在就插入物料信息
            materialNo = materialNo.Replace("\0", "");
            if (!await _materialService.Set().AnyAsync(e => e.Code == materialNo && e.IsEnable))
            {
                MaterialDto materialDto = new MaterialDto()
                {
                    Code = materialNo,
                    Name = materialNo,
                    Type = MaterialConst.Type.Default,
                    State = MaterialConst.State.UnBind,
                    IsLock = false,
                    Quantity = 1
                };
                await _materialService.AddAsync(materialDto);
            }
            var material = await _materialService.Set().FirstOrDefaultAsync(e => e.Code == materialNo && e.IsEnable);
            //4、匹配是否物料被预定到指定机台
            /*
            * 4.1物料被预定  1)被预定且可直送时，直接送烘干/煅烧机台   2)被预定且不能直送(机台有桶)，先送满桶周转区，再送机台
            * 4.2物料没有被预定  送满桶周转区并清除预定记录
            * 
            */
            StorageDto? storageFull = null;
            //判断物料是否被预定
            var matchPreWorkDto = await _preWorkService.Set().AsNoTracking().FirstOrDefaultAsync(e => e.MaterialCode == materialNo && e.State == WorkConst.State.Created);
            if (matchPreWorkDto is not null)
            {
                //匹配到之后
                var matchStorageDto = await _storageService.Set().Where(e => e.Id == matchPreWorkDto.DestStorageId && e.IsEnable && e.State == StorageConst.State.NoneContainer && e.IsLock == false).FirstOrDefaultAsync();
                if (matchStorageDto is not null)
                {
                    //可以直送预定的机台、删除预定物料信息、删除预定搬运任务
                    storageFull = matchStorageDto.DeepClone();
                    await _preMaterialService.ExecuteDeleteAsync(e => e.Code == materialNo);
                    await _preWorkService.ExecuteDeleteAsync(e => e.MaterialCode == materialNo && e.SrcStorageId == callStorage.Id);
                }
                else
                {
                    //不能直送，只能先到满桶周转区、 删除预定物料信息、更新预定搬运任务
                    storageFull = await _storageService.Set().Where(e => e.IsEnable && e.State == StorageConst.State.NoneContainer && e.IsLock == false && e.AreaCode == "MTZZQ").OrderBy(e => e.SortNumber).FirstOrDefaultAsync();
                    await _preMaterialService.DeleteAsync(e => e.Code == materialNo);
                    //预定任务置为Released，通过服务定时发送
                    await _preWorkService.Repository.ExecuteUpdateAsync(e => e.MaterialCode == materialNo, s => s.SetProperty(b => b.State, WorkConst.State.Released));
                }
            }
            else
            {
                //送到满桶周转区、删除预定物料信息
                storageFull = await _storageService.Set().Where(e => e.IsEnable && e.State == StorageConst.State.NoneContainer && e.IsLock == false && e.AreaCode == "MTZZQ").OrderBy(e => e.SortNumber).FirstOrDefaultAsync();
                await _preMaterialService.ExecuteDeleteAsync(e => e.Code == materialNo);
            }
            if (storageFull == null)
            {
                return 3;//满桶周转区不存在空库位
            }
            //5、锁定起始库位和终点库位、绑定容器物料关系，变更容器状态
            //await _storageService.Repository.ExecuteUpdateAsync(e => new List<StorageDto> { callStorage, storageFull }.Select(e => e.Id).Contains(e.Id), s => s.SetProperty(b => b.IsLock, true));
            //添加容器物料绑定关系
            var container = containers.First();
            //清空容器绑定关系        
            await _containerService.MaterialDeleteAsync(container.Id);
            if (material is not null)
            {
                await _containerService.MaterialAddAsync(container.Id, new List<MaterialDto> { material });
            }
            await _containerService.Repository.ExecuteUpdateAsync(e => e.Id == container.Id, s => s.SetProperty(b => b.State, ContainerConst.State.FullMaterial));
            //6、创建taskRecord
            var taskRecordDto = CarTaskExtension.CreateTaskRecord("Double", callStorage, storageFull);
            taskRecordDto.Remark = materialNo;//添加物料信息
            //7、添加work记录
            var result = await _workService.AddWorkAsync(callStorage.AreaId, $"{callStorage.Name}-满桶下线", taskRecordDto, container, materialNo);
            if (result == 0)
            {
                return 4;//创建任务失败
            }
            //8、自动发布任务
            var dto = Guard.NotNull(await _taskRecordService.Set().Where(e => e.Id == taskRecordDto.Id && TaskRecordConst.State.Release.Contains(e.State)).FirstOrDefaultAsync());
            try
            {
                var startNode = new Service.Models.FlowExtend.TaskReqNode()
                {
                    Code = Guard.NotNull(taskRecordDto.SrcNodeCode)
                };
                var endNode = new Service.Models.FlowExtend.TaskReqNode()
                {
                    Code = Guard.NotNull(taskRecordDto.DestNodeCode)
                };
                var containerSize = new Service.Models.FlowExtend.ContainerSize()
                {
                    Width = container.Width,
                    Length = container.Length,
                    Height = container.Height
                };
                var req = CarTaskExtension.ToCarTask(dto, startNode, endNode, containerSize, null);
                var resp = WebAPIHelper.Instance.Post<ResponseResult, object>(
                    $"{ExtendService.AppSettings.Mdcs.SimpleUrl}/car/carTask", req
                );
                if (resp.Code == 200)
                {
                    dto.State = TaskInstanceConst.State.Released;
                    await _taskRecordService.UpdateTaskRecordStateAsync(dto.Id, dto.State);
                    //更具taskid更新work的状态
                    var workDto = await _workService.FirstOrDefaultAsync(e => e.TaskId == dto.Id);
                    if (workDto != null)
                    {
                        workDto.State = TaskInstanceConst.State.Released;
                        await _workService.UpdateWorkStateAsync(workDto.Id, workDto.State);
                    }
                    return 0;
                }
                else
                {
                    return 5;
                }
            }
            catch (Exception ex)
            {
                ExtendService.Logger.LogError($"任务发布失败 =>{ex}");
                return 6;
            }
        }

        public async Task<int> PlcEmptyOnline(int storageCode, string materialNo)
        {
            /*
             * 1、叫料工位是否有桶，且库位状态为未锁定
             * 2、空桶缓存区/空桶周转区是否存在空桶
             */
            //1、判定叫料工位状态
            var callStorage = await _storageService.Set().FirstOrDefaultAsync(e => e.IsEnable && e.Code == storageCode.ToString() && e.IsLock == false && e.State == StorageConst.State.NoneContainer);//无容器，没锁定
            if (callStorage == null)
            {
                return 1;//叫料工位无容器，未锁定状态不成立
            }
            //2、判定空桶库位状态
            var areaCode = "KTHCQ1";//默认空桶缓存区1
            if (storageCode == 4 || storageCode == 5 || storageCode == 6)
            {
                areaCode = "KTHCQ2";//应对左右两侧的空桶缓存位
            }
            var storageEmpty = await _storageService.Set().Where(e => e.State == StorageConst.State.EmptyContainer && e.IsLock == false && e.AreaCode == areaCode && e.IsEnable).OrderBy(e => e.SortNumber).FirstOrDefaultAsync();//找对应区域的空桶缓存位
            if (storageEmpty == null)
            {
                //空桶周转区是否存在空桶,且为未锁定状态
                storageEmpty = await _storageService.Set().Where(e => e.State == StorageConst.State.EmptyContainer && e.IsLock == false && e.AreaCode == "KTZZQ" && e.IsEnable).OrderByDescending(e => e.SortNumber).FirstOrDefaultAsync();//放空桶放里层，取SortNumber大的库位
            }
            if (storageEmpty == null)
            {
                return 2;
            }
            //3、获取空容器
            var emptyContainers = await _storageService.GetContainersAsync(storageEmpty);
            if (emptyContainers == null || emptyContainers.Count() != 1)
            {
                return 3;//库位容器绑定关系错误
            }
            var container = emptyContainers.First();
            //清空容器绑定关系        
            await _containerService.MaterialDeleteAsync(container.Id);
            //4、创建taskRecord
            var taskRecordDto = CarTaskExtension.CreateTaskRecord("Double", storageEmpty, callStorage);
            //5、添加work记录
            var result = await _workService.AddWorkAsync(callStorage.AreaId, $"{callStorage.Name}-空桶上线", taskRecordDto, container, "");
            if (result == 0)
            {
                return 4;//任务插入失败
            }
            //6、判定物料信息是否在预叫物料信息表中
            materialNo = materialNo.Replace("\0", "");
            if (!string.IsNullOrEmpty(materialNo) && !await _preMaterialService.Set().AsNoTracking().AnyAsync(e => e.Code == materialNo && e.IsEnable))
            {
                PreMaterialDto preMaterialDto = new PreMaterialDto()
                {
                    Code = materialNo,
                    Name = materialNo,
                    StorageId = callStorage.Id,
                    StorageName = callStorage.Name,
                    Type = MaterialConst.Type.Default,
                    State = MaterialConst.State.UnBind,
                    IsLock = false
                };
                await _preMaterialService.AddAsync(preMaterialDto);
            }
            //7、自动发布任务
            var dto = Guard.NotNull(await _taskRecordService.Set().Where(e => e.Id == taskRecordDto.Id && TaskRecordConst.State.Release.Contains(e.State)).FirstOrDefaultAsync());
            try
            {
                var startNode = new Service.Models.FlowExtend.TaskReqNode()
                {
                    Code = Guard.NotNull(taskRecordDto.SrcNodeCode)
                };
                var endNode = new Service.Models.FlowExtend.TaskReqNode()
                {
                    Code = Guard.NotNull(taskRecordDto.DestNodeCode)
                };
                var containerSize = new Service.Models.FlowExtend.ContainerSize()
                {
                    Width = container.Width,
                    Length = container.Length,
                    Height = container.Height
                };
                var req = CarTaskExtension.ToCarTask(dto, startNode, endNode, containerSize, null);
                var resp = WebAPIHelper.Instance.Post<ResponseResult, object>(
                    $"{ExtendService.AppSettings.Mdcs.SimpleUrl}/car/carTask", req
                );
                if (resp.Code == 200)
                {
                    dto.State = TaskInstanceConst.State.Released;
                    await _taskRecordService.UpdateTaskRecordStateAsync(dto.Id, dto.State);
                    //更具taskid更新work的状态
                    var workDto = await _workService.FirstOrDefaultAsync(e => e.TaskId == dto.Id);
                    if (workDto != null)
                    {
                        workDto.State = TaskInstanceConst.State.Released;
                        await _workService.UpdateWorkStateAsync(workDto.Id, workDto.State);
                    }
                    return 0;
                }
                else
                {
                    return 5;
                }
            }
            catch (Exception ex)
            {
                ExtendService.Logger.LogError($"任务发布失败 =>{ex}");
                return 6;
            }
        }

        public async Task<int> PlcEmptyOffline(int storageCode)
        {
            /*
             * 1、叫料工位是否有桶，且库位状态为未锁定
             * 2、空桶缓存区/空桶周转区是否存在空库位
             */
            //1、判定叫料工位状态
            var callStorage = await _storageService.Set().FirstOrDefaultAsync(e => e.IsEnable && e.Code == storageCode.ToString() && e.IsLock == false && (e.State == StorageConst.State.EmptyContainer || e.State == StorageConst.State.FullContainer));//有容器【空/满】，没锁定
            if (callStorage == null)
            {
                return 1;//叫料工位无容器，未锁定状态不成立
            }
            //2、判定空桶库位状态.优先放置到空桶缓存位，空桶缓存位不存在时，放置到空桶周转区
            var storageEmpty = await _storageService.Set().Where(e => e.State == StorageConst.State.NoneContainer && e.IsLock == false && e.IsEnable && e.AreaCode != null && e.AreaCode.Contains("KTHCQ")).OrderBy(e => e.SortNumber).FirstOrDefaultAsync();//找左侧空桶缓存区和右侧空桶缓存区
            if (storageEmpty == null)
            {
                //先判定是否存在空桶中转位,且为未锁定状态
                storageEmpty = await _storageService.Set().Where(e => e.State == StorageConst.State.NoneContainer && e.IsLock == false && e.AreaCode == "KTZZQ" && e.IsEnable).OrderBy(e => e.SortNumber).FirstOrDefaultAsync();//放空桶放里层，取SortNumber小的库位
            }
            if (storageEmpty == null)
            {
                return 2;
            }
            //3、获取空容器
            var emptyContainers = await _storageService.GetContainersAsync(callStorage);//呼叫位容器
            if (emptyContainers == null || emptyContainers.Count() != 1)
            {
                return 3;//库位容器绑定关系错误
            }
            var container = emptyContainers.First();
            //清空容器绑定关系        
            await _containerService.MaterialDeleteAsync(container.Id);
            //4、创建taskRecord
            var taskRecordDto = CarTaskExtension.CreateTaskRecord("Double", callStorage, storageEmpty);
            //5、添加work记录
            var result = await _workService.AddWorkAsync(callStorage.AreaId, $"{callStorage.Name}-空桶下线", taskRecordDto, container, "");
            if (result == 0)
            {
                return 4;//任务插入失败
            }
            //6、自动发布任务
            var dto = Guard.NotNull(await _taskRecordService.Set().Where(e => e.Id == taskRecordDto.Id && TaskRecordConst.State.Release.Contains(e.State)).FirstOrDefaultAsync());
            try
            {
                var startNode = new Service.Models.FlowExtend.TaskReqNode()
                {
                    Code = Guard.NotNull(taskRecordDto.SrcNodeCode)
                };
                var endNode = new Service.Models.FlowExtend.TaskReqNode()
                {
                    Code = Guard.NotNull(taskRecordDto.DestNodeCode)
                };
                var containerSize = new Service.Models.FlowExtend.ContainerSize()
                {
                    Width = container.Width,
                    Length = container.Length,
                    Height = container.Height
                };
                var req = CarTaskExtension.ToCarTask(dto, startNode, endNode, containerSize, null);
                var resp = WebAPIHelper.Instance.Post<ResponseResult, object>(
                    $"{ExtendService.AppSettings.Mdcs.SimpleUrl}/car/carTask", req
                );
                if (resp.Code == 200)
                {
                    dto.State = TaskInstanceConst.State.Released;
                    await _taskRecordService.UpdateTaskRecordStateAsync(dto.Id, dto.State);
                    //更具taskid更新work的状态
                    var workDto = await _workService.FirstOrDefaultAsync(e => e.TaskId == dto.Id);
                    if (workDto != null)
                    {
                        workDto.State = TaskInstanceConst.State.Released;
                        await _workService.UpdateWorkStateAsync(workDto.Id, workDto.State);
                    }
                    return 0;
                }
                else
                {
                    return 5;
                }
            }
            catch (Exception ex)
            {
                ExtendService.Logger.LogError($"任务发布失败 =>{ex}");
                return 6;
            }
        }

    }
}