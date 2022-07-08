namespace LSTY.Sdtd.Shared.Models
{
    /// <summary>
    /// 玩家库存
    /// </summary>
    public class PlayerInventory
    {
        /// <summary>
        /// 实体Id
        /// </summary>
        public int EntityId { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        public Inventory Inventory { get; set; }
    }
}