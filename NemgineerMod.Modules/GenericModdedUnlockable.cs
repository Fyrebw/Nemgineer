using R2API;
using RoR2;
using System;
using UnityEngine;

namespace NemgineerMod.Modules
{
    public abstract class GenericModdedUnlockable : ModdedUnlockable
    {
        public abstract string AchievementTokenPrefix { get; }

        public abstract string AchievementSpriteName { get; }

        public virtual string AchievementIdentifier => this.AchievementTokenPrefix + "UNLOCKABLE_ACHIEVEMENT_ID";

        public virtual string UnlockableIdentifier => this.AchievementTokenPrefix + "UNLOCKABLE_REWARD_ID";

        public virtual string AchievementNameToken => this.AchievementTokenPrefix + "UNLOCKABLE_ACHIEVEMENT_NAME";

        public virtual string AchievementDescToken => this.AchievementTokenPrefix + "UNLOCKABLE_ACHIEVEMENT_DESC";

        public virtual string UnlockableNameToken => this.AchievementTokenPrefix + "UNLOCKABLE_UNLOCKABLE_NAME";

        public virtual Sprite Sprite => Assets.mainAssetBundle.LoadAsset<Sprite>(this.AchievementSpriteName);

        public virtual Func<string> GetHowToUnlock => (Func<string>)(() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", (object)Language.GetString(base.AchievementNameToken), (object)Language.GetString(base.AchievementDescToken)));

        public virtual Func<string> GetUnlocked => (Func<string>)(() => Language.GetStringFormatted("UNLOCKED_FORMAT", (object)Language.GetString(base.AchievementNameToken), (object)Language.GetString(base.AchievementDescToken)));
    }
}
