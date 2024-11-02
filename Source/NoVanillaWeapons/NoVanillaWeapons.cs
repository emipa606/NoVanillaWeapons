using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace NoVanillaWeapons;

[StaticConstructorOnStartup]
internal static class NoVanillaWeapons
{
    static NoVanillaWeapons()
    {
        var vanillaWeapons = DefDatabase<ThingDef>.AllDefsListForReading
            .Where(weapon => weapon is { IsWeapon: true, modContentPack.IsOfficialMod: true } &&
                             (NoVanillaWeaponsMod.instance.Settings.Melee || weapon.IsMeleeWeapon != true) &&
                             (NoVanillaWeaponsMod.instance.Settings.Ranged || weapon.IsRangedWeapon != true) &&
                             (NoVanillaWeaponsMod.instance.Settings.Grenades ||
                              weapon.weaponTags?.Any(tag => tag.ToLower().Contains("grenade")) != true) &&
                             !weapon.IsStuff && weapon.weaponTags?.Contains("TurretGun") == false &&
                             !weapon.destroyOnDrop).ToList();

        foreach (var thingDef in vanillaWeapons)
        {
            thingDef.destroyOnDrop = true;
            thingDef.weaponTags?.RemoveAll(tag => !tag.Contains("Mechanoid"));
            thingDef.thingSetMakerTags?.Clear();
            if (thingDef.weaponTags?.Any() == false)
            {
                thingDef.generateCommonality = 0;
                thingDef.generateAllowChance = 0;
            }
            else
            {
                Log.Message(
                    $"[NoVanillaWeapons]: Skipping hard removal of {thingDef} from the database as its used by mechanoids. It will just be blocked from humanoids.");
            }

            thingDef.recipeMaker = null;
            thingDef.scatterableOnMapGen = false;
            thingDef.tradeability = Tradeability.None;
            thingDef.tradeTags?.Clear();
        }

        for (var i = vanillaWeapons.Count - 1; i > 0; i--)
        {
            if (vanillaWeapons[i].weaponTags?.Any(tag => tag.Contains("Mechanoid")) == true)
            {
                continue;
            }

            GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), typeof(ThingDef), "Remove",
                vanillaWeapons[i]);
        }

        DefDatabase<ThingDef>.ResolveAllReferences();

        var weaponRecipes = from recipe in DefDatabase<RecipeDef>.AllDefsListForReading
            where vanillaWeapons.Contains(recipe.ProducedThingDef) || (from product in recipe.products
                where vanillaWeapons.Contains(product.thingDef)
                select product).Any()
            select recipe;

        foreach (var weaponRecipe in weaponRecipes)
        {
            weaponRecipe.factionPrerequisiteTags = ["NotForYou"];
        }

        DefDatabase<RecipeDef>.ResolveAllReferences();

        var vanillaNames = new List<string>();
        vanillaWeapons.ForEach(def => vanillaNames.Add(def.label));
        Log.Message(
            $"[NoVanillaWeapons]: Removed {vanillaWeapons.Count} vanilla weapons: {string.Join(",", vanillaNames)}");
    }
}