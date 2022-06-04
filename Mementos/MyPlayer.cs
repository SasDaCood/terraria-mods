using Microsoft.Xna.Framework;
using Mementos.Dusts;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;


namespace Mementos
{
	public class ThePlayerTM:ModPlayer
	{
		private const int dust_count = 20, dust_maxspeed_x = 4, dust_maxspeed_y = 7, no_of_sounds = 21;

		public override bool PreKill(double dmg, int hit_direction, bool pvp, ref bool play_deathsound, ref bool gore, ref PlayerDeathReason dmg_source) {
			play_deathsound = false;						// disable deathsound to add custom ones
			return true;
		}		

		public override void Kill(double dmg, int hit_direction, bool pvp, PlayerDeathReason dmg_source) {
			int randint_sound = Main.rand.Next(no_of_sounds);
			Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, $"Sounds/Deaths/{randint_sound}"));
			Main.NewText($"[c/FF0044:lmao noob] [c/BB3399:@{player.name}]");

			for(int dust_counter = 0; dust_counter < dust_count; dust_counter++) {
				float rand_speedx = Main.rand.Next(dust_maxspeed_x*2 + 1) - dust_maxspeed_x, rand_speedy = -Main.rand.Next(dust_maxspeed_y + 1);
				Dust.NewDust(player.position, player.width, player.height, (player.name == "Sas" ? DustType<ShadowflameRed>() : DustType<ShadowflameBlue>()), rand_speedx, rand_speedy, 140, default(Color), 3.75f);
			}
		}
		
	}
}