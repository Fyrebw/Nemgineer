using System;
using BepInEx;
using EntityStates;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace NemgineerModPummelProjectile
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin(
        "com.Fyrebw.NemgineerMod",
        "Nemesis Engineer",
        "0.1.0")]
    [R2APISubmoduleDependency(nameof(LanguageAPI), nameof(ContentAddition))]
    public class PummelProjectile : BaseUnityPlugin
    {
        public void Awake()
        {
            //First we must load our survivor's Body prefab. For this tutorial, we are making a skill for Commando
            //If you would like to load a different survivor, you can find the key for their Body prefab at the following link
            //https://xiaoxiao921.github.io/GithubActionCacheTest/assetPathsDump.html
            GameObject commandoBodyPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Commando/CommandoBody.prefab").WaitForCompletion();

            //We use LanguageAPI to add strings to the game, in the form of tokens
            LanguageAPI.Add("NEMGINEER_PRIMARY_PUMMELPROJECTILE_NAME", "Pummel Projectile");
            LanguageAPI.Add("NEMGINEER_PRIMARY_PUMMELPROJECTILE_DESCRIPTION", $"Fire a projectile from your fist for <style=cIsDamage>100% damage</style>.");

            //Now we must create a SkillDef
            SkillDef PummelProjectileSkillDef = ScriptableObject.CreateInstance<SkillDef>();

            //Check step 2 for the code of the CustomSkillsTutorial.MyEntityStates.SimpleBulletAttack class
            PummelProjectileSkillDef.activationState = new SerializableEntityStateType(typeof(NemgineerModPummelProjectile.Nemgineer.SimplePummelProjectileAttack));
            PummelProjectileSkillDef.activationStateMachineName = "Weapon";
            PummelProjectileSkillDef.baseMaxStock = 1;
            PummelProjectileSkillDef.baseRechargeInterval = 0f;
            PummelProjectileSkillDef.beginSkillCooldownOnSkillEnd = true;
            PummelProjectileSkillDef.canceledFromSprinting = false;
            PummelProjectileSkillDef.cancelSprintingOnActivation = true;
            PummelProjectileSkillDef.fullRestockOnAssign = true;
            PummelProjectileSkillDef.interruptPriority = InterruptPriority.Any;
            PummelProjectileSkillDef.isCombatSkill = true;
            PummelProjectileSkillDef.mustKeyPress = false;
            PummelProjectileSkillDef.rechargeStock = 1;
            PummelProjectileSkillDef.requiredStock = 1;
            PummelProjectileSkillDef.stockToConsume = 1;
            //For the skill icon, you will have to load a Sprite from your own AssetBundle
            PummelProjectileSkillDef.icon = null;
            PummelProjectileSkillDef.skillDescriptionToken = "NEMGINEER_PRIMARY_PUMMELPROJECTILE_DESCRIPTION";
            PummelProjectileSkillDef.skillName = "NEMGINEER_PRIMARY_PUMMELPROJECTILE_NAME";
            PummelProjectileSkillDef.skillNameToken = "NEMGINEER_PRIMARY_PUMMELPROJECTILE_NAME";

            //This adds our skilldef. If you don't do this, the skill will not work.
            ContentAddition.AddSkillDef(PummelProjectileSkillDef);

            //Now we add our skill to one of the survivor's skill families
            //You can change component.primary to component.secondary, component.utility and component.special
            SkillLocator skillLocator = commandoBodyPrefab.GetComponent<SkillLocator>();
            SkillFamily skillFamily = skillLocator.primary.skillFamily;

            //If this is an alternate skill, use this code.
            //Here, we add our skill as a variant to the existing Skill Family.
            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = PummelProjectileSkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(PummelProjectileSkillDef.skillNameToken, false, null)
            };
        }
    }
}
