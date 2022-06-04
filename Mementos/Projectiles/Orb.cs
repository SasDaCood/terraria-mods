// projectile with some weird animation; used with SwordOfDivineWisdom
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;


namespace Mementos.Projectiles
{
	public class Orb:ModProjectile
	{
		public static int gravity_index = 0;
		public static int[] hit_debuffs = new int[] {BuffID.Daybreak, BuffID.BoneJavelin, BuffID.StardustMinionBleed, 30, 20, 24, 70, 39, 144, 67, 44, 153, 186, 204};
		private const float gravity_acc = 0.2f;
		private const int hop_interval = 90;	// 1.5 seconds
		private const int max_soundDelay = 18;	// Bop.mp3 is 15.3 ticks
		private const float MAX_SPEED = 12f, AIR_RESISTANCE = 0.996f;
		private int grav_index;

		public override void SetStaticDefaults() {
			DisplayName.SetDefault("[c/FF0000:le orb]");
		}

		public override void SetDefaults() {
			projectile.width = 16;
			projectile.height = 16;
			projectile.alpha = 255;	// initialised as invisible so that it doesn't overlap with the sword, which'd be weird

			projectile.friendly = true;
			projectile.hostile = true;
			projectile.ranged = true;
			projectile.penetrate = 4;

			projectile.timeLeft = 900;	// 15 seconds
			projectile.soundDelay = max_soundDelay;

			gravity_index = (gravity_index+1) % 4;	// 0 to 3 rotate CCW from east
			grav_index = gravity_index;
		}

		public override void OnHitNPC(NPC npc, int dmg, float knockback, bool crit) {
			ApplyDebuff(npc, null);
		}
		public override void OnHitPlayer(Player player, int dmg, bool crit) {
			if(player != Main.player[projectile.owner])
				ApplyDebuff(null, player);
			else
				player.statLife = player.statLifeMax2;
		}
		public override void OnHitPvp(Player player, int dmg, bool crit) {
			ApplyDebuff(null, player);
		}

		public override void AI() {
			projectile.ai[0]++;	// used for determining the projectile's "hops"

			switch(grav_index) {
				case 0:	// right -> X++
					projectile.velocity.X = GravityUpdate(projectile.velocity.X, gravity_acc);
					projectile.velocity.Y *= AIR_RESISTANCE;
					break;
				case 1:	// up -> Y--
					projectile.velocity.Y = GravityUpdate(projectile.velocity.Y, -gravity_acc);
					projectile.velocity.X *= AIR_RESISTANCE;
					break;
				case 2:	// left -> X--
					projectile.velocity.X = GravityUpdate(projectile.velocity.X, -gravity_acc);
					projectile.velocity.Y *= AIR_RESISTANCE;
					break;
				case 3:	// down -> Y++
					projectile.velocity.Y = GravityUpdate(projectile.velocity.Y, gravity_acc);
					projectile.velocity.X *= AIR_RESISTANCE;
					break;
			}

			if(projectile.soundDelay == 0) {
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item,"Sounds/Item/Bop"), projectile.position);
				projectile.soundDelay = max_soundDelay;
			}

			if(projectile.alpha > 0)
				projectile.alpha -= 15;

			projectile.rotation += 0.5f * (float)projectile.direction;
			if(Main.rand.NextBool(2))
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 31, projectile.velocity.X * 0.4f, projectile.velocity.Y * 0.4f, 170, Color.Gray, 1.5f);
			Lighting.AddLight(projectile.Center, 0.8f, 0.5f, 0.2f);
		}

		public override bool OnTileCollide(Vector2 old_velocity) {
			projectile.velocity.X = old_velocity.X;
			projectile.velocity.Y = old_velocity.Y;
			
			int dust_i = Dust.NewDust(projectile.position, projectile.width, projectile.height, 152, 0f, 0f, 0, default(Color), 2f);
			Main.dust[dust_i].noGravity = true;
			Lighting.AddLight(Main.dust[dust_i].position, 1f, 0.7f, 0.2f);

			return false;
		}


		public void ApplyDebuff(NPC npc_target, Player player_target) {
			foreach(int ID in hit_debuffs) {
				if(!Main.rand.NextBool(10)) {	// !1 in 10 = 9 in 10 chance to inflict 1 debuff = ~25% chance to inflict all debuffs
					if(npc_target == null)
						player_target.AddBuff(ID,480);
					else
						npc_target.AddBuff(ID,480);
				}
			}
		}

		public float GravityUpdate(float gravity_vel, float update_by) {
			gravity_vel += update_by;

			if(gravity_vel > MAX_SPEED)
				gravity_vel = MAX_SPEED;
			else if(gravity_vel < -MAX_SPEED)
				gravity_vel = -MAX_SPEED;

			if(projectile.penetrate > 1 && projectile.ai[0] >= hop_interval) {
				gravity_vel *= -1;	// reverse the velocity to simulate a perfect bounce. Will bounce wrongly on wrong velocity but ehh
				projectile.ai[0] = 0f;	// reset hop timer to 0
				projectile.penetrate--;
				projectile.alpha = 225;
			}

			return gravity_vel;
		}
	}
}