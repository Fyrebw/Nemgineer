using NemgineerMod;
using NemgineerMod.Content;
using Nemgineer.Content.NemgineerSurvivor.Components.Body;
using NemgineerMod.SkillStates.BaseStates;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace EntityStates.Nemgineer.Primary
{
    public class SwingPunch : BaseMeleeAttack
    {
        public static NetworkSoundEventDef networkHitSound = (NetworkSoundEventDef)null;
        public static GameObject swingEffect = (GameObject)null;
        public static GameObject swingEffectFocus = (GameObject)null;
        public static GameObject hitEffect = (GameObject)null;
        public static AnimationCurve swingCurve = new AnimationCurve(new UnityEngine.Keyframe[3]
        {
      new UnityEngine.Keyframe(0.0f, 0.0f, 0.253129661f, float.PositiveInfinity, 0.0f, 0.333333343f),
      new UnityEngine.Keyframe(0.249295324f, 0.2f, -1.34473991f, -1.34473991f, 0.333333343f, 0.09076658f),
      new UnityEngine.Keyframe(0.6705322f, 0.0f, -0.102350622f, -0.102350622f, 0.733244061f, 0.0f)
        });
        private bool setNextState = false;
        private string animationLayer;
        public static float force = 1600f;
        private bool hitEnemy = false;

        public override void OnEnter()
        {
            this.bonusForce = Vector3.zero;
            this.attackRecoil = 0.0f;
            this.muzzleString = this.swingIndex == 1 ? "MuzzleHandL" : "MuzzleHandR";
            if ((Object)SwingPunch.networkHitSound != (Object)null)
                this.impactSound = SwingPunch.networkHitSound.index;
            this.damageType = DamageType.Generic;
            this.hitHopVelocity = 4f;
            this.scaleHitHopWithAttackSpeed = true;
            this.hitStopDuration = 0.1f;
            this.hitSoundString = "";
            this.swingSoundString = "Play_loader_m1_swing";
            this.hitboxName = "FistHitbox";
            this.damageCoefficient = 2f;
            this.procCoefficient = 1f;
            this.baseDuration = 1.3f;
            this.baseEarlyExitTime = 0.35f;
            this.attackStartTime = 0.283f;
            this.attackEndTime = 0.565f;
            this.pushForce = 0.0f;
            Vector3 direction = this.GetAimRay().direction with
            {
                y = 0.0f
            };
            direction.Normalize();
            this.bonusForce = SwingPunch.force * direction;
            this.forceForwardVelocity = true;
            this.forwardVelocityCurve = SwingPunch.swingCurve;
            this.animationLayer = "FullBody, Override";
            int num = (int)Util.PlaySound("Play_Nemgineer_StartPunch", this.gameObject);
            OverclockController component1 = this.GetComponent<OverclockController>();
            bool flag = (bool)(Object)component1 && component1.BuffActive();
            Animator modelAnimator = this.GetModelAnimator();
            if ((bool)(Object)modelAnimator)
                modelAnimator.SetFloat("hammerIdle", 0.0f);
            this.swingEffectPrefab = SwingPunch.swingEffect;
            if ((bool)(Object)this.characterBody)
            
            base.OnEnter();
            if ((bool)(Object)this.characterBody)
            {
                if (this.swingIndex != 0)
                    this.characterBody.OnSkillActivated(this.skillLocator.primary);
                HammerVisibilityController component2 = this.GetComponent<HammerVisibilityController>();
                if ((bool)(Object)component2)
                    component2.SetHammerEnabled(false);
                if (this.isAuthority && !flag)
                    this.characterBody.isSprinting = false;
                this.characterBody.SetAimTimer(3f);
            }
            
        }

        public override void FixedUpdate()
        {
            Vector3 direction = this.GetAimRay().direction with
            {
                y = 0.0f
            };
            direction.Normalize();
            if (this.attack != null)
                this.attack.forceVector = SwingPunch.force * direction;
            if ((bool)(Object)this.characterBody)
            {
                this.damageStat = this.characterBody.damage;
                
                {
                    this.swingEffectPrefab = SwingPunch.swingEffect;
                    this.attack.damageColorIndex = DamageColorIndex.Default;
                }
            }
            base.FixedUpdate();
        }

        public override void OnFiredAttack()
        {
            if (this.isAuthority)
                ShakeEmitter.CreateSimpleShakeEmitter(this.transform.position, new Wave()
                {
                    amplitude = 3f,
                    cycleOffset = 0.0f,
                    frequency = 4f
                }, 0.25f, 20f, true).transform.parent = this.transform;
            
        }

        protected override void PlayAttackAnimation()
        {
            if (this.swingIndex == 1)
                this.PlayCrossfade(this.animationLayer, "PunchR", "Punch.playbackRate", this.duration, 0.2f);
            else
                this.PlayCrossfade(this.animationLayer, "PunchL", "Punch.playbackRate", this.duration, 0.2f);
        }

        public override void OnExit()
        {
            if (!this.outer.destroying && !this.setNextState)
                this.PlayCrossfade(this.animationLayer, "BufferEmpty", "Punch.playbackRate", 0.2f, 0.2f);
            base.OnExit();
        }

        protected override void OnHitEnemyAuthority()
        
            
            
        

        protected override void SetNextState()
        {
            int num = this.swingIndex;
            switch (num)
            {
                case 0:
                case 2:
                    num = 1;
                    break;
                case 1:
                    num = 2;
                    break;
            }
            this.setNextState = true;
            EntityStateMachine outer = this.outer;
            SwingPunch newNextState = new SwingPunch();
            newNextState.swingIndex = num;
            outer.SetNextState((EntityState)newNextState);
        }

        public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.Skill;
    }
}

