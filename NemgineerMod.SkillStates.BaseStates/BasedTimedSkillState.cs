using EntityStates;

namespace NemgineerMod.SkillStates.BaseStates
{
    public class BaseTimedSkillState : BaseSkillState
    {
        public static float TimedBaseDuration;
        public static float TimedBaseCastStartTime;
        public static float TimedBaseCastEndTime;
        protected float duration;
        protected float castStartTime;
        protected float castEndTime;
        protected bool hasFired;
        protected bool isFiring;
        protected bool hasExited;

        protected virtual void InitDurationValues(
          float baseDuration,
          float baseCastStartTime,
          float baseCastEndTime = 1f)
        {
            BaseTimedSkillState.TimedBaseDuration = baseDuration;
            BaseTimedSkillState.TimedBaseCastStartTime = baseCastStartTime;
            BaseTimedSkillState.TimedBaseCastEndTime = baseCastEndTime;
            this.duration = BaseTimedSkillState.TimedBaseDuration / this.attackSpeedStat;
            this.castStartTime = baseCastStartTime * this.duration;
            this.castEndTime = baseCastEndTime * this.duration;
        }

        protected virtual void OnCastEnter()
        {
        }

        protected virtual void OnCastFixedUpdate()
        {
        }

        protected virtual void OnCastUpdate()
        {
        }

        protected virtual void OnCastExit()
        {
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!this.hasFired && (double)this.fixedAge > (double)this.castStartTime)
            {
                this.hasFired = true;
                this.OnCastEnter();
            }
            bool flag1 = (double)this.fixedAge >= (double)this.castStartTime;
            bool flag2 = (double)this.fixedAge >= (double)this.castEndTime;
            this.isFiring = false;
            if (flag1 && !flag2 || flag1 & flag2 && !this.hasFired)
            {
                this.isFiring = true;
                this.OnCastFixedUpdate();
            }
            if (flag2 && !this.hasExited)
            {
                this.hasExited = true;
                this.OnCastExit();
            }
            if ((double)this.fixedAge <= (double)this.duration)
                return;
            this.outer.SetNextStateToMain();
        }

        public override void Update()
        {
            base.Update();
            if (!this.isFiring)
                return;
            this.OnCastUpdate();
        }
    }
}
