namespace LSTY.Sdtd.Shared.Models
{
    public class SpawnEntity
    {
        /// <summary>
        /// 生成的实体Id或名称
        /// </summary>
        public string SpawnEntityIdOrName { get; set; }

        /// <summary>
        /// 目标玩家的Id或昵称
        /// </summary>
        public string TargetPlayerIdOrName { get; set; }
    }
}