using System.Collections.Generic;
using Newtonsoft.Json;

namespace Oxide.Plugins
{
    [Info("AdvanceCraft", "Default", "1.0.0")] // Not made by Default. I don't know the original creator, but I rewrote the plugin to work with workbenches and added more items.
    [Description("Allows the crafting of some uncraftable items")]
    internal class AdvanceCraft : RustPlugin
    {
        private readonly HashSet<ulong> _crafting = new HashSet<ulong>();

        [ChatCommand("craft")]
                private void CmdCraftUser(BasePlayer player, string command, string[] args)
                {
                    
                    if (args.Length == 0)
                    {
                        player.ChatMessage($"<b><color=#ff0000ff>Craft a variety of otherwise uncraftable items!</color></b>\n" +
                                           $"<b><color=#ce422b> <size=15>Use a valid item name to craft them:</size></color></b>" +
                                           $"\n<b>/craft lr300</b>" +
                                           $"\n/craft m249" +
                                           $"\n/craft m92" +
                                           $"\n/craft spas" +
                                           $"\n/craft l96" +
                                           $"\n/craft coffin" +
                                           $"\n/craft supply" +
                                           $"\nWith more items coming soon!");
                    }

                    if (_crafting.Contains(player.userID))
                    {
                        player.ChatMessage($"<color=#ff0000ff>You are already crafting something, please wait before crafting something else.</color>");
                      
                    }

                    else if (args.Length == 1)
                    {
                        switch (args[0].ToLower())
                        {

                            case "lr300":
                            {
                                int hqm = player.inventory.GetAmount(317398316); //317398316 metal.refined
                                int metal = player.inventory.GetAmount(69511070); //69511070 metal.fragments
                                int rifle = player.inventory.GetAmount(176787552); //176787552 riflebody
                                int spring = player.inventory.GetAmount(-1021495308); //-1021495308 metalspring

                                

                                if ((player.playerFlags & BasePlayer.PlayerFlags.Workbench3) != 0)
                                {
                                    if (hqm >= _configData.Cc.Lr300.Comp1 & metal >= _configData.Cc.Lr300.Comp2 & rifle >= _configData.Cc.Lr300.Comp3 & spring >= _configData.Cc.Lr300.Comp4)
                                    {
                                        player.inventory.Take(null, 317398316, _configData.Cc.Lr300.Comp1);
                                        player.inventory.Take(null, 69511070, _configData.Cc.Lr300.Comp2);
                                        player.inventory.Take(null, 176787552, _configData.Cc.Lr300.Comp3);
                                        player.inventory.Take(null, -1021495308, _configData.Cc.Lr300.Comp4);
                                    }
                                    else
                                    {
                                        player.ChatMessage($"<b><color=#ff0000ff>Not enough resources!</color></b>\n<b><color=#ce422b> <size=15>You need these to craft:</size></color></b>\n<b>High qual:<color=#ce422b>{_configData.Cc.Lr300.Comp1}</color>. </b>\nMetal frags <color=#ce422b>{_configData.Cc.Lr300.Comp2}</color>.\n<b>Rifle bodies <color=#ce422b>{_configData.Cc.Lr300.Comp3}</color>.</b>\n<b>Springs <color=#ce422b>{_configData.Cc.Lr300.Comp4}</color>.</b>");
                                        return;
                                    }

                                    _crafting.Add(player.userID);
                                    player.ChatMessage("Crafting an LR will take 180 seconds. Please wait.");
                                    timer.Once(180f, () =>
                                    {
                                        player.inventory.GiveItem(ItemManager.CreateByItemID(-1812555177));
                                        player.Command("note.inv", -1812555177, 1);
                                        player.ChatMessage($"<b>Sir yes sir. You done crafted an LR.</b>");
                                        _crafting.Remove(player.userID);
                                    });

                                    return;

                                }

                                player.ChatMessage("You require workbench 3 to craft this item.");
                                return;

                            }
                            case "m249":
                            {
                                int hqm = player.inventory.GetAmount(317398316);
                                int metal = player.inventory.GetAmount(69511070);
                                int rifle = player.inventory.GetAmount(176787552);
                                int spring = player.inventory.GetAmount(-1021495308);

                                if ((player.playerFlags & BasePlayer.PlayerFlags.Workbench3) != 0)
                                {
                                    if (hqm >= _configData.Cc.M249.Comp1 & metal >= _configData.Cc.M249.Comp2 & rifle >= _configData.Cc.M249.Comp3 & spring >= _configData.Cc.M249.Comp4)
                                    {
                                        player.inventory.Take(null, 317398316, _configData.Cc.M249.Comp1);
                                        player.inventory.Take(null, 69511070, _configData.Cc.M249.Comp2);
                                        player.inventory.Take(null, 176787552, _configData.Cc.M249.Comp3);
                                        player.inventory.Take(null, -1021495308, _configData.Cc.M249.Comp4);
                                    }
                                    else
                                    {
                                        player.ChatMessage($"<b><color=#ff0000ff>Not enough resources!</color></b>\n<b><color=#ce422b> <size=15>What you need to craft:</size></color></b>\n<b>High qual <color=#ce422b>{_configData.Cc.M249.Comp1}</color>.</b>\nMetal frags <color=#ce422b>{_configData.Cc.M249.Comp2}</color>.\n<b>Rifle bodies <color=#ce422b>{_configData.Cc.M249.Comp3}</color>.</b>\n<b>Springs <color=#ce422b>{_configData.Cc.M249.Comp4}</color>.</b>");
                                        return;
                                    }

                                    player.ChatMessage("Crafting an M249 will take 210 seconds. Please wait.");
                                    _crafting.Add(player.userID);
                                    timer.Once(210f, () =>
                                    {
                                        player.inventory.GiveItem(ItemManager.CreateByItemID(-2069578888));
                                        player.Command("note.inv", -2069578888, 1);
                                        player.ChatMessage($"<b>Uh oh, we got a badass here. Got themselves an M249.</b>");
                                        _crafting.Remove(player.userID);
                                    });
                                    return;
                                }

                                player.ChatMessage("You require workbench 3 to craft this item.");
                                return;
                            }
                            case "m92":
                            {
                                int hqm = player.inventory.GetAmount(317398316);
                                int pipe = player.inventory.GetAmount(95950017);
                                int semi = player.inventory.GetAmount(573926264);
                                int spring = player.inventory.GetAmount(-1021495308);

                                if ((player.playerFlags & BasePlayer.PlayerFlags.Workbench2) != 0)
                                {

                                    if (hqm >= _configData.Cc.M92.Comp1 & pipe >= _configData.Cc.M92.Comp2 &
                                        semi >= _configData.Cc.M92.Comp3 & spring >= _configData.Cc.M92.Comp4)
                                    {
                                        player.inventory.Take(null, 317398316, _configData.Cc.M92.Comp1);
                                        player.inventory.Take(null, 95950017, _configData.Cc.M92.Comp2);
                                        player.inventory.Take(null, 573926264, _configData.Cc.M92.Comp3);
                                        player.inventory.Take(null, -1021495308, _configData.Cc.M92.Comp4);
                                    }
                                    else
                                    {
                                        player.ChatMessage($"<b><color=#ff0000ff>Not enough resources!</color></b>\n<b><color=#ce422b> <size=15>What you need to craft:</size></color></b>\n<b>High qual <color=#ce422b>{_configData.Cc.M92.Comp1}</color>.</b>\nPipes <color=#ce422b>{_configData.Cc.M92.Comp2}</color>.\n<b>Semi bodies <color=#ce422b>{_configData.Cc.M92.Comp3}</color>.</b>\n<b>Springs <color=#ce422b>{_configData.Cc.M92.Comp4}</color>.</b>");
                                        return;
                                    }

                                    player.ChatMessage("Crafting an M92 will take 60 seconds. Please wait.");
                                    _crafting.Add(player.userID);
                                    timer.Once(60f, () =>
                                    {
                                        player.inventory.GiveItem(ItemManager.CreateByItemID(-852563019));
                                        player.Command("note.inv", -852563019, 1);
                                        player.ChatMessage($"<b>Done got yourself an M92. All acting like a scientist now?</b>");
                                        _crafting.Remove(player.userID);
                                    });
                                    return;

                                }

                                if ((player.playerFlags & BasePlayer.PlayerFlags.Workbench3) != 0)
                                {
                                    if (hqm >= _configData.Cc.M92.Comp1 & pipe >= _configData.Cc.M92.Comp2 & semi >= _configData.Cc.M92.Comp3 & spring >= _configData.Cc.M92.Comp4)
                                    {
                                        player.inventory.Take(null, 317398316, _configData.Cc.M92.Comp1);
                                        player.inventory.Take(null, 95950017, _configData.Cc.M92.Comp2);
                                        player.inventory.Take(null, 573926264, _configData.Cc.M92.Comp3);
                                        player.inventory.Take(null, -1021495308, _configData.Cc.M92.Comp4);
                                    }
                                    else
                                    {
                                        player.ChatMessage($"<b><color=#ff0000ff>Not enough resources!</color></b>\n<b><color=#ce422b> <size=15>What you need to craft:</size></color></b>\n<b>High qual <color=#ce422b>{_configData.Cc.M92.Comp1}</color>.</b>\nPipes <color=#ce422b>{_configData.Cc.M92.Comp2}</color>.\n<b>Semi bodies <color=#ce422b>{_configData.Cc.M92.Comp3}</color>.</b>\n<b>Springs <color=#ce422b>{_configData.Cc.M92.Comp4}</color>.</b>");
                                        return;
                                    }

                                    player.ChatMessage("Crafting an M92 will take 45 seconds. Please wait.");
                                    _crafting.Add(player.userID);
                                    timer.Once(45f, () =>
                                    {
                                        player.inventory.GiveItem(ItemManager.CreateByItemID(-852563019));
                                        player.Command("note.inv", -852563019, 1);
                                        player.ChatMessage($"<b>Done got yourself an M92. All acting like a scientist now?</b>");
                                        _crafting.Remove(player.userID);
                                    });
                                    return;
                                }

                                player.ChatMessage("You require workbench 2 to craft this item.");

                                return;
                            }
                            case "spas":
                            {
                                int hqm = player.inventory.GetAmount(317398316);
                                int pipe = player.inventory.GetAmount(95950017);
                                int metal = player.inventory.GetAmount(69511070);
                                int spring = player.inventory.GetAmount(-1021495308);

                                if ((player.playerFlags & BasePlayer.PlayerFlags.Workbench3) != 0)
                                {

                                    if (hqm >= _configData.Cc.Spas.Comp1 & pipe >= _configData.Cc.Spas.Comp2 & metal >= _configData.Cc.Spas.Comp3 & spring >= _configData.Cc.Spas.Comp4)
                                    {
                                        player.inventory.Take(null, 317398316, _configData.Cc.Spas.Comp1);
                                        player.inventory.Take(null, 95950017, _configData.Cc.Spas.Comp2);
                                        player.inventory.Take(null, 69511070, _configData.Cc.Spas.Comp3);
                                        player.inventory.Take(null, -1021495308, _configData.Cc.Spas.Comp4);
                                    }
                                    else
                                    {
                                        player.ChatMessage($"<b><color=#ff0000ff>Not enough resources!</color></b>\n<b><color=#ce422b> <size=15>What you need to craft:</size></color></b>\n<b>High qual <color=#ce422b>{_configData.Cc.Spas.Comp1}</color>.</b>\npipes <color=#ce422b>{_configData.Cc.Spas.Comp2}</color>.\n<b>Metal frags <color=#ce422b>{_configData.Cc.Spas.Comp3}</color>.</b>\n<b>Springs <color=#ce422b>{_configData.Cc.Spas.Comp4}</color>.</b>");
                                        return;
                                    }

                                    player.ChatMessage("Crafting the Spas 12 will take 120 seconds. Please wait.");
                                    _crafting.Add(player.userID);
                                    timer.Once(120f, () =>
                                    {
                                        player.inventory.GiveItem(ItemManager.CreateByItemID(-41440462));
                                        player.Command("note.inv", -41440462, 1);
                                        player.ChatMessage($"<b>Noice, ya made a spas. Happy killings!");
                                        _crafting.Remove(player.userID);
                                    });
                                    return;

                                }

                                player.ChatMessage("You require workbench 3 to craft this item.");
                                return;
                            }
                            case "l96":
                            {
                                int hqm = player.inventory.GetAmount(317398316);
                                int metal = player.inventory.GetAmount(69511070);
                                int rifle = player.inventory.GetAmount(176787552);
                                int spring = player.inventory.GetAmount(-1021495308);

                                if ((player.playerFlags & BasePlayer.PlayerFlags.Workbench3) != 0)
                                {
                                    if (hqm >= _configData.Cc.L96.Comp1 & rifle >= _configData.Cc.L96.Comp2 & metal >= _configData.Cc.L96.Comp3 & spring >= _configData.Cc.L96.Comp4)
                                    {
                                        player.inventory.Take(null, 317398316, _configData.Cc.L96.Comp1);
                                        player.inventory.Take(null, 69511070, _configData.Cc.L96.Comp3);
                                        player.inventory.Take(null, 176787552, _configData.Cc.L96.Comp2);
                                        player.inventory.Take(null, -1021495308, _configData.Cc.L96.Comp4);
                                    }
                                    else
                                    {
                                        player.ChatMessage($"<b><color=#ff0000ff>Not enough resources!</color></b>\n<b><color=#ce422b> <size=15>What you need to craft:</size></color></b>\n<b>High qual <color=#ce422b>{_configData.Cc.L96.Comp1}</color>.</b>\nMetal frags <color=#ce422b>{_configData.Cc.L96.Comp3}</color>.\n<b>Rifle bodies <color=#ce422b>{_configData.Cc.L96.Comp2}</color>.</b>\n<b>Springs <color=#ce422b>{_configData.Cc.L96.Comp4}</color>.</b>");
                                        return;
                                    }
                                    _crafting.Add(player.userID);
                                    player.ChatMessage("Crafting an L96 will take 180 seconds. Please wait.");
                                    timer.Once(180f, () =>
                                    {
                                        player.inventory.GiveItem(ItemManager.CreateByItemID(-778367295));
                                        player.Command("note.inv", -778367295, 1);
                                        player.ChatMessage("You just made a L96, nobody is gonna know what hit them.");
                                        _crafting.Remove(player.userID);

                                    });

                                    return;

                                }

                                player.ChatMessage("You require workbench 3 to craft this item.");
                                return;
                            }
                            case "supply":
                            {
                                // Getting item id for crafting
                                int flare = player.inventory.GetAmount(304481038);
                                int smoke = player.inventory.GetAmount(1263920163);
                                int powder = player.inventory.GetAmount(-265876753);

                                // Workbench requirement
                                if ((player.playerFlags & BasePlayer.PlayerFlags.Workbench3) == 0)
                                {
                                    // Let the player know the workbench requirement
                                    player.ChatMessage($"You require workbench 3 to craft this item.");
                                    return;
                                }
                                // Check if the player has enough resources
                                if (flare >= _configData.Cc.Supply.Comp1 & smoke >= _configData.Cc.Supply.Comp2 & powder >= _configData.Cc.Supply.Comp3)
                                {
                                    // Take the resources if player has enough
                                    player.inventory.Take(null, 1263920163, _configData.Cc.Supply.Comp2);
                                    player.inventory.Take(null, 304481038, _configData.Cc.Supply.Comp1);
                                    player.inventory.Take(null, -265876753, _configData.Cc.Supply.Comp3);
                                }
                                else
                                {
                                    // Tell the player they don't have enough resources
                                    player.ChatMessage($"<b><color=#ff0000ff>Not enough resources!</color></b>" +
                                                       $"\n<b><color=#ce422b> <size=15>What you need to craft:</size></color></b>" +
                                                       $"\n<b>Flares <color=#ce422b>{_configData.Cc.Supply.Comp1}</color>.</b>" +
                                                       $"\n<b>Smoke grenades <color=#ce422b>{_configData.Cc.Supply.Comp2}</color>.</b>" +
                                                       $"\n<b>Gunpowder <color=#ce422b>{_configData.Cc.Supply.Comp3}</color></b>");
                                    return;
                                }
                                // Craft the supply drop
                                player.ChatMessage($"Crafting a supply drop will take 30 seconds. Please wait.");
                                _crafting.Add(player.userID);
                                timer.Once(30f, () =>
                                {
                                    // Giving the supply drop to the player
                                    player.inventory.GiveItem(ItemManager.CreateByItemID(1397052267));
                                    player.Command($"note.inv", 1397052267, 1);
                                    player.ChatMessage($"Time for that air support. You just made a supply signal.");
                                    _crafting.Remove(player.userID);
                                });
                                return;
                            }
                            case "coffin":
                            {
                                // Getting the id for crafting
                                int wood = player.inventory.GetAmount(-151838493);
                                int frags = player.inventory.GetAmount(69511070);

                                // Workbench requirement
                                if ((player.playerFlags & BasePlayer.PlayerFlags.Workbench1) != 0)
                                {
                                    
                                    // Check if the player has enough resources
                                    if (wood >= _configData.Cc.Coffin.Comp1 & frags >= _configData.Cc.Coffin.Comp2)
                                    {
                                        player.inventory.Take(null, -151838493, _configData.Cc.Coffin.Comp1);
                                        player.inventory.Take(null, 69511070, _configData.Cc.Coffin.Comp2);
                                    }
                                    else
                                    {
                                        player.ChatMessage($"<b><color=#ff0000ff>Not enough resources!</color></b>" +
                                                           $"\n<b><color=#ce422b> <size=15>What you need to craft:</size></color></b>" +
                                                           $"\n<b>Wood <color=#ce422b>{_configData.Cc.Coffin.Comp1}</color>.</b>" +
                                                           $"\n<b>Metal frags <color=#ce422b>{_configData.Cc.Coffin.Comp2}</color>.</b>");
                                        return;
                                    }
                                    // Crafting
                                    player.ChatMessage($"Crafting a coffin will take 30 seconds.");
                                _crafting.Add(player.userID);
                                timer.Once(30f, () =>
                                    {
                                        player.inventory.GiveItem(ItemManager.CreateByItemID(573676040));
                                        player.Command("note.inv", 573676040, 1);
                                        player.ChatMessage($"It's either you or your items getting stored. You just made a coffin.");
                                        _crafting.Remove(player.userID);

                                    });
                                    return;
                                }

                                if ((player.playerFlags & BasePlayer.PlayerFlags.Workbench2) != 0)
                                {
                                // Check if the player has enough resources
                                    if (wood >= _configData.Cc.Coffin.Comp1 & frags >= _configData.Cc.Coffin.Comp2)
                                    {
                                        player.inventory.Take(null, -151838493, _configData.Cc.Coffin.Comp1);
                                        player.inventory.Take(null, 69511070, _configData.Cc.Coffin.Comp2);
                                    }
                                    else
                                    {
                                        player.ChatMessage($"<b><color=#ff0000ff>Not enough resources!</color></b>" +
                                                           $"\n<b><color=#ce422b> <size=15>What you need to craft:</size></color></b>" +
                                                           $"\n<b>Wood <color=#ce422b>{_configData.Cc.Coffin.Comp1}</color>.</b>" +
                                                           $"\n<b>Metal frags <color=#ce422b>{_configData.Cc.Coffin.Comp2}</color>.</b>");
                                        return;
                                    }
                                    // Crafting
                                    player.ChatMessage($"Crafting a coffin will take 20 seconds.");
                                    _crafting.Add(player.userID);
                                    timer.Once(20f, () =>
                                    {
                                        player.inventory.GiveItem(ItemManager.CreateByItemID(573676040));
                                        player.Command("note.inv", 573676040, 1);
                                        player.ChatMessage($"It's either you or your items getting stored. You just made a coffin.");
                                        _crafting.Remove(player.userID);

                                    });
                                    return;
                                }
                                if ((player.playerFlags & BasePlayer.PlayerFlags.Workbench3) != 0)
                                {
                                    // Check if the player has enough resources
                                    if (wood >= _configData.Cc.Coffin.Comp1 & frags >= _configData.Cc.Coffin.Comp2)
                                    {
                                        player.inventory.Take(null, -151838493, _configData.Cc.Coffin.Comp1);
                                        player.inventory.Take(null, 69511070, _configData.Cc.Coffin.Comp2);
                                    }
                                    else
                                    {
                                        player.ChatMessage($"<b><color=#ff0000ff>Not enough resources!</color></b>" +
                                                           $"\n<b><color=#ce422b> <size=15>What you need to craft:</size></color></b>" +
                                                           $"\n<b>Wood <color=#ce422b>{_configData.Cc.Coffin.Comp1}</color>.</b>" +
                                                           $"\n<b>Metal frags <color=#ce422b>{_configData.Cc.Coffin.Comp2}</color>.</b>");
                                        return;
                                    }
                                    // Crafting
                                    player.ChatMessage($"Crafting a coffin will take 10 seconds.");
                                    _crafting.Add(player.userID);
                                    timer.Once(10f, () =>
                                    {
                                        player.inventory.GiveItem(ItemManager.CreateByItemID(573676040));
                                        player.Command("note.inv", 573676040, 1);
                                        player.ChatMessage($"It's either you or your items getting stored. You just made a coffin.");
                                        _crafting.Remove(player.userID);

                                    });
                                    return;
                                }
                                // Let player know workbench is required.
                                player.ChatMessage(lang.GetMessage("Tier1", this));
                                return;
                            }
                        }
                    }
                }


     
                private ConfigData _configData;

        #region lang
        Dictionary<string, string> Messages = new Dictionary<string, string>()
        {
            { "Tier1", "You require workbench 1 to craft this item!" },
            {"Tier2", "<color=#FF0000>{0}</color> You're already crafting a blood bag!" },
            {"Tier3", "Crafting the blood bags will take {0} seconds" },
            {"NoResources", "<color=#FF0000>{0}</color> You lack the required resources." },
            {"IncorrectFormat", "<color=#FF0000>{0}</color> Incorrect format! Use /{0} craft" },
            {"CraftCost", "To craft blood bags you will need {0} bandages and {1} med syringes" },
            {"HUsage", "<color=#FF0000>{0}</color> You have just used a blood bag and have gained {1} health as well as {2} max health for {3} seconds!" }
        };
        #endregion
        private class ConfigData
                {
                    [JsonProperty(PropertyName = "Components needed to craft")]
                    public CrafterComponents Cc { get; set; }

                    public class CrafterComponents
                    {
                        [JsonProperty(PropertyName = "LR-300 resources needed")]
                        public CClr300 Lr300 { get; set; }
                        [JsonProperty(PropertyName = "M249 resources needed")]
                        public CCm249 M249 { get; set; }
                        [JsonProperty(PropertyName = "M92 resources needed")]
                        public CCm92 M92 { get; set; }
                        [JsonProperty(PropertyName = "Spas 12 resources needed")]
                        public CCspas Spas { get; set; }
                        [JsonProperty(PropertyName = "L96 resources needed")]
                        public CCl96 L96 { get; set; }
                        [JsonProperty(PropertyName = "Supply Signal resources needed")] 
                        public CCsupply Supply { get; set; }
                        [JsonProperty(PropertyName = "Coffin crafting requirements")]
                        public CCcoffin Coffin { get; set; }
                /*[JsonProperty(PropertyName = "L96 resources needed")] TODO Reference for more
                public CCl96 l96 { get; set; }*/

                        public class CClr300
                        {
                            [JsonProperty(PropertyName = "High qual")]
                            public int Comp1 { get; set; }
                            [JsonProperty(PropertyName = "Metal frags")]
                            public int Comp2 { get; set; }
                            [JsonProperty(PropertyName = "Rifle bodies")]
                            public int Comp3 { get; set; }
                            [JsonProperty(PropertyName = "Springs")]
                            public int Comp4 { get; set; }
                        }
                        public class CCm249
                        {
                            [JsonProperty(PropertyName = "High qual")]
                            public int Comp1 { get; set; }
                            [JsonProperty(PropertyName = "Metal frags")]
                            public int Comp2 { get; set; }
                            [JsonProperty(PropertyName = "Rifle bodies")]
                            public int Comp3 { get; set; }
                            [JsonProperty(PropertyName = "Springs")]
                            public int Comp4 { get; set; }
                        }
                        public class CCm92
                        {
                            [JsonProperty(PropertyName = "High qual")]
                            public int Comp1 { get; set; }
                            [JsonProperty(PropertyName = "pipes")]
                            public int Comp2 { get; set; }
                            [JsonProperty(PropertyName = "semi bodies")]
                            public int Comp3 { get; set; }
                            [JsonProperty(PropertyName = "Springs")]
                            public int Comp4 { get; set; }
                        }
                        public class CCspas
                        {
                            [JsonProperty(PropertyName = "High qual")]
                            public int Comp1 { get; set; }
                            [JsonProperty(PropertyName = "pipes")]
                            public int Comp2 { get; set; }
                            [JsonProperty(PropertyName = "Metal frags")]
                            public int Comp3 { get; set; }
                            [JsonProperty(PropertyName = "Springs")]
                            public int Comp4 { get; set; }
                        }

                        public class CCl96
                        {
                            [JsonProperty(PropertyName = "High qual")]
                            public int Comp1 { get; set; }
                            [JsonProperty(PropertyName = "Rifle bodies")]
                            public int Comp2 { get; set; }
                            [JsonProperty(PropertyName = "Metal frags")]
                            public int Comp3 { get; set; }
                            [JsonProperty(PropertyName = "Springs")]
                            public int Comp4 { get; set; }
                        }

                        public class CCsupply
                        {
                            [JsonProperty(PropertyName = "Flares")]
                            public int Comp1 { get; set; }
                            [JsonProperty(PropertyName = "Smoke grenades")]
                            public int Comp2 { get; set; }
                            [JsonProperty(PropertyName = "Gunpowder")]
                            public int Comp3 { get; set; }
                        }

                        public class CCcoffin
                        {
                            [JsonProperty(PropertyName = "Wood")]
                            public int Comp1 { get; set; }
                            [JsonProperty(PropertyName = "Metal frags")]
                            public int Comp2 { get; set; }
                        }

                        /*public class CCl96 TODO Reference for more
                        {
                            [JsonProperty(PropertyName = "High qual")]
                            public int comp1 { get; set; }
                            [JsonProperty(PropertyName = "Rifle bodies")]
                            public int comp2 { get; set; }
                            [JsonProperty(PropertyName = "Metal frags")]
                            public int comp3 { get; set; }
                            [JsonProperty(PropertyName = "Springs")]
                            public int comp4 { get; set; }
                        }*/
                    }
                }

                protected override void LoadConfig()
                {
                    base.LoadConfig();
                    _configData = Config.ReadObject<ConfigData>();

                    Config.WriteObject(_configData, true);
                }

                protected override void LoadDefaultConfig()
                {
                    _configData = GetBaseConfig();
                }

                protected override void SaveConfig()
                {
                    Config.WriteObject(_configData, true);
                }

                private ConfigData GetBaseConfig()
                {
                    return new ConfigData
                    {
                        Cc = new ConfigData.CrafterComponents
                        {
                            Lr300 = new ConfigData.CrafterComponents.CClr300
                            {
                                Comp1 = 60,
                                Comp2 = 200,
                                Comp3 = 2,
                                Comp4 = 10
                            },
                            M249 = new ConfigData.CrafterComponents.CCm249
                            {
                                Comp1 = 120,
                                Comp2 = 400,
                                Comp3 = 4,
                                Comp4 = 20
                            },
                            M92 = new ConfigData.CrafterComponents.CCm92
                            {
                                Comp1 = 15,
                                Comp2 = 3,
                                Comp3 = 1,
                                Comp4 = 2
                            },
                            Spas = new ConfigData.CrafterComponents.CCspas
                            {
                                Comp1 = 15,
                                Comp2 = 3,
                                Comp3 = 200,
                                Comp4 = 1
                            },
                            L96 = new ConfigData.CrafterComponents.CCl96
                            {
                                Comp1 = 175,
                                Comp2 = 3,
                                Comp3 = 1500,
                                Comp4 = 5
                            },
                            Supply = new ConfigData.CrafterComponents.CCsupply
                            {
                                Comp1 = 10,
                                Comp2 = 3,
                                Comp3 = 1000
                            },
                            Coffin = new ConfigData.CrafterComponents.CCcoffin
                            {
                                Comp1 = 400,
                                Comp2 = 75
                            }
                            /*l96 = new ConfigData.CrafterComponents.CCl96 //TODO Reference for more
                            {
                            comp1 = 175,
                            comp2 = 3,
                            comp3 = 1500,
                            comp4 = 5
                            }*/
                        }
                    };
                }

    }
}
