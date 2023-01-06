using static NemgineerMod.NemgineerPlugin;
using RoR2;
using UnityEngine;

namespace NemgineerMod.Modules
{
    internal static class Buffs
    {
        internal static BuffDef BlockBuff;
        internal static BuffDef energyShieldBuff;

        internal static void RegisterBuffs()
        {
            Buffs.BlockBuff = Buffs.AddNewBuff("Heavyweight", Assets.MainAssetBundle.LoadAsset<Sprite>("texBuffBlock"), NemgineerPlugin.characterColor, false, false);
            Buffs.energyShieldBuff = Buffs.AddNewBuff("EnergyShield", Assets.MainAssetBundle.LoadAsset<Sprite>("texBuffBlock"), NemgineerPlugin.characterColor, false, false);
            Sprite buffIcon1 = Assets.LoadBuffSprite("RoR2/Base/Common/bdSlow50.asset");
            Sprite buffIcon2 = Assets.LoadBuffSprite("RoR2/Base/Common/bdCloak.asset");
        }

        internal static BuffDef AddNewBuff(
          string buffName,
          Sprite buffIcon,
          Color buffColor,
          bool canStack,
          bool isDebuff)
        {
            BuffDef instance = ScriptableObject.CreateInstance<BuffDef>();
            instance.name = buffName;
            instance.buffColor = buffColor;
            instance.canStack = canStack;
            instance.isDebuff = isDebuff;
            instance.eliteDef = (EliteDef)null;
            instance.iconSprite = buffIcon;
            Content.AddBuffDef(instance);
            return instance;
        }
    }
}