using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Newtonsoft.Json;
using UnityEngine;

//TODO Not base off of config file, make use of data files 

namespace Oxide.Plugins
{

    [Info("MassWhitelist", "Default", "1.1.5")]
    public class MassWhitelist : CovalencePlugin
    {
        public const string perm = "masswhitelist.perm";

        public class whitelistIDs
        {
            public ulong whitelistid;
        }

        private static ConfigFile config;

        private class ConfigFile
        {

            //public Dictionary<ulong, string> WhiteList { get; set;  }

            [JsonProperty(PropertyName = "Whitelisting IDs")]
            public List<whitelistIDs> whitelistids { get; set; }

            public static ConfigFile DefaultConfig()
            {
                return new ConfigFile
                {
                    whitelistids = new List<whitelistIDs>()
                    {
                        new whitelistIDs { whitelistid = 76561198205187064 }

                    }
                };
            }

        }
        protected override void LoadDefaultConfig()
        {
            config = ConfigFile.DefaultConfig();
            PrintWarning("No configuration file found, generating a new one");
        }
        protected override void LoadConfig()
        {
            base.LoadConfig();
            try
            {
                config = Config.ReadObject<ConfigFile>();
                if (config == null)
                {
                    Regenerate();
                }
            }
            catch { Regenerate(); }
        }
        protected override void SaveConfig()
        {
            Config.WriteObject(config);
        }

        private void Regenerate()
        {
            PrintWarning($"Your configuration file is damaged here. 'oxide/config/{Name}.json' Creating a new file...");
            LoadDefaultConfig();
        }


        void OnServerInitialized()
        {
            config = Config.ReadObject<ConfigFile>();

            permission.RegisterPermission(perm, this);
        }

        private object CanUserLogin(string name, string id)
        {
            var player = players.FindPlayerById(id);
            if (player != null && player.IsAdmin)
                return null;

            foreach (var t in config.whitelistids)
                if (player != null && (t.whitelistid.ToString() == id || player.HasPermission(perm)))
                    return null;

            return "You are not whitelisted!";
        }
    }
}