using BepInEx.Configuration;
using RiskOfOptions;
using RiskOfOptions.Options;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace NemgineerMod.Modules
{
    public static class Config
    {
        public static bool forceUnlock = false;
        public static float sortPosition = 7.51f;
        public static void ReadConfig()
        {
            Config.sortPosition = RMORPlugin.instance.Config.Bind<float>("General", "Survivor Sort Position", 4.51f, "Controls where Nemesis Engineer is placed in the character select screen.").Value;
            Config.forceUnlock = RMORPlugin.instance.Config.Bind<bool>("General", "Force Unlock", false, "Automatically unlock Nemesis Engineer and his skills by default.").Value;
            if (!NemgineerPlugin.RiskOfOptionsLoaded)
                return;
            Config.RiskOfOptionsCompat();
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void RiskOfOptionsCompat()
        {
            
        }

        public static ConfigEntry<bool> CharacterEnableConfig(
          string characterName,
          string description = "Set to false to disable this character",
          bool enabledDefault = true)
        {
            return NemgineerPlugin.instance.Config.Bind<bool>("General", "Enable " + characterName, enabledDefault, description);
        }

        public static bool GetKeyPressed(ConfigEntry<KeyboardShortcut> entry)
        {
            KeyboardShortcut keyboardShortcut1 = entry.Value;
            foreach (KeyCode modifier in ((KeyboardShortcut)ref keyboardShortcut1).Modifiers)
            {
                if (!Input.GetKey(modifier))
                    return false;
            }
            KeyboardShortcut keyboardShortcut2 = entry.Value;
            return Input.GetKeyDown(((KeyboardShortcut)ref keyboardShortcut2).MainKey);
        }
    }
}
