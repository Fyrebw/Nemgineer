using EntityStates;
using RoR2;
using RoR2.Audio;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace NemgineerMod.SkillStates.BaseStates
{
    public class BaseMeleeAttack : BaseSkillState
    {
        public int swingIndex;
        protected string hitboxName = "Sword";
        protected DamageType damageType = DamageType.Generic;
        protected float damageCoefficient = 2f;
        protected float procCoefficient = 1f;
        protected float pushForce = 0f;
        protected Vector3 bonusForce = Vector3.zero;
        protected float baseDuration = 1f;
        protected float attackStartTime = 0.2f;
        protected float attackEndTime = 0.4f;
        protected float baseEarlyExitTime = 0.4f;
        protected float hitStopDuration = 0.012f;
        protected float attackRecoil = 0f;
        protected float hitHopVelocity = 4f;
        protected bool cancelled = false;
        protected bool forceForwardVelocity = false;
        protected AnimationCurve forwardVelocityCurve;
        protected bool startedSkillStationary = false;
        protected string swingSoundString = "";
        protected string hitSoundString = "";
        protected string muzzleString = "SwingCenter";
        protected GameObject swingEffectPrefab;
        protected GameObject hitEffectPrefab;
        protected NetworkSoundEventIndex impactSound;
        protected bool scaleHitHopWithAttackSpeed = false;
        protected float earlyExitTime;
        public float duration;
        protected bool hasFired;
        private float hitPauseTimer;
        protected OverlapAttack attack;
        protected bool inHitPause;
        private bool hasHopped;
        protected float stopwatch;
        protected Animator animator;
        private BaseState.HitStopCachedState hitStopCachedState;
        private Vector3 storedVelocity;

        public override void OnEnter()
        {
            base.OnEnter();
            this.startedSkillStationary = (bool)(UnityEngine.Object)this.inputBank && this.inputBank.moveVector == Vector3.zero;
            this.duration = this.baseDuration;
            this.earlyExitTime = this.baseEarlyExitTime / this.attackSpeedStat;
            this.hasFired = false;
            this.animator = this.GetModelAnimator();
            this.StartAimMode(0.5f + this.duration);
            this.characterBody.outOfCombatStopwatch = 0.0f;
            this.animator.SetBool("attacking", true);
            HitBoxGroup hitBoxGroup = (HitBoxGroup)null;
            Transform modelTransform = this.GetModelTransform();
            if ((bool)(UnityEngine.Object)modelTransform)
                hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (Predicate<HitBoxGroup>)(element => element.groupName == this.hitboxName));
            this.PlayAttackAnimation();
            this.attack = new OverlapAttack();
            this.attack.damageType = this.damageType;
            this.attack.attacker = this.gameObject;
            this.attack.inflictor = this.gameObject;
            this.attack.teamIndex = this.GetTeam();
            this.attack.damage = this.damageCoefficient * this.damageStat*(this.attackSpeedStat/2);
            this.attack.procCoefficient = this.procCoefficient;
            this.attack.hitEffectPrefab = this.hitEffectPrefab;
            this.attack.forceVector = this.bonusForce;
            this.attack.pushAwayForce = this.pushForce;
            this.attack.hitBoxGroup = hitBoxGroup;
            this.attack.isCrit = this.RollCrit();
            this.attack.impactSound = this.impactSound;
        }

        protected virtual void PlayAttackAnimation() => this.PlayCrossfade("Gesture, Override", "Slash" + (1 + this.swingIndex).ToString(), "Slash.playbackRate", this.duration, 0.05f);

        public override void OnExit()
        {
            if (!this.hasFired && !this.cancelled)
                this.FireAttack();
            base.OnExit();
            this.animator.SetBool("attacking", false);
        }

        protected virtual void PlaySwingEffect()
        {
            if (!(bool)(UnityEngine.Object)this.swingEffectPrefab)
                return;
            EffectManager.SimpleMuzzleFlash(this.swingEffectPrefab, this.gameObject, this.muzzleString, true);
        }

        protected virtual void OnHitEnemyAuthority()
        {
            int num = (int)Util.PlaySound(this.hitSoundString, this.gameObject);
            if (!this.hasHopped)
            {
                if ((bool)(UnityEngine.Object)this.characterMotor && !this.characterMotor.isGrounded && (double)this.hitHopVelocity > 0.0)
                {
                    float hitHopVelocity = this.hitHopVelocity;
                    if (this.scaleHitHopWithAttackSpeed)
                        hitHopVelocity /= Mathf.Sqrt(this.attackSpeedStat);
                    this.SmallHop(this.characterMotor, hitHopVelocity);
                }
                this.hasHopped = true;
            }
            if (this.inHitPause || (double)this.hitStopDuration <= 0.0)
                return;
            this.storedVelocity = this.characterMotor.velocity;
            this.hitStopCachedState = this.CreateHitStopCachedState(this.characterMotor, this.animator, "Slash.playbackRate");
            this.hitPauseTimer = this.hitStopDuration / this.attackSpeedStat;
            this.inHitPause = true;
        }

        private void FireAttack()
        {
            if (!this.hasFired)
            {
                if ((bool)(UnityEngine.Object)this.characterDirection)
                    this.characterDirection.forward = this.GetAimRay().direction;
                this.hasFired = true;
                int num = (int)Util.PlayAttackSpeedSound(this.swingSoundString, this.gameObject, this.attackSpeedStat);
                this.OnFiredAttack();
                if (this.isAuthority)
                {
                    this.PlaySwingEffect();
                    this.AddRecoil(-1f * this.attackRecoil, -2f * this.attackRecoil, -0.5f * this.attackRecoil, 0.5f * this.attackRecoil);
                }
            }
            if (!this.isAuthority || !this.attack.Fire())
                return;
            this.OnHitEnemyAuthority();
        }

        public virtual void OnFiredAttack()
        {
        }

        protected virtual void SetNextState()
        {
            int num = this.swingIndex != 0 ? 0 : 1;
            this.outer.SetNextState((EntityState)new BaseMeleeAttack()
            {
                swingIndex = num
            });
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.hitPauseTimer -= Time.fixedDeltaTime;
            if (!this.inHitPause)
                this.stopwatch += Time.fixedDeltaTime;
            if (this.isAuthority)
            {
                if ((double)this.hitPauseTimer <= 0.0 && this.inHitPause)
                {
                    this.ConsumeHitStopCachedState(this.hitStopCachedState, this.characterMotor, this.animator);
                    this.inHitPause = false;
                    this.characterMotor.velocity = this.storedVelocity;
                }
                if (!this.inHitPause)
                {
                    if (this.forceForwardVelocity && (bool)(UnityEngine.Object)this.characterMotor && (bool)(UnityEngine.Object)this.characterDirection && !this.startedSkillStationary)
                    {
                        Vector3 vector3 = this.characterDirection.forward * this.forwardVelocityCurve.Evaluate(this.fixedAge / this.duration);
                        this.characterMotor.AddDisplacement(new Vector3(vector3.x, 0.0f, vector3.z));
                    }
                }
                else
                {
                    if ((bool)(UnityEngine.Object)this.characterMotor)
                        this.characterMotor.velocity = Vector3.zero;
                    if ((bool)(UnityEngine.Object)this.animator)
                        this.animator.SetFloat("Swing.playbackRate", 0.0f);
                }
            }
            if ((double)this.stopwatch >= (double)this.duration * (double)this.attackStartTime && (double)this.stopwatch <= (double)this.duration * (double)this.attackEndTime)
                this.FireAttack();
            if ((double)this.stopwatch >= (double)this.duration - (double)this.earlyExitTime && this.isAuthority && this.inputBank.skill1.down)
            {
                if (!this.hasFired)
                    this.FireAttack();
                this.SetNextState();
            }
            else
            {
                if (!this.isAuthority || (double)this.stopwatch < (double)this.duration)
                    return;
                this.outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.Skill;

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(this.swingIndex);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            this.swingIndex = reader.ReadInt32();
        }
    }
}