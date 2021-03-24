using Oxide.Core.Plugins;
using System;
using System.Collections.Generic;


namespace Oxide.Plugins
{
    [Info("Blood Bags", "Default", "1.7.1")]
    [Description("Blood bags now recover health")]
    internal class BloodBags : RustPlugin
    {
        #region variables

        public bool Changed = true;
        public float bagpending = 50f;
        //public string imgonnaputthisinconsolebecausewhynot = "what the fuck";
        public float baginsta = 50f;
        public bool bagbleeding;
        public int bloodbagid = 1776460938;
        public  int resource1 = -2072273936; //Bandages
        public int resource1amount = 5;
        public int resource2 = 1079279582; //Med syringe
        public Dictionary<int, int> craftingresource = null; //TODO Add a list of items users can add and remove crafting materials 
        public int resource2amount = 1;
        public float crafttime = 30f;
        public bool canCraft = true;
        public string Chatprefix;
        const string craftPerm = "bloodbags.cancraft";
        const string usePerm = "bloodbags.use";
        public float heallimit = 10f;
        public bool limitCrafting = true;
        public string craftcommand = "craftbag";
        public bool betterhealthsupport = false;
        public float maxhealthincrease = 10f;
        public float healthtime = 15f;
        public int maxUses = 45;
        //public bool dunnowhattocallthis = false;

        static Dictionary<int, int> craftingresources() 
        {
            var cr = new Dictionary<int, int>();
            cr.Add(-2072273936, 5);
            cr.Add(1079279582, 2);
            return cr;
        }

        private readonly HashSet<ulong> _healing = new HashSet<ulong>();
        private readonly HashSet<ulong> _crafting = new HashSet<ulong>();
        private readonly HashSet<int> _uses = new HashSet<int>();
        private readonly HashSet<ulong> _blocked = new HashSet<ulong>();

        private int bloodID = 1776460938;
        [PluginReference] private Plugin BetterHealth;

        #endregion
        void Init()
        {
            LoadVariables();
            lang.RegisterMessages(Messages, this);
            permission.RegisterPermission(craftPerm, this);
            permission.RegisterPermission(usePerm, this);
            cmd.AddChatCommand(craftcommand, this, nameof(CmdcraftingBags));
            _healing.Clear();
            _crafting.Clear();
        }
        private void OnPlayerInput(BasePlayer player, InputState input)
        {
            if (!input.WasJustPressed(BUTTON.FIRE_PRIMARY) || player == null) { return; }

            DealWithBags(player);
        }

        void DealWithBags(BasePlayer p)
        {
            if (p == null || !permission.UserHasPermission(p.UserIDString, usePerm)) return;
            if (_healing.Contains(p.userID)) return; //TODO Overhaul this so it just doesn't deny, rather takes a specified time to apply the healing
            var item = p.GetActiveItem();
            if (!item.ToString().Contains("blood")) return;
            if (bagbleeding)
            {
                p.metabolism.bleeding.value = 0f;
            }

           // var modDefs = Modifier.ModifierType.Ore_Yield.Equals(20);



            p.metabolism.pending_health.value += bagpending;
            p.health += baginsta;
            //p._maxHealth += bagpending;
            //PrintError($"{p._maxHealth}");
            p.inventory.Take(null, bloodbagid, 1);
            //a = Modifier.ModifierType.Max_Health;
            //p.modifiers.Add(ModifierType.Max_Health); 
            if (BetterHealth && betterhealthsupport) //TODO Make an OD feature, or make it to where server admins can limit how many times they can increase health
            {
                //p._maxHealth += maxhealthincrease;
                DealWithHealth(p);
                
            }
            PrintToChat(p, string.Format(lang.GetMessage("Usage", this, p.UserIDString), Chatprefix, baginsta));
            _healing.Add(p.userID);
            

            timer.Once(heallimit, () => 
            {
                _healing.Remove(p.userID);
            });

        }


        void DealWithHealth(BasePlayer player) 
        {
            if (BetterHealth && betterhealthsupport) 
            {
                player.SetMaxHealth(maxhealthincrease);

                timer.Once(healthtime, () => { player.SetMaxHealth(100); });
                return;

            }
        }
        

        //[ChatCommand("craftbag")]
        private void CmdcraftingBags(BasePlayer p, string command, string[] args)
        {
            if (p == null) return;
            if (!canCraft || !permission.UserHasPermission(p.UserIDString, craftPerm)) return;
            

            switch (args.Length > 0 ? args[0] : "")
            {
                default:
                {
                    
                    PrintToChat(p, string.Format(lang.GetMessage("IncorrectFormat", this), craftcommand));
                    break;
                }
                case "craft":
                {

                        /*foreach (var x in craftingresource) 
                        {
                            int am = x.Key;
                        }*/


                    int craft1 = p.inventory.GetAmount(resource1);
                    int craft2 = p.inventory.GetAmount(resource2);

                    if (_crafting.Contains(p.userID) && limitCrafting)
                    {
                        string message = lang.GetMessage("AlreadyCrafting", this);
                        PrintToChat(p, string.Format(message, Chatprefix));
                        return;
                    }


                        if (craft1 >= resource1amount & craft2 >= resource2amount)
                    {
                        p.inventory.Take(null, resource1, resource1amount);
                        p.inventory.Take(null, resource2, resource2amount);
                    }
                    else
                    {
                        p.ChatMessage(lang.GetMessage("NoResources", this));
                        return;
                    }

                    p.ChatMessage(string.Format(lang.GetMessage("Crafting", this), crafttime));
                    _crafting.Add(p.userID);
                    timer.Once(crafttime, () =>
                    {
                        p.inventory.GiveItem(ItemManager.CreateByItemID(bloodID));
                        p.Command("note.inv", bloodID, 1);
                        if (!limitCrafting) return;
                        _crafting.Remove(p.userID);

                        
                    });
                    break;
                }
                case "cost": 
                {
                        PrintToChat(p, string.Format(lang.GetMessage("CraftCost", this), resource1amount, resource2amount));
                        break;
                }

            }
        }

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

            baginsta = Convert.ToSingle(GetConfig("Blood bags", "Instant health to recover.", 50f));
            bagpending = Convert.ToSingle(GetConfig("Blood bags", "Pending health to recover", 50f));
            bagbleeding = Convert.ToBoolean(GetConfig("Blood bags", "Blood bags remove bleeding", false));
            Chatprefix = Convert.ToString(GetConfig("Chat prefix", "The chat prefix used for the plugin", "Blood Bags"));
            resource1 = Convert.ToInt32(GetConfig("Crafting", "First item used for crafting (Item IDs, default value is -2072273936 for bandages)", -2072273936));
            resource1amount = Convert.ToInt32(GetConfig("Crafting", "Amount of item 1 to use", 5));
            resource2 = Convert.ToInt32(GetConfig("Crafting", "Second item used for crafting (Item IDs, default value is 1079279582 for med syringes", 1079279582));
            craftingresource = (Dictionary<int, int>)GetConfig("Crafting", "Items to craft", new Dictionary<int, int> { { 2, 4}, { 5,6} });
            resource2amount = Convert.ToInt32(GetConfig("Crafting", "Amount of item 2 to use", 1));
            craftcommand = Convert.ToString(GetConfig("Crafting", "Craft command to use", "craftbag"));
            crafttime = Convert.ToSingle(GetConfig("Crafting", "Time to craft", 30f));
            canCraft = Convert.ToBoolean(GetConfig("Blood bags", "Can players use the craft command?", true));
            heallimit = Convert.ToSingle(GetConfig("Blood bags", "How many seconds until another bag can be used", 10f));
            limitCrafting = Convert.ToBoolean(GetConfig("Crafting", "Limit the crafting?", true));
            maxUses = Convert.ToInt32(GetConfig("Blood bags", "Max uses", 45));
            //dunnowhattocallthis = Convert.ToBoolean(GetConfig("Blood bags", "Limit uses on blood bags?", false));
            //imgonnaputthisinconsolebecausewhynot = Convert.ToString(GetConfig("Random ass organization shit", "This gets out put to the console", "what the fuck"));
            

            if (!Changed) return;
            SaveConfig();
            Changed = false;
        }

        protected override void LoadDefaultConfig()
        {
            Config.Clear();
            LoadVariables();
        }
        #endregion


        Dictionary<string, string> Messages = new Dictionary<string, string>()
        {
            { "Usage", "<color=#FF0000>{0}</color> You have just used a blood bag and have gained {1} health!" },
            {"AlreadyCrafting", "<color=#FF0000>{0}</color> You're already crafting a blood bag!" },
            {"Crafting", "Crafting the blood bags will take {0} seconds" },
            {"NoResources", "<color=#FF0000>{0}</color> You lack the required resources." },
            {"IncorrectFormat", "<color=#FF0000>{0}</color> Incorrect format! Use /{0} craft" },
            {"CraftCost", "To craft blood bags you will need {0} bandages and {1} med syringes" },
            {"HUsage", "<color=#FF0000>{0}</color> You have just used a blood bag and have gained {1} health as well as {2} max health for {3} seconds!" }
        };

    }
}