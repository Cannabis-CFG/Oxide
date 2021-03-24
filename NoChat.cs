using ConVar;
using Oxide.Core.Libraries.Covalence;
using Oxide.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oxide.Plugins
{
    [Info("No Chat", "Default", "1.0.0")]
    [Description("No permission no chat")]
    class NoChat : CovalencePlugin
    {
        public string perm = "nochat.chat";

        [PluginReference] private Plugin BetterChat;

        void OnServerInitialized()
        {
            permission.RegisterPermission(perm, this);
            
        }

        void Init() 
        {
            server.Command("CmdGlobalMute");
                //Register messages to reply to the player
            lang.RegisterMessages(new Dictionary<string, string>
            {
                //List of messages
                ["NoChat"] = "Unavailable to speak",

            }, this);
            //Send a message to every online player
            foreach (var p in BasePlayer.activePlayerList) 
            {
                //PrintToChat(p, "NoChat loaded | this is getting annoying, why is betterchat so weird..... But I'll do it don't worry");
            }
        }

        object OnPlayerChat(BasePlayer player, string message, Chat.ChatChannel channel)
        {
            //Get the message of the player if using betterchat
            string a = GetMessage(player, message);
            if (!permission.UserHasPermission(player.UserIDString, perm)) 
            {
                //if (plugins.Exists("BetterChat")) return null;
                a.Replace($"{message}", " ");
                PrintToChat(player, lang.GetMessage("NoChat", this));
                return null;
            }
            return null;
        }

        string GetMessage(BasePlayer p, string message, bool console = false) 
        {
            IPlayer pp = p.IPlayer;
            return BetterChat?.Call<string>("API_GetFormattedMessage", pp, message, false);
        }

    }
}
