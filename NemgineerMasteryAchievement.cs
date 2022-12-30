using RoR2;
using RoR2.Achievements;

namespace NemgineerMod.Content.Nemgineer.Achievements
{
    [RegisterAchievement("FyrebwNemgineerClearGameMonsoon", "Skins.Nemgineer.Mastery", null, null)]
    public class NemgineerMasteryAchievement : BasePerSurvivorClearGameMonsoonAchievement
    {
        public override BodyIndex LookUpRequiredBodyIndex() => BodyCatalog.FindBodyIndex("NemgineerBody");
    }
}