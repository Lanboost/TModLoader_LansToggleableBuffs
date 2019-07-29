using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace AutoBuff
{
    public class BuffValue
    {

        public delegate void BuffFunction(Player player);

        public bool IsMainGame;
        public int id;
        public string name;
        public string mod;
        public int itemid;
        public int count;
        public BuffFunction func;
        public bool useMainBuff;

        public BuffValue(bool isMainGame, int id, string name, string mod, int itemid, int count, BuffFunction func, bool useMainBuff)
        {
            IsMainGame = isMainGame;
            this.id = id;
            this.name = name;
            this.mod = mod;
            this.itemid = itemid;
            this.count = count;
            this.func = func;
            this.useMainBuff = useMainBuff;
        }
    }

    public class AutoBuffBuffs
    {
        public static BuffValue[] buffs = new BuffValue[]
        {
            new BuffValue(true, BuffID.AmmoReservation, "AmmoReservation", null, ItemID.AmmoReservationPotion, 30, delegate(Player player) {
                player.ammoPotion = true;
            }, false),
            new BuffValue(true, BuffID.Archery, "Archery", null, ItemID.ArcheryPotion , 30, delegate(Player player) {
                player.archery = true;
            }, false),
            new BuffValue(true, BuffID.Battle, "Battle", null, ItemID.BattlePotion , 30, delegate(Player player) {
                player.enemySpawns = true;
            }, false),
            new BuffValue(true, BuffID.Builder, "Builder", null, ItemID.BuilderPotion , 30, null, true),
            new BuffValue(true, BuffID.Calm, "Calm", null, ItemID.CalmingPotion , 30, delegate(Player player) {
                player.calmed = true;
            }, false),
            new BuffValue(true, BuffID.Crate, "Crate", null, ItemID.CratePotion , 30, delegate(Player player) {
                player.cratePotion = true;
            }, false),
            new BuffValue(true, BuffID.Dangersense, "Dangersense", null, ItemID.TrapsightPotion , 30, delegate(Player player) {
                player.dangerSense = true;
            }, false),
            new BuffValue(true, BuffID.Endurance, "Endurance", null, ItemID.EndurancePotion , 30, delegate(Player player) {
                player.endurance += 0.1f;
            }, false),
            new BuffValue(true, BuffID.Featherfall, "Featherfall", null, ItemID.FeatherfallPotion , 30, delegate(Player player) {
                player.slowFall = true;
            }, false),
            new BuffValue(true, BuffID.Fishing, "Fishing", null, ItemID.FishingPotion , 30, null, true),
            new BuffValue(true, BuffID.Flipper, "Flipper", null, ItemID.FlipperPotion , 30, null, true),
            new BuffValue(true, BuffID.Gills, "Gills", null, ItemID.GillsPotion , 30, delegate(Player player) {
                player.gills = true;
            }, false),
            new BuffValue(true, BuffID.Gravitation, "Gravitation", null, ItemID.GravitationPotion , 30, delegate(Player player) {
                player.gravControl = true;
            }, false),
            new BuffValue(true, BuffID.Heartreach, "Heartreach", null, ItemID.HeartreachPotion , 30, null, true),
            new BuffValue(true, BuffID.Hunter, "Hunter", null, ItemID.HunterPotion , 30, delegate(Player player) {
                player.detectCreature = true;
            }, false),
            new BuffValue(true, BuffID.Inferno, "Inferno", null, ItemID.InfernoPotion , 30,null, true),
            new BuffValue(true, BuffID.Invisibility, "Invisibility", null, ItemID.InvisibilityPotion , 30, delegate(Player player) {
                player.invis = true;
            }, false),
            new BuffValue(true, BuffID.Ironskin, "Ironskin", null, ItemID.IronskinPotion , 30, delegate(Player player) {
                player.statDefense += 8;
            }, false),
            new BuffValue(true, BuffID.Lifeforce, "Lifeforce", null, ItemID.LifeforcePotion , 30, delegate(Player player) {
                player.lifeForce = true;
            }, false),
            new BuffValue(true, BuffID.MagicPower, "MagicPower", null, ItemID.MagicPowerPotion , 30, delegate(Player player) {
                player.magicDamage += 0.2f;
            }, false),
            new BuffValue(true, BuffID.ManaRegeneration, "ManaRegeneration", null, ItemID.ManaRegenerationPotion , 30, delegate(Player player) {
                player.manaRegenBuff = true;
            }, false),
            new BuffValue(true, BuffID.Mining, "Mining", null, ItemID.MiningPotion , 30, null, true),
            new BuffValue(true, BuffID.NightOwl, "NightOwl", null, ItemID.NightOwlPotion , 30, delegate(Player player) {
                player.nightVision = true;
            }, false),
            new BuffValue(true, BuffID.ObsidianSkin, "ObsidianSkin", null, ItemID.ObsidianSkinPotion , 30, delegate(Player player) {
                player.lavaImmune = true;
                player.fireWalk = true;
            }, false),
            new BuffValue(true, BuffID.Rage, "Rage", null, ItemID.RagePotion , 30, null, true),
            new BuffValue(true, BuffID.Regeneration, "Regeneration", null, ItemID.RegenerationPotion , 30, delegate(Player player) {
                player.lifeRegen += 2;
            }, false),
            new BuffValue(true, BuffID.Shine, "Shine", null, ItemID.ShinePotion , 30, null, true),
            new BuffValue(true, BuffID.Sonar, "Sonar", null, ItemID.SonarPotion , 30, delegate(Player player) {
                player.sonarPotion = true;
            }, false),
            new BuffValue(true, BuffID.Spelunker, "Spelunker", null, ItemID.SpelunkerPotion , 30, delegate(Player player) {
                player.findTreasure = true;
            }, false),
            new BuffValue(true, BuffID.Summoning, "Summoning", null, ItemID.SummoningPotion , 30, delegate(Player player) {
                player.maxMinions += 1;
            }, false),
            new BuffValue(true, BuffID.Swiftness, "Swiftness", null, ItemID.SwiftnessPotion , 30, delegate(Player player) {
                player.moveSpeed += 0.25f;
            }, false),
            new BuffValue(true, BuffID.Thorns, "Thorns", null, ItemID.ThornsPotion , 30, delegate(Player player) {
                player.thorns += 0.33f;
            }, false),
            new BuffValue(true, BuffID.Titan, "Titan", null, ItemID.TitanPotion , 30, null, true),
            new BuffValue(true, BuffID.Warmth, "Warmth", null, ItemID.WarmthPotion , 30, null, true),
            new BuffValue(true, BuffID.WaterWalking, "WaterWalking", null, ItemID.WaterWalkingPotion , 30, delegate(Player player) {
                player.waterWalk = true;
            }, false),
            new BuffValue(true, BuffID.Wrath, "Wrath", null, ItemID.WrathPotion , 30, null, true),
            new BuffValue(true, BuffID.WellFed, "WellFed", null, ItemID.PlatinumCoin , 1, delegate(Player player) {
                player.wellFed = true;
            }, false),


            new BuffValue(true, BuffID.WeaponImbueConfetti, "WeaponImbueConfetti", null, ItemID.FlaskofParty , 30, null, true),
            new BuffValue(true, BuffID.WeaponImbueCursedFlames, "WeaponImbueCursedFlames", null, ItemID.FlaskofCursedFlames , 30, null, true),
            new BuffValue(true, BuffID.WeaponImbueFire, "WeaponImbueFire", null, ItemID.FlaskofFire , 30, null, true),
            new BuffValue(true, BuffID.WeaponImbueGold, "WeaponImbueGold", null, ItemID.FlaskofGold , 30, null, true),
            new BuffValue(true, BuffID.WeaponImbueIchor, "WeaponImbueIchor", null, ItemID.FlaskofIchor , 30, null, true),
            new BuffValue(true, BuffID.WeaponImbueNanites, "WeaponImbueNanites", null, ItemID.FlaskofNanites , 30, null, true),
            new BuffValue(true, BuffID.WeaponImbuePoison, "WeaponImbuePoison", null, ItemID.FlaskofPoison , 30, null, true),
            new BuffValue(true, BuffID.WeaponImbueVenom, "WeaponImbueVenom", null, ItemID.FlaskofVenom , 30, null, true),


            new BuffValue(true, BuffID.AmmoBox, "AmmoBox", null, ItemID.AmmoBox , 1, null, true),
            new BuffValue(true, BuffID.Bewitched, "Bewitched", null, ItemID.BewitchingTable , 1, null, true),
            new BuffValue(true, BuffID.Clairvoyance, "Clairvoyance", null, ItemID.CrystalBall , 1, null, true),
            new BuffValue(true, BuffID.Sharpened, "Sharpened", null, ItemID.SharpeningStation , 1, null, true),
        };




    }
}
