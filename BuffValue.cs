using Microsoft.Xna.Framework.Graphics;
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
		public Texture2D texture;

        public BuffValue(bool isMainGame, int id, string name, string effect, string mod, CostValue[] cost, BuffFunction func, bool useMainBuff, Texture2D texture = null)
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

			
        }
    }
}
