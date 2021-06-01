using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LansToggleableBuffs
{

	public class VanilaBuffs
    {


		public static BuffValue[] getVanilla()
		{
			BuffValue[] buffs = new BuffValue[]
			{
				new BuffValue(true, BuffID.AmmoReservation, "AmmoReservation", "Reduces the chance of consuming any ammunition by 20%.", null, new CostValue[] {new ItemCostValue(ItemID.AmmoReservationPotion, 30) }, delegate(Player player) {
					player.ammoPotion = true;
				}, false),
				new BuffValue(true, BuffID.Archery, "Archery", "Increases arrow damage and firing speed by 20%.", null, new CostValue[] {new ItemCostValue(ItemID.ArcheryPotion , 30) }, delegate(Player player) {
					player.archery = true;
				}, false),
				new BuffValue(true, BuffID.Battle, "Battle", " 	Increases the spawn rate of enemies and critters by 50%.", null, new CostValue[] {new ItemCostValue(ItemID.BattlePotion , 30) }, delegate(Player player) {
					player.enemySpawns = true;
				}, false),
				new BuffValue(true, BuffID.Builder, "Builder", "Increases placement speed by 25% and extends placement range by one tile. ", null, new CostValue[] {new ItemCostValue(ItemID.BuilderPotion , 30) }, null, true),
				new BuffValue(true, BuffID.Calm, "Calm", "Reduces enemy spawn rate by 17%. ", null, new CostValue[] {new ItemCostValue(ItemID.CalmingPotion , 30) }, delegate(Player player) {
					player.calmed = true;
				}, false),
				new BuffValue(true, BuffID.Crate, "Crate", "Doubles the chance of catching a Crate. ", null, new CostValue[] {new ItemCostValue(ItemID.CratePotion , 30) }, delegate(Player player) {
					player.cratePotion = true;
				}, false),
				new BuffValue(true, BuffID.Dangersense, "Dangersense", "Highlights hazardous blocks and objects like traps. ", null, new CostValue[] {new ItemCostValue(ItemID.TrapsightPotion , 30) }, delegate(Player player) {
					player.dangerSense = true;
				}, false),
				new BuffValue(true, BuffID.Endurance, "Endurance", "Reduces all damage taken by 10%. ", null, new CostValue[] {new ItemCostValue(ItemID.EndurancePotion , 30) }, delegate(Player player) {
					player.endurance += 0.1f;
				}, false),
				new BuffValue(true, BuffID.Featherfall, "Featherfall", "Grants control over the player's falling speed and negate fall damage. ", null, new CostValue[] {new ItemCostValue(ItemID.FeatherfallPotion , 30) }, delegate(Player player) {
					player.slowFall = true;
				}, false),
				new BuffValue(true, BuffID.Fishing, "Fishing", "Increases Fishing Power. ", null, new CostValue[] {new ItemCostValue(ItemID.FishingPotion , 30) }, null, true),
				new BuffValue(true, BuffID.Flipper, "Flipper", "Liquids do not impede movement speed. ", null, new CostValue[] {new ItemCostValue(ItemID.FlipperPotion , 30) }, null, true),
				new BuffValue(true, BuffID.Gills, "Gills", "Allows the player to breathe underwater, preventing drowning. ", null, new CostValue[] {new ItemCostValue(ItemID.GillsPotion , 30) }, delegate(Player player) {
					player.gills = true;
				}, false),
				new BuffValue(true, BuffID.Gravitation, "Gravitation", "Allows the player to invert gravity with Key UP", null, new CostValue[] {new ItemCostValue(ItemID.GravitationPotion , 30) }, delegate(Player player) {
					player.gravControl = true;
				}, false),
				new BuffValue(true, BuffID.Heartreach, "Heartreach", " 	Increases heart pickup range. ", null, new CostValue[] {new ItemCostValue(ItemID.HeartreachPotion , 30) }, null, true),
				new BuffValue(true, BuffID.Hunter, "Hunter", "Highlights all enemies on-screen. ", null, new CostValue[] {new ItemCostValue(ItemID.HunterPotion , 30) }, delegate(Player player) {
					player.detectCreature = true;
				}, false),
				new BuffValue(true, BuffID.Inferno, "Inferno", "Casts a damaging ring of fire around the playe", null, new CostValue[] {new ItemCostValue(ItemID.InfernoPotion , 30) },null, true),
				new BuffValue(true, BuffID.Invisibility, "Invisibility", "Turns the player invisible. ", null, new CostValue[] {new ItemCostValue(ItemID.InvisibilityPotion , 30) }, delegate(Player player) {
					player.invis = true;
				}, false),
				new BuffValue(true, BuffID.Ironskin, "Ironskin", "Increases defense by 8. ", null, new CostValue[] {new ItemCostValue(ItemID.IronskinPotion , 30) }, delegate(Player player) {
					player.statDefense += 8;
				}, false),
				new BuffValue(true, BuffID.Lifeforce, "Lifeforce", "Increases max health by 20%. ", null, new CostValue[] {new ItemCostValue(ItemID.LifeforcePotion , 30) }, null, true),
				new BuffValue(true, BuffID.MagicPower, "MagicPower", "Increases Magic damage by 20%. ", null, new CostValue[] {new ItemCostValue(ItemID.MagicPowerPotion , 30) }, delegate(Player player) {
					player.magicDamage += 0.2f;
				}, false),
				new BuffValue(true, BuffID.ManaRegeneration, "ManaRegeneration", "Increases Mana regeneration. ", null, new CostValue[] {new ItemCostValue(ItemID.ManaRegenerationPotion , 30) }, delegate(Player player) {
					player.manaRegenBuff = true;
				}, false),
				new BuffValue(true, BuffID.Mining, "Mining", "Increases mining speed. ", null, new CostValue[] {new ItemCostValue(ItemID.MiningPotion , 30) }, null, true),
				new BuffValue(true, BuffID.NightOwl, "NightOwl", "Improves the player's night vision and increases the radius of any available light source. ", null, new CostValue[] {new ItemCostValue(ItemID.NightOwlPotion , 30) }, delegate(Player player) {
					player.nightVision = true;
				}, false),
				new BuffValue(true, BuffID.ObsidianSkin, "ObsidianSkin", "Grants invulnerability to damage from lava. ", null, new CostValue[] {new ItemCostValue(ItemID.ObsidianSkinPotion , 30) }, delegate(Player player) {
					player.lavaImmune = true;
					player.fireWalk = true;
				}, false),
				new BuffValue(true, BuffID.Rage, "Rage", "Increases critical strike chance by 10%. ", null, new CostValue[] {new ItemCostValue(ItemID.RagePotion , 30) }, null, true),
				new BuffValue(true, BuffID.Regeneration, "Regeneration", "Increases life regeneration by 2 health per second. ", null, new CostValue[] {new ItemCostValue(ItemID.RegenerationPotion , 30) }, delegate(Player player) {
					player.lifeRegen += 2;
				}, false),
				new BuffValue(true, BuffID.Shine, "Shine", "Causes the player to glow brightly. ", null, new CostValue[] {new ItemCostValue(ItemID.ShinePotion , 30) }, null, true),
				new BuffValue(true, BuffID.Sonar, "Sonar", "Reveals the name of the catch while fishing ", null, new CostValue[] {new ItemCostValue(ItemID.SonarPotion , 30) }, delegate(Player player) {
					player.sonarPotion = true;
				}, false),
				new BuffValue(true, BuffID.Spelunker, "Spelunker", "Highlights ore, chests, and other treasure. ", null, new CostValue[] {new ItemCostValue(ItemID.SpelunkerPotion , 30) }, delegate(Player player) {
					player.findTreasure = true;
				}, false),
				new BuffValue(true, BuffID.Summoning, "Summoning", "Increases number of maximum minions by +1. ", null, new CostValue[] {new ItemCostValue(ItemID.SummoningPotion , 30) }, delegate(Player player) {
					player.maxMinions += 1;
				}, false),
				new BuffValue(true, BuffID.Swiftness, "Swiftness", "Increases movement speed by 25%. ", null, new CostValue[] {new ItemCostValue(ItemID.SwiftnessPotion , 30) }, delegate(Player player) {
					player.moveSpeed *= 1.25f;
				}, false),
				new BuffValue(true, BuffID.Thorns, "Thorns", "Melee attackers take a small amount of damage upon harming the player. ", null, new CostValue[] {new ItemCostValue(ItemID.ThornsPotion , 30) }, delegate(Player player) {
					player.thorns += 0.33f;
				}, false),
				new BuffValue(true, BuffID.Titan, "Titan", "Increases knockback of all weapons. ", null, new CostValue[] {new ItemCostValue(ItemID.TitanPotion , 30) }, null, true),
				new BuffValue(true, BuffID.Warmth, "Warmth", "Reduces damage from cold-themed enemies. ", null, new CostValue[] {new ItemCostValue(ItemID.WarmthPotion , 30) }, null, true),
				new BuffValue(true, BuffID.WaterWalking, "WaterWalking", "Allows the player to walk on liquids. ▼ Down key allows the player to fall in liquids.", null, new CostValue[] {new ItemCostValue(ItemID.WaterWalkingPotion , 30) }, delegate(Player player) {
					player.waterWalk = true;
				}, false),
				new BuffValue(true, BuffID.Wrath, "Wrath", "Increases damage dealt with weapons by 10%.", null, new CostValue[] {new ItemCostValue(ItemID.WrathPotion , 30) }, null, true),
				new BuffValue(true, BuffID.WellFed, "WellFed", "Grants several minor improvements of base stats", null, new CostValue[] {new MoneyCostValue(50*100*100) }, delegate(Player player) {
					player.wellFed = true;
				}, false),
				new BuffValue(true, BuffID.Tipsy, "Tipsy", "Inflicts -4 defense, grants +2% critical hit on melee attacks, +10% melee attack speed and damage.", null, new CostValue[] {new MoneyCostValue(50*100*100) }, delegate(Player player) {
					player.statDefense -= 4;
					player.meleeCrit += 2;
					player.meleeSpeed += 0.1f;
				}, false),


				new BuffValue(true, BuffID.WeaponImbueConfetti, "WeaponImbueConfetti", "Melee attacks cause bursts of confetti. ", null, new CostValue[] {new ItemCostValue(ItemID.FlaskofParty , 30) }, null, true),
				new BuffValue(true, BuffID.WeaponImbueCursedFlames, "WeaponImbueCursedFlames", "Melee attacks inflict Cursed Inferno debuff on enemies. ", null, new CostValue[] {new ItemCostValue(ItemID.FlaskofCursedFlames , 30) }, null, true),
				new BuffValue(true, BuffID.WeaponImbueFire, "WeaponImbueFire", "Melee attacks inflict On Fire! debuff on enemies. ", null, new CostValue[] {new ItemCostValue(ItemID.FlaskofFire , 30) }, null, true),
				new BuffValue(true, BuffID.WeaponImbueGold, "WeaponImbueGold", "Melee attacks inflict Midas debuff on enemies. ", null, new CostValue[] {new ItemCostValue(ItemID.FlaskofGold , 30) }, null, true),
				new BuffValue(true, BuffID.WeaponImbueIchor, "WeaponImbueIchor", "Melee attacks inflict Ichor debuff on enemies.", null, new CostValue[] {new ItemCostValue(ItemID.FlaskofIchor , 30) }, null, true),
				new BuffValue(true, BuffID.WeaponImbueNanites, "WeaponImbueNanites", "Melee attacks inflict Confused on enemies. ", null, new CostValue[] {new ItemCostValue(ItemID.FlaskofNanites , 30) }, null, true),
				new BuffValue(true, BuffID.WeaponImbuePoison, "WeaponImbuePoison", "Melee attacks inflict Poisoned on enemies. ", null, new CostValue[] {new ItemCostValue(ItemID.FlaskofPoison , 30) }, null, true),
				new BuffValue(true, BuffID.WeaponImbueVenom, "WeaponImbueVenom", "Melee attacks inflict Venom debuff on enemies. ", null, new CostValue[] {new ItemCostValue(ItemID.FlaskofVenom , 30) }, null, true),


				new BuffValue(true, BuffID.AmmoBox, "AmmoBox", "Grants a 20% chance not to consume ammo. ", null, new CostValue[] {new ItemCostValue(ItemID.AmmoBox , 1) }, delegate(Player player) {
					player.ammoBox = true;
				}, false),
				new BuffValue(true, BuffID.Bewitched, "Bewitched", "Increases number of maximum minions by +1. ", null, new CostValue[] {new ItemCostValue(ItemID.BewitchingTable , 1) }, delegate(Player player) {
					player.maxMinions += 1;
				}, false),
				new BuffValue(true, BuffID.Clairvoyance, "Clairvoyance", "Grants the following magic boosts: +20 maximum Mana, +5% Magic damage, +2% Magic critical strike chance, -2% Mana usage. ", null, new CostValue[] {new ItemCostValue(ItemID.CrystalBall , 1) }, delegate(Player player) {
					player.statManaMax2 += 20;
					player.magicDamageMult += 0.05f;
					player.magicCrit += 2;
					player.manaCost -= 0.02f;
				}, false),
				new BuffValue(true, BuffID.Sharpened, "Sharpened", "Increases melee weapons armor penetration by 4. ", null, new CostValue[] {new ItemCostValue(ItemID.SharpeningStation , 1) }, delegate(Player player) {
					player.armorPenetration += 4;
				}, false),
				new BuffValue(true, BuffID.SugarRush, "Suger Rush", "Increases the player's movement and mining speed by 20%.", null, new CostValue[] {new ItemCostValue(ItemID.SliceOfCake, 10) }, null, true),
				new BuffValue(true, BuffID.Campfire, "Campfire", "Increases life regeneration by 0.5 HP/s, and multiplies current healing rate by 1.1.", null, new CostValue[] {new ItemCostValue(ItemID.Campfire, 10) }, null, true),
				new BuffValue(true, BuffID.Sunflower, "Happy!", "Increases movement speed by 10% and reduces enemy spawns by 17%.", null, new CostValue[] {new ItemCostValue(ItemID.Sunflower, 10) }, null, true),
				new BuffValue(true, BuffID.HeartLamp, "Heart Lantern", "Increases life regeneration by 1 HP/s. ", null, new CostValue[] {new ItemCostValue(ItemID.HeartLantern, 4) }, null, true),
				new BuffValue(true, BuffID.Honey, "Honey", "Increases life regeneration by 1 HP/s and multiplies natural regeneration by 3.", null, new CostValue[] {new ItemCostValue(ItemID.HoneyBucket, 4) }, null, true),
				new BuffValue(true, BuffID.PeaceCandle, "Peace Candle", "Decreases enemy spawn rate by 23%, and maximum enemies on-screen by 30%.", null, new CostValue[] {new ItemCostValue(ItemID.PeaceCandle, 4) }, null, true),
				new BuffValue(true, BuffID.StarInBottle, "Star in a Bottle", "Increases mana regeneration by about 2 MP/s.", null, new CostValue[] {new ItemCostValue(ItemID.StarinaBottle, 10) }, null, true)
			};

			return buffs;
		}
	}
}
