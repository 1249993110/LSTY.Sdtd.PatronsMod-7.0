namespace LSTY.Sdtd.Shared.Models
{
    /// <summary>
    /// 实体位置
    /// </summary>
    public class EntityLocation
    {
        /// <summary>
        /// 实体Id
        /// </summary>
        public int EntityId { get; set; }

        /// <summary>
        /// 实体名称，如果为空则返回或实体类名
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// 是否为玩家
        /// </summary>
        public bool IsPlayer { get; set; }

        /// <summary>
        /// 坐标
        /// </summary>
        public Position Position { get; set; }
    }
}