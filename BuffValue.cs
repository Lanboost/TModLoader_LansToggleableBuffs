using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;

namespace LansToggleableBuffs
{
	public class BuffValue
    {

        public delegate void BuffFunction(Player player);

        public bool IsMainGame;
        public int id;
        public string name;
        public string mod;
        public CostValue[] cost;
        public BuffFunction func;
        public bool useMainBuff;
        public string effect;
		public Asset<Texture2D> texture;
        public bool isDebuff = false;

        public BuffValue(bool isMainGame, int id, string name, string effect, string mod, CostValue[] cost, BuffFunction func, bool useMainBuff, bool isDebuff, Asset<Texture2D> texture = null)
        {
            IsMainGame = isMainGame;
            this.id = id;
            this.name = name;
            this.mod = mod;
            this.cost = cost;
            this.func = func;
            this.useMainBuff = useMainBuff;
            this.effect = effect;
			this.texture = texture;
            this.isDebuff = isDebuff;
        }
    }
}
