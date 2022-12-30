using RoR2;
using RoR2.Achievements;
using System;

namespace NemgineerMod.Content.Nemgineer.Achievements
{
    [RegisterAchievement("FyrebwNemgineerSurvivorUnlock", "Characters.Nemgineer", null, null)]
    public class NemgineerSurvivorUnlockAchievement : BaseAchievement
    {
        this.Grant();
    }
}