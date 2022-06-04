using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace Mementos.Items
{
	public class TokyoSkiesMB:ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("[c/0022AA:Tokyo Skies] [c/000000:-] [c/FF3377:Chipzel]");
		}

		public override void SetDefaults() {
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.consumable = false;
			item.createTile = TileType<Tiles.TokyoSkiesMBTile>();
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.Purple;
			item.value = 100000;
			item.accessory = true;
			item.maxStack = 69;
		}

		public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(173,50);	// obby
			recipe.AddIngredient(1346,999);	// nanites
			recipe.AddIngredient(1082,1000);// purple
			recipe.AddIngredient(1094,100);	// deep purple
			recipe.AddTile(228);			// dye vat
			recipe.AddTile(56);				// obsidian
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
