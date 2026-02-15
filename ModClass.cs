using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using MagicUI.Core;
using MagicUI.Elements;
using MagicUI.Graphics;
using Modding;
using Satchel;
using SFCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Lego_Power_Bricks
{
    /*To do list:
     * Add thorns brick?
     * Start working on placing them in the world with SFCore somehow
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

    public class SuperSlap : EasyCharm
    {
        protected override int GetCharmCost() => 0;
        protected override string GetDescription() => "Significantly increases nail damage";
        protected override string GetName() => "Super Slap";
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("Red_brick.png");
    }
    public class SoftFall : EasyCharm
    {
        protected override int GetCharmCost() => 0;
        protected override string GetDescription() => "Removes hard falls";
        protected override string GetName() => "Soft Fall";
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("Red_brick.png");
    }
    public class InfiniteBlast : EasyCharm
    {
        protected override int GetCharmCost() => 0;
        protected override string GetDescription() => "So anyway, I started blasting.\n\n Makes Vengeful Spirit/Shade Soul cheaper to use";
        protected override string GetName() => "Infinite blast";
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
        private bool nailDamageIncreased = false;
        private bool hardFallTimeIncreased = false;
        private int geoMultiplier;
        private float vanillaHardFallTime = 1.1f;
        private LayoutRoot? layout;
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
            {"increaseHealth", new IncreaseHealth()},
            {"superSlap", new SuperSlap()},
            {"softFall", new SoftFall()},
            {"infiniteBlast", new InfiniteBlast()}
        };


        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            On.HeroController.Awake += OnAwake;
            On.HeroController.AddGeo += AddGeo;
            ModHooks.CharmUpdateHook += OnCharmUpdate;
            ModHooks.HeroUpdateHook += OnHeroUpdate;
            On.GameCameras.Start += AddMasks;
        }

        public void OnHeroUpdate()
        {
            if (PlayerData.instance.health < PlayerData.instance.maxHealth
                && Charms["regenerateHealth"].IsEquipped && !healing)
            {
                Log("Starting health regeneration");
                GameManager.instance.StartCoroutine(RegenerateHealth());
            }
            if (Charms["softFall"].IsEquipped && HeroController.instance.fallTimer > HeroController.instance.BIG_FALL_TIME)
            {
                HeroController.instance.BIG_FALL_TIME *= 2;
            }
            if (layout != null && GameManager.instance.inventoryFSM.ActiveStateName == "Opened" || GameManager.instance.gameState.ToString() != "PLAYING")
            {
                DestroyUI();
            }
            if (layout == null && GameManager.instance.inventoryFSM.ActiveStateName == "Closed" && GameManager.instance.gameState.ToString() == "PLAYING")
            {
                CalculateMultiplier();
            }
        }

        public void OnAwake(On.HeroController.orig_Awake orig, HeroController self)
        {
            healthIncreased = Charms["increaseHealth"].IsEquipped;
            nailDamageIncreased = Charms["superSlap"].IsEquipped;
            hardFallTimeIncreased = Charms["softFall"].IsEquipped;
            orig(self);
        }
        public void OnCharmUpdate(PlayerData data, HeroController hc)
        {
            Log($"OnCharmUpdate called");
            if (Charms["geoMagnet"].IsEquipped)
            {
                data.charmCost_1 = 0;
                data.CalculateNotchesUsed();
            }
            else
            {
                data.charmCost_1 = 1;
                data.CalculateNotchesUsed();
            }
            if (!Charms["geoMagnet"].IsEquipped && data.equippedCharm_1)
            {
                data.CalculateNotchesUsed();
            }
            if (Charms["increaseHealth"].IsEquipped && !healthIncreased)
            {
                healthIncreased = true;
                hc.AddToMaxHealth(2);
            }
            else if (!Charms["increaseHealth"].IsEquipped && healthIncreased)
            {
                healthIncreased = false;
                hc.AddToMaxHealth(-2);
            }
            if (Charms["x2Multiplier"].IsEquipped || Charms["x4Multiplier"].IsEquipped || Charms["x6Multiplier"].IsEquipped || Charms["x8Multiplier"].IsEquipped || Charms["x10Multiplier"].IsEquipped)
            {
                geoMultiplier = CalculateMultiplier();
            }
            if (Charms["superSlap"].IsEquipped && !nailDamageIncreased)
            {
                data.nailDamage *= 2;
                nailDamageIncreased = true;
            }
            else if (!Charms["superSlap"].IsEquipped && nailDamageIncreased)
            {
                data.nailDamage /= 2;
                nailDamageIncreased = false;
            }
            if (Charms["softFall"].IsEquipped && !hardFallTimeIncreased)
            {
                hc.BIG_FALL_TIME = 999f;
                hardFallTimeIncreased = true;
            }
            else if (hardFallTimeIncreased)
            {
                hc.BIG_FALL_TIME = vanillaHardFallTime;
                hardFallTimeIncreased = false;
            }
            if (Charms["infiniteBlast"].IsEquipped)
            {
                ModifyVengefulSpirit(hc);
            }
            else
            {
                UnModifyVengefulSpirit(hc);
            }

        }
        private IEnumerator RegenerateHealth()
        {
            Log("Started healing");
            healing = true;
            yield return new WaitForSeconds(2f);
            HeroController.instance.AddHealth(1);
            Log("Finished healing");
            healing = false;
        }

        private void ModifyVengefulSpirit(HeroController self)
        {
            PlayMakerFSM fsm = self.gameObject.LocateMyFSM("Spell Control");
            if (fsm == null) return;
            int newCost = 5;
            fsm.GetAction<SendMessage>("Fireball 2", 2).functionCall.IntParameter = newCost;
            fsm.GetAction<SendMessage>("Fireball 1", 2).functionCall.IntParameter = newCost;
        }

        private void UnModifyVengefulSpirit(HeroController self)
        {
            PlayMakerFSM fsm = self.gameObject.LocateMyFSM("Spell Control");
            if (fsm == null) return;
            int vanillaCost = (PlayerData.instance.equippedCharm_33) ? 24 : 33;
            fsm.GetAction<SendMessage>("Fireball 2", 2).functionCall.IntParameter = vanillaCost;
            fsm.GetAction<SendMessage>("Fireball 1", 2).functionCall.IntParameter = vanillaCost;
        }

        private void AddMasks(On.GameCameras.orig_Start orig, GameCameras self)
        {
            orig(self);
            Log("Adding masks");
            MasksOverflow(self);
        }

        private void MasksOverflow(GameCameras self)
        {
            GameObject mask = self.gameObject.Find("HudCamera").Find("Hud Canvas").Find("Health").Find("Health 1");
            for (int i = 12; i <= 13; i++)
            {
                if (mask.transform.parent.gameObject.Find($"Health {i}") == null)
                {
                    Log("Adding mask " + i);
                    GameObject newMask = Object.Instantiate(mask, mask.transform.parent);
                    newMask.name = $"Health {i}";
                    newMask.SetActive(true);

                    PlayMakerFSM healthFsm = newMask.LocateMyFSM("health_display");
                    FsmVariables healthFsmVars = healthFsm.FsmVariables;
                    healthFsmVars.GetFsmInt("Health Number").Value = i;

                    float xPos = -10.32f + (0.94f * i - 1);
                    float yPos = 7.7f;
                    newMask.transform.localPosition = new Vector3(xPos, yPos, -2);
                }
            }
        }

        public void AddGeo(On.HeroController.orig_AddGeo orig, HeroController self, int amount)
        {
            geoMultiplier = CalculateMultiplier();
            orig(self, amount * geoMultiplier);
        }

        private int CalculateMultiplier()
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
            UpdateMultiplierText(multiplier);
            return multiplier;
        }

        private void UpdateMultiplierText(int newMultiplier)
        {
            DestroyUI();
            if (newMultiplier == 1) return;
            if (layout == null)
            {
                layout = new(true, "Persistent layout");
                layout.RenderDebugLayoutBounds = false;
                SimpleLayout.Setup(layout, newMultiplier);
            }
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

        private void DestroyUI()
        {
            layout?.Destroy();
            layout = null;
        }

        public static class SimpleLayout
        {
            public static void Setup(LayoutRoot layout, int multiplier)
            {
                // a grid demonstrating basic proportional control
                new TextObject(layout)
                {
                    TextAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top,
                    Text = $"X{multiplier}",
                    FontSize = 35,
                    Font = UI.TrajanBold,
                    ContentColor = UnityEngine.Color.red,
                    Padding = new(265)
                };
            }
        }
    }
}