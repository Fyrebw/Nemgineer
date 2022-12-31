using AncientScepter;
using BepInEx;
using BepInEx.Bootstrap;
using On.RoR2;
using R2API.Utils;
using NemgineerMod.Modules;
using NemgineerMod.Modules.Characters;
using RoR2;
using RoR2.Skills;
using System.Runtime.CompilerServices;
using ThinkInvisible.ClassicItems;
using TILER2;
using UnityEngine;

namespace NemgineerPlugin
{
    [BepInDependency]
    [BepInDependency]
    [NetworkCompatibility]
    [BepInDependency]
    [BepInDependency]
    [BepInDependency]
    [R2APISubmoduleDependency(new string[] { "PrefabAPI", "LanguageAPI", "SoundAPI", "UnlockableAPI", "DamageAPI", "RecalculateStatsAPI" })]
    [BepInDependency]
    [BepInPlugin("com.Fyrebw.Nemgineer", "Nemgineer", "0.1.0")]
    public class NemgineerPlugin : BaseUnityPlugin
    {
        public const string MODUID = "com.Fyrebw.Nemgineer";
        public const string MODNAME = "Nemgineer";
        public const string MODVERSION = "0.1.0";
        public const string DEVELOPER_PREFIX = "Fyrebw";
        public static NemgineerPlugin instance;
        public static bool infernoPluginLoaded = false;
        public static bool scepterStandaloneLoaded = false;
        public static bool scepterClassicLoaded = false;
        public static bool emoteAPILoaded = false;
        public static bool riskOfOptionsLoaded = false;
        public static bool msPaintIcons = false;
        public static bool pocketICBM = true;
        public static bool pocketICBMEnableKnockback = false;
        public static bool samTracking = true;

        private void Awake()
        {
            NemgineerPlugin.instance = this;
            NemgineerPlugin.infernoPluginLoaded = Chainloader.PluginInfos.ContainsKey("HIFU.Inferno");
            NemgineerPlugin.scepterStandaloneLoaded = Chainloader.PluginInfos.ContainsKey("com.DestroyedClone.AncientScepter");
            NemgineerPlugin.scepterClassicLoaded = Chainloader.PluginInfos.ContainsKey("com.ThinkInvisible.ClassicItems");
            NemgineerPlugin.emoteAPILoaded = Chainloader.PluginInfos.ContainsKey("com.weliveinasociety.CustomEmotesAPI");
            NemgineerPlugin.riskOfOptionsLoaded = Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions");
            Log.Init(this.Logger);
            this.ReadConfig();
            DamageTypes.Initialize();
            Buffs.Initialize();
            Assets.Initialize();
            Config.ReadConfig();
            States.RegisterStates();
            Projectiles.RegisterProjectiles();
            Tokens.AddTokens();
            ItemDisplays.PopulateDisplays();
            new NemgineerSetup().Initialize();
            new ContentPacks().Initialize();
            if (!NemgineerPlugin.emoteAPILoaded)
                return;
            this.EmoteAPICompat();
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void EmoteAPICompat() => SurvivorCatalog.Init += NemgineerPlugin.\u003C\u003Ec.\u003C\u003E9__15_0 ?? (NemgineerPlugin.\u003C\u003Ec.\u003C\u003E9__15_0 = new SurvivorCatalog.hook_Init((object) RocketSurvivorPlugin.\u003C\u003Ec.\u003C\u003E9, __methodptr(\u003CEmoteAPICompat\u003Eb__15_0)));

    private void ReadConfig()
        {
            NemgineerPlugin.msPaintIcons = this.Config.Bind<bool>("General", "Use MSPaint icons", false, "Use the original MSPaint icons from the mod's release.").Value;
            NemgineerPlugin.pocketICBM = this.Config.Bind<bool>("Gameplay", "Pocket ICBM Interaction", true, "Pocket ICBM works with Nemgineer's utility.").Value;
            NemgineerPlugin.pocketICBMEnableKnockback = this.Config.Bind<bool>("Gameplay", "Pocket ICBM Knockback", false, "Extra rockets from Pocket ICBM have knockback.").Value;
   
        }

        public static float GetICBMDamageMult(CharacterBody body)
        {
            float icbmDamageMult = 1f;
            if ((bool)(Object)body && (bool)(Object)body.inventory)
            {
                int num = body.inventory.GetItemCount(DLC1Content.Items.MoreMissile) - 1;
                if (num > 0)
                    icbmDamageMult += (float)num * 0.5f;
            }
            return icbmDamageMult;
        }

        public static void SetupScepterClassic(
          string bodyName,
          SkillDef scepterSkill,
          SkillDef origSkill)
        {
            if (!NemgineerPlugin.scepterClassicLoaded)
                return;
            NemgineerPlugin.SetupScepterClassicInternal(bodyName, scepterSkill, origSkill);
        }

        public static void SetupScepterStandalone(
          string bodyName,
          SkillDef scepterSkill,
          SkillSlot skillSlot,
          int skillIndex)
        {
            if (!NemgineerPlugin.scepterStandaloneLoaded)
                return;
            NemgineerPlugin.SetupScepterStandaloneInternal(bodyName, scepterSkill, skillSlot, skillIndex);
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static void SetupScepterStandaloneInternal(
          string bodyName,
          SkillDef scepterSkill,
          SkillSlot skillSlot,
          int skillIndex)
        {
            ItemBase<AncientScepterItem>.instance.RegisterScepterSkill(scepterSkill, bodyName, skillSlot, skillIndex);
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static void SetupScepterClassicInternal(
          string bodyName,
          SkillDef scepterSkill,
          SkillDef origSkill)
        {
            Item<Scepter>.instance.RegisterScepterSkill(scepterSkill, bodyName, SkillSlot.Special, origSkill);
        }
    }
}
