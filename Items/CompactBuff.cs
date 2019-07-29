using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace AutoBuff.Items
{
    public class CompactBuff : ModBuff
    {
        public static int AmmoReservation = 0;
        public static int Archery = 1;
        public static int Battle = 2;
        public static int Builder = 3;
        public static int Calm = 4;
        public static int Crate = 5;
        public static int Dangersense = 6;
        public static int Endurance = 7;
        public static int Featherfall = 8;
        public static int Fishing = 9;
        public static int Flipper = 10;
        public static int Gills = 11;
        public static int Gravitation = 12;
        public static int Heartreach = 13;
        public static int Hunter = 14;
        public static int Inferno  = 15;
        public static int Invisibility = 16;
        public static int Ironskin = 17;
        public static int Lifeforce = 18;
        public static int MagicPower = 19;
        public static int ManaRegeneration = 20;
        public static int Mining = 21;
        public static int NightOwl = 22;
        public static int ObsidianSkin = 23;
        public static int Rage = 24;
        public static int Regeneration = 25;
        public static int Shine = 26;
        public static int Sonar = 27;
        public static int Spelunker = 28;
        public static int Summoning = 29;
        public static int Swiftness = 30;
        public static int Thorns = 31;
        public static int Titan = 32;
        public static int Warmth = 33;
        public static int WaterWalking = 34;
        public static int Wrath = 35;
        public static int WellFed = 36;

        
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Compact Buff");

            Main.debuff[Type] = false;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            canBeCleared = false;
        }



        public override void Update(Player player, ref int buffIndex)
        {
            var mp = player.GetModPlayer<AutoBuffPlayer>();


            for (int i = 0; i < AutoBuffBuffs.buffs.Length; i++) {
                if(mp.boughtbuffsavail[i] && mp.buffsavail[i])
                {
                    if (!AutoBuffBuffs.buffs[i].useMainBuff) {
                        AutoBuffBuffs.buffs[i].func(player);
                    }
                }
            }




            /*var buffsavail = player.GetModPlayer< AutoBuffPlayer>().buffsavail;

            if (buffsavail[AmmoReservation])
            {
                player.ammoPotion = true;
            }

            if (buffsavail[Archery])
            {
                player.archery = true;
            }

            if (buffsavail[Battle])
            {
                player.enemySpawns = true;

            }

            if (buffsavail[Builder])
            {
                //TODO ADD REACh

            }

            if (buffsavail[Calm])
            {
                player.calmed = true;

            }
            if (buffsavail[Crate])
            {
                player.cratePotion = true;

            }
            if (buffsavail[Dangersense])
            {
                player.dangerSense = true;

            }
            if (buffsavail[Endurance])
            {
                player.endurance += 0.1f;

            }
            if (buffsavail[Featherfall])
            {
                player.slowFall = true;

            }
            if (buffsavail[Fishing])
            {
                //TODO

            }
            if (buffsavail[Flipper])
            {
                //TODO

            }
            if (buffsavail[Gills])
            {
                player.gills = true;

            }
            if (buffsavail[Gravitation])
            {
                player.gravControl = true;

            }
            if (buffsavail[Heartreach])
            {
                //TODO

            }
            if (buffsavail[Hunter])
            {
                player.detectCreature = true;

            }
            if (buffsavail[Inferno])
            {
                //TODO
                //player.inferno = true;
                //player.infernoCounter = 10;

            }
            if (buffsavail[Invisibility])
            {
                player.invis = true;
            }
            if (buffsavail[Ironskin])
            {
                player.statDefense += 8;
            }
            if (buffsavail[Lifeforce])
            {
                player.lifeForce = true;
            }
            if (buffsavail[MagicPower])
            {
                player.magicDamage += 0.2f;
            }
            if (buffsavail[ManaRegeneration])
            {
                player.manaRegenBuff = true;
            }
            if (buffsavail[Mining])
            {
                //TODO
            }
            if (buffsavail[NightOwl])
            {
                player.nightVision = true;
            }
            if (buffsavail[ObsidianSkin])
            {
                player.lavaImmune = true;
                player.fireWalk = true;
            }
            if (buffsavail[Rage])
            {
                //TODO
            }
            if (buffsavail[Regeneration])
            {
                player.lifeRegen += 2;
            }
            if (buffsavail[Shine])
            {
                //TODO
            }
            if (buffsavail[Sonar])
            {
                player.sonarPotion = true;
            }
            if (buffsavail[Spelunker])
            {
                player.findTreasure = true;
            }
            if (buffsavail[Summoning])
            {
                player.maxMinions += 1;
            }
            if (buffsavail[Swiftness])
            {
                player.moveSpeed += 0.25f;
            }
            if (buffsavail[Thorns])
            {
                player.thorns += 0.3f;
            }
            if (buffsavail[Titan])
            {
                //TODO
            }
            if (buffsavail[Warmth])
            {
                //TODO
            }
            if (buffsavail[WaterWalking])
            {
                player.waterWalk = true;
            }
            if (buffsavail[Wrath])
            {
                //TODO
            }
            if (buffsavail[WellFed])
            {
                player.wellFed = true;
            }*/
        }
    }
}
