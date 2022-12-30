using BepInEx.Configuration;
using NemgineerMod.Modules.Characters;
using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using UnityEngine;

namespace NemgineerMod.Modules.Survivors
{
    internal abstract class SurvivorBase : CharacterBase
    {
        public SurvivorDef survivorDef;

        public abstract string survivorTokenPrefix { get; }

        public abstract string cachedName { get; }

        public abstract UnlockableDef characterUnlockableDef { get; }

        public virtual ConfigEntry<bool> characterEnabledConfig { get; }

        public virtual GameObject displayPrefab { get; set; }

        public override void InitializeCharacter()
        {
            if (this.characterEnabledConfig != null && !this.characterEnabledConfig.Value)
                return;
            this.InitializeUnlockables();
            base.InitializeCharacter();
            this.InitializeSurvivor();
        }

        protected override void InitializeCharacterBodyAndModel()
        {
            base.InitializeCharacterBodyAndModel();
            this.InitializeDisplayPrefab();
        }

        protected virtual void InitializeSurvivor() => this.survivorDef = SurvivorBase.RegisterNewSurvivor(this.bodyPrefab, this.displayPrefab, Color.grey, this.survivorTokenPrefix, this.characterUnlockableDef, this.bodyInfo.sortPosition, this.cachedName);

        protected virtual void InitializeDisplayPrefab() => this.displayPrefab = Prefabs.CreateDisplayPrefab(this.bodyName + "Display", this.bodyPrefab, this.bodyInfo);

        public virtual void InitializeUnlockables()
        {
        }

        public static SurvivorDef RegisterNewSurvivor(
          GameObject bodyPrefab,
          GameObject displayPrefab,
          Color charColor,
          string tokenPrefix,
          UnlockableDef unlockableDef,
          float sortPosition,
          string cachedName)
        {
            SurvivorDef instance = ScriptableObject.CreateInstance<SurvivorDef>();
            instance.bodyPrefab = bodyPrefab;
            instance.displayPrefab = displayPrefab;
            instance.primaryColor = charColor;
            instance.displayNameToken = tokenPrefix + "NAME";
            instance.descriptionToken = tokenPrefix + "DESCRIPTION";
            instance.outroFlavorToken = tokenPrefix + "OUTRO_FLAVOR";
            instance.mainEndingEscapeFailureFlavorToken = tokenPrefix + "OUTRO_FAILURE";
            instance.desiredSortPosition = sortPosition;
            instance.unlockableDef = unlockableDef;
            instance.cachedName = cachedName;
            Content.AddSurvivorDef(instance);
            return instance;
        }

        protected virtual void AddCssPreviewSkill(
          int indexFromEditor,
          SkillFamily skillFamily,
          SkillDef skillDef)
        {
            CharacterSelectSurvivorPreviewDisplayController component = this.displayPrefab.GetComponent<CharacterSelectSurvivorPreviewDisplayController>();
            if (!(bool)(Object)component)
            {
                RMORMod.Log.Error((object)"trying to add skillChangeResponse to null CharacterSelectSurvivorPreviewDisplayController.\nMake sure you created one on your Display prefab in editor");
            }
            else
            {
                component.skillChangeResponses[indexFromEditor].triggerSkillFamily = skillFamily;
                component.skillChangeResponses[indexFromEditor].triggerSkill = skillDef;
            }
        }

        protected virtual void AddCssPreviewSkin(int indexFromEditor, SkinDef skinDef)
        {
            CharacterSelectSurvivorPreviewDisplayController component = this.displayPrefab.GetComponent<CharacterSelectSurvivorPreviewDisplayController>();
            if (!(bool)(Object)component)
                RMORMod.Log.Error((object)"trying to add skinChangeResponse to null CharacterSelectSurvivorPreviewDisplayController.\nMake sure you created one on your Display prefab in editor");
            else
                component.skinChangeResponses[indexFromEditor].triggerSkin = skinDef;
        }

        protected virtual void FinalizeCSSPreviewDisplayController()
        {
            if (!(bool)(Object)this.displayPrefab)
                return;
            CharacterSelectSurvivorPreviewDisplayController component = this.displayPrefab.GetComponent<CharacterSelectSurvivorPreviewDisplayController>();
            if (!(bool)(Object)component)
                return;
            component.bodyPrefab = this.bodyPrefab;
            List<CharacterSelectSurvivorPreviewDisplayController.SkillChangeResponse> skillChangeResponseList = new List<CharacterSelectSurvivorPreviewDisplayController.SkillChangeResponse>();
            for (int index = 0; index < component.skillChangeResponses.Length; ++index)
            {
                if ((Object)component.skillChangeResponses[index].triggerSkillFamily != (Object)null)
                    skillChangeResponseList.Add(component.skillChangeResponses[index]);
            }
            component.skillChangeResponses = skillChangeResponseList.ToArray();
        }
    }
}

