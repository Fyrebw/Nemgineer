namespace EntityStates.Nemgineer.NemgineerMissilePainter
{
    public class BaseNemgineerMissilePainterState : BaseSkillState
    {
        public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.Pain;
    }
}
