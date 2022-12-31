# TILER2

## SUPPORT DISCLAIMER

### Use of a mod manager is STRONGLY RECOMMENDED.

Seriously, use a mod manager.

If the versions of TILER2 (or possibly any other mods) are different between your game and other players' in multiplayer, things WILL break. If TILER2 is causing kicks for "unspecified reason", it's likely due to a mod version mismatch. Ensure that all players in a server, including the host and/or dedicated server, are using the same mod versions before reporting a bug.

**While reporting a bug, make sure to post a console log** (`path/to/RoR2/BepInEx/LogOutput.log`) from a run of the game where the bug happened; this often provides important information about why the bug is happening. If the bug is multiplayer-only, please try to include logs from both server and client.

## Description

TILER2 is a library mod. It won't do much on its own, but it may be required for some other mods.

### User-Facing Features

TILER2 mostly contains features that are useful for mod developers, but it also adds some things that normal users can take advantage of.

#### DebugUtil

The DebugUtil module adds several console commands:

- `evo_setitem itemIndexOrName count`: Sets the count of the target item in the Artifact of Evolution item pool. Marked as a cheat command.
- `goto_itemrender`: Travels to the ingame item rendering scene. Can only be used from the main menu. Best paired with a runtime inspector mod.
- `ir_sim itemIndexOrName`: Spawns an item model while in the ingame item rendering scene.
- `ir_sqm itemIndexOrName`: Spawns an equipment model while in the ingame item rendering scene.

#### NetConfig

The NetConfig module automatically syncs important config settings from the server to any connecting clients, and kicks clients with critical config mismatches which can't be resolved (i.e. settings that can't be changed while the game is running, or client has different mods than server).

NetConfig also adds the console commands `ncfg_get`, `ncfg_set`, `ncfg_settemp`, and `ncfg`; and the convar `ncfg_allowclientset`.

- `ncfg_get "path1" "optional path2" "optional path3"`: Attempts to find a config entry. Path matches, in order: mod name, config section, config key. If you weren't specific enough, it will print all matching paths to console; otherwise, it will print detailed information about one specific config entry.
- `ncfg_set "path1" "optional path2" "optional path3" value`: Attempts to permanently set a config entry (writes to config file AND changes the ingame value), following the same search rules as ncfg_get. Not usable by non-host players; will route to ncfg_settemp instead.
- `ncfg_settemp "path1" "optional path2" "optional path3" value`: Attempts to temporarily set a config entry until the end of the current run, following the same search rules as ncfg_get. Can be blocked from use by non-host players via ncfg_allowclientset.
- `ncfg "cmd" ...`: Routes to ncfg_get, ncfg_set, or ncfg_settemp (for when you forget the underscore).
- `ncfg_allowclientset` (bool): If 1, any player on a server can use ncfg_settemp. If 0, only the host can use ncfg_settemp.

## Issues/TODO

- Items which players have but were disabled mid-run need a UI indicator for such.
- If a client gets kicked by R2API mod mismatch, NetConfig will attempt to kick them again (to no effect) due to timeout.
- See the GitHub repo for more!

## Changelog

The 5 latest updates are listed below. For a full changelog, see: https://github.com/ThinkInvis/RoR2-TILER2/blob/master/changelog.md

**7.3.2**

- Various fixes and improvements to ConCmds `ir_sim` and `ir_sqm`:
	- Fixed hiding the entire model placeholder instead of individual models.
	- Fixed failing if the model placeholder is inactive but present.
	- Now searches by internal code name instead of by language token lookup (latter failed inexplicably in some cases).
- For developers: NuGet config is now localized (building project no longer requires end-user modification of system or directory NuGet config).

**7.3.1**

- Migrated private method CatalogBoilerplate.GetBestLanguage to public in MiscUtil.

**7.3.0**

- Refactored AutoConfig Risk of Options integration into a much more extensible pattern.
	- Attributes inheriting from `BaseAutoConfigRoOAttribute` will be automatically scanned during `AutoConfigContainer.BindRoO()`.
- Overhauled CatalogBoilerplate language systems to work with R2API.LanguageAPI's language file auto-loading.
	- Non-breaking change: `GetNameString()` et. al. remain in place, and are now virtual instead of abstract.
	- Default behavior is to read tokens with old names ("ModName_ItemName_NAME"), and use the new methods `GetNameStringArgs()` et. al. to retrieve strings to insert during interpolation into a rendered-at-runtime token ("ModName_ItemName_NAME_RENDERED").
		- ItemDefs, EquipmentDefs, etc. now use these rendered tokens.
- Language tokens managed by TILER2 are now completely rebuilt and reloaded during ConCmd language_reload.
- Also migrated some internal strings (mostly in NetConfig/AutoConfig) to language tokens.

**7.2.1**

- Added a performance option to hide duplicate or all Item Ward displays.
- Patched a null safety hole in MiscUtil.GetRootWithLocators.
- Removed some unused BepInEx plugin soft dependency flags.

**7.2.0**

- Added barebones config preset support to the AutoConfig module.
	- See `AutoConfigPresetAttribute`, `AutoConfigContainer.ApplyPreset()`.
- Added support for Risk of Options buttons.
	- No attribute, must use `Compat_RiskOfOptions.AddOption_Button()` manually.
- Publicized `AutoConfigContainer.FindConfig()`.
- Removed remaining unused BetterUI references.
- Updated lang version to C#9 and implemented its features for some minor project cleanup.
- Updated dependencies.