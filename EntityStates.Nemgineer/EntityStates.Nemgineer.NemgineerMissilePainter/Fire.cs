using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace EntityStates.Nemgineer.NemgineerMissilePainter
{
    public class Fire : BaseNemgineerMissilePainterState
    {
        public static float baseDurationPerMissile;
        public static float damageCoefficient;
        public static GameObject projectilePrefab;
        public static GameObject muzzleflashEffectPrefab;
        public List<HurtBox> targetsList;
        private int fireIndex;
        private float durationPerMissile;
        private float stopwatch;

        public override void OnEnter()
        {
            base.OnEnter();
            this.durationPerMissile = Fire.baseDurationPerMissile / this.attackSpeedStat;
            this.PlayAnimation("Gesture, Additive", "IdleHarpoons");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            bool flag = false;
            if (this.isAuthority)
            {
                this.stopwatch += Time.fixedDeltaTime;
                if ((double)this.stopwatch >= (double)this.durationPerMissile)
                {
                    this.stopwatch -= this.durationPerMissile;
                    while (this.fireIndex < this.targetsList.Count)
                    {
                        HurtBox targets = this.targetsList[this.fireIndex++];
                        if (!(bool)(Object)targets.healthComponent || !targets.healthComponent.alive)
                        {
                            this.activatorSkillSlot.AddOneStock();
                        }
                        else
                        {
                            string str = this.fireIndex % 2 == 0 ? "MuzzleLeft" : "MuzzleRight";
                            Vector3 position = this.inputBank.aimOrigin;
                            Transform modelChild = this.FindModelChild(str);
                            if ((Object)modelChild != (Object)null)
                                position = modelChild.position;
                            EffectManager.SimpleMuzzleFlash(Fire.muzzleflashEffectPrefab, this.gameObject, str, true);
                            this.FireMissile(targets, position);
                            flag = true;
                            break;
                        }
                    }
                    if (this.fireIndex >= this.targetsList.Count)
                        this.outer.SetNextState((EntityState)new Finish());
                }
            }
            if (!flag)
                return;
            this.PlayAnimation(this.fireIndex % 2 == 0 ? "Gesture Left Cannon, Additive" : "Gesture Right Cannon, Additive", "FireHarpoon");
        }

        private void FireMissile(HurtBox target, Vector3 position) => MissileUtils.FireMissile(this.inputBank.aimOrigin, this.characterBody, new ProcChainMask(), target.gameObject, this.damageStat * Fire.damageCoefficient, this.RollCrit(), Fire.projectilePrefab, DamageColorIndex.Default, Vector3.up, 0.0f, false);

        public override void OnExit()
        {
            base.OnExit();
            this.PlayCrossfade("Gesture, Additive", "ExitHarpoons", 0.1f);
        }
    }
}
