using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Drawing;

using Terraria_Server;
using Terraria_Server.Plugin;
using Terraria_Server.Collections;
using Terraria_Server.Commands;
using Terraria_Server.Events;
using Terraria_Server.Logging;

using System.Xml;

namespace Login
{
    public class Login : Plugin
    {
        public const int PLAYER_VALID = 0;
        public const int PLAYER_INVALID_LOGOFF = 1;
        public const int PLAYER_NO_INVENTORY_MATCH = 2;

        public const byte SOFTCORE = 0;
        public const byte MEDIUMCORE = 1;
        public const byte HARDCORE = 2;
        
        public const int LOBBY = 0;
        public const int VALIDATED = 1;

        public int coreMask;
        public int[] coreMaskArray = { 1, 2, 4 };
        public string[] coreStrings = { "softcore", "mediumcore", "hardcore" };

        public static Login plugin;
        public Properties properties;
        public String playersXMLFilename;
        public String xmlNamespace = "loginPlugin";
        public Terraria_Server.Misc.Color chatColor = new Terraria_Server.Misc.Color(100, 200, 100);
        public XmlDocument playersXML = new XmlDocument();
        public string pluginFolder, playersFolder;

        public bool kickIfHasInventory;
        public bool onlyAllowNewPlayers;
        public bool forceSoftcore;
        public bool forceMediumcore;
        public bool forceHardcore;
        public String inventoryMessage;
        public String differentInventoryMessage;
        public String validatedMessage;
        public bool banPiggyBanks;
        public bool banSafes;
        public String startingEquipment;
        public String lobbyPoint;
        public String validatedPoint;

        public Hashtable validPlayers = new Hashtable();

        public override void Load()
        {
            Name = "Login";
            Description = "A plugin to restrict logins in various ways";
            Author = "amarriner";
            Version = "0.2.1";
            TDSMBuild = 31;

            plugin = this;

            this.registerHook(Hooks.PLAYER_LOGIN);
            this.registerHook(Hooks.PLAYER_LOGOUT);
            this.registerHook(Hooks.PLAYER_TILECHANGE);
            this.registerHook(Hooks.PLAYER_PROJECTILE);
            this.registerHook(Hooks.PLAYER_PARTYCHANGE);
            this.registerHook(Hooks.PLAYER_FLOWLIQUID);
            this.registerHook(Hooks.PLAYER_EDITSIGN);
            this.registerHook(Hooks.PLAYER_CHEST);
            this.registerHook(Hooks.PLAYER_CHESTBREAK);

            AddCommand("invalidate")
                .WithAccessLevel(AccessLevel.OP)
                .WithDescription("Allows OPs to invalidate players manually")
                .WithHelpText("/invalidate <player>")
                .Calls(Commands.Commands.InvalidatePlayer);

            AddCommand("setpoint")
                .WithAccessLevel(AccessLevel.OP)
                .WithDescription("Allows OPs to set lobby or validated points")
                .WithHelpText("setpoint lobby|validated")
                .Calls(Commands.Commands.SetPoint);

            AddCommand("testpoint")
                .WithAccessLevel(AccessLevel.OP)
                .WithDescription("Allows OPs to test lobby or validated points")
                .WithHelpText("testpoint lobby|validated")
                .Calls(Commands.Commands.TestPoint);

            AddCommand("validate")
                .WithAccessLevel(AccessLevel.OP)
                .WithDescription("Allows OPs to validate players manually")
                .WithHelpText("/validate <player>")
                .Calls(Commands.Commands.ValidatePlayer);
        }

        public override void Enable()
        {
            pluginFolder = Statics.PluginPath + Path.DirectorySeparatorChar + "Login";
            playersFolder = pluginFolder + Path.DirectorySeparatorChar + "Players";
            CreateDirectory(pluginFolder);
            CreateDirectory(playersFolder);
            CreateLobby();

            properties = new Properties(pluginFolder + Path.DirectorySeparatorChar + "login.properties");
            properties.Load();
            kickIfHasInventory = properties.KickIfHasInventory;
            onlyAllowNewPlayers = properties.OnlyAllowNewPlayers;
            forceSoftcore = properties.ForceSoftcore;
            forceMediumcore = properties.ForceMediumcore;
            forceHardcore = properties.ForceHardcore;
            inventoryMessage = properties.InventoryMessage;
            differentInventoryMessage = properties.DifferentInventoryMessage;
            validatedMessage = properties.ValidatedMessage;
            banPiggyBanks = properties.BanPiggyBanks;
            banSafes = properties.BanSafes;
            startingEquipment = properties.StartingEquipment;
            lobbyPoint = properties.LobbyPoint;
            validatedPoint = properties.ValidatedPoint;
            properties.Save();

            BuildCoreMask();

            Program.tConsole.WriteLine(base.Name + " enabled.");
        }

        public void BuildCoreMask()
        {
            coreMask = 0;

            if (forceSoftcore)
                coreMask += 1;

            if (forceMediumcore)
                coreMask += 2;

            if (forceHardcore)
                coreMask += 4;
        }

        public void CreateLobby()
        {
            //if (Server.tile.At(0, 0).Data.Type == 0)
            //{
            //    for (int j = 1; j <= 11; j += 10)
            //    {
            //        for (int i = 0; i < 20; i++)
            //        {
            //            TileRef tileRef = Server.tile.CreateTileAt(i, j);
            //            bool check = Terraria_Server.WorldMod.WorldModify.PlaceTile(i, j, 1, false, true, -1, 1);
            //            Program.tConsole.WriteLine("Placed at " + i + "," + j + " (" + check.ToString() + ") REF: " + tileRef.Data.Type);
            //        }
            //    }
            //}
        }

        public override void Disable()
        {
            Program.tConsole.WriteLine(base.Name + " disabled.");
        }

        public override void  onPlayerJoin(PlayerLoginEvent Event)
        {
            if ((coreMask & coreMaskArray[Event.Player.Difficulty]) == 0)
                KickPlayer(Event.Player, "This server doesn't allow " + coreStrings[Event.Player.Difficulty] + " characters");

            else if (onlyAllowNewPlayers)
            {
                int error;
                if (File.Exists(playersFolder + Path.DirectorySeparatorChar + Event.Player.Name + ".xml"))
                {
                    error = LoadPlayerData(Event.Player);

                    switch (error)
                    {
                        case PLAYER_NO_INVENTORY_MATCH:
                            SetPlayerInvalid(Event.Player);
                            KickPlayer(Event.Player, differentInventoryMessage);
                            break;
                        case PLAYER_VALID:
                            SetPlayerValid(Event.Player);
                            break;
                    }
                }
                else
                {
                    if (!PlayerHasNullInventory(Event.Player))
                    {
                        if (kickIfHasInventory)
                            KickPlayer(Event.Player, inventoryMessage);
                        else
                        {
                            Event.Player.sendMessage(inventoryMessage, chatColor);
                            SetPlayerInvalid(Event.Player);
                        }
                    }
                    else
                    {
                        GivePlayerStartingEquipment(Server.GetPlayerByName(Event.Player.Name));
                        SetPlayerValid(Event.Player, true);
                    }
                }
            }

            // base.onPlayerJoin(Event);
        }

        public void KickPlayer(Player InPlayer, String message)
        {
            InPlayer.PluginData["kicked"] = true;
            InPlayer.Kick(message);
        }


        public void GivePlayerStartingEquipment(Player InPlayer)
        {
            string[] equipment = new string[100];
            string[] data = new string[2];

            ArgumentList giveArgs = new ArgumentList(Server);
            giveArgs.Add(InPlayer.Name);
            giveArgs.Add("0");
            giveArgs.Add("0");

            equipment = startingEquipment.Split(':');
            for (int i = 0; i < equipment.Length; i++)
            {
                data = equipment[i].ToString().Split('-');
                giveArgs[1] = data[1];
                giveArgs[2] = data[0];
                Terraria_Server.Commands.Commands.Give(Server, InPlayer, giveArgs);
            }

        }

        public override void onPlayerLogout(PlayerLogoutEvent Event)
        {
            if (Event.Player != null)
            {
                bool kicked = Event.Player.PluginData.ContainsKey("kicked") ? (bool)Event.Player.PluginData["kicked"] : false;
                bool playerIsValid = validPlayers.ContainsKey(Event.Sender.Name) ? (bool)validPlayers[Event.Sender.Name] : false;

                if (onlyAllowNewPlayers && !kicked && playerIsValid)
                {
                    SavePlayerData(Event.Player, true);
                    base.onPlayerLogout(Event);
                }
            }
        }

        public void TeleportPlayerToPoint(Player InPlayer, int PointType)
        {
            string[] point = new string[2];

            if (PointType == LOBBY)
            {
                if (lobbyPoint != "0,0")
                {
                    point = lobbyPoint.Split(',');
                    InPlayer.teleportTo(float.Parse(point[0]) * 16, float.Parse(point[1]) * 16);
                }
            }

            else if (PointType == VALIDATED)
            {
                if (validatedPoint != "0,0")
                {
                    point = validatedPoint.Split(',');
                    InPlayer.teleportTo(float.Parse(point[0]) * 16, float.Parse(point[1]) * 16);
                }
            }
        }

        public override void onPlayerTileChange(PlayerTileChangeEvent Event)
        {
            Player player = Server.GetPlayerByName(Event.Sender.Name);
            bool lobby = player.PluginData.ContainsKey("lobby") ? (bool)player.PluginData["lobby"] : false;
            bool validated = player.PluginData.ContainsKey("validated") ? (bool)player.PluginData["validated"] : false;

            Program.tConsole.WriteLine("Tile Type: " + Event.Tile.Type.ToString());

            if ((lobby || validated) && player.Op)
            {
                if (lobby)
                {
                    lobbyPoint = (int)Event.Position.X + "," + (int)Event.Position.Y;
                    properties.LobbyPoint = lobbyPoint;
                    properties.Save();
                    player.sendMessage("You have set the lobby point", chatColor);
                }
                else if (validated)
                {
                    validatedPoint = (int)Event.Position.X + "," + (int)Event.Position.Y;
                    properties.ValidatedPoint = validatedPoint;
                    properties.Save();
                    player.sendMessage("You have set the validated point", chatColor);
                }

                Event.Cancelled = true;
                player.PluginData["lobby"] = false;
                player.PluginData["validated"] = false;
            }
            else
            {
                if (Event.Tile.Type != 29 || !banPiggyBanks)
                    Event.Cancelled = CancelEvent(Event.Sender.Name);

                if (Event.Tile.Type == 97 && banSafes)
                {
                    player.sendMessage("Safes are banned on this server", chatColor);
                    Event.Cancelled = true;
                }

                base.onPlayerTileChange(Event);

                if (Event.Tile.Type == 29 && banPiggyBanks)
                {
                    for (int i = (int)Event.Position.X; i <= (int)Event.Position.X + 1; i++)
                    {
                        Terraria_Server.WorldMod.WorldModify.KillTile(i, (int)Event.Position.Y);
                        Terraria_Server.WorldMod.WorldModify.meteor(i, (int)Event.Position.Y);
                        NetMessage.SendTileSquare(Server.GetPlayerByName(Event.Sender.Name).whoAmi, i, (int)Event.Position.Y, 1);
                        player.sendMessage("Piggy banks are banned on this server", chatColor);
                    }
                }
            }
        }

        public override void onPlayerProjectileUse(PlayerProjectileEvent Event)
        {
            Event.Cancelled = CancelEvent(Event.Sender.Name);
            base.onPlayerProjectileUse(Event);
        }

        public override void onPlayerPartyChange(PartyChangeEvent Event)
        {
            Event.Cancelled = CancelEvent(Event.Sender.Name);
            base.onPlayerPartyChange(Event);
        }

        public override void onPlayerFlowLiquid(PlayerFlowLiquidEvent Event)
        {
            Event.Cancelled = CancelEvent(Event.Sender.Name);
            base.onPlayerFlowLiquid(Event);
        }

        public override void onPlayerEditSign(PlayerEditSignEvent Event)
        {
            Event.Cancelled = CancelEvent(Event.Sender.Name);
            base.onPlayerEditSign(Event);
        }

        public override void onPlayerChestBreak(PlayerChestBreakEvent Event)
        {
            Event.Cancelled = CancelEvent(Event.Sender.Name);
            base.onPlayerChestBreak(Event);
        }

        public override void onPlayerOpenChest(PlayerChestOpenEvent Event)
        {
            TileRef tileRef = Server.tile.At((int)Server.chest[Event.ID].x, (int)Server.chest[Event.ID].y);
            Program.tConsole.WriteLine("Chest Type: " + tileRef.Data.Type.ToString());
            Event.Cancelled = CancelEvent(Event.Sender.Name);
            base.onPlayerOpenChest(Event);
        }

        public bool CancelEvent(string PlayerName)
        {
            bool playerIsValid = validPlayers.ContainsKey(PlayerName) ? (bool)validPlayers[PlayerName] : false;
            if (!playerIsValid)
            {
                Server.GetPlayerByName(PlayerName).sendMessage("You cannot change the world until you're validated", chatColor);
                return true;
            }     

            return false;
        }

        public bool PlayerHasNullInventory(Player InPlayer)
        {
            for (int i = 0; i < InPlayer.inventory.Length; i++)
            {
                if (InPlayer.inventory[i].Stack > 0)
                    return false;
            }

            for (int i = 0; i < InPlayer.armor.Length; i++)
            {
                if (InPlayer.armor[i].Stack > 0)
                    return false;
            }

            return true;
        }

        private bool IsPlayerValid(Player InPlayer)
        {
            return validPlayers.ContainsKey(InPlayer.Name) ? (bool)validPlayers[InPlayer.Name] : false;
        }

        public void SetPlayerValid(Player InPlayer, bool Teleport = false)
        {
            validPlayers[InPlayer.Name] = true;

            if (Teleport)
                TeleportPlayerToPoint(InPlayer, VALIDATED);

            SavePlayerData(InPlayer, false);
            InPlayer.sendMessage(validatedMessage, chatColor);
        }

        public void SetPlayerInvalid(Player InPlayer)
        {
            validPlayers[InPlayer.Name] = false;
            TeleportPlayerToPoint(InPlayer, LOBBY);
        }

        private int LoadPlayerData(Player InPlayer)
        {
            IEnumerator ienum, cIenum;
            XmlNodeList nodes;
            XmlNode node, child;
            bool loggedOff = false, validInventory = false;

            playersXML.Load(playersFolder + Path.DirectorySeparatorChar + InPlayer.Name + ".xml");

            nodes = playersXML.GetElementsByTagName("player");
            ienum = nodes.GetEnumerator();
            while (ienum.MoveNext())
            {
                node = (XmlNode)ienum.Current;

                cIenum = node.ChildNodes.GetEnumerator();
                while (cIenum.MoveNext())
                {
                    child = (XmlNode)cIenum.Current;

                    switch (child.Name.ToUpper())
                    {
                        case "LOGGEDOFF":
                            loggedOff = Boolean.Parse(child.InnerXml);
                            break;

                        case "ITEMS":
                            validInventory = (StringifyInventory(InPlayer) == child.InnerXml);
                            break;
                    }
                }
            }

            if (validInventory)
                return PLAYER_VALID;

            if (!loggedOff)
                return PLAYER_INVALID_LOGOFF;

            if (!validInventory)
                return PLAYER_NO_INVENTORY_MATCH;

            return PLAYER_VALID;
        }

        private void SavePlayerData(Player InPlayer, bool LoggingOff)
        {
            XmlNode dec, node, items, data;
            XmlAttribute attr;

            playersXMLFilename = playersFolder + Path.DirectorySeparatorChar + InPlayer.Name + ".xml";

            playersXML = new XmlDocument();
            dec = playersXML.CreateNode(XmlNodeType.XmlDeclaration, "xml", xmlNamespace);
            playersXML.AppendChild(dec);

            node = playersXML.CreateNode(XmlNodeType.Element, "player", xmlNamespace);
            attr = playersXML.CreateAttribute("id");
            attr.Value = InPlayer.Name;
            node.Attributes.Append(attr);

            data = playersXML.CreateNode(XmlNodeType.Element, "loggedoff", xmlNamespace);
            data.InnerXml = LoggingOff.ToString();
            node.AppendChild(data);

            items = playersXML.CreateNode(XmlNodeType.Element, "items", xmlNamespace);
            items.InnerXml = StringifyInventory(InPlayer);

            node.AppendChild(items);
            playersXML.AppendChild(node);
            playersXML.Save(playersXMLFilename);
        }

        private string StringifyInventory(Player InPlayer)
        {
            string returnValue = "";
            string inventory = "";

            for (int i = 0; i < InPlayer.inventory.Length; i++)
            {
                if (i > 0)
                    inventory += "-";

                inventory += InPlayer.inventory[i].Type.ToString() + ":" + InPlayer.inventory[i].Stack.ToString();
            }

            returnValue = inventory;

            string ammo = StringifyAmmo(InPlayer);
            if (returnValue != "" && ammo != "")
                returnValue += "-";

            returnValue += ammo;

            string bank = StringifyBank(InPlayer);
            if (returnValue != "" && bank != "")
                returnValue += "-";

            returnValue += bank;

            string safe = StringifySafe(InPlayer);
            if (returnValue != "" && safe != "")
                returnValue += "-";

            returnValue += safe;

            string armor = StringifyArmor(InPlayer);
            if (returnValue != "" && armor != "")
                returnValue += "-";

            returnValue += armor;

            return returnValue;
        }

        private string StringifyAmmo(Player InPlayer)
        {
            string ammo = "";

            for (int i = 0; i < InPlayer.ammo.Length; i++)
            {
                if (i > 0)
                    ammo += "-";

                ammo += InPlayer.ammo[i].Type.ToString() + ":" + InPlayer.ammo[i].Stack.ToString();
            }

            return ammo;
        }

        private string StringifyArmor(Player InPlayer)
        {
            string armor = "";

            for (int i = 0; i < InPlayer.armor.Length; i++)
            {
                if (i > 0)
                    armor += "-";

                armor += InPlayer.armor[i].Type.ToString() + ":" + InPlayer.armor[i].Stack.ToString();
            }

            return armor;
        }

        private string StringifyBank(Player InPlayer)
        {
            string bank = "";

            for (int i = 0; i < InPlayer.bank.Length; i++)
            {
                if (i > 0)
                    bank += "-";

                bank += InPlayer.bank[i].Type.ToString() + ":" + InPlayer.bank[i].Stack.ToString();
            }

            return bank;
        }

        private string StringifySafe(Player InPlayer)
        {
            string safe = "";

            for (int i = 0; i < InPlayer.bank2.Length; i++)
            {
                if (i > 0)
                    safe += "-";

                safe += InPlayer.bank2[i].Type.ToString() + ":" + InPlayer.bank2[i].Stack.ToString();
            }

            return safe;
        }

        private static void CreateDirectory(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }
    }
}
