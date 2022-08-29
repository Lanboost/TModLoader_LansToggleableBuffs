using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace LansToggleableBuffs.ui
{

    public enum LItemSlotType
    {
        PetAndLight,
        Pet,
        Light,
        Item
    }
    public class LItemSlot
    {
        public LItemSlotType type;
        public LItemSlot(LItemSlotType type)
        {
            this.type = type;
        }
        public Item Item = new Item();

        public void Update()
        {
            if(this.type == LItemSlotType.PetAndLight || this.type == LItemSlotType.Pet)
            {
                Utils.Swap(ref Main.LocalPlayer.miscEquips[0], ref Item);
                Main.LocalPlayer.UpdatePet(Main.myPlayer);
                Utils.Swap(ref Main.LocalPlayer.miscEquips[0], ref Item);
            }
            if (this.type == LItemSlotType.PetAndLight || this.type == LItemSlotType.Light)
            {
                Utils.Swap(ref Main.LocalPlayer.miscEquips[1], ref Item);
                Main.LocalPlayer.UpdatePetLight(Main.myPlayer);
                Utils.Swap(ref Main.LocalPlayer.miscEquips[1], ref Item);
            }
        }
    }
}
