using Facepunch.Extend;
using UnityEngine;

namespace Oxide.Plugins
{
    [Info("TurretNerf", "Default", "1.0.0")]
    public class TurretNerf : RustPlugin
    {
        //Biggest of all thanks to Misticos and Jake_Rich for helping me with CanPickupEntity.

		//TODO Overhaul and make usable on uMod
        //private string ChatPrefix = "<color=blue>TurretNerf</color>: {0}";
        private readonly string autoTurretPrefab = "assets/prefabs/npc/autoturret/autoturret_deployed.prefab";

        void OnEntitySpawned(AutoTurret entity)
        {
            if (entity.PrefabName != autoTurretPrefab) { return;}
            entity.inventory.capacity = 2;
        }
       



        private object CanPickupEntity(BasePlayer player, BaseCombatEntity entity)
        {
            if (entity.name != autoTurretPrefab)
            {
                return true;
            }
            var item = ItemManager.Create(entity.pickup.itemTarget, entity.pickup.itemCount);
            if (item.hasCondition)
            {
                item.conditionNormalized =  entity.Health() / entity.MaxHealth() / 2;
            }
            player.GiveItem(item, BaseEntity.GiveItemReason.PickedUp);
            entity.OnPickedUp(item, player);
            entity.Kill();
            return false;
        }

    }
}