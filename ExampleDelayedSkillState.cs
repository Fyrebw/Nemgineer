namespace NemgineerMod.SkillStates.BaseStates
{
    public class ExampleDelayedSkillState : BaseTimedSkillState
    {
        public static float SkillBaseDuration = 1.5f;
        public static float SkillStartTime = 0.2f;

        public override void OnEnter()
        {
            base.OnEnter();
            this.InitDurationValues(ExampleDelayedSkillState.SkillBaseDuration, ExampleDelayedSkillState.SkillStartTime);
        }

        protected override void OnCastEnter()
        {
        }
    }
}

