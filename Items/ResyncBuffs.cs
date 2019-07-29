using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AutoBuff.Items
{
    /*
	public class ResyncBuffs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("ResyncBuffs");
			Tooltip.SetDefault("This is a modded sword.");
		}
		public override void SetDefaults()
		{
			item.damage = 50;
			item.melee = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 1;
			item.useAnimation = 1;
			item.useStyle = 1;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DirtBlock, 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}


        public override bool UseItem(Player player)
        {
            
            var mp = player.GetModPlayer<AutoBuffPlayer>();
            
            if (mp.buffsavail[15])
            {
                mp.boughtbuffsavail[15] = false;
                mp.buffsavail[15] = false;
            }
            else
            {
                mp.boughtbuffsavail[15] = true;
                mp.buffsavail[15] = true;
            }

            return base.UseItem(player);
        }
    }*/
}
