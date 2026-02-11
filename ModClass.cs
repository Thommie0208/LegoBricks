using System.Collections.Generic;
using Modding;
using UnityEngine;
using SFCore;
using Satchel;
using JetBrains.Annotations;
using System.Threading.Tasks;

namespace Lego_Power_Bricks
{
    /*To do list:
    - Add current multiplier next to geo count
    - Geo magnet
    - Regenerate hp
    - Increase hp
    - Contact Damage 
    - Brick detector
    - Super slap (everything dies in 1 hit)
    - Soft landing

     */
    public class x2Multiplier : EasyCharm
    {
        protected override int GetCharmCost() => 0;
        protected override string GetDescription() => "Multiplies geo worth by 2";
        protected override string GetName() => "Geo x2";
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("Red_brick.png");
    }

    public class x4Multiplier : EasyCharm
    {
        protected override int GetCharmCost() => 0;
        protected override string GetDescription() => "Multiplies geo worth by 4";
        protected override string GetName() => "Geo x4";
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("Red_brick.png");
    }

    public class x6Multiplier : EasyCharm
    {
        protected override int GetCharmCost() => 0;
        protected override string GetDescription() => "Multiplies geo worth by 6";
        protected override string GetName() => "Geo x6";
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("Red_brick.png");
    }

    public class x8Multiplier : EasyCharm
    {
        protected override int GetCharmCost() => 0;
        protected override string GetDescription() => "Multiplies geo worth by 8";
        protected override string GetName() => "Geo x8";
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("Red_brick.png");
    }

    public class x10Multiplier : EasyCharm
    {
        protected override int GetCharmCost() => 0;
        protected override string GetDescription() => "Multiplies geo worth by 10";
        protected override string GetName() => "Geo x10";
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("Red_brick.png");
    }

    public class GeoMagnet : EasyCharm
    {
        protected override int GetCharmCost() => 0;
        protected override string GetDescription() => "Makes gathering swarm free";
        protected override string GetName() => "Geo Magnet";
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("Red_brick.png");
    }

    public class RegenerateHeatlh : EasyCharm
    {
        protected override int GetCharmCost() => 0;
        protected override string GetDescription() => "Regenerates health";
        protected override string GetName() => "Regenerates health";
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("Red_brick.png");
    }

    public class Settings
    {
        public Dictionary<string, EasyCharmState> Charms;
    }
    public class Lego_Power_Bricks : Mod, ILocalSettings<Settings>
    {
        internal static Lego_Power_Bricks Instance;
        internal Settings localSettings = new Settings();
        internal Dictionary<string, EasyCharm> Charms = new Dictionary<string, EasyCharm>
        {
            {"x2Multiplier", new x2Multiplier()},
            {"x4Multiplier", new x4Multiplier()},
            {"x6Multiplier", new x6Multiplier()},
            {"x8Multiplier", new x8Multiplier()},
            {"x10Multiplier", new x10Multiplier()},
            {"geoMagnet", new GeoMagnet()},
            {"regenerateHealth", new RegenerateHeatlh()}
        };


        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            On.HeroController.AddGeo += AddGeo;
            //On.GameManager.CalculateNotchesUsed += OnCalculateNotches;
            ModHooks.CharmUpdateHook += OnCharmUpdate;
            ModHooks.HeroUpdateHook += OnHeroUpdate;
        }
        public void OnHeroUpdate()
        {
            //if (PlayerData.instance.health < PlayerData.instance.maxHealth && Charms["regenerateHealth"].IsEquipped)
            //{
            //    Log("PlayerHealth is lower, run Add Health");
            //    AddHealth(); // Explicitly discard the returned Task to suppress CS4014
            //}
        }
        public void OnCharmUpdate(PlayerData data, HeroController controller)
        {
            Log($"OnCharmUpdate called with data: {data} and {controller}");
            if (Charms["geoMagnet"].IsEquipped)
            {
                data.CalculateNotchesUsed();
                data.charmCost_1 = 0;
            }
            else
            {
                data.CalculateNotchesUsed();
                data.charmCost_1 = 1;
            }
            if (!Charms["geoMagnet"].IsEquipped && data.equippedCharm_1)
            {
                data.CalculateNotchesUsed();
            }
        }
        //async Task AddHealth()
        //{
        //    if (!Charms["regenerateHealth"].IsEquipped) return;
        //    await Task.Delay(5000); // Wait for 5 seconds before regenerating health
        //    PlayerData.instance.health++;
        //}

        public void AddGeo(On.HeroController.orig_AddGeo orig, HeroController self, int amount)
        {
            if (Charms["x2Multiplier"].IsEquipped)
            {
                amount *= 2; // Double the amount of geo gained
            }
            if (Charms["x4Multiplier"].IsEquipped)
            {
                amount *= 4; // Quadruple the amount of geo gained
            }
            if (Charms["x6Multiplier"].IsEquipped)
            {
                amount *= 6; // Sextuple the amount of geo gained
            }
            if (Charms["x8Multiplier"].IsEquipped)
            {
                amount *= 8; // Octuple the amount of geo gained
            }
            if (Charms["x10Multiplier"].IsEquipped)
            {
                amount *= 10; // Decuple the amount of geo gained
            }
            // Check if the player has the x2 multiplier charm equipped
            orig(self, amount);
        }
        public void OnLoadLocal(Settings s)
        {
            localSettings = s;
            if (s.Charms != null)
            {
                foreach (var kvp in s.Charms)
                {
                    if (Charms.TryGetValue(kvp.Key, out EasyCharm m))
                    {
                        m.RestoreCharmState(kvp.Value);
                    }
                }
            }
        }
        public Settings OnSaveLocal()
        {
            localSettings.Charms = new Dictionary<string, EasyCharmState>();
            foreach (var kvp in Charms)
            {
                if (Charms.TryGetValue(kvp.Key, out EasyCharm m))
                {
                    localSettings.Charms[kvp.Key] = m.GetCharmState();
                }
            }
            return localSettings;
        }
    }

}