using RoR2;
using UnityEngine;

namespace NemgineerMod.SkillStates.BaseStates
{
    public class FireGauss : BaseState
    {
        public static GameObject effectPrefab;
        public static GameObject hitEffectPrefab;
        public static GameObject tracerEffectPrefab;
        public static string attackSoundString;
        public static float damageCoefficient;
        public static float force;
        public static float minSpread;
        public static float maxSpread;
        public static int bulletCount;
        public static float baseDuration = 2f;
        public int bulletCountCurrent = 1;
        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = FireGauss.baseDuration / this.attackSpeedStat;
            int num = (int)Util.PlaySound(FireGauss.attackSoundString, this.gameObject);
            Ray aimRay = this.GetAimRay();
            this.StartAimMode(aimRay);
            this.PlayAnimation("Gesture", nameof(FireGauss), "FireGauss.playbackRate", this.duration);
            string muzzleName = "Muzzle";
            if ((bool)(Object)FireGauss.effectPrefab)
                EffectManager.SimpleMuzzleFlash(FireGauss.effectPrefab, this.gameObject, muzzleName, false);
            if (!this.isAuthority)
                return;
            new BulletAttack()
            {
                owner = this.gameObject,
                weapon = this.gameObject,
                origin = aimRay.origin,
                aimVector = aimRay.direction,
                minSpread = FireGauss.minSpread,
                maxSpread = FireGauss.maxSpread,
                bulletCount = 1U,
                damage = (FireGauss.damageCoefficient * this.damageStat),
                force = FireGauss.force,
                tracerEffectPrefab = FireGauss.tracerEffectPrefab,
                muzzleName = muzzleName,
                hitEffectPrefab = FireGauss.hitEffectPrefab,
                isCrit = Util.CheckRoll(this.critStat, this.characterBody.master),
                HitEffectNormal = false,
                radius = 0.15f
            }.Fire();
        }

        public override void OnExit() => base.OnExit();

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if ((double)this.fixedAge < (double)this.duration || !this.isAuthority)
                return;
            this.outer.SetNextStateToMain();
        }

        public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.Skill;
    }
}


