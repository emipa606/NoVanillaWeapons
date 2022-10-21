using Verse;

namespace NoVanillaWeapons;

/// <summary>
///     Definition of the settings for the mod
/// </summary>
internal class NoVanillaWeaponsSettings : ModSettings
{
    public bool Melee = true;
    public bool Ranged = true;

    /// <summary>
    ///     Saving and loading the values
    /// </summary>
    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref Melee, "Melee", true);
        Scribe_Values.Look(ref Ranged, "Ranged", true);
    }
}