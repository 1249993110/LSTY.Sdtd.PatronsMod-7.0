namespace LSTY.Sdtd.Shared.Models
{
    /// <summary>
    /// 生成的玩家
    /// </summary>
    public class PlayerSpawned : PlayerBase
    {
        /// <summary>
        /// 坐标
        /// </summary>
        public Position Position { get; set; }

        /// <summary>
        /// 生成类型
        /// </summary>
        public RespawnType RespawnType { get; set; }
    }
}