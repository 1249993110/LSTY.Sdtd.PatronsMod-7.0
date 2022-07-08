using HarmonyLib;
using LSTY.Sdtd.PatronsMod.Extensions;
using LSTY.Sdtd.PatronsMod;
using LSTY.Sdtd.Shared.Models;

namespace LSTY.Sdtd.PatronsMod.HarmonyPatchers
{
    [HarmonyPatch(typeof(World), nameof(World.SpawnEntityInWorld))]
    public static class World_SpawnEntityInWorld_Patcher
    {
        [HarmonyPostfix]
        public static void Postfix(Entity _entity)
        {
            if (_entity is EntityAlive entityAlive)
            {
                ModEventHook.EntitySpawned(new EntityLocation()
                {
                    EntityId = entityAlive.entityId,
                    EntityName = entityAlive.EntityName,
                    Position = entityAlive.position.ToPosition(),
                    IsPlayer = entityAlive is EntityPlayer
                });
            }
        }
    }
}