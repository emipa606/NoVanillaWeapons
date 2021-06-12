using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace NoVanillaWeapons
{
    [StaticConstructorOnStartup]
    internal static class NoVanillaWeapons
    {
        static NoVanillaWeapons()
        {
            var vanillaWeapons = (from ThingDef weapon in DefDatabase<ThingDef>.AllDefsListForReading
                where weapon?.IsWeapon == true &&
                      weapon.modContentPack?.PackageId?.Contains(ModContentPack.CoreModPackageId) == true
                select weapon).ToList();

            foreach (var thingDef in vanillaWeapons)
            {
                thingDef.destroyOnDrop = true;
                thingDef.generateCommonality = 0;
                thingDef.thingSetMakerTags?.Clear();
                thingDef.weaponTags?.Clear();
                thingDef.generateAllowChance = 0;
                thingDef.recipeMaker = null;
                thingDef.scatterableOnMapGen = false;
                thingDef.tradeability = Tradeability.None;
                thingDef.tradeTags?.Clear();
            }

            for (var i = vanillaWeapons.Count - 1; i > 0; i--)
            {
                GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), typeof(ThingDef), "Remove",
                    vanillaWeapons[i]);
            }

            var weaponRecipes = from recipe in DefDatabase<RecipeDef>.AllDefsListForReading
                where vanillaWeapons.Contains(recipe.ProducedThingDef) || (from product in recipe.products
                    where vanillaWeapons.Contains(product.thingDef)
                    select product).Any()
                select recipe;

            foreach (var weaponRecipe in weaponRecipes)
            {
                weaponRecipe.factionPrerequisiteTags = new List<string> {"NotForYou"};
            }
        }
    }
}