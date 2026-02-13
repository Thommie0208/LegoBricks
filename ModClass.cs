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
    - Add current multiplier next to geo count
    - Geo magnet
    - Contact Damage 
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
        private float vanillaHardFallTime;
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
            {"softFall", new SoftFall()}
        };


        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            On.HeroController.Awake += OnAwake;
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
            if (Charms["softFall"].IsEquipped && HeroController.instance.fallTimer > HeroController.instance.BIG_FALL_TIME)
            {
                HeroController.instance.BIG_FALL_TIME *= 2;
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
            else if (nailDamageIncreased)
            {
                data.nailDamage /= 2;
                nailDamageIncreased = false;
            }
            if (Charms["softFall"].IsEquipped && !hardFallTimeIncreased)
            {
                vanillaHardFallTime = hc.BIG_FALL_TIME;
                hc.BIG_FALL_TIME = 999f;
                hardFallTimeIncreased = true;
            }
            else if (hardFallTimeIncreased)
            {
                hc.BIG_FALL_TIME = vanillaHardFallTime;
                hardFallTimeIncreased = false;
            }
        }
        private IEnumerator RegenerateHealth()
        {
            healing = true;
            yield return new WaitForSeconds(10f);
            HeroController.instance.AddHealth(1);
            healing = false;
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
            if (newMultiplier != geoMultiplier)
            {
                //layout?.Destroy();
                //layout = null;
                //Log("Update Multiplier Text Called");
                //if (layout == null)
                //{
                //    layout = new(true, "Persistent layout");
                //    layout.RenderDebugLayoutBounds = false;
                //    GridExample.Setup(layout, newMultiplier);
                //}
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

        public static class GridExample
        {
            public static void Setup(LayoutRoot layout, int multiplier)
            {
                // a grid demonstrating basic proportional control
                new MagicUI.Elements.GridLayout(layout, "Proportional Grid Example 1")
                {
                    MinWidth = 1920, // divide the entire screen's width
                    ColumnDefinitions =
                {
                    new GridDimension(2, GridUnit.Proportional),
                    new GridDimension(1, GridUnit.Proportional),
                    new GridDimension(1, GridUnit.Proportional),
                },
                    RowDefinitions =
                {
                    new GridDimension(2, GridUnit.Proportional),
                    new GridDimension(1, GridUnit.Proportional),
                    new GridDimension(1, GridUnit.Proportional),
                }, 
                    VerticalAlignment = VerticalAlignment.Bottom,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Children =
                {
                    new TextObject(layout)
                    {
                        FontSize = 15,
                        Text = $"{multiplier}",
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                    }.WithProp(MagicUI.Elements.GridLayout.Row, 0), // technically you don't need to set this
                    new TextObject(layout)
                    {
                        FontSize = 20,
                        Text = "This text spans only 1 proportional column\nand drives the height of the grid\nbecause it is tallest.",
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        TextAlignment = HorizontalAlignment.Center,
                    }.WithProp(MagicUI.Elements.GridLayout.Column, 1),
                    new TextObject(layout)
                    {
                        FontSize = 15,
                        Text = "Hope you're having a nice day :)",
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                    }.WithProp(MagicUI.Elements.GridLayout.Column, 2),
                }
                };

                // another way to do the same thing, demonstrates columnspans
                new MagicUI.Elements.GridLayout(layout, "Proportional Grid Example 2")
                {
                    MinWidth = 1920, // divide the entire screen's width
                    ColumnDefinitions =
                {
                    new GridDimension(1, GridUnit.Proportional),
                    new GridDimension(1, GridUnit.Proportional),
                    new GridDimension(1, GridUnit.Proportional),
                    new GridDimension(1, GridUnit.Proportional),
                },
                    VerticalAlignment = VerticalAlignment.Bottom,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Children =
                {
                    new TextObject(layout)
                    {
                        FontSize = 15,
                        Text = "This spans 2 proportional columns the same width as the others",
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                    }.WithProp(MagicUI.Elements.GridLayout.Column, 0).WithProp(MagicUI.Elements.GridLayout.ColumnSpan, 2),
                    new TextObject(layout)
                    {
                        FontSize = 20,
                        Text = "This text spans only 1 proportional column\nand drives the height of the grid\nbecause it is tallest.",
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        TextAlignment = HorizontalAlignment.Center,
                    }.WithProp(MagicUI.Elements.GridLayout.Column, 2),
                    new TextObject(layout)
                    {
                        FontSize = 15,
                        Text = "Hope you're having a nice day :)",
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                    }.WithProp(MagicUI.Elements.GridLayout.Column, 3)
                }
                };

                // more complex usage of grids, mix and match proportional and absolute columns
                new MagicUI.Elements.GridLayout(layout, "Complex Grid Example")
                {
                    Padding = new Padding(10),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    RowDefinitions =
                {
                    new GridDimension(1, GridUnit.Proportional),
                    new GridDimension(50, GridUnit.AbsoluteMin),
                    new GridDimension(50, GridUnit.AbsoluteMin),
                    new GridDimension(3, GridUnit.Proportional),
                },
                    ColumnDefinitions =
                {
                    new GridDimension(1.5f, GridUnit.Proportional),
                    new GridDimension(50, GridUnit.AbsoluteMin),
                    new GridDimension(50, GridUnit.AbsoluteMin),
                    new GridDimension(5, GridUnit.Proportional),
                },
                    MinWidth = 400,
                    MinHeight = 400,
                    Children =
                {
                    new Image(layout, BuiltInSprites.CreateSlicedBorderRect())
                    {
                        Width = 90,
                        Height = 90,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                    }.WithProp(MagicUI.Elements.GridLayout.Row, 1).WithProp(MagicUI.Elements.GridLayout.Column, 1)
                    .WithProp(MagicUI.Elements.GridLayout.RowSpan, 2).WithProp(MagicUI.Elements.GridLayout.ColumnSpan, 2),
                    new Image(layout, BuiltInSprites.CreateQuill())
                    {
                        Width = 25,
                        Height = 25,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Tint = UnityEngine.Color.magenta,
                    }.WithProp(MagicUI.Elements.GridLayout.Row, 1).WithProp(MagicUI.Elements.GridLayout.Column, 1),
                    new Image(layout, BuiltInSprites.CreateQuill())
                    {
                        Width = 25,
                        Height = 25,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Tint = UnityEngine.Color.green,
                    }.WithProp(MagicUI.Elements.GridLayout.Row, 2).WithProp(MagicUI.Elements.GridLayout.Column, 1),
                    new Image(layout, BuiltInSprites.CreateQuill())
                    {
                        Width = 25,
                        Height = 25,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Tint = UnityEngine.Color.blue,
                    }.WithProp(MagicUI.Elements.GridLayout.Row, 1).WithProp(MagicUI.Elements.GridLayout.Column, 2),
                    new Image(layout, BuiltInSprites.CreateQuill())
                    {
                        Width = 25,
                        Height = 25,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Tint = UnityEngine.Color.cyan,
                    }.WithProp(MagicUI.Elements.GridLayout.Row, 2).WithProp(MagicUI.Elements.GridLayout.Column, 2),
                    new TextObject(layout)
                    {
                        FontSize = 22,
                        Text = "This is in\nthe largest\ncell",
                        TextAlignment = HorizontalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        VerticalAlignment = VerticalAlignment.Bottom,
                    }.WithProp(MagicUI.Elements.GridLayout.Row, 3).WithProp(MagicUI.Elements.GridLayout.Column, 3),
                    new TextObject(layout)
                    {
                        // using only the available space provided by min size this column should be
                        // ~70px wide; (400 - 50 - 50) / 6.5 * 1.5 to respect proportionality.
                        // note that by adding this longer text, the grid becomes much wider than 400px
                        // to retain the proportionality constraint as the cell grows.
                        Text = "Wider than 70px"
                    } // default row and column are 0
                }
                };
            }
        }
    }
}