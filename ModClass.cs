using Modding;
using Satchel;
using SFCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lego_Power_Bricks
{
    /*To do list:
    - Add current multiplier next to geo count
    - Geo magnet
    - Contact Damage 
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

    public class IncreaseHealth : EasyCharm
    {
        protected override int GetCharmCost() => 0;
        protected override string GetDescription() => "Increases max health";
        protected override string GetName() => "Increase health";
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("Red_brick.png");
    }

    public class Settings
    {
        public Dictionary<string, EasyCharmState> Charms;
    }
    public class Lego_Power_Bricks : Mod, ILocalSettings<Settings>
    {
        public Lego_Power_Bricks() : base("Lego Power Bricks") { }
        public override string GetVersion() => "0.1";
        private bool healing = false;
        private bool healthIncreased = false;
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
            {"regenerateHealth", new RegenerateHeatlh()},
            {"increaseHealth", new IncreaseHealth()}
        };


        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            On.HeroController.AddGeo += AddGeo;
            ModHooks.CharmUpdateHook += OnCharmUpdate;
            ModHooks.HeroUpdateHook += OnHeroUpdate;
        }
        public void OnHeroUpdate()
        {
            if (PlayerData.instance.health < PlayerData.instance.maxHealth
                && Charms["regenerateHealth"].IsEquipped && !healing)
            {
                GameManager.instance.StartCoroutine(RegenerateHealth());
            }
        }

        public void OnCharmUpdate(PlayerData data, HeroController controller)
        {
            Log($"OnCharmUpdate called");
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
            if (Charms["increaseHealth"].IsEquipped && !healthIncreased)
            {
                healthIncreased = true;
                HeroController.instance.AddToMaxHealth(2);
            }
            else if (healthIncreased)
            {
                healthIncreased = false;
                HeroController.instance.AddToMaxHealth(-2);
            }
        }
        private IEnumerator RegenerateHealth()
        {
            healing = true;
            yield return new WaitForSeconds(5f);
            HeroController.instance.AddHealth(1);
            healing = false;
        }


        public void AddGeo(On.HeroController.orig_AddGeo orig, HeroController self, int amount)
        {
            int multiplier = 1;
            if (Charms["x2Multiplier"].IsEquipped)
            {
                multiplier *= 2; // Double the amount of geo gained
            }
            if (Charms["x4Multiplier"].IsEquipped)
            {
                multiplier *= 4; // Quadruple the amount of geo gained
            }
            if (Charms["x6Multiplier"].IsEquipped)
            {
                multiplier *= 6; // Sextuple the amount of geo gained
            }
            if (Charms["x8Multiplier"].IsEquipped)
            {
                multiplier *= 8; // Octuple the amount of geo gained
            }
            if (Charms["x10Multiplier"].IsEquipped)
            {
                multiplier *= 10; // Decuple the amount of geo gained
            }
            // Check if the player has the x2 multiplier charm equipped
            orig(self, amount * multiplier);
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