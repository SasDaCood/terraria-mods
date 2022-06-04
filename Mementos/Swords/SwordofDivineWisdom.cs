using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using static Terraria.ModLoader.ModContent;

namespace Mementos.Swords
{
	public class SwordofDivineWisdom : ModItem
	{
		private const int health_penalty = 15, crit_dust_max_speed = 10, crit_dust_count = 20;
		private string[] sas_crit_msgs = {"stop hitting me :(","ouchies","The industrial revolution and its consequences","):"};		// testing to see if i can trigger chat messages through random events like these

		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Longsword of Divinity");
			ItemID.Sets.ItemNoGravity[item.type] = true;
			Tooltip.SetDefault($"very holy, [c/FF5544:no touchy]\nuses hopes and dreams ([i:23]) as ammo");
		}

		public override void SetDefaults() {
			item.damage = 100;
			item.crit = 50;
			item.melee = true;
			item.ranged = false;
			item.knockBack = 10;
			item.autoReuse = true;
			
			item.mana = 3;
			item.shoot = 10;
			item.shootSpeed = 9f;
			item.useAmmo = AmmoID.Gel;
			
			item.width = 80;
			item.height = 80;
			//item.color = Color.Red;
			item.value = 1;
			item.rare = 7;
			item.maxStack = 1;
			
			item.useTime = 13;
			item.useAnimation = 13;
			item.useStyle = 3;	// shortsword: 3
			item.useTurn = true;
			item.holdStyle = 2;
			
			if(!Main.dedServ)
				item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/Bop2").WithPitchVariance(0.3f);
			//item.UseSound = SoundID.Item1;
		}

		public override void OnHitNPC(Player player, NPC npc, int dmg, float knockback, bool crit) {
			if(crit) {
				if(player.statLife > health_penalty)
					player.statLife -= health_penalty;
				else
					player.statLife = 42;

				for(int dust_counter = 0; dust_counter < crit_dust_count; dust_counter++) {
					float rand_float1 = Main.rand.Next(crit_dust_max_speed*2 + 1) - crit_dust_max_speed, rand_float2 = Main.rand.Next(crit_dust_max_speed*2 + 1) - crit_dust_max_speed;
					Dust.NewDust(npc.position, npc.width, npc.height, 238, rand_float1, rand_float2, 120, default(Color), 2.75f);
				}

				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/Bop3").WithPitchVariance(0.25f), Main.player[item.owner].position);

				if(Main.rand.NextBool(10)) {
					Main.NewText($"<SasDaCood> {Main.rand.Next(sas_crit_msgs)}");
				}

			}
		}

		public override void MeleeEffects(Player player, Rectangle hittoboksu) {
			Vector2 zone = new Vector2(hittoboksu.X, hittoboksu.Y);

			if(Main.rand.NextBool(1)) {
				if(player.altFunctionUse == 2) {
					Dust.NewDust(zone, hittoboksu.Width, hittoboksu.Height, 31, 0f, 0f, 80, Color.Red);
					Lighting.AddLight(zone,1f,0.3f,0f);
				}
				else {
					Dust.NewDust(zone, hittoboksu.Width, hittoboksu.Height, 31);
					Lighting.AddLight(zone,1f,0.8f,0.5f);
				}
			}
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			var line = new TooltipLine(mod, "dumb", $"'im dumb' - {Main.player[item.owner].name}") {
				overrideColor = new Color(40, 130, 255)
			};
			tooltips.Add(line);

			foreach(TooltipLine tooltip_line in tooltips) {
				if(tooltip_line.mod == "Terraria") {
					switch(tooltip_line.Name) {
						case "ItemName":
							tooltip_line.overrideColor = new Color(255, 150, 40);
							break;
						case "Favorite":
							tooltip_line.overrideColor = new Color(255, 80, 80);
							break;
						case "Damage":
							tooltip_line.overrideColor = new Color(150, 0, 0);
							break;
						case "UseMana":
							tooltip_line.overrideColor = new Color(80, 80, 255);
							break;
						case "CritChance":
							tooltip_line.overrideColor = new Color(120, 80, 0);
							break;
						case "Speed":
							tooltip_line.overrideColor = new Color(200, 200, 50);
							break;
						case "Knockback":
							tooltip_line.overrideColor = new Color(40, 40, 40);
							break;
					}
				}
			}
		}

		public override bool AltFunctionUse(Player player)
			=> true;

		public override bool CanUseItem(Player player) {
			if(player.altFunctionUse == 2) {	// 2 -> RMB
				item.useStyle = 5;	// gun: 5
				item.useTime = 10;
				item.useAnimation = 10;
				
				item.damage = 20;
				item.melee = false;
				item.ranged = true;
				item.mana = 2;
				
				item.rare = 9;
				item.shoot = ProjectileType<Projectiles.Orb>();
			} else {
				item.useStyle = 3;	// shortsword: 3
				item.useTime = 13;
				item.useAnimation = 13;
				
				item.damage = 100;
				item.melee = true;
				item.ranged = false;
				item.mana = 3;
				
				item.rare = 7;
				item.shoot = ProjectileID.None;
			}
			return base.CanUseItem(player);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
			// randomise the speed first and only the speed (magnitude), not the velocity/angle
			float speed_skew = (7f + Main.rand.Next(7))/10;	// 0.7 to 1.3
			speedX *= speed_skew;
			speedY *= speed_skew;

			Vector2 rotated_vel = new Vector2(speedX, speedY);
			rotated_vel = rotated_vel.RotatedByRandom(MathHelper.ToRadians(25));
			speedX = rotated_vel.X;
			speedY = rotated_vel.Y;

			return true;
		}

		public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DirtBlock, 42);
			recipe.AddTile(412);	// Ancient manipulator
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}