using R2API;
using System;

namespace NemgineerMod.Modules
{
    internal static class Tokens
    {
        internal static void AddTokens()
        {
            #region Henry
            string prefix = NemgineerPlugin.DEVELOPER_PREFIX + "_NEMGINEER_BODY_";

            string desc = "Nemesis engineer is a medium range/melee slow and armored survivor, carrying his turrets on him instead of installing them.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Spear punch (?): Piercing, spear styled. Deal more damage in melee. Focus the healthier enemies since the weaker ones will die of piercing damage.200 % if it is a direct hit, 100 % for the piercing projectile. 0 sec cd.Proc coefficient: 1.25 for fists, 0.8 for spear damages." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Block: Negates all incoming damages in front of you, in a 180° radius, at the cost of being slow down by 30%, and that you cannot use your primary.Charge up to 10 % your battery depending on projectiles blocked(4 = max value).8 seconds cooldown(?)." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Explosive ideas: Launch: While sprinting: Your missiles launcher fires exploding missiles behind you, launching you in a curved form if you jumped and in front of you if you were on the ground. Needs both charges to do so.If standing still: Aim to launch exploding missiles like engineer’s harpoons.Deal 1000 % damage each.Can store up to 2.Proc coefficient of 1 per missile.Cooldown of 3 per missile." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Gear Up:Opens a menu in your abilities, replacing your M1, M2, Utility and Special.Repair: Retracts your turrets into your shoulders armor parts, where mini factories will fully repair your turrets.At full capacity: Drains charge at a speed of 10 % per second, boosting up damage(10 %), attack speed(10 %) and movement speed(30 %).Regular capacity: Stops the boost, letting you charge up your battery.Cheap charge: Consumes 25 % of your segmented health bar, giving you 10 % battery charge.Leave up to a maximum of 4 mines, starting by one, scaling with attack speed.Proc coefficient of 1 per mine, deals 600 % damage when an enemy walks nearby" + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, less human than ever before…";
            string outroFailure = "..and so he vanished, knowing the nonsense of his existence…";

            LanguageAPI.Add(prefix + "NAME", "Nemesis Engineer");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "Fortified expert");
            LanguageAPI.Add(prefix + "LORE", "sample lore");
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Warmonger");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Nemgineer passive");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Sample text.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_SLASH_NAME", "Spear Punch");
            LanguageAPI.Add(prefix + "PRIMARY_SLASH_DESCRIPTION", Helpers.agilePrefix + $"Punch forward for <style=cIsDamage>{100f * StaticValues.swordDamageCoefficient}% damage</style>.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_GUN_NAME", "Block");
            LanguageAPI.Add(prefix + "SECONDARY_GUN_DESCRIPTION", Helpers.agilePrefix + $"Negates damage in front of you, in a 180° radius.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_ROLL_NAME", "Explosive Ideas");
            LanguageAPI.Add(prefix + "UTILITY_ROLL_DESCRIPTION", "Launch: While sprinting: Your missiles launcher fires exploding missiles behind you, launching you in a curved form if you jumped and in front of you if you were on the ground. Needs both charges to do so.If standing still: Aim to launch exploding missiles like engineer’s harpoons.Deal 1000 % damage each.Can store up to 2.Proc coefficient of 1 per missile.Cooldown of 3 per missile.");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_BOMB_NAME", "Gear Up");
            LanguageAPI.Add(prefix + "SPECIAL_BOMB_DESCRIPTION", $"Opens a menu in your abilities, replacing your M1, M2, Utility and Special.Repair: Retracts your turrets into your shoulders armor parts, where mini factories will fully repair your turrets.At full capacity: Drains charge at a speed of 10 % per second, boosting up damage(10 %), attack speed(10 %) and movement speed(30 %).Regular capacity: Stops the boost, letting you charge up your battery.Cheap charge: Consumes 25 % of your segmented health bar, giving you 10 % battery charge.Leave up to a maximum of 4 mines, starting by one, scaling with attack speed.Proc coefficient of 1 per mine, deals 600 % damage when an enemy walks nearby.");
            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Nemesis Engineer: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As Nemesis Engineer, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Nemesis Engineer: Mastery");
            #endregion
            #endregion
        }
    }
}