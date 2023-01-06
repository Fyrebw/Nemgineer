
using BepInEx.Configuration;
using static NemgineerMod.NemgineerPlugin;
using EntityStates;
using EntityStates.Nemgineer;
using R2API;
using RoR2;
using RoR2.CharacterAI;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using NemgineerMod;

namespace Modules.Characters
{
    public class NemgineerSurvivor : SurvivorBase
    {
        public const string Nemgineer_PREFIX = "NEMGINEER_";
        public static SkillDef shieldEnterDef;
        public static SkillDef shieldExitDef;
        public static SkillDef energyShieldEnterDef;
        public static SkillDef energyShieldExitDef;
        public static SkillDef boardEnterDef;
        public static SkillDef boardExitDef;

     
       

        public override CustomRendererInfo[] customRendererInfos => new CustomRendererInfo[9]
        {
      new CustomRendererInfo() { childName = "ShieldModel" },

        };

        public override Type characterMainState => typeof(NemgineerMain);

        public override ItemDisplaysBase itemDisplays => (ItemDisplaysBase)null;

        public override UnlockableDef characterUnlockableDef => NemgineerUnlockables.enforcerUnlockableDef;

        public override ConfigEntry<bool> characterEnabledConfig => (ConfigEntry<bool>)null;

        public override void Initialize()
        {
            base.Initialize();
            this.Hooks();
        }

        private void Hooks()
        {
        }

        protected override void InitializeCharacterBodyAndModel()
        {
            base.InitializeCharacterBodyAndModel();
            this.bodyPrefab.GetComponent<SfxLocator>().deathSound = Sounds.DeathSound;
            this.characterBodyModel.GetComponent<ChildLocator>().FindChild("Chair").GetComponent<MeshRenderer>().material.SetHotpooMaterial();
            this.characterBodyModel.GetComponent<ChildLocator>();
            this.bodyPrefab.gameObject.AddComponent<NemgineerComponent>().Init();
            this.bodyPrefab.gameObject.AddComponent<NemgineerWeaponComponent>().Init();
           
        }

       

        public override void InitializeHurtboxes(HealthComponent healthComponent)
        {
            HurtBoxGroup component1 = this.characterBodyModel.gameObject.GetComponent<HurtBoxGroup>();
            ChildLocator component2 = this.characterBodyModel.GetComponent<ChildLocator>();
            HurtBox hurtBox1 = component2.FindChild("ShieldHurtbox").gameObject.AddComponent<HurtBox>();
            hurtBox1.gameObject.layer = LayerIndex.entityPrecise.intVal;
            hurtBox1.healthComponent = healthComponent;
            hurtBox1.isBullseye = false;
            hurtBox1.damageModifier = HurtBox.DamageModifier.Barrier;
            hurtBox1.hurtBoxGroup = component1;
            HurtBox hurtBox2 = component2.FindChild("ShieldHurtbox2").gameObject.AddComponent<HurtBox>();
            hurtBox2.gameObject.layer = LayerIndex.entityPrecise.intVal;
            hurtBox2.healthComponent = healthComponent;
            hurtBox2.isBullseye = false;
            hurtBox2.damageModifier = HurtBox.DamageModifier.Barrier;
            hurtBox2.hurtBoxGroup = component1;
            component1.hurtBoxes = new HurtBox[3]
            {
        hurtBox1,
        hurtBox2,
        component1.hurtBoxes[0]
            };
        }

        public override void InitializeHitboxes()
        {
            ChildLocator component = this.characterBodyModel.GetComponent<ChildLocator>();
            HitBoxGroup hitBoxGroup1 = this.characterBodyModel.gameObject.AddComponent<HitBoxGroup>();
            GameObject gameObject1 = component.FindChild("ChargeHitbox").gameObject;
            HitBox hitBox1 = gameObject1.AddComponent<HitBox>();
            gameObject1.layer = LayerIndex.projectile.intVal;
            hitBoxGroup1.hitBoxes = new HitBox[1] { hitBox1 };
            hitBoxGroup1.groupName = "Charge";
            HitBoxGroup hitBoxGroup2 = this.characterBodyModel.gameObject.AddComponent<HitBoxGroup>();
            GameObject gameObject2 = component.FindChild("ActualHammerHitbox").gameObject;
            HitBox hitBox2 = gameObject2.AddComponent<HitBox>();
            gameObject2.layer = LayerIndex.projectile.intVal;
            hitBoxGroup2.hitBoxes = new HitBox[1] { hitBox2 };
            hitBoxGroup2.groupName = "Hammer";
            HitBoxGroup hitBoxGroup3 = this.characterBodyModel.gameObject.AddComponent<HitBoxGroup>();
            GameObject gameObject3 = component.FindChild("HammerHitboxBig").gameObject;
            HitBox hitBox3 = gameObject3.AddComponent<HitBox>();
            gameObject3.layer = LayerIndex.projectile.intVal;
            hitBoxGroup3.hitBoxes = new HitBox[1] { hitBox3 };
            hitBoxGroup3.groupName = "HammerBig";
            FootstepHandler footstepHandler = this.characterBodyModel.gameObject.AddComponent<FootstepHandler>();
            footstepHandler.baseFootstepString = "Play_player_footstep";
            footstepHandler.sprintFootstepOverrideString = "";
            footstepHandler.enableFootstepDust = true;
            footstepHandler.footstepDustPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/GenericFootstepDust");
        }

      

        public override void InitializeSkills()
        {
            Modules.Skills.CreateSkillFamilies(this.bodyPrefab);
            this.InitializePrimarySkills();
            this.InitializeSecondarySkills();
            this.InitializeUtilitySkills();
            this.InitializeSpecialSkills();
            
            this.FinalizeCSSPreviewDisplayController();
            
        }

       

        private void InitializePrimarySkills()
        {
            Content.AddEntityState(typeof(Pummel));
            SkillDef skillDef1 = NemgineerSkillDefs.PrimarySkillDef_Pummel();
            NemgineerMod.Modules.Skills.AddPrimarySkills(this.bodyPrefab, skillDef1);
            NemgineerMod.Modules.Skills.AddUnlockablesToFamily(this.bodyPrefab.GetComponent<SkillLocator>().primary.skillFamily, null);
            SkillLocator component1 = this.bodyPrefab.GetComponent<SkillLocator>();
            CharacterSelectSurvivorPreviewDisplayController component2 = this.displayPrefab.GetComponent<CharacterSelectSurvivorPreviewDisplayController>();
            component2.skillChangeResponses[0].triggerSkillFamily = component1.primary.skillFamily;
            component2.skillChangeResponses[0].triggerSkill = skillDef1;
        }

        
      
        private void InitializeSecondarySkills()
        {
            NemgineerMod.AddEntityState(typeof(Block));
            NemgineerMod.AddEntityState(typeof(EnergyShield));
            NemgineerSurvivor.shieldEnterDef = NemgineerSkillDefs.SecondarySkillDef_Block();
            NemgineerSurvivor.shieldExitDef = NemgineerSkillDefs.SecondarySkillDef_ShieldDown();
            NemgineerSurvivor.energyShieldEnterDef = NemgineerSkillDefs.SecondarySkillDef_EnergyShield();
            NemgineerSurvivor.energyShieldExitDef = NemgineerSkillDefs.SecondarySkillDef_EnergyShieldDown();
            NemgineerMod.Modules.Skills.AddSpecialSkills(this.bodyPrefab, NemgineerSurvivor.shieldEnterDef);
            SkillLocator component1 = this.bodyPrefab.GetComponent<SkillLocator>();
            CharacterSelectSurvivorPreviewDisplayController component2 = this.displayPrefab.GetComponent<CharacterSelectSurvivorPreviewDisplayController>();
            component2.skillChangeResponses[4].triggerSkillFamily = component1.special.skillFamily;
            component2.skillChangeResponses[4].triggerSkill = NemgineerSurvivor.shieldEnterDef;
         
        }

     
        

        

    }
}
