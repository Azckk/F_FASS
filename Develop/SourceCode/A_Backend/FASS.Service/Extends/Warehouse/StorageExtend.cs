namespace FASS.Service.Extends.Warehouse
{
    public class StorageExtend
    {
        public double[] OffsetCoordinate { get; set; } = [];//偏移的XY坐标
        public double[] TextCoordinate { get; set; } = [];//文本偏移坐标
        public double Coefficient { get; set; }//缩放比例
        public string? Sketchpad { get; set; } = "";//画板缩放比例
    }
}
