using Common.NETCore.Extensions;
using FASS.Data.Consts.Warehouse;
using FASS.Data.Dtos.Warehouse;
using FASS.Scheduler.Attributes;
using FASS.Scheduler.Controllers.Base;
using FASS.Scheduler.Controllers.Models.Response;
using FASS.Service.Dtos.Warehouse;
using FASS.Service.Services.Warehouse.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FASS.Scheduler.Controllers
{
    [AllowAnonymous]
    [TypeFilter(typeof(AuthorizeActionIgonreAttribute))]
    [TypeFilter(typeof(ActionLogIgonreAttribute))]
    [Tags("库位接口")]
    public class StorageController : BaseController
    {
        private readonly ILogger<StorageController> _logger;
        private readonly IContainerService _containerService;
        private readonly IStorageService _storageService;
        private readonly IStorageTagService _storageTagService;
        private readonly ITagService _tagService;

        public StorageController(
            ILogger<StorageController> logger,
            IContainerService containerService,
            IStorageService storageService,
            IStorageTagService storageTagService,
            ITagService tagService)
        {
            _logger = logger;
            _containerService = containerService;
            _storageService = storageService;
            _storageTagService = storageTagService;
            _tagService = tagService;
        }

        [Tags("容器放入库位")]
        [HttpPost]
        public IActionResult ContainerAdd(Models.Request.StorageBinding request)
        {
            var storageDto = _storageService.Set().FirstOrDefault(e => e.Code == request.StorageCode);
            if (storageDto == null)
            {
                return BadRequest($"获取库位失败 [{request.StorageCode}]");
            }
            if (storageDto.State == StorageConst.State.FullContainer || storageDto.State == StorageConst.State.EmptyContainer)
            {
                return BadRequest($"库位已绑定容器 [{request.StorageCode}]");
            }
            var containerDto = _containerService.Set().FirstOrDefault(e => e.Code == request.ContainerCode);
            if (containerDto == null)
            {
                return BadRequest($"获取容器失败 [{request.ContainerCode}]");
            }
            var storageDtos = _containerService.GetStorages(containerDto);
            if (storageDtos.Count() > 0)
                return BadRequest($"容器已被绑定 [{request.ContainerCode}]");
            var result = _storageService.ContainerAdd(storageDto.Id, new List<ContainerDto>() { containerDto });
            return Ok();
        }

        [Tags("获取库位上容器")]
        [HttpPost]
        public IActionResult GetContainer(Models.Request.StorageQuery request)
        {
            var storageDto = _storageService.Set().FirstOrDefault(e => e.Id == request.StorageCode);
            if (storageDto == null)
            {
                return BadRequest($"获取库位失败 [{request.StorageCode}]");
            }
            var result = _storageService.GetContainers(storageDto);
            return Ok(result);
        }

        [Tags("库位添加标签")]
        [HttpPost]
        public IActionResult TagAdd(Models.Request.TagBinding request)
        {
            var storageDto = _storageService.Set().FirstOrDefault(e => e.NodeCode == request.NodeCode);
            if (storageDto == null)
            {
                return BadRequest($"获取库位失败 [{request.NodeCode}]");
            }
            var tagDto = _tagService.Set().FirstOrDefault(e => e.Code == request.TagCode);
            if (tagDto == null)
            {
                return BadRequest($"获取标签失败 [{request.TagCode}]");
            }
            var result = _storageService.TagAdd(storageDto.Id, new List<TagDto>() { tagDto });
            return Ok();
        }

        [Tags("库位移除标签")]
        [HttpPost]
        public IActionResult TagDelete(Models.Request.TagBinding request)
        {
            var storageDto = _storageService.Set().FirstOrDefault(e => e.NodeCode == request.NodeCode);
            if (storageDto == null)
            {
                return BadRequest($"获取库位失败 [{request.NodeCode}]");
            }
            var tagDto = _tagService.Set().FirstOrDefault(e => e.Code == request.TagCode);
            if (tagDto == null)
            {
                return BadRequest($"获取标签失败 [{request.TagCode}]");
            }
            var result = _storageService.TagDelete(storageDto.Id, new List<TagDto>() { tagDto });
            return Ok();
        }

        [Tags("获取站点绑定的库位是否存在容器")]
        [HttpPost]
        public IActionResult NotExistContainer(Models.Request.StorageQuery request)
        {
            if (string.IsNullOrEmpty(request.NodeCode))
            {
                return BadRequest($"站点编码 [{request.NodeCode}]不能为空，获取库位失败!");
            }

            var storageDto = _storageService.Set().FirstOrDefault(e => e.NodeCode == request.NodeCode && e.IsEnable);
            if (storageDto == null)
            {
                return BadRequest($"站点编码 [{request.NodeCode}]无绑定的库位，获取库位失败!");
            }
            if (storageDto.State != StorageConst.State.NoneContainer)
            {
                return BadRequest($"站点编码 [{request.NodeCode}]对应库位[{storageDto.Code}]存在容器，获取失败!");
            }
            return Ok(storageDto);
        }

        [Tags("获取库位状态")]
        [HttpPost]
        public IActionResult StorageState(Models.Request.StorageQuery request)
        {
            _logger.LogInformation($"StorageQuery:{request.ToJson()}");
            if (string.IsNullOrEmpty(request.StorageCode))
            {
                return BadRequest($"库位编码 [{request.StorageCode}]不能为空，获取库位失败!");
            }
            var storageDto = _storageService.Set().FirstOrDefault(e => e.Code == request.StorageCode && e.IsEnable);
            if (storageDto == null)
            {
                return BadRequest($"库位编码 [{request.StorageCode}]不存在，获取库位失败!");
            }
            var storageState = new StorageState
            {
                Code = storageDto.Code,
                Name = storageDto.Name,
                AreaName = storageDto.AreaName,
                State = (storageDto.State == StorageConst.State.FullContainer || storageDto.State == StorageConst.State.EmptyContainer) ? "ExistContainer" : "NotExistContainer"
            };
            return Ok(storageState);
        }

        [Tags("获取开关盖工位锁")]
        [HttpPost]
        public IActionResult GetLidStorageLock(Models.Request.LidStorageLock request)
        {
            if (string.IsNullOrEmpty(request.AreaCode))
            {
                return BadRequest($"区域编码 [{request.AreaCode}]不能为空，获取库位锁失败!");
            }
            var storageDtos = _storageService.Set().Where(e => e.AreaCode == request.AreaCode && e.IsEnable && !e.IsLock).ToList();
            if (storageDtos == null)
            {
                return BadRequest($"区域编码 [{request.AreaCode}]区域内开关盖工位已锁定，获取库位失败!");
            }
            //获取Lid标签
            var tagDto = _tagService.Set().FirstOrDefault(e => e.Code.ToLower() == "lid");
            if (tagDto == null)
            {
                return BadRequest($"不存在盖子标签[lid]，获取库位失败!");
            }
            //获取储位对应的标签
            var storageTagDtos = _storageTagService.Set().Where(e => storageDtos.Select(e => e.Id).Contains(e.StorageId) && e.TagId == tagDto.Id);
            if (request.Operate.ToLower() == "open")
            {
                //开盖时，库位不存在标签
                var enableStorage = storageDtos.Select(e => e.Id).Except(storageTagDtos.Select(e => e.StorageId)).ToList();
                if (enableStorage == null)
                {
                    return BadRequest($"区域编码 [{request.AreaCode}] 操作类型[{request.Operate}] 不存在无盖子标签工位，获取库位锁失败!");
                }
                else
                {
                    var storageDto = storageDtos.FirstOrDefault(e => e.Id == enableStorage[0]);
                    if (storageDto == null)
                    {
                        return BadRequest($"区域编码 [{request.AreaCode}] 操作类型[{request.Operate}] 不存在无盖子标签的工位，获取库位锁失败!");
                    }
                    else
                    {
                        _storageService.Repository.ExecuteUpdate(e => e.Id == storageDto.Id, s => s.SetProperty(b => b.IsLock, true));
                        var storage = new StorageLockInfo()
                        {
                            NodeCode = storageDto.NodeCode,
                            Code = storageDto.Code,
                            Name = storageDto.Name,
                            AreaCode = storageDto.AreaCode,
                            Operate = request.Operate
                        };
                        return Ok(storage);
                    }
                }
            }
            else if (request.Operate.ToLower() == "close")
            {
                //关盖时，库位必须存在Lid标签
                if (storageTagDtos == null)
                {
                    return BadRequest($"区域编码 [{request.AreaCode}] 操作类型[{request.Operate}] 不存在盖子标签[lid]工位，获取库位锁失败!");
                }
                else
                {
                    //默认返回第一个，并给库位加锁
                    var storageDto = storageDtos.FirstOrDefault(e => e.Id == storageTagDtos.ToList()[0].StorageId);
                    if (storageDto == null)
                    {
                        return BadRequest($"区域编码 [{request.AreaCode}] 操作类型[{request.Operate}] 不存在盖子标签[lid]工位，获取库位锁失败!");
                    }
                    else
                    {
                        _storageService.Repository.ExecuteUpdate(e => e.Id == storageDto.Id, s => s.SetProperty(b => b.IsLock, true));
                        var storage = new StorageLockInfo()
                        {
                            NodeCode = storageDto.NodeCode,
                            Code = storageDto.Code,
                            Name = storageDto.Name,
                            AreaCode = storageDto.AreaCode,
                            Operate = request.Operate
                        };
                        return Ok(storage);
                    }
                }
            }
            else
            {
                return BadRequest($"区域编码 [{request.AreaCode}] 不存在操作类型[{request.Operate}]，获取库位失败!");
            }
        }

        [Tags("添加桶盖标签")]
        [HttpPost]
        public IActionResult AddLidTag(Models.Request.StorageTag request)
        {
            if (request.Operate != "open")
            {
                return BadRequest($"上报操作类型 [{request.Operate}]错误，添加库位桶盖标签失败!");
            }
            if (string.IsNullOrEmpty(request.NodeCode))
            {
                return BadRequest($"上报站点 [{request.NodeCode}]不能为空，添加库位桶盖标签失败!");
            }
            //获取已经锁定的开盖工位
            var storageDto = _storageService.Set().FirstOrDefault(e => e.NodeCode == request.NodeCode && e.IsEnable && e.IsLock);
            if (storageDto == null)
            {
                return BadRequest($"上报站点 [{request.NodeCode}]对应库位不存在，添加库位桶盖标签失败!");
            }
            //获取Lid标签
            var tagDto = _tagService.Set().FirstOrDefault(e => e.Code.ToLower() == "lid");
            if (tagDto == null)
            {
                return BadRequest($"桶盖标签[Lid]信息不存在，添加库位桶盖标签失败!");
            }
            //添加标签
            _storageService.TagAdd(storageDto.Id, new List<TagDto> { tagDto });
            //变更库位锁
            _storageService.Repository.ExecuteUpdate(e => e.Id == storageDto.Id, s => s.SetProperty(b => b.IsLock, false));
            var storage = new StorageLockInfo()
            {
                NodeCode = storageDto.NodeCode,
                Code = storageDto.Code,
                Name = storageDto.Name,
                AreaCode = storageDto.AreaCode,
                Operate = request.Operate
            };
            return Ok(storage);
        }

        [Tags("移除桶盖标签")]
        [HttpPost]
        public IActionResult RemoveLidTag(Models.Request.StorageTag request)
        {
            if (request.Operate.ToLower() != "close")
            {
                return BadRequest($"上报操作类型 [{request.Operate}]错误，移除库位桶盖标签失败!");
            }
            if (string.IsNullOrEmpty(request.NodeCode))
            {
                return BadRequest($"上报站点 [{request.NodeCode}]不能为空，移除库位桶盖标签失败!");
            }
            //获取已经锁定的关盖工位
            var storageDto = _storageService.Set().FirstOrDefault(e => e.NodeCode == request.NodeCode && e.IsEnable && e.IsLock);
            if (storageDto == null)
            {
                return BadRequest($"上报站点 [{request.NodeCode}]对应库位不存在，移除库位桶盖标签失败!");
            }
            //获取Lid标签
            var tagDto = _tagService.Set().FirstOrDefault(e => e.Code.ToLower() == "lid");
            if (tagDto == null)
            {
                return BadRequest($"桶盖标签[Lid]信息不存在，添加库位桶盖标签失败!");
            }
            //根据库位信息获取库位标签信息
            var tagDtos = _storageService.GetTags(new List<StorageDto> { storageDto }).Where(e => e.Code.ToLower() == "lid");
            if (tagDtos != null)
            {
                //移除标签
                _storageService.TagDelete(storageDto.Id, tagDtos);
            }
            //变更库位锁
            _storageService.Repository.ExecuteUpdate(e => e.Id == storageDto.Id, s => s.SetProperty(b => b.IsLock, false));
            var storage = new StorageLockInfo()
            {
                NodeCode = storageDto.NodeCode,
                Code = storageDto.Code,
                Name = storageDto.Name,
                AreaCode = storageDto.AreaCode,
                Operate = request.Operate
            };
            return Ok(storage);
        }
    }
}
