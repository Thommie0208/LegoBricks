using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using MagicUI.Core;
using MagicUI.Elements;
using Modding;
using Satchel;
using SFCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Lego_Power_Bricks
{
    public class x2Multiplier : EasyCharm
    {
        protected override int GetCharmCost() => 0;
        protected override string GetDescription() => "Pocket change to be honest, by far the worst brick.\n\nMultiplies geo worth by 2";
        protected override string GetName() => "Geo x2";
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("Red_brick.png");
    }

    public class x4Multiplier : EasyCharm
    {
        protected override int GetCharmCost() => 0;
        protected override string GetDescription() => "There is no way I am thinking of something original for all 5 geo multipliers.\n\nMultiplies geo worth by 4";
        protected override string GetName() => "Geo x4";
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("Red_brick.png");
    }

    public class x6Multiplier : EasyCharm
    {
        protected override int GetCharmCost() => 0;
        protected override string GetDescription() => "Completely wrecks the economy. Thankfully there are only 2 stronger multipliers.\n\nMultiplies geo worth by 6";
        protected override string GetName() => "Geo x6";
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("Red_brick.png");
    }

    public class x8Multiplier : EasyCharm
    {
        protected override int GetCharmCost() => 0;
        protected override string GetDescription() => "To put things in perspective, with this you can do colo 3 three times and be able to affort everything in the game.\n\nMultiplies geo worth by 8";
        protected override string GetName() => "Geo x8";
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("Red_brick.png");
    }

    public class x10Multiplier : EasyCharm
    {
        protected override int GetCharmCost() => 0;
        protected override string GetDescription() => "https://youtu.be/W8Z3MfNpJpE \n\nMultiplies geo worth by 10";
        protected override string GetName() => "Geo x10";
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("Red_brick.png");
    }

    public class GeoMagnet : EasyCharm
    {
        protected override int GetCharmCost() => 0;
        protected override string GetDescription() => "As if you ever needed more geo with the other bricks.\n\nMakes gathering swarm free";
        protected override string GetName() => "Geo Magnet";
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("Red_brick.png");
    }

    public class RegenerateHeatlh : EasyCharm
    {
        protected override int GetCharmCost() => 0;
        protected override string GetDescription() => "Don't you agree that pesky hiveblood takes too long?\n\nRegenerates lost hearts, once per 7 seconds.";
        protected override string GetName() => "Health regenerator";
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("Red_brick.png");
    }

    public class IncreaseHealth : EasyCharm
    {
        protected override int GetCharmCost() => 0;
        protected override string GetDescription() => "Does exactly what the name implies.\n\nSeriously, I'm not explaining this one";
        protected override string GetName() => "Increase health";
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("Red_brick.png");
    }

    public class SuperSlap : EasyCharm
    {
        protected override int GetCharmCost() => 0;
        protected override string GetDescription() => "If Fury of the Fallen and Strength had a child and the child overshadowed both of them.\n\nStrongly increased nail damage";
        protected override string GetName() => "Super Slap";
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("Red_brick.png");
    }
    public class SoftFall : EasyCharm
    {
        protected override int GetCharmCost() => 0;
        protected override string GetDescription() => "For when no fall damage is not luxerious enough.\n\nRemoves the hard fall stun";
        protected override string GetName() => "Soft Fall";
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("Red_brick.png");
    }
    public class InfiniteBlast : EasyCharm
    {
        protected override int GetCharmCost() => 0;
        protected override string GetDescription() => "So anyway, I started blasting.\n\nMakes Vengeful Spirit/Shade Soul cheaper to use";
        protected override string GetName() => "Infinite blast";
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("Red_brick.png");
    }
    public class OverrideGeoCap : EasyCharm
    {
        protected override int GetCharmCost() => 0;
        protected override string GetDescription() => "The only thing making the Geo multipliers worth it.\n\nIncreases geo cap to the 32 bit limit";
        protected override string GetName() => "Raise Geo Cap";
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("Red_brick.png");
    }

    public class Settings
    {
        public Dictionary<string, EasyCharmState> Charms;
    }
    public class Lego_Power_Bricks : Mod, ILocalSettings<Settings>
    {
        public Lego_Power_Bricks() : base("Lego Power Bricks") { }
        public override string GetVersion() => "0.1.1";
        private bool healing = false;
        private bool healthIncreased = false;
        private bool nailDamageIncreased = false;
        private bool hardFallTimeIncreased = false;
        private bool vengefulSpiritModified = false;
        private bool geoCapModified = false;
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
            {"infiniteBlast", new InfiniteBlast()},
            {"overrideGeoCap", new OverrideGeoCap() }
        };


        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            On.HeroController.Awake += OnAwake;
            //On.HeroController.AddGeo += AddGeo;
            ModHooks.CharmUpdateHook += OnCharmUpdate;
            ModHooks.HeroUpdateHook += OnHeroUpdate;
            On.GameCameras.StartScene += AddMasks;
            ModHooks.GetPlayerIntHook += BuffNail;
            On.PlayerData.AddGeo += PlayerData_AddGeo;
            if (ModHooks.GetMod("DebugMod") is Mod)
            {
                HookDebug();
            }
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
            if (layout != null && GameManager.instance.inventoryFSM.ActiveStateName == "Opened" || GameManager.instance.inventoryFSM.ActiveStateName == "Open Current Pane" || GameManager.instance.gameState.ToString() != "PLAYING")
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
            vengefulSpiritModified = Charms["infiniteBlast"].IsEquipped;
            geoCapModified = Charms["overrideGeoCap"].IsEquipped;
            orig(self);
        }
        public void OnCharmUpdate(PlayerData data, HeroController hc)
        {
            Log($"OnCharmUpdate called");
            if (Charms["geoMagnet"].IsEquipped)
            {
                Log("GeoMagnet activated");
                data.charmCost_1 = 0;
                data.CalculateNotchesUsed();
            }
            else
            {
                Log("GeoMagnet deactivated");
                data.charmCost_1 = 1;
                data.CalculateNotchesUsed();
            }
            if (!Charms["geoMagnet"].IsEquipped && data.equippedCharm_1)
            {
                data.CalculateNotchesUsed();
            }
            if (Charms["increaseHealth"].IsEquipped && !healthIncreased)
            {
                Log("increaseHealth activated");
                healthIncreased = true;
                hc.AddToMaxHealth(2);
            }
            else if (!Charms["increaseHealth"].IsEquipped && healthIncreased)
            {
                Log("increaseHealth deactivated");
                healthIncreased = false;
                hc.AddToMaxHealth(-2);
            }
            if (Charms["softFall"].IsEquipped && !hardFallTimeIncreased)
            {
                Log("softFall activated");
                hc.BIG_FALL_TIME = 999f;
                hardFallTimeIncreased = true;
            }
            else if (!Charms["softFall"].IsEquipped && hardFallTimeIncreased)
            {
                Log("softFall deactivated");
                hc.BIG_FALL_TIME = vanillaHardFallTime;
                hardFallTimeIncreased = false;
            }
            if (Charms["infiniteBlast"].IsEquipped && !vengefulSpiritModified)
            {
                Log("infiniteBlast activated");
                ModifyVengefulSpirit(hc);
                vengefulSpiritModified = true;
            }
            else if (!Charms["infiniteBlast"].IsEquipped && vengefulSpiritModified)
            {
                Log("infiniteBlast deactivated");
                UnModifyVengefulSpirit(hc);
                vengefulSpiritModified = false;
            }
            if (Charms["overrideGeoCap"].IsEquipped && !geoCapModified)
            {
                Log("overrideGeoCap activated");
                //Set current geo count to saved count;
                geoCapModified = true;
            }
            else if (!Charms["overrideGeoCap"].IsEquipped && vengefulSpiritModified)
            {
                Log("overrideGeoCap deactivated");
                //Save geo count if it's above 9_999_999
                geoCapModified = false;
            }
            CalculateMultiplier();

        }
        private IEnumerator RegenerateHealth()
        {
            Log("Started healing");
            healing = true;
            yield return new WaitForSeconds(7f);
            HeroController.instance.AddHealth(1);
            Log("Finished healing");
            healing = false;
        }
        private void PlayerData_AddGeo(On.PlayerData.orig_AddGeo orig, PlayerData self, int amount)
        {
            int current = self.GetInt("geo");
            int newGeo = current + (amount * geoMultiplier);

            int customCap = (Charms["overrideGeoCap"].IsEquipped) ? 2147483647 : 9999999;
            if (customCap == 2147483647)
            {
                Log("OverrideGeoCap Active");
            }
            if (newGeo > customCap)
                newGeo = customCap;

            self.geo = newGeo;
        }

        private int BuffNail(string intName, int damage)
        {
            if (intName == "nailDamage" && Charms["superSlap"].IsEquipped)
            {
                Log("SuperSlap active");
                float addition = (float)damage * 0.75f;
                damage += (int)addition; 
            }
            return damage;
        }

        private void ModifyVengefulSpirit(HeroController self)
        {
            PlayMakerFSM fsm = self.gameObject.LocateMyFSM("Spell Control");
            if (fsm == null) return;
            int newCost = 10;
            FsmState CanCastOld = fsm.GetValidState("Can Cast?");
            var CanCastVS = fsm.AddState("Can Cast? FIREBALL");
            var CanCastQuake = fsm.AddState("Can Cast? QUAKE");
            var CanCastScream = fsm.AddState("Can Cast? SCREAM");
            fsm.AddTransition("Can Cast? FIREBALL", "CANCEL", "Inactive");
            fsm.AddTransition("Can Cast? FIREBALL", "FINISHED", "Has Fireball?");
            fsm.AddTransition("Can Cast? QUAKE", "CANCEL", "Inactive");
            fsm.AddTransition("Can Cast? QUAKE", "FINISHED", "Has Quake?");
            fsm.AddTransition("Can Cast? SCREAM", "CANCEL", "Inactive");
            fsm.AddTransition("Can Cast? SCREAM", "FINISHED", "Has Scream?");
            fsm.ChangeTransition("Button Down", "BUTTON UP", "QC");
            fsm.ChangeTransition("Inactive", "QUICK CAST", "QC");
            fsm.ChangeTransition("QC", "FIREBALL", "Can Cast? FIREBALL");
            fsm.ChangeTransition("QC", "QUAKE", "Can Cast? QUAKE");
            fsm.ChangeTransition("QC", "SCREAM", "Can Cast? SCREAM");
            CanCastVS.CopyActionData(CanCastOld);
            CanCastQuake.CopyActionData(CanCastOld);
            CanCastScream.CopyActionData(CanCastOld);
            fsm.GetAction<IntCompare>("Can Cast? FIREBALL", 2).integer2 = newCost;
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
            fsm.RemoveState("Can Cast? FIREBALL");
            fsm.RemoveState("Can Cast? QUAKE");
            fsm.RemoveState("Can Cast? SCREAM");
            fsm.ChangeTransition("Button Down", "BUTTON UP", "Can Cast?");
            fsm.ChangeTransition("Inactive", "QUICK CAST", "Can Cast? QC");
            fsm.ChangeTransition("QC", "FIREBALL", "Has Fireball?");
            fsm.ChangeTransition("QC", "QUAKE", "Has Quake?");
            fsm.ChangeTransition("QC", "SCREAM", "Has Scream?");
        }

        private void AddMasks(On.GameCameras.orig_StartScene orig, GameCameras self)
        {
            orig(self);
            Log("Adding masks");
            MasksOverflow(self);
            On.GameCameras.StartScene -= AddMasks;
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

        //public void AddGeo(On.HeroController.orig_AddGeo orig, HeroController self, int amount)
        //{
        //    orig(self, amount * geoMultiplier);
        //}

        private int CalculateMultiplier()
        {
            int multiplier = 1;
            for (int i = 2; i<=10; i+=2)
            {
                if (Charms[$"x{i}Multiplier"].IsEquipped)
                {
                    multiplier *= i;
                }
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

        private void HookDebug()
        {
            DebugMod.BindableFunctions.OnGiveAllCharms += () => {
                foreach (var charm in Charms.Values)
                {
                    charm.GiveCharm();
                }
                PlayerData.instance.CountCharms();
            };

            DebugMod.BindableFunctions.OnRemoveAllCharms += () => {
                foreach (var charm in Charms.Values)
                {
                    charm.TakeCharm();
                }
                PlayerData.instance.CountCharms();
                OnCharmUpdate(PlayerData.instance, HeroController.instance);
            };
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