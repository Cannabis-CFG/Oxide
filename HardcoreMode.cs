using System.Collections.Generic;
using System;
using Network;
using Rust;

namespace Oxide.Plugins
{
    [Info("Hardcore Mode", "Default", "0.1.0")]
    [Description("Brings an overhaul to the base game.")]
    class HardcoreMode : RustPlugin
    {
        /*
         * TODO Limit building entities. Limit damage, inventory space, etc. Create entire gamemode. Essentially overhaul Rust.
         * TODO Support for other ban options.
		 * TODO Finish the damn plugin me
		 * TODO Null check. Null check. Null check.
         */

        #region Variables

        private bool Changed = false;
        public bool oneLife;
        public float damageAmount;

        
        #endregion


        #region Main plugin

        void OnPlayerDie(BasePlayer player, HitInfo info)
        {
            if (player is NPCPlayer || player == null || info == null)
            {
                return;
            }

            if (oneLife)
            {
                Server.Command($"ban {player.userID} \"You died in hardcore mode.\" ");
            }

            PrintToChat(player, lang.GetMessage("Died", this, player.UserIDString));
        }

        void OnEntityTakeDamage(BaseCombatEntity entity, HitInfo info)
        {

            if (info.isHeadshot)
            {
                info.damageTypes.ScaleAll(100f);
                return;
            }

            info.damageTypes.ScaleAll(55f);

            
        }
        


        private void OnPlayerInit(BasePlayer player)
        {
            if (player.IsReceivingSnapshot)
            {
                timer.Once(1, () => OnPlayerInit(player));
                return;
            }

            AddGui(player);
        }

        private void OnPlayerDisconnected(BasePlayer player) => DestroyGui(player);


        private void OnPlayerDie(BasePlayer player) => DestroyGui(player);


        private void OnPlayerRespawn(BasePlayer player) => AddGui(player);


        private void LoadMessages()
        {

            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["Died"] = "You just died in hardcore mode.",
            }, this);
        }

        #endregion

        #region HardcoreGUI

        #region JSON
        private string json = @"[  
		{ 
			""name"": ""HardcoreOverlay"",
			""parent"": ""Overlay"",
			""components"":
			[
				{
					 ""type"":""UnityEngine.UI.Image"", 
					 ""color"":""0 0 0 0"",
				},
				{
					""type"":""RectTransform"",
					""anchormin"": ""0.837 0.0"",
					""anchormax"": ""0.986 0.089""
				}
			]
		},
        { 
			""parent"": ""HardcoreOverlay"",
			""components"":
			[
				{
					 ""type"":""UnityEngine.UI.Image"",
					 ""color"":""0.1 0.1 0.1 1"",
				},
				{
					""type"":""RectTransform"",
					""anchormin"": ""0 0.20"",
					""anchormax"": ""1 1.5""
				}
			]
		}
        ]
		";
        #endregion

        private void Loaded()
        {
            foreach (var player in BasePlayer.activePlayerList)
            {
                AddGui(player);
            }
        }

        private void Unload()
        {
            foreach (var player in BasePlayer.activePlayerList)
            {
                DestroyGui(player);
            }
        }

        private void AddGui(BasePlayer player)
        {

            CommunityEntity.ServerInstance.ClientRPCEx(new SendInfo { connection = player.net.connection }, null, "AddUI", json.Replace("{ACTIVATED}", "Hardcore Mode is activated!"));
        }

        private void DestroyGui(BasePlayer player)
        {
            CommunityEntity.ServerInstance.ClientRPCEx(new SendInfo { connection = player.net.connection }, null, "DestroyUI", "HardcoreOverlay");
        }



        #endregion

        #region Config 


        object GetConfig(string menu, string datavalue, object defaultValue)
        {
            var data = Config[menu] as Dictionary<string, object>;
            if (data == null)
            {
                data = new Dictionary<string, object>();
                Config[menu] = data;
                Changed = true;
            }
            object value;
            if (!data.TryGetValue(datavalue, out value))
            {
                value = defaultValue;
                data[datavalue] = value;
                Changed = true;
            }
            return value;
        }

        void LoadVariables()
        {

            oneLife = Convert.ToBoolean(GetConfig("Hardcore Mode", "One life?", false));
            damageAmount = Convert.ToSingle(GetConfig("Hardcore Mode", "Damage multiplier", 0.5f));

            if (!Changed) return;
            SaveConfig();
            Changed = false;
        }

        protected override void LoadDefaultConfig()
        {
            Config.Clear();
            LoadVariables();
        }

        void Init()
        {
            LoadVariables();
            LoadMessages();
        }
        #endregion

    }
}