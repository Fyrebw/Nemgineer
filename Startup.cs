namespace EntityStates.Nemgineer.NemgineerMissilePainter
{
    public class Startup : BaseNemgineerMissilePainterState
    {
        public static float baseDuration;
        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = Startup.baseDuration / this.attackSpeedStat;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!this.isAuthority || (double)this.duration > (double)this.fixedAge)
                return;
            this.outer.SetNextState((EntityState)new Paint());
        }
    }
}
