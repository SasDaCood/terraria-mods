using Microsoft.Xna.Framework;
using Terraria;
using System;
using Terraria.ID;
using Terraria.ModLoader;

namespace Mementos.Dusts
{
	public class ShadowflameBlue:ModDust
	{
		public const float jump_threshold_vel = -3f, x_acc_scale = 1.022f, rotation_acc = 0.15f;

		public override void OnSpawn(Dust dust) {
			dust.velocity.X = Math.Abs(dust.velocity.X);
		}

		public override bool Update(Dust dust) {
			Lighting.AddLight(dust.position, 0f, 0.35f, 1f);

			dust.rotation += rotation_acc * (dust.dustIndex%2 == 0 ? 1 : -1);
			dust.velocity.X *= x_acc_scale;

			if(Main.rand.NextBool(100) && dust.velocity.Y > jump_threshold_vel) {
				dust.velocity.Y = jump_threshold_vel;
			}

			return true;
		}

		public override Color? GetAlpha(Dust dust, Color lighting)
			=> new Color(lighting.R, lighting.G, lighting.B, 25);
	}
}