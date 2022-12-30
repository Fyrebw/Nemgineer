using RoR2;
using UnityEngine;

namespace NemgineerMod.Modules
{
    public class Buffs
    {
        public static BuffDef CreateBuffDef(
          string name,
          bool canStack,
          bool isCooldown,
          bool isDebuff,
          Color color,
          Sprite iconSprite)
        {
            BuffDef instance = ScriptableObject.CreateInstance<BuffDef>();
            instance.name = name;
            instance.canStack = canStack;
            instance.isCooldown = isCooldown;
            instance.isDebuff = isDebuff;
            instance.buffColor = color;
            instance.iconSprite = iconSprite;
            ContentPacks.buffDefs.Add(instance);
            instance.name = instance.name;
            return instance;
        }
    }
}