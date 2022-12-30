using EntityStates;
using EntityStates.Nemgineer;
using EntityStates.Nemgineer.Primary;
using RoR2.Skills;
using UnityEngine;

namespace Modules.Characters
{
    internal class NemgineerSkillDefs
    {
        public static SkillDef PrimarySkillDef_ShotgunPunch()
        {
            SkillDef instance = ScriptableObject.CreateInstance<SkillDef>();
            instance.activationState = new SerializableEntityStateType(typeof(ShotgunPunch));
            instance.activationStateMachineName = "Weapon";
            instance.baseMaxStock = 1;
            instance.baseRechargeInterval = 0.0f;
            instance.beginSkillCooldownOnSkillEnd = false;
            instance.canceledFromSprinting = false;
            instance.fullRestockOnAssign = true;
            instance.interruptPriority = InterruptPriority.Any;
            instance.resetCooldownTimerOnUse = false;
            instance.isCombatSkill = true;
            instance.mustKeyPress = false;
            instance.cancelSprintingOnActivation = true;
            instance.rechargeStock = 1;
            instance.requiredStock = 1;
            instance.stockToConsume = 1;
            instance.icon = Assets.iconShotgunPunch;
            instance.skillDescriptionToken = "Nemgineer_Primary_ShotgunPunch_Description";
            instance.skillName = "Nemgineer_Primary_ShotgunPunch_Name";
            instance.skillNameToken = "Nemgineer_Primary_ShotgunPunch_Description";
            return instance;
        }

        public static SkillDef PrimarySkillDef_PunchSpear()
        {
            SkillDef instance = ScriptableObject.CreateInstance<SkillDef>();
            instance.activationState = new SerializableEntityStateType(typeof(PunchSpear));
            instance.activationStateMachineName = "Weapon";
            instance.baseMaxStock = 1;
            instance.baseRechargeInterval = 0.0f;
            instance.beginSkillCooldownOnSkillEnd = false;
            instance.canceledFromSprinting = false;
            instance.fullRestockOnAssign = true;
            instance.interruptPriority = InterruptPriority.Any;
            instance.resetCooldownTimerOnUse = false;
            instance.isCombatSkill = true;
            instance.mustKeyPress = false;
            instance.cancelSprintingOnActivation = true;
            instance.rechargeStock = 1;
            instance.requiredStock = 1;
            instance.stockToConsume = 1;
            instance.icon = Assets.iconPunchSpear;
            instance.skillDescriptionToken = "Nemgineer_Primary_PunchSpear_Description";
            instance.skillName = "Nemgineer_Primary_PunchSpear_Name";
            instance.skillNameToken = "Nemgineer_Primary_PunchSpear_Name";
            return instance;
        }

        

       
        }

        public static SkillDef SecondarySkillDef_Block()
        {
            SkillDef instance = ScriptableObject.CreateInstance<SkillDef>();
            instance.activationState = new SerializableEntityStateType(typeof(Block));
            instance.activationStateMachineName = "Weapon";
            instance.baseMaxStock = 1;
            instance.baseRechargeInterval = 8f;
            instance.beginSkillCooldownOnSkillEnd = false;
            instance.canceledFromSprinting = false;
            instance.fullRestockOnAssign = true;
            instance.interruptPriority = InterruptPriority.Skill;
            instance.resetCooldownTimerOnUse = false;
            instance.isCombatSkill = true;
            instance.mustKeyPress = false;
            instance.cancelSprintingOnActivation = false;
            instance.rechargeStock = 1;
            instance.requiredStock = 1;
            instance.stockToConsume = 1;
            instance.icon = Assets.iconBlock;
            instance.skillName = "Nemgineer_Secondary_Block_Name";
            instance.skillNameToken = "Nemgineer_Secondary_Block_Name";
            instance.skillDescriptionToken = "Nemgineer_Secondary_Block_Description";
            return instance;
         
        }
        public static SkillDef SecondarySkillDef_ShieldBubbleMK2()
        {
            SkillDef instance = ScriptableObject.CreateInstance<SkillDef>();
            instance.activationState = new SerializableEntityStateType(typeof(ShieldBubbleMK2));
            instance.activationStateMachineName = "Weapon";
            instance.baseMaxStock = 1;
            instance.baseRechargeInterval = 16f;
            instance.beginSkillCooldownOnSkillEnd = false;
            instance.canceledFromSprinting = false;
            instance.fullRestockOnAssign = true;
            instance.interruptPriority = InterruptPriority.Skill;
            instance.resetCooldownTimerOnUse = false;
            instance.isCombatSkill = true;
            instance.mustKeyPress = false;
            instance.cancelSprintingOnActivation = false;
            instance.rechargeStock = 1;
            instance.requiredStock = 1;
            instance.stockToConsume = 1;
            instance.icon = Assets.iconShieldBubbleMK2;
            instance.skillName = "Nemgineer_Secondary_ShieldBubbleMK2_Name";
            instance.skillNameToken = "Nemgineer_Secondary_ShieldBubbleMK2_Name";
            instance.skillDescriptionToken = "Nemgineer_Secondary_ShieldBubbleMK2_Description";
            return instance;

        }

    public static SkillDef UtilitySkillDef_ExplosiveIdeas()
        {
            SkillDef instance = ScriptableObject.CreateInstance<SkillDef>();
            instance.activationState = new SerializableEntityStateType(typeof(ExplosiveIdeas));
            instance.activationStateMachineName = "Weapon";
            instance.baseMaxStock = 2;
            instance.baseRechargeInterval = 3f;
            instance.beginSkillCooldownOnSkillEnd = true;
            instance.canceledFromSprinting = false;
            instance.fullRestockOnAssign = true;
            instance.interruptPriority = InterruptPriority.Skill;
            instance.resetCooldownTimerOnUse = false;
            instance.isCombatSkill = true;
            instance.mustKeyPress = true;
            instance.cancelSprintingOnActivation = true;
            instance.rechargeStock = 1;
            instance.requiredStock = 1;
            instance.stockToConsume = 1;
            instance.icon = Assets.iconExplosiveIdeas;
            instance.skillDescriptionToken = "Nemgineer_Utility_ExplosiveIdeas_Description";
            instance.skillName = "Nemgineer_Utility_TEARGAS_Name";
            instance.skillNameToken = "Nemgineer_Utility_ExplosiveIdeas_Name";
           
            return instance;
        

        public static SkillDef SpecialSkillDef_GearUp()
        {
            SkillDef instance = ScriptableObject.CreateInstance<SkillDef>();
            instance.activationState = new SerializableEntityStateType(typeof(GearUp));
            instance.activationStateMachineName = "Weapon";
            instance.baseMaxStock = 1;
            instance.baseRechargeInterval = 0.0f;
            instance.beginSkillCooldownOnSkillEnd = false;
            instance.canceledFromSprinting = false;
            instance.fullRestockOnAssign = true;
            instance.interruptPriority = InterruptPriority.PrioritySkill;
            instance.resetCooldownTimerOnUse = false;
            instance.isCombatSkill = true;
            instance.mustKeyPress = true;
            instance.cancelSprintingOnActivation = true;
            instance.rechargeStock = 1;
            instance.requiredStock = 1;
            instance.stockToConsume = 1;
            instance.icon = Assets.icon40Shield;
            instance.skillDescriptionToken = "Nemgineer_Special_GearUp_Description";
            instance.skillName = "Nemgineer_Special_GearUp_Name";
            instance.skillNameToken = "Nemgineer_Special_GearUp_Name";
            return instance;
        }


        public static SkillDef SpecialSkillDef_EnergyShield()
        {
            SkillDef instance = ScriptableObject.CreateInstance<SkillDef>();
            instance.activationState = new SerializableEntityStateType(typeof(EnergyShield));
            instance.activationStateMachineName = "Weapon";
            instance.baseMaxStock = 1;
            instance.baseRechargeInterval = 0.0f;
            instance.beginSkillCooldownOnSkillEnd = false;
            instance.canceledFromSprinting = false;
            instance.fullRestockOnAssign = true;
            instance.interruptPriority = InterruptPriority.PrioritySkill;
            instance.resetCooldownTimerOnUse = false;
            instance.isCombatSkill = true;
            instance.mustKeyPress = true;
            instance.cancelSprintingOnActivation = true;
            instance.rechargeStock = 1;
            instance.requiredStock = 1;
            instance.stockToConsume = 1;
            instance.icon = Assets.testIcon;
            instance.skillDescriptionToken = "Nemgineer_Secondary_ShieldOn_Description";
            instance.skillName = "Nemgineer_Secondary_ShieldOn_Name";
            instance.skillNameToken = "Nemgineer_Secondary_ShieldOn_Name";
            return instance;
        }

        public static SkillDef SpecialSkillDef_EnergyShieldDown()
        {
            SkillDef instance = ScriptableObject.CreateInstance<SkillDef>();
            instance.activationState = new SerializableEntityStateType(typeof(EnergyShield));
            instance.activationStateMachineName = "Weapon";
            instance.baseMaxStock = 1;
            instance.baseRechargeInterval = 0.0f;
            instance.beginSkillCooldownOnSkillEnd = false;
            instance.canceledFromSprinting = false;
            instance.fullRestockOnAssign = true;
            instance.interruptPriority = InterruptPriority.PrioritySkill;
            instance.resetCooldownTimerOnUse = false;
            instance.isCombatSkill = true;
            instance.mustKeyPress = true;
            instance.cancelSprintingOnActivation = false;
            instance.rechargeStock = 1;
            instance.requiredStock = 1;
            instance.stockToConsume = 1;
            instance.icon = Assets.testIcon;
            instance.skillDescriptionToken = "Nemgineer_Secondary_ShieldOff_Description";
            instance.skillName = "Nemgineer_Secondary_ShieldOff_Name";
            instance.skillNameToken = "Nemgineer_Secondary_ShieldOff_Name";
            return instance;
        }

        
        
    }
}

