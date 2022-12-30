using RoR2;
using System.Collections.Generic;

namespace NemgineerMod.Modules.Characters
{
    public abstract class ItemDisplaysBase
    {
        public void SetItemDisplays(ItemDisplayRuleSet itemDisplayRuleSet)
        {
            List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules = new List<ItemDisplayRuleSet.KeyAssetRuleGroup>();
            this.SetItemDisplayRules(itemDisplayRules);
            itemDisplayRuleSet.keyAssetRuleGroups = itemDisplayRules.ToArray();
        }

        protected abstract void SetItemDisplayRules(
          List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules);
    }
}