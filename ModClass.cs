using System.Collections.Generic;
using Modding;
using UnityEngine;
using SFCore;
using Satchel;

namespace Lego_Power_Bricks
{

    public class x2Multiplier : EasyCharm
    {
        protected override int GetCharmCost() => 0;
        protected override string GetDescription() => "Double the worth of geo by 2";
        protected override string GetName() => "Geo x2";
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
              {"x2Multiplier", new x2Multiplier()}
        };


        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Log("Initializing");

            Instance = this;

            Log("Initialized");
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