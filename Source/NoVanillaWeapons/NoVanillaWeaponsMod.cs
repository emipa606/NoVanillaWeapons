using Mlie;
using UnityEngine;
using Verse;

namespace NoVanillaWeapons;

[StaticConstructorOnStartup]
internal class NoVanillaWeaponsMod : Mod
{
    /// <summary>
    ///     The instance of the settings to be read by the mod
    /// </summary>
    public static NoVanillaWeaponsMod Instance;

    private static string currentVersion;

    /// <summary>
    ///     The private settings
    /// </summary>
    private NoVanillaWeaponsSettings settings;

    /// <summary>
    ///     Cunstructor
    /// </summary>
    /// <param name="content"></param>
    public NoVanillaWeaponsMod(ModContentPack content) : base(content)
    {
        Instance = this;
        currentVersion =
            VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    /// <summary>
    ///     The instance-settings for the mod
    /// </summary>
    public NoVanillaWeaponsSettings Settings
    {
        get
        {
            settings ??= GetSettings<NoVanillaWeaponsSettings>();

            return settings;
        }
    }

    /// <summary>
    ///     The title for the mod-settings
    /// </summary>
    /// <returns></returns>
    public override string SettingsCategory()
    {
        return "No Vanilla Weapons";
    }

    /// <summary>
    ///     The settings-window
    ///     For more info: https://rimworldwiki.com/wiki/Modding_Tutorials/ModSettings
    /// </summary>
    /// <param name="rect"></param>
    public override void DoSettingsWindowContents(Rect rect)
    {
        var listingStandard = new Listing_Standard();
        listingStandard.Begin(rect);
        listingStandard.Gap();
        listingStandard.Label("NVW.Restart".Translate());
        listingStandard.CheckboxLabeled("NVW.Melee".Translate(), ref Settings.Melee, "NVW.Melee.Tip".Translate());
        listingStandard.CheckboxLabeled("NVW.Ranged".Translate(), ref Settings.Ranged,
            "NVW.Ranged.Tip".Translate());
        listingStandard.CheckboxLabeled("NVW.Grenades".Translate(), ref Settings.Grenades,
            "NVW.Grenades.Tip".Translate());
        listingStandard.Gap();
        if (!Settings.Ranged && !Settings.Melee && !Settings.Grenades)
        {
            listingStandard.Label("NVW.Why".Translate());
        }

        if (currentVersion != null)
        {
            listingStandard.Gap();
            GUI.contentColor = Color.gray;
            listingStandard.Label("NVW.ModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard.End();
    }
}