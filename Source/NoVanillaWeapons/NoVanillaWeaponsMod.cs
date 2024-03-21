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
    public static NoVanillaWeaponsMod instance;

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
        instance = this;
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
            if (settings == null)
            {
                settings = GetSettings<NoVanillaWeaponsSettings>();
            }

            return settings;
        }
        set => settings = value;
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
        var listing_Standard = new Listing_Standard();
        listing_Standard.Begin(rect);
        listing_Standard.Gap();
        listing_Standard.Label("NVW.Restart".Translate());
        listing_Standard.CheckboxLabeled("NVW.Melee".Translate(), ref Settings.Melee, "NVW.Melee.Tip".Translate());
        listing_Standard.CheckboxLabeled("NVW.Ranged".Translate(), ref Settings.Ranged,
            "NVW.Ranged.Tip".Translate());
        listing_Standard.Gap();
        if (!Settings.Ranged && !Settings.Melee)
        {
            listing_Standard.Label("NVW.Why".Translate());
        }

        if (currentVersion != null)
        {
            listing_Standard.Gap();
            GUI.contentColor = Color.gray;
            listing_Standard.Label("NVW.ModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listing_Standard.End();
        Settings.Write();
    }
}