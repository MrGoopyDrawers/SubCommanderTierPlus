using MelonLoader;
using BTD_Mod_Helper;
using PathsPlusPlus;
using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Api.Enums;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using JetBrains.Annotations;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppSystem.IO;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Utils;
using System.Collections.Generic;
using System.Linq;
using Il2CppAssets.Scripts.Models.TowerSets;
using BTD_Mod_Helper.Api.Towers;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Display;
using BTD_Mod_Helper.Api.Display;
using UnityEngine;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Simulation.SMath;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Models.Towers.TowerFilters;
using Il2CppAssets.Scripts.Models.Map;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using System.Runtime.CompilerServices;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

using SubCommanderTier;

[assembly: MelonInfo(typeof(SubCommanderTier.SubCommanderTier), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace SubCommanderTier;

public class SubCommanderTier : BloonsTD6Mod
{
    public override void OnApplicationStart()
    {
        ModHelper.Msg<SubCommanderTier>("SubCommanderTier loaded!");
    }
    public class RateIcon : ModBuffIcon
    {
        protected override int Order => 1;
        public override string Icon => "navyForce";
        public override int MaxStackSize => 0;
    }
    public class SubTopPath : PathPlusPlus
    {
        public override string Tower => TowerType.MonkeySub;

        public override int ExtendVanillaPath => 2;

        public override int UpgradeCount => 8; // Adding one new upgrade to bring the total upgrades up to 6
    }
    public class NavalMissles : UpgradePlusPlus<SubTopPath>
    {
        public override int Cost => 18000;
        public override int Tier => 6;
        public override string Icon => "nukeIcon";
        public override string Description => "Deals increased damage to ceramics with missles.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var missle = Game.instance.model.GetTowerFromId("BombShooter-320").GetWeapon().Duplicate();
            missle.rate = 0.05f;
            attackModel.AddWeapon(missle);
        }
    }
    public class EliteSubmarine : UpgradePlusPlus<SubTopPath>
    {
        public override int Cost => 34000;
        public override int Tier => 7;
        public override string Icon => "navyForce";
        public override string Description => "Double damage for all attacks. Offers the naval force buff which increases attack speed of most water based towers.";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            foreach (var dmgModel in towerModel.GetDescendants<DamageModel>().ToArray())
            {
                dmgModel.damage *= 2;
            }
            var buff = new RateSupportModel("rate_", 0.1f, true, "idk", true, 1, new Il2CppReferenceArray<TowerFilterModel>(new TowerFilterModel[]
                {
                    new FilterInBaseTowerIdModel("FilterInBaseTowerIdModel_",
                        new Il2CppStringArray(new[] { TowerType.MonkeySub, TowerType.MonkeyBuccaneer, TowerType.AdmiralBrickell }))
                }), null, null, false);
            buff.ApplyBuffIcon<RateIcon>();
            towerModel.AddBehavior(buff);
        }
    }
    public class navyWarHead : UpgradePlusPlus<SubTopPath>
    {
        public override int Cost => 250000;
        public override int Tier => 8;
        public override string Icon => "navyWarHead";
        public override string Description => "MANY more damage and unlocks the warhead ability which temporarily increases damage by ALOT. All buffs are even better!";

        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            foreach (var dmgModel in towerModel.GetDescendants<DamageModel>().ToArray())
            {
                dmgModel.damage *= 9;
            }
            var abilityModel = Game.instance.model.GetTowerFromId("BoomerangMonkey-040").GetAbility().Duplicate();
            abilityModel.GetBehavior<TurboModel>().extraDamage += 275;
            abilityModel.RemoveBehavior<CreateSoundOnAbilityModel>();
            abilityModel.icon = GetSpriteReference("navyWarHead");
            towerModel.AddBehavior(abilityModel);
            towerModel.GetBehavior<SubCommanderSupportModel>().pierceIncrease += 20;
            towerModel.GetBehavior<SubCommanderSupportModel>().damageScale = 20;
            towerModel.GetBehavior<RateSupportModel>().multiplier = 0.02f;
            abilityModel.GetBehavior<TurboModel>().projectileDisplay.assetPath = CreatePrefabReference<stuffff>();
        }
    }
    public class stuffff : ModDisplay
    {
        public override string BaseDisplay => Game.instance.model.GetTowerFromId("BombShooter-320").GetWeapon().projectile.display.guidRef;
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {

        }
    }
}