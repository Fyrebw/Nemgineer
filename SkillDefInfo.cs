using EntityStates;
using UnityEngine;

namespace NemgineerMod.Modules
{
    internal class SkillDefInfo
    {
        public string skillName;
        public string skillNameToken;
        public string skillDescriptionToken;
        public string[] keywordTokens = new string[0];
        public Sprite skillIcon;
        public SerializableEntityStateType activationState;
        public InterruptPriority interruptPriority;
        public string activationStateMachineName;
        public float baseRechargeInterval;
        public int baseMaxStock = 1;
        public int rechargeStock = 1;
        public int requiredStock = 1;
        public int stockToConsume = 1;
        public bool isCombatSkill = true;
        public bool canceledFromSprinting;
        public bool forceSprintDuringState;
        public bool cancelSprintingOnActivation = true;
        public bool beginSkillCooldownOnSkillEnd;
        public bool fullRestockOnAssign = true;
        public bool resetCooldownTimerOnUse;
        public bool mustKeyPress;

        public SkillDefInfo()
        {
        }

        public SkillDefInfo(
          string skillNameToken,
          string skillDescriptionToken,
          Sprite skillIcon,
          SerializableEntityStateType activationState,
          string activationStateMachineName = "Weapon",
          bool agile = false)
        {
            this.skillName = skillNameToken;
            this.skillNameToken = skillNameToken;
            this.skillDescriptionToken = skillDescriptionToken;
            this.skillIcon = skillIcon;
            this.activationState = activationState;
            this.activationStateMachineName = activationStateMachineName;
            this.interruptPriority = InterruptPriority.Any;
            this.isCombatSkill = true;
            this.baseRechargeInterval = 0.0f;
            this.requiredStock = 0;
            this.stockToConsume = 0;
            this.cancelSprintingOnActivation = !agile;
            if (!agile)
                return;
            this.keywordTokens = new string[1] { "KEYWORD_AGILE" };
        }
    }
}

