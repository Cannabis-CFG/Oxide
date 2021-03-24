using System;
using System.Collections.Generic;
using Facepunch.Extend;
using Newtonsoft.Json;

namespace Oxide.Plugins
{
    [Info("Better Healing", "Default", "2.0.0")]
    [Description("Buffs healing items.")]
    public class BetterHealing : RustPlugin
    {
        #region variables
        public bool Changed = true; // If this is set to true, will wipe the config with new values. Only use when adding or deleting things from the config
        // Healing items
        private float syringeHealAmount = 35f;
        private float syringePendingAmount = 25f;
        private float syringeRadRemoveAmount = 25f;
        private bool syringeBleedCancel = true;
        private float bandageHealAmount = 10f;
        private float bandagePendingAmount = 5f;
        private float medkitHealAmount = 15f;



        //IGNORE THE REST OF THE VARIABLES

        // Food shit
        private bool handleFood = true;
        // Human meat
        private float cookedhuman = 10f;
        private float rawhuman = 3f;
        private float burnthuman = 5f;
        private float cookedhumanpending = 5f;
        private float rawhumanpending = 1f;
        private float burnthumanpending = 2.5f;
        // Horse meat
        private float cookedhorse = 10f;
        private float rawhorse = 3f;
        private float burnthorse = 5f;
        private float cookedhorsepending = 5f;
        private float rawhorsepending = 1f;
        private float burnthorsepending = 2.5f;

        // Bear meat
        
        // Cans of shit

        // Deer meat

        // Fish shit

        // Plants

        // Chicken meat

        // Pork

        // Wolf meat
        //private float medkitPendingAmount = 35f;
        #endregion

        // Declares the permission to use this plugin
        private static string permissionName = "bettersyringe.use";

        void Init()
        {
            LoadVariables(); // Loads the config and initializes the variables
            permission.RegisterPermission(permissionName, this); // Registers the permission for use on the server

        }

        object OnHealingItemUse(MedicalTool tool, BasePlayer player)
        {
            if (!permission.UserHasPermission(player.UserIDString, permissionName)) {return null;} // Checks if the player using a healing item has permission


            switch (tool.GetItem()?.info.shortname) // Switch out the tool that the player is using
            {
                case "syringe.medical": // If the tool the player is using matches this shrotname, then it will continue below
                    player.health += syringeHealAmount; // Adds the new healing amount to the player
                    player.metabolism.pending_health.value += syringePendingAmount; // Adds pending health to the player
                    player.metabolism.radiation_poison.value -= syringeRadRemoveAmount; // Removes a set amount of radiation from the player
                    if (syringeBleedCancel) // Checks if this is set true in the config
                    {
                        player.metabolism.bleeding.value = 0f; // Sets bleeding to 0 if true in the config 
                    }
                    break;

                case "bandage": // Same as above
                    player.health += bandageHealAmount;
                    player.metabolism.pending_health.value += bandagePendingAmount;
                    player.metabolism.bleeding.value = 0f;
                    break;
            };

            #region unused
            // This below is useless, don't bother reading it, it is placed elsewhere
            /*if (tool.GetItem()?.info.shortname.Contains("syringe") == true) // Checks that the tool is a syringe
            {
                player.health += syringeHealAmount; // Adds the new healing amount to the player
                player.metabolism.pending_health.value += syringePendingAmount; // Adds pending health to the player
                player.metabolism.radiation_poison.value -= syringeRadRemoveAmount; // Removes a set amount of radiation from the player
                if (syringeBleedCancel) // Checks if this is set true in the config
                {
                    player.metabolism.bleeding.value = 0f; // Sets bleeding to 0 if true in the config 
                }

            }

            else if (tool.GetItem()?.info.shortname.Contains("bandage") == true) // Additional check to see if it's a bandage instead
            {

                player.health += bandageHealAmount;
                player.metabolism.pending_health.value += bandagePendingAmount;
                player.metabolism.bleeding.value = 0f;

            }*/
            /*else if (tool.GetItem()?.info.shortname.Contains("medkit") == true)
            {
                player.metabolism.pending_health.value = medkitPendingAmount;
            }*/
            #endregion
            return true; // Returns true if there's a healing item that the plugin doesn't handle (should be all of them)
        }

        object OnItemAction(Item item, string action, BasePlayer player)
        {
            if (!permission.UserHasPermission(player.UserIDString, permissionName)) // Permission check
            {
                return null;
            }

            if (item.info.shortname == "largemedkit" && action == "USE") // Checks if the player has made an action with out medkit, and checks if the action is USE (prevents players from clicking drop to recieve healing
            {
                player.health += medkitHealAmount; // Adds the instant health to the player
                player.metabolism.bleeding.value = 0f; // Removes bleeding effect from the player
                return null;
            }

            else // Don't bother with this, I never got around to finishing this bit
            {
                if (handleFood)
                {
                    switch (item.info.shortname)
                    {
                        case "humanmeat.cooked":
                            player.health += cookedhuman;
                            player.metabolism.pending_health.value += cookedhumanpending;
                            break;
                        case "horsemeat.cooked":

                            break;

                        case "horsemeat.burned":

                            break;

                        case "horsemeat.raw":

                            break;

                        case "humanmeat.burned":

                            break;

                        case "humanmeat.raw":

                            break;

                        case "granolabar":

                            break;

                        case "fish.cooked":

                            break;

                        case "fish.raw":

                            break;

                        case "fish.troutsmall":

                            break;

                        case "fish.minnows":

                            break;

                        case "deermeat.raw":

                            break;

                        case "deermeat.cooked":

                            break;

                        case "deermeat.burned":

                            break;

                        case "corn":

                            break;

                        case "chicken.raw":

                            break;

                        case "chicken.cooked":

                            break;

                        case "chicken.burned":

                            break;

                        case "can.tuna":

                            break;

                        case "can.beans":

                            break;

                        case "cactusflesh":

                            break;

                        case "blueberries":

                            break;

                        case "black.raspberries":

                            break;

                        case "bearmeat.cooked":

                            break;

                        case "bearmeat.burned":

                            break;

                        case "bearmeat":

                            break;

                        case "apple":

                            break;

                        case "jar.pickle":

                            break;

                        case "meat.boar":

                            break;

                        case "meat.pork.burned":

                            break;

                        case "meat.pork.cooked":

                            break;

                        case "mushroom":

                            break;

                        case "potato":

                            break;

                        case "pumpkin":

                            break;

                        case "wolfmeat.burned":

                            break;

                        case "wolfmeat.cooked":

                            break;

                        case "wolfmeat.raw":

                            break;

                        default:
                            break;
                    }

                    return null;
                }

                return null;
            }
        }


            object GetConfig(string menu, string datavalue, object defaultValue) // Handles the entire config
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
            // These are all the variables used in the plugin. Make sure they're created above before placing them here.
            syringeHealAmount = Convert.ToSingle(GetConfig("Syringes", "Amount to heal (on use)", 5f));
            syringePendingAmount = Convert.ToSingle(GetConfig("Syringes", "Pending health to add", 15f));
            syringeRadRemoveAmount = Convert.ToSingle(GetConfig("Syringes", "Amount of rads to remove on use", 25f));
            syringeBleedCancel = Convert.ToBoolean(GetConfig("Syringes", "Should syringes remove bleeding effect?", true));
            bandageHealAmount = Convert.ToSingle(GetConfig("Bandages", "Amount to heal (on use)", 30f));
            bandagePendingAmount = Convert.ToSingle(GetConfig("Bandages", "Pending health to add", 60f));
            medkitHealAmount = Convert.ToSingle(GetConfig("Medkits", "Amount to heal (on use)", 15f));
            handleFood = Convert.ToBoolean(GetConfig("Food", "Handle food?", true));
            cookedhuman = Convert.ToSingle(GetConfig("Food", "Cooked human meat heal amount", 10f));
            cookedhumanpending = Convert.ToSingle(GetConfig("Food", "Cooked human meat pending health amount", 5f));
            //medkitPendingAmount = Convert.ToSingle(GetConfig("Medkits", "Pending health to add", 35f));

            if (!Changed) return;
            SaveConfig();
            Changed = false;
        }
        

        protected override void LoadDefaultConfig()
        {
            Config.Clear();
            LoadVariables();
        }


    }
}