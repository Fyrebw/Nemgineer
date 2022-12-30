using NemgineerPlugin;
using NemgineerMod.Modules;
using NemgineerMod.Modules.Characters;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace EntityStates.Enforcer
{
    public class Block : BaseSkillState
    {
        public static float enterDuration = 0.5f;
        public static float exitDuration = 0.6f;
        public static float bonusMass = 15000f;
        private float duration;
        private NemgineerComponent shieldComponent;
        private Animator animator;
        private ChildLocator childLocator;
        private NemgineerWeaponComponent weaponComponent;

        public override void OnEnter()
        {
            base.OnEnter();
            this.animator = this.GetModelAnimator();
            this.shieldComponent = this.characterBody.GetComponent<EnforcerComponent>();
            this.childLocator = this.GetModelChildLocator();
            this.weaponComponent = this.GetComponent<NemgineerWeaponComponent>();
            if (this.HasBuff(Modules.Buffs.protectAndServeBuff))
            {
                this.duration = Block.exitDuration / this.attackSpeedStat;
                this.shieldComponent.isShielding = false;
                this.PlayAnimation("Gesture, Override", "BufferEmpty");
                this.PlayAnimation("Grenade, Override", "BufferEmpty");
                this.PlayAnimation("FullBody, Override", "BufferEmpty");
                this.PlayAnimation("Shield", "ShieldDown", "ShieldMode.playbackRate", this.duration);
                this.childLocator.FindChild("ShieldHurtboxParent").gameObject.SetActive(false);
                if ((bool)(Object)this.skillLocator)
                    this.skillLocator.secondary.SetBaseSkill(NemgineerSurvivor.shieldEnterDef);
                if ((bool)(Object)this.characterMotor)
                    this.characterMotor.mass = 200f;
                if (NetworkServer.active)
                    this.characterBody.RemoveBuff(Modules.Buffs.BlockBuff);
                int num = (int)Util.PlaySound(Sounds.ShieldDown, VRAPICompat.IsLocalVRPlayer(this.characterBody) ? VRAPICompat.GetShieldMuzzleObject() : this.gameObject);
                if (VRAPICompat.IsLocalVRPlayer(this.characterBody))
                    return;
                this.characterBody.aimOriginTransform = this.shieldComponent.origOrigin;
            }
            else
            {
                this.duration = Block.enterDuration / this.attackSpeedStat;
                this.shieldComponent.isShielding = true;
                this.PlayAnimation("Gesture, Override", "BufferEmpty");
                this.PlayAnimation("Grenade, Override", "BufferEmpty");
                this.PlayAnimation("FullBody, Override", "BufferEmpty");
                this.PlayAnimation("Shield", "ShieldUp", "ShieldMode.playbackRate", this.duration);
                this.childLocator.FindChild("ShieldHurtboxParent").gameObject.SetActive(true);
                if ((bool)(Object)this.skillLocator)
                    this.skillLocator.special.SetBaseSkill(Nemgineer.shieldExitDef);
                if ((bool)(Object)this.characterMotor)
                    this.characterMotor.mass = Block.bonusMass;
                if (NetworkServer.active)
                    this.characterBody.AddBuff(Modules.Buffs.BlockBuff);
                int num = (int)Util.PlaySound(Sounds.ShieldUp, VRAPICompat.IsLocalVRPlayer(this.characterBody) ? VRAPICompat.GetShieldMuzzleObject() : this.gameObject);
                if (!VRAPICompat.IsLocalVRPlayer(this.characterBody))
                    this.characterBody.aimOriginTransform = this.childLocator.FindChild("ShieldAimOrigin");
            }
        }

        public override void OnExit()
        {
            if ((bool)(Object)this.characterBody)
                this.characterBody.SetSpreadBloom(0.0f, false);
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if ((double)this.fixedAge < (double)this.duration || !this.isAuthority)
                return;
            this.outer.SetNextStateToMain();
        }

        public override InterruptPriority GetMinimumInterruptPriority() => this.HasBuff(Modules.Buffs.BlockBuff) ? InterruptPriority.PrioritySkill : InterruptPriority.Skill;
    }
}
