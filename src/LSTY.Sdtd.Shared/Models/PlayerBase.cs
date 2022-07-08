namespace LSTY.Sdtd.Shared.Models
{
    /// <summary>
    /// 玩家基础信息
    /// </summary>
    public class PlayerBase
    {
        /// <summary>
        /// 平台Id
        /// </summary>
        public string PlatformId { get; set; }

        /// <summary>
        /// 平台类型
        /// </summary>
        public string PlatformType { get; set; }

        /// <summary>
        /// 跨平台Id
        /// </summary>
        public string CrossplatformId { get; set; }

        /// <summary>
        /// 实体Id
        /// </summary>
        public int EntityId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}