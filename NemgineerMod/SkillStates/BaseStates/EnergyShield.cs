using NemgineerPlugin;
using Modules;
using Modules.Characters;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace EntityStates.Nemgineer
{
    public class EnergyShield : BaseSkillState
    {
        public static float enterDuration = 0.7f;
        public static float exitDuration = 0.9f;
        public static float bonusMass = 15000f;
        private float duration;
        private NemgineerComponent shieldComponent;
        private Animator animator;
        private ChildLocator childLocator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.animator = this.GetModelAnimator();
            this.shieldComponent = this.characterBody.GetComponent<NemgineerComponent>();
            this.childLocator = this.GetModelTransform().GetComponent<ChildLocator>();
            this.shieldComponent.isShielding = !this.HasBuff(NemgineerMod.Modules.Buffs.energyShieldBuff);
            if (this.HasBuff(Modules.Buffs.energyShieldBuff))
            {
                this.duration = EnergyShield.exitDuration / this.attackSpeedStat;
                this.EnableEnergyShield(false);
                this.PlayAnimation("LeftArm, Override", "ShieldDown", "ShieldUp.playbackRate", this.duration);
                if ((bool)(Object)this.skillLocator)
                    this.skillLocator.special.SetBaseSkill(NemgineerSurvivor.energyShieldEnterDef);
                if ((bool)(Object)this.characterMotor)
                    this.characterMotor.mass = 200f;
                if (NetworkServer.active)
                    this.characterBody.RemoveBuff(NemgineerMod.Modules.Buffs.energyShieldBuff);
            }
            else
            {
                this.duration = EnergyShield.enterDuration / this.attackSpeedStat;
                this.EnableEnergyShield(true);
                this.PlayAnimation("LeftArm, Override", "ShieldUp", "ShieldUp.playbackRate", this.duration);
                if ((bool)(Object)this.skillLocator)
                    this.skillLocator.special.SetBaseSkill(NemgineerSurvivor.energyShieldExitDef);
                if ((bool)(Object)this.characterMotor)
                    this.characterMotor.mass = EnergyShield.bonusMass;
                if (NetworkServer.active)
                    this.characterBody.AddBuff(NemgineerMod.Modules.Buffs.energyShieldBuff);
            }
        }

        private void EnableEnergyShield(bool what)
        {
            if (!(bool)(Object)this.childLocator)
                return;
            this.childLocator.FindChild(nameof(EnergyShield)).gameObject.SetActive(what);
        }

        public override void OnExit() => base.OnExit();

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if ((double)this.fixedAge < (double)this.duration || !this.isAuthority)
                return;
            this.outer.SetNextStateToMain();
        }

        public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.Death;
    }
}

