using Common.NETCore.Extensions;
using FASS.Data.Dtos.Warehouse;
using FASS.Service.Consts.FlowExtend;
using FASS.Service.Dtos.FlowExtend;

namespace FASS.Scheduler.Controllers.Extensions
{
    public static class CarTaskExtension
    {
        public static Extend.Car.Fairyland.Pc.Models.Request.CarTask ToCarTask(Service.Dtos.FlowExtend.TaskRecordDto taskRecordDto, Service.Models.FlowExtend.TaskReqNode startNode, Service.Models.FlowExtend.TaskReqNode endNode, Service.Models.FlowExtend.ContainerSize containerSize, Data.Models.Data.Car? car, string CarTypeCode = "Car")
        {
            var request = new Extend.Car.Fairyland.Pc.Models.Request.CarTask 
            {
                CarCode = car?.Code,
                CarType = CarTypeCode,
                TaskCode = taskRecordDto.Id,
                TaskType = taskRecordDto.TaskTemplateId
            };
            var extend = taskRecordDto.Extend?.JsonTo<Service.Extends.Flow.TaskRecordExtend>();
            request.Priority = taskRecordDto.Priority;
            request.Material = extend?.Material;//获取物料信息
            request.Nodes = new List<Extend.Car.Fairyland.Pc.Models.Request.Node>
            {
                new Extend.Car.Fairyland.Pc.Models.Request.Node()
                {
                    Code = startNode.Code
                },
                new Extend.Car.Fairyland.Pc.Models.Request.Node()
                {
                    Code = endNode.Code
                }
            };
            request.ContainerSize = new Extend.Car.Fairyland.Pc.Models.Request.ContainerSize()
            {
                Length = containerSize.Length,
                Width = containerSize.Width,
                Height = containerSize.Height
            };
            return request;
        }

        public static TaskRecordDto CreateTaskRecord(string taskTemplateId, StorageDto srcStorage, StorageDto destStorage)
        {
            TaskRecordDto taskRecordDto = new TaskRecordDto
            {
                TaskTemplateId = taskTemplateId,
                Type = "Template",
                SrcNodeId = srcStorage.NodeId,
                SrcNodeCode = srcStorage.NodeCode,
                SrcAreaId = srcStorage.AreaId,
                SrcStorageId = srcStorage.Id,
                DestNodeId = destStorage.NodeId,
                DestNodeCode = destStorage.NodeCode,
                DestAreaId = destStorage.AreaId,
                DestStorageId = destStorage.Id,
                Code = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                Name = $"车辆 [] 从起点站点 [{srcStorage.NodeCode}] 到终点站点 [{destStorage.NodeCode}]",
                State = TaskRecordConst.State.Created,
                Description = $"{srcStorage.Code} => {destStorage.Code}"
            };
            return taskRecordDto;
        }
    }
}
