using Common.Service.Models;

namespace FASS.Service.Models.Object
{
    public class ThirdpartySystem : AuditModel
    {
        /// <summary>
        /// 系统名称
        /// </summary>
        public required string SystemName { get; set; }

        /// <summary>
        /// 接口名称
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// 通讯方式
        /// </summary>
        public required string CommunicatioMode { get; set; }

        /// <summary>
        /// 通讯地址
        /// </summary>
        public required string Url { get; set; }

        /// <summary>
        /// 请求方法
        /// </summary>
        public required string Method { get; set; }

        /// <summary>
        /// 请求头
        /// </summary>
        public string? Headers { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public string? QueryParams { get; set; }

        /// <summary>
        /// 响应参数
        /// </summary>
        public string? ResponseParams { get; set; }

        /// <summary>
        /// 接口状态
        /// </summary>
        public string? State { get; set; }

        /// <summary>
        /// 超时重传
        /// </summary>
        public bool IsRetransmit { get; set; }
    }
}
