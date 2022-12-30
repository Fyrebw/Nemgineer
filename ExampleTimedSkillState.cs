
namespace NemgineerMod.SkillStates.BaseStates
{
    public class ExampleTimedSkillState : BaseTimedSkillState
    {
        public static float SkillBaseDuration = 1.5f;
        public static float SkillStartTime = 0.2f;
        public static float SkillEndTime = 0.9f;

        public override void OnEnter()
        {
            base.OnEnter();
            this.InitDurationValues(ExampleTimedSkillState.SkillBaseDuration, ExampleTimedSkillState.SkillStartTime, ExampleTimedSkillState.SkillEndTime);
        }

        protected override void OnCastEnter()
        {
        }

        protected override void OnCastFixedUpdate()
        {
        }

        protected override void OnCastExit()
        {
        }
    }
}
