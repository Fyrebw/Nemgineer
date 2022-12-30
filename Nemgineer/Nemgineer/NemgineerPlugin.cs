using RoR2;
using UnityEngine;
using RoR2.Skills;
using System.Runtime.CompilerServices;
using UnityEngine.AddressableAssets;
using System.Collections;

namespace Moonstorm.Starstorm2.Survivors
{
    public sealed class Nemgineer : SurvivorBase
    {
        public override GameObject BodyPrefab { get; } = SS2Assets.LoadAsset<GameObject>("NemgineerBody");
        public override GameObject MasterPrefab { get; } = SS2Assets.LoadAsset<GameObject>("NemgineerMonsterMaster");
        public override SurvivorDef SurvivorDef { get; } = SS2Assets.LoadAsset<SurvivorDef>("SurvivorNemgineer");
        GameObject footstepDust { get; set; } = Resources.Load<GameObject>("Prefabs/GenericFootstepDust");

        public override void Initialize()
        {
            base.Initialize();
            if (Starstorm.ScepterInstalled)
            {
                ScepterCompat();
            }
            [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
            public void ScepterCompat()
            {
                AncientScepter.AncientScepterItem.instance.RegisterScepterSkill(SS2Assets.LoadAsset<SkillDef>("NemgineerScepterPrototypeΩ "), "NemgineerBody", SkillSlot.Special, 0);
                AncientScepter.AncientScepterItem.instance.RegisterScepterSkill(SS2Assets.LoadAsset<SkillDef>("NemgineerScepterPrototypeΣ "), "NemgineerBody", SkillSlot.Special, 1);
            }

        public override void ModifyPrefab()
        {
            var cb = BodyPrefab.GetComponent<CharacterBody>();
            cb._defaultCrosshairPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/UI/SimpleDotCrosshair.prefab").WaitForCompletion();
