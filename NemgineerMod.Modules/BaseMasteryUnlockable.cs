using RoR2;
using RoR2.Achievements;
using System;

namespace NemgineerMod.Modules
{
    public abstract class BaseMasteryUnlockable : GenericModdedUnlockable
    {
        public abstract string RequiredCharacterBody { get; }

        public abstract float RequiredDifficultyCoefficient { get; }

        public virtual void OnBodyRequirementMet()
        {
            base.OnBodyRequirementMet();
            Run.onClientGameOverGlobal += new Action<Run, RunReport>(this.OnClientGameOverGlobal);
        }

        public virtual void OnBodyRequirementBroken()
        {
            Run.onClientGameOverGlobal -= new Action<Run, RunReport>(this.OnClientGameOverGlobal);
            base.OnBodyRequirementBroken();
        }

        private void OnClientGameOverGlobal(Run run, RunReport runReport)
        {
            if (!(bool)(UnityEngine.Object)runReport.gameEnding || !runReport.gameEnding.isWin)
                return;
            DifficultyIndex difficulty = runReport.ruleBook.FindDifficulty();
            DifficultyDef difficultyDef = DifficultyCatalog.GetDifficultyDef(runReport.ruleBook.FindDifficulty());
            if (difficultyDef.countsAsHardMode && (double)difficultyDef.scalingValue >= (double)this.RequiredDifficultyCoefficient || difficulty >= DifficultyIndex.Eclipse1 && difficulty <= DifficultyIndex.Eclipse8 || difficultyDef.nameToken == "INFERNO_NAME")
                ((BaseAchievement)this).Grant();
        }

        public virtual BodyIndex LookUpRequiredBodyIndex() => BodyCatalog.FindBodyIndex(this.RequiredCharacterBody);
    }
}