using NemgineerPlugin;
using EntityStates.Commando.CommandoWeapon;
using NemgineerMod.Modules;
using RoR2;
using UnityEngine;

namespace EntityStates.Nemgineer.Primary
{
    public class ShotgunPunch : BaseSkillState
    {
        public const float RAD2 = 1.414f;
        public static float damageCoefficient = Config.shotgunPunchDamage.Value;
        public static float procCoefficient = Config.shotgunPunchProcCoefficient.Value;
        public float baseDuration = 1f;
        public static int bulletCount = Config.shotgunPunchBulletCount.Value;
        public static float bulletSpread = Config.shotgunPunchSpread.Value;
        public static float bulletRecoil = 8f;
        public static float bulletRange = Config.shotgunPunchRange.Value;
        public static float bulletThiccness = 0.7f;
        public static bool levelHasChanged;
        private float originalBulletThiccness = 0.7f;
        protected float duration;
        protected float fireDuration;
        protected float attackStopDuration;
        protected bool hasFired;
        private Animator animator;
        protected string muzzleString;
        protected NemgineerComponent NemgineerComponent;

        public override void OnEnter()
        {
            base.OnEnter();
            this.characterBody.SetAimTimer(5f);
            this.animator = this.GetModelAnimator();
            this.enforcerComponent = this.GetComponent<EnforcerComponent>();
            this.muzzleString = "Muzzle";
            this.hasFired = false;
            if (this.HasBuff(Modules.Buffs.BlockBuff) || this.HasBuff(Modules.Buffs.energyShieldBuff))
            {
                this.duration = this.baseShieldDuration / this.attackSpeedStat;
                this.attackStopDuration = RiotShotgun.beefDurationShield / this.attackSpeedStat;
                this.PlayAnimation("Gesture, Override", "ShieldFireShotgun", "FireShotgun.playbackRate", Mathf.Max(0.069f, this.duration));
            }
            else
            {
                this.duration = this.baseDuration / this.attackSpeedStat;
                this.attackStopDuration = RiotShotgun.beefDurationNoShield / this.attackSpeedStat;
                this.PlayCrossfade("Gesture, Override", "FireShotgun", "FireShotgun.playbackRate", Mathf.Max(0.05f, 1.75f * this.duration), 0.06f);
            }
            this.fireDuration = 0.0f;
        }

        public virtual void FireBullet()
        {
            if (this.hasFired)
                return;
            this.hasFired = true;
            bool flag = this.RollCrit();
            string soundString = flag ? Sounds.FireShotgunCrit : Sounds.FireShotgun;
            if (Config.classicShotgun.Value)
                soundString = Sounds.FireClassicShotgun;
            if (this.isStormtrooper)
                soundString = Sounds.FireBlasterShotgun;
            if (this.isEngi)
                soundString = Sounds.FireBungusShotgun;
            int num1 = (int)Util.PlayAttackSpeedSound(soundString, VRAPICompat.IsLocalVRPlayer(this.characterBody) ? VRAPICompat.GetPrimaryMuzzleObject() : this.gameObject, this.attackSpeedStat);
            float num2 = RiotShotgun.bulletRecoil / this.attackSpeedStat;
            if (this.HasBuff(Modules.Buffs.protectAndServeBuff) || this.HasBuff(Modules.Buffs.energyShieldBuff))
                num2 = RiotShotgun.shieldedBulletRecoil;
            this.AddRecoil(-0.4f * num2, -0.8f * num2, -0.3f * num2, 0.3f * num2);
            this.characterBody.AddSpreadBloom(4f);
            EffectManager.SimpleMuzzleFlash(FireBarrage.effectPrefab, this.gameObject, this.muzzleString, false);
            this.GetComponent<EnforcerWeaponComponent>().DropShell(-this.GetModelBaseTransform().transform.right * (float)-Random.Range(4, 12));
            if (this.isAuthority)
            {
                float num3 = RiotShotgun.damageCoefficient * this.damageStat;
                GameObject gameObject = EnforcerModPlugin.bulletTracer;
                if (this.isStormtrooper)
                    gameObject = EnforcerModPlugin.laserTracer;
                if (this.isEngi)
                    gameObject = EnforcerModPlugin.bungusTracer;
                Ray aimRay = this.GetAimRay();
                float bulletSpread = RiotShotgun.bulletSpread;
                float num4 = RiotShotgun.bulletThiccness;
                float num5 = 50f;
                if (this.HasBuff(Modules.Buffs.protectAndServeBuff))
                {
                    bulletSpread *= 0.8f;
                    num4 = 0.69f * num4;
                    num5 = 30f;
                }
                BulletAttack bulletAttack = new BulletAttack()
                {
                    aimVector = aimRay.direction,
                    origin = aimRay.origin,
                    damage = num3,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Generic,
                    falloffModel = BulletAttack.FalloffModel.None,
                    maxDistance = RiotShotgun.bulletRange,
                    force = num5,
                    hitMask = LayerIndex.CommonMasks.bullet,
                    isCrit = flag,
                    owner = this.gameObject,
                    muzzleName = this.muzzleString,
                    smartCollision = false,
                    procChainMask = new ProcChainMask(),
                    procCoefficient = RiotShotgun.procCoefficient,
                    radius = num4,
                    sniper = false,
                    stopperMask = LayerIndex.world.collisionMask,
                    weapon = (GameObject)null,
                    tracerEffectPrefab = gameObject,
                    spreadPitchScale = 1f,
                    spreadYawScale = 1f,
                    queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                    hitEffectPrefab = FireBarrage.hitEffectPrefab,
                    HitEffectNormal = false
                };
                bulletAttack.minSpread = 0.0f;
                bulletAttack.maxSpread = 0.0f;
                bulletAttack.bulletCount = 1U;
                bulletAttack.Fire();
                uint num6 = (uint)(Mathf.CeilToInt((float)RiotShotgun.bulletCount / 2f) - 1);
                bulletAttack.minSpread = 0.0f;
                bulletAttack.maxSpread = bulletSpread / 1.45f;
                bulletAttack.bulletCount = num6;
                bulletAttack.Fire();
                bulletAttack.minSpread = bulletSpread / 1.45f;
                bulletAttack.maxSpread = bulletSpread;
                bulletAttack.bulletCount = (uint)Mathf.FloorToInt((float)RiotShotgun.bulletCount / 2f);
                bulletAttack.Fire();
            }
        }

        private void thiccenTracer(ref GameObject tracerEffect)
        {
            foreach (LineRenderer componentsInChild in tracerEffect.GetComponentsInChildren<LineRenderer>())
            {
                if ((bool)(Object)componentsInChild)
                {
                    float num = RiotShotgun.bulletThiccness - this.originalBulletThiccness;
                    componentsInChild.widthMultiplier = (float)((1.0 + (double)num) * 0.5);
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.enforcerComponent.beefStop = false;
            if ((double)this.fixedAge > (double)this.fireDuration && (double)this.fixedAge < (double)this.attackStopDuration + (double)this.fireDuration && (bool)(Object)this.characterMotor)
            {
                this.characterMotor.moveDirection = Vector3.zero;
                this.enforcerComponent.beefStop = true;
            }
            if ((double)this.fixedAge >= (double)this.fireDuration)
                this.FireBullet();
            if ((double)this.fixedAge < (double)this.duration || !this.isAuthority)
                return;
            this.outer.SetNextStateToMain();
        }

        public override void OnExit()
        {
            base.OnExit();
            this.enforcerComponent.beefStop = false;
        }

        public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.Skill;
    }
}
