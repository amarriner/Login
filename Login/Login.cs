using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

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

        public static Login plugin;
        public Properties properties;
        public String xmlFilename = "login.xml";
        public String xmlNamespace = "loginPlugin";
        public Terraria_Server.Misc.Color chatColor = new Terraria_Server.Misc.Color(100, 200, 100);
        public XmlDocument loginXML = new XmlDocument();
        public string pluginFolder, playersFolder;

        public bool kickIfHasInventory;
        public bool onlyAllowNewPlayers;
        public bool onlyAllowSoftcore;
        public bool onlyAllowMediumcore;
        public bool onlyAllowHardcore;

        public Hashtable validPlayers = new Hashtable();

        public override void Load()
        {
            Name = "Login";
            Description = "A plugin to restrict logins in various ways";
            Author = "amarriner";
            Version = "0.1";
            TDSMBuild = 31;

            plugin = this;

            this.registerHook(Hooks.PLAYER_LOGIN);
            this.registerHook(Hooks.PLAYER_LOGOUT);

            if (onlyAllowNewPlayers)
            {
                AddCommand("getequip")
                    .WithAccessLevel(AccessLevel.PLAYER)
                    .WithDescription("Retrieve starting equipment")
                    .WithHelpText("Usage: /getequip")
                    .Calls(Commands.Commands.GetEquip);
            }
        }

        public override void Enable()
        {
            pluginFolder = Statics.PluginPath + Path.DirectorySeparatorChar + "Login";
            playersFolder = pluginFolder + Path.DirectorySeparatorChar + "Players";
            CreateDirectory(pluginFolder);
            CreateDirectory(playersFolder);

            properties = new Properties(pluginFolder + Path.DirectorySeparatorChar + "login.properties");
            properties.Load();
            kickIfHasInventory = properties.KickIfHasInventory;
            onlyAllowNewPlayers = properties.OnlyAllowNewPlayers;
            onlyAllowSoftcore = properties.OnlyAllowSoftcore;
            onlyAllowMediumcore = properties.OnlyAllowMediumcore;
            onlyAllowHardcore = properties.OnlyAllowHardcore;
            properties.Save();

            Program.tConsole.WriteLine(base.Name + " enabled.");
        }

        public override void Disable()
        {
            Program.tConsole.WriteLine(base.Name + " disabled.");
        }

        public override void onPlayerJoin(PlayerLoginEvent Event)
        {
            if (onlyAllowSoftcore && Event.Player.Difficulty != SOFTCORE)
                Event.Player.Kick("Server only allows softcore players");

            else if (onlyAllowMediumcore && Event.Player.Difficulty != MEDIUMCORE)
                Event.Player.Kick("Server only allows mediumcore players");

            else if (onlyAllowHardcore && Event.Player.Difficulty != HARDCORE)
                Event.Player.Kick("Server only allows hardcore players");

            else if (onlyAllowNewPlayers)
            {
                int error;
                if (File.Exists(playersFolder + Path.DirectorySeparatorChar + Event.Player.Name + ".xml"))
                {
                    error = LoadPlayerData(Event.Player);

                    switch (error)
                    {
                        case PLAYER_INVALID_LOGOFF:
                            Event.Player.Kick("You didn't log off cleanly last time");
                            break;
                        case PLAYER_NO_INVENTORY_MATCH:
                            Event.Player.Kick("You have different inventory");
                            break;
                        case PLAYER_VALID:
                            Event.Player.sendMessage("You've been validated", chatColor);
                            SavePlayerData(Event.Player, false);
                            SetPlayerValid(Event.Player);
                            break;
                    }
                }
                else
                {
                    if (!PlayerHasNullInventory(Event.Player))
                    {
                        if (kickIfHasInventory)
                            Event.Player.Kick("You must have an empty inventory");
                        else
                            Event.Player.sendMessage("This server requires that your inventory is empty upon first login", chatColor);
                    }
                    else
                    {
                        Event.Player.sendMessage("Type /getequip to receive your starting equipment", chatColor);
                        SetPlayerValid(Event.Player);
                    }
                }
            }

            // base.onPlayerJoin(Event);
        }

        public override void onPlayerLogout(PlayerLogoutEvent Event)
        {
            if (onlyAllowNewPlayers)
            {
                SavePlayerData(Event.Player, true);
            }

            base.onPlayerLogout(Event);
        }

        public bool PlayerHasNullInventory(Player InPlayer)
        {
            for (int i = 0; i < InPlayer.inventory.Length; i++)
            {
                if (InPlayer.inventory[i].Stack > 0)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsPlayerValid(Player Player)
        {
            return validPlayers.ContainsKey(Player.Name) ? (bool)validPlayers[Player.Name] : false;
        }

        private void SetPlayerValid(Player Player)
        {
            validPlayers[Player.Name] = true;
        }

        private int LoadPlayerData(Player Player)
        {
            IEnumerator ienum, cIenum;
            XmlNodeList nodes;
            XmlNode node, child;
            bool loggedOff = false, validInventory = false;

            loginXML.Load(playersFolder + Path.DirectorySeparatorChar + Player.Name + ".xml");

            nodes = loginXML.GetElementsByTagName("player");
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
                            validInventory = (StringifyInventory(Player) == child.InnerXml);
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

        private void SavePlayerData(Player Player, bool LoggingOff)
        {
            XmlNode dec, node, items, data;
            XmlAttribute attr;

            xmlFilename = playersFolder + Path.DirectorySeparatorChar + Player.Name + ".xml";

            loginXML = new XmlDocument();
            dec = loginXML.CreateNode(XmlNodeType.XmlDeclaration, "xml", xmlNamespace);
            loginXML.AppendChild(dec);

            node = loginXML.CreateNode(XmlNodeType.Element, "player", xmlNamespace);
            attr = loginXML.CreateAttribute("id");
            attr.Value = Player.Name;
            node.Attributes.Append(attr);

            data = loginXML.CreateNode(XmlNodeType.Element, "loggedoff", xmlNamespace);
            data.InnerXml = LoggingOff.ToString();
            node.AppendChild(data);

            items = loginXML.CreateNode(XmlNodeType.Element, "items", xmlNamespace);
            items.InnerXml = StringifyInventory(Player);

            node.AppendChild(items);
            loginXML.AppendChild(node);
            loginXML.Save(xmlFilename);
        }

        private string StringifyInventory(Player Player)
        {
            string inventory = "";

            for (int i = 0; i < Player.inventory.Length; i++)
            {
                if (i > 0)
                    inventory += "-";

                inventory += Player.inventory[i].Type.ToString() + ":" + Player.inventory[i].Stack.ToString();
            }

            return inventory;
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
