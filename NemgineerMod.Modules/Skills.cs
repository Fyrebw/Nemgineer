using RoR2;
using RoR2.Skills;
using System;
using UnityEngine;

namespace NemgineerMod.Modules
{
    internal static class Skills
    {
        public static void CreateSkillFamilies(GameObject targetPrefab, bool destroyExisting = true)
        {
            if (destroyExisting)
            {
                foreach (UnityEngine.Object componentsInChild in targetPrefab.GetComponentsInChildren<GenericSkill>())
                    UnityEngine.Object.DestroyImmediate(componentsInChild);
            }
            SkillLocator component = targetPrefab.GetComponent<SkillLocator>();
            component.primary = NemgineerMod.Modules.Skills.CreateGenericSkillWithSkillFamily(targetPrefab, "Primary");
            component.secondary = NemgineerMod.Modules.Skills.CreateGenericSkillWithSkillFamily(targetPrefab, "Secondary");
            component.utility = NemgineerMod.Modules.Skills.CreateGenericSkillWithSkillFamily(targetPrefab, "Utility");
            component.special = NemgineerMod.Modules.Skills.CreateGenericSkillWithSkillFamily(targetPrefab, "Special");
        }

        public static GenericSkill CreateGenericSkillWithSkillFamily(
          GameObject targetPrefab,
          string familyName,
          bool hidden = false)
        {
            GenericSkill skillWithSkillFamily = targetPrefab.AddComponent<GenericSkill>();
            skillWithSkillFamily.skillName = familyName;
            skillWithSkillFamily.hideInCharacterSelect = hidden;
            SkillFamily instance = ScriptableObject.CreateInstance<SkillFamily>();
            instance.name = targetPrefab.name + familyName + "Family";
            instance.variants = new SkillFamily.Variant[0];
            skillWithSkillFamily._skillFamily = instance;
            Content.AddSkillFamily(instance);
            return skillWithSkillFamily;
        }

        public static void AddSkillToFamily(
          SkillFamily skillFamily,
          SkillDef skillDef,
          UnlockableDef unlockableDef = null)
        {
            Array.Resize<SkillFamily.Variant>(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant()
            {
                skillDef = skillDef,
                unlockableDef = unlockableDef,
                viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false)
            };
        }

        public static void AddSkillsToFamily(SkillFamily skillFamily, params SkillDef[] skillDefs)
        {
            foreach (SkillDef skillDef in skillDefs)
                RMORMod.Modules.Skills.AddSkillToFamily(skillFamily, skillDef);
        }

        public static void AddPrimarySkills(GameObject targetPrefab, params SkillDef[] skillDefs) => NemgineerMod.Modules.Skills.AddSkillsToFamily(targetPrefab.GetComponent<SkillLocator>().primary.skillFamily, skillDefs);

        public static void AddSecondarySkills(GameObject targetPrefab, params SkillDef[] skillDefs) => NemgineerMod.Modules.Skills.AddSkillsToFamily(targetPrefab.GetComponent<SkillLocator>().secondary.skillFamily, skillDefs);

        public static void AddUtilitySkills(GameObject targetPrefab, params SkillDef[] skillDefs) => NemgineerMod.Modules.Skills.AddSkillsToFamily(targetPrefab.GetComponent<SkillLocator>().utility.skillFamily, skillDefs);

        public static void AddSpecialSkills(GameObject targetPrefab, params SkillDef[] skillDefs) => NemgineerMod.Modules.Skills.AddSkillsToFamily(targetPrefab.GetComponent<SkillLocator>().special.skillFamily, skillDefs);

        public static void AddUnlockablesToFamily(
          SkillFamily skillFamily,
          params UnlockableDef[] unlockableDefs)
        {
            for (int index = 0; index < unlockableDefs.Length; ++index)
            {
                SkillFamily.Variant variant = skillFamily.variants[index] with
                {
                    unlockableDef = unlockableDefs[index]
                };
                skillFamily.variants[index] = variant;
            }
        }

        public static SkillDef CreateSkillDef(SkillDefInfo skillDefInfo) => NemgineerMod.Modules.Skills.CreateSkillDef<SkillDef>(skillDefInfo);

        public static T CreateSkillDef<T>(SkillDefInfo skillDefInfo) where T : SkillDef
        {
            T instance = ScriptableObject.CreateInstance<T>();
            instance.skillName = skillDefInfo.skillName;
            instance.name = skillDefInfo.skillName;
            instance.skillNameToken = skillDefInfo.skillNameToken;
            instance.skillDescriptionToken = skillDefInfo.skillDescriptionToken;
            instance.icon = skillDefInfo.skillIcon;
            instance.activationState = skillDefInfo.activationState;
            instance.activationStateMachineName = skillDefInfo.activationStateMachineName;
            instance.baseMaxStock = skillDefInfo.baseMaxStock;
            instance.baseRechargeInterval = skillDefInfo.baseRechargeInterval;
            instance.beginSkillCooldownOnSkillEnd = skillDefInfo.beginSkillCooldownOnSkillEnd;
            instance.canceledFromSprinting = skillDefInfo.canceledFromSprinting;
            instance.forceSprintDuringState = skillDefInfo.forceSprintDuringState;
            instance.fullRestockOnAssign = skillDefInfo.fullRestockOnAssign;
            instance.interruptPriority = skillDefInfo.interruptPriority;
            instance.resetCooldownTimerOnUse = skillDefInfo.resetCooldownTimerOnUse;
            instance.isCombatSkill = skillDefInfo.isCombatSkill;
            instance.mustKeyPress = skillDefInfo.mustKeyPress;
            instance.cancelSprintingOnActivation = skillDefInfo.cancelSprintingOnActivation;
            instance.rechargeStock = skillDefInfo.rechargeStock;
            instance.requiredStock = skillDefInfo.requiredStock;
            instance.stockToConsume = skillDefInfo.stockToConsume;
            instance.keywordTokens = skillDefInfo.keywordTokens;
            Content.AddSkillDef((SkillDef)instance);
            return instance;
        }

        public static void FixScriptableObjectName(SkillDef sk) => sk.name = sk.skillName;
    }
}
