namespace EntityStates.Nemgineer.NemgineerMissilePainter
{
    public class Finish : BaseNamegineerMissilePainterState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            if (!this.isAuthority)
                return;
            this.outer.SetNextState((EntityState)new Idle());
        }
    }
}