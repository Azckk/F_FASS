using Common.NETCore.Extensions;
using FASS.Data.Dtos.Warehouse;
using FASS.Service.Consts.FlowExtend;
using FASS.Service.Dtos.FlowExtend;

namespace FASS.Web.Api.Models.Pc.Extensions
{
    public static class CarTaskExtension
    {
        public static Web.Api.Models.Pc.Request.CarTask ToCarTask(Service.Dtos.FlowExtend.TaskRecordDto taskRecordDto, Service.Models.FlowExtend.TaskReqNode startNode, Service.Models.FlowExtend.TaskReqNode endNode, Service.Models.FlowExtend.ContainerSize containerSize, Data.Dtos.Data.CarDto? carDto, string CarTypeCode = "Car")
        {
            var request = new Web.Api.Models.Pc.Request.CarTask
            {
                CarCode = carDto?.Code ?? "",
                CarType = CarTypeCode,
                TaskCode = taskRecordDto.Id,
                TaskType = taskRecordDto.TaskTemplateId
            };
            if (!string.IsNullOrWhiteSpace(taskRecordDto.Extend))
            {
                var extend = taskRecordDto.Extend.JsonTo<Service.Extends.Flow.TaskRecordExtend>();
                request.Material = extend?.Material ?? "";//获取物料信息
            }
            request.Priority = taskRecordDto.Priority;
            request.Nodes = new List<Web.Api.Models.Pc.Node>
            {
                new Web.Api.Models.Pc.Node()
                {
                    Code = startNode.Code
                },
                new Web.Api.Models.Pc.Node()
                {
                    Code = endNode.Code
                }
            };
            request.ContainerSize = new Service.Models.FlowExtend.ContainerSize()
            {
                Length = containerSize.Length,
                Width = containerSize.Width,
                Height = containerSize.Height
            };
            return request;
        }

        public static Service.Models.FlowExtend.ContainerSize ToContainerSize(double length, double width, double height)
        {
            var request = new Service.Models.FlowExtend.ContainerSize()
            {
                Length = length,
                Width = width,
                Height = height
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
