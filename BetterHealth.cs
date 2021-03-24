using Newtonsoft.Json;
using Oxide.Game.Rust.Cui;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Oxide.Plugins
{
    [Info("Better Health", "birthdates & Default", "1.1.1")]
    [Description("Ability to customize the max health")]
    public class BetterHealth : RustPlugin
    {
        #region Variables
        private readonly List<BasePlayer> InUI = new List<BasePlayer>();
        private const string permission_use = "betterhealth.use";
        #endregion

        #region Hooks
        void Init()
        {
            permission.RegisterPermission(permission_use, this);
            LoadConfig();
            if(Config1.MaxHealth <= 100)
            {
                Unsubscribe(nameof(OnPlayerDie));
                Unsubscribe(nameof(OnPlayerHealthChange));
            }

            foreach (var P in Config1.Permissions.Keys)
            {
                var Perm = $"betterhealth.{P}";
                if(!permission.PermissionExists(Perm, this)) permission.RegisterPermission(Perm, this);
            }
        }

        void OnPlayerInit(BasePlayer player) => timer.In(1f, () =>
        {
            if (!player.IPlayer.HasPermission(permission_use)) return;
            var h = Config1.MaxHealth;
            player._maxHealth = h;
            if(h > 100)
            {
                OpenUI(player);
            }
        });

        void Unload()
        {
            InUI.ForEach(player =>
            {
                CloseUi(player, true);
            });
        }

        void OnPlayerDie(BasePlayer player, HitInfo info) => CloseUi(player);

        void OnPlayerHealthChange(BasePlayer player, float oldValue, float newValue)
        {
            NextTick(() =>
            {
                if (!player.IPlayer.HasPermission(permission_use)) return;
                OpenUI(player, true);
            });
        }

        #endregion

        #region UI
        void OpenUI(BasePlayer Player, bool Close = false)
        {
            if(Close)
            {
                CloseUi(Player);
            }
            var Health = Player.Health();
            var Max = GetHealth(Player);
            var HealthX = Health / Max;
            var PendingX = Mathf.Clamp(HealthX + Player.metabolism.pending_health.value / Max, 0, 1);

            //CuiHelper.AddUi(Player, @"");
  

            CuiHelper.AddUi(Player, $"[{{\"name\":\"HealthOverlay\",\"parent\":\"Overlay\",\"components\":[{{\"type\":\"UnityEngine.UI.Image\",\"color\":\"0.4375 0.4375 0.4375 1\"}},{{\"type\":\"RectTransform\",\"anchormin\":\"0.856 0.104\",\"anchormax\":\"0.984 0.1309\"}}]}},{{\"name\":\"HealthBar\",\"parent\":\"HealthOverlay\",\"components\":[{{\"type\":\"UnityEngine.UI.Image\",\"color\":\"0.5546875 0.7265625 0.3125 1\"}},{{\"type\":\"RectTransform\",\"anchormin\":\"0 0\",\"anchormax\":\"{HealthX} 0.97\"}}]}},{{\"name\":\"HealthText\",\"parent\":\"HealthBar\",\"components\":[{{\"type\":\"UnityEngine.UI.Text\",\"text\":\"  {Math.Round(Health).ToString()}\",\"fontSize\":15,\"align\":\"MiddleLeft\",\"color\":\"1 1 1 0.65\"}},{{\"type\":\"RectTransform\",\"anchormin\":\"0 0\",\"anchormax\":\"1 1\"}}]}},{{\"name\":\"PendingHealth\",\"parent\":\"HealthOverlay\",\"components\":[{{\"type\":\"UnityEngine.UI.Image\",\"color\":\"0.5546875 0.7265625 0.3125 0.3\"}}, {{ \"type\":\"RectTransform\", \"anchormin\":\"0 0\", \"anchormax\":\"{PendingX} 0.97\"}}]}} ]");
            InUI.Add(Player);
        }

        float GetHealth(BasePlayer Player)
        {
            var health = Config1.Permissions.Where(p => Player.IPlayer.HasPermission(p.Key)).DefaultIfEmpty();
            var keyValuePairs = health as KeyValuePair<string, float>[] ?? health.ToArray();
            return !keyValuePairs.Any() ? Config1.MaxHealth : keyValuePairs.Max(a => a.Value);
        }

        private void CloseUi(BasePlayer player, bool unload = false)
        {
            CuiHelper.DestroyUi(player, "HealthOverlay");
            if(!unload)
            {
                InUI.Remove(player);
            }
        }
        #endregion

        #region Configuration, Language & Data

        public ConfigFile Config1 { get; set; }

        public class ConfigFile
        {
            [JsonProperty("Default Max Health")]
            public float MaxHealth = 200f;
            [JsonProperty("Max Health Permissions")]
            public Dictionary<string, float> Permissions = new Dictionary<string, float>
            {
                {"vip", 300f}
            };
        }

        protected override void LoadConfig()
        {
            base.LoadConfig();
            Config1 = Config.ReadObject<ConfigFile>();
            if(Config1 == null)
            {
                LoadDefaultConfig();
            }
        }

        protected override void LoadDefaultConfig()
        {
            Config1 = new ConfigFile();
            PrintWarning("Default configuration has been loaded.");
        }

        protected override void SaveConfig() => Config.WriteObject(Config1);
        #endregion
    }
}
//Generated with birthdates' Plugin Maker
