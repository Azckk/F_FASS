using Common.Service.Dtos;

namespace FASS.Service.Dtos.Object
{
    public class SafetyLightGridDto : AuditDto
    {
        /// <summary>
        /// 通讯协议类型
        /// </summary>
        public required string ProtocolType { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public required string Ip { get; set; }

        /// <summary>
        /// 端口号
        /// </summary>
        public required string Port { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public required string Code { get; set; }

        /// <summary>
        /// 品牌
        /// </summary>
        public string? Brand { get; set; }
    }
}
