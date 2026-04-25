using Common.Service.Dtos;
using FASS.Service.Dtos.Warehouse;

namespace FASS.Data.Dtos.Warehouse
{
    public class StorageDto : AuditDto
    {
        public required string AreaId { get; set; }

        public required string NodeId { get; set; }
        public required string NodeCode { get; set; }

        public required string Code { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public required string State { get; set; }

        public bool IsLock { get; set; }

        public string? Barcode { get; set; }

        public string? AreaCode { get; set; }
        public string? AreaName { get; set; }

        public List<TagDto> Tags { get; set; } = [];//库位可视化扩展
        public List<ContainerDto> Containers { get; set; } = [];//库位可视化扩展
        public List<MaterialDto> Materials { get; set; } = [];//库位可视化扩展
        public double[] Coordinate { get; set; } = [];//库位绑定的站点XY坐标
        public double[] OffsetCoordinate { get; set; } = [];//偏移的XY坐标
        public double[] TextCoordinate { get; set; } = [];//文本偏移坐标
        public double Coefficient { get; set; }//缩放比例
        public string? Sketchpad { get; set; }//画板缩放比例
    }
}
