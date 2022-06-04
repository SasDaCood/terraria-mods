using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Mementos
{
	public class Mementos : Mod
	{
		public override void Load() {
			if(!Main.dedServ){
				AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/TokyoSkies"), ItemType("TokyoSkiesMB"), TileType("TokyoSkiesMBTile"));
			}
		}

		public Mementos()
		{
			Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
		}
	}
}