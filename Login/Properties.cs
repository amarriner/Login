using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria_Server.Misc;

namespace Login
{
    public class Properties : PropertiesFile
    {
        private const bool DEFAULT_KICK_IF_HAS_INVENTORY = true;
        private const bool DEFAULT_ONLY_ALLOW_NEW_PLAYERS = true;
        private const bool DEFAULT_ONLY_ALLOW_SOFTCORE = false;
        private const bool DEFAULT_ONLY_ALLOW_MEDIUMCORE = false;
        private const bool DEFAULT_ONLY_ALLOW_HARDCORE = false;
        private const String DEFAULT_INVENTORY_MESSAGE = "Server only allows empty inventory";
        private const String DEFAULT_SOFTCORE_MESSAGE = "Server only allows softcore players";
        private const String DEFAULT_MEDIUMCORE_MESSAGE = "Server only allows mediumcore players";
        private const String DEFAULT_HARDCORE_MESSAGE = "Server only allows hardcore players";

        private const String KICK_IF_HAS_INVENTORY = "kickifhasinventory";
        private const String ONLY_ALLOW_NEW_PLAYERS = "onlyallownewplayers";
        private const String ONLY_ALLOW_SOFTCORE = "onlyallowsoftcore";
        private const String ONLY_ALLOW_MEDIUMCORE = "onlyallowmediumcore";
        private const String ONLY_ALLOW_HARDCORE = "onlyallowhardcore";
        private const String INVENTORY_MESSAGE = "inventorymessage";
        private const String SOFTCORE_MESSAGE = "softcoremessage";
        private const String MEDIUMCORE_MESSAGE = "mediumcoremessage";
        private const String HARDCORE_MESSAGE = "hardcoremessage";

        public Properties(String propertiesPath) : base(propertiesPath) { }

        public bool KickIfHasInventory
        {
            get
            {
                return getValue(KICK_IF_HAS_INVENTORY, DEFAULT_KICK_IF_HAS_INVENTORY);
            }
        }

        public bool OnlyAllowNewPlayers
        {
            get
            {
                return getValue(ONLY_ALLOW_NEW_PLAYERS, DEFAULT_ONLY_ALLOW_NEW_PLAYERS);
            }
        }

        public bool OnlyAllowSoftcore
        {
            get
            {
                return getValue(ONLY_ALLOW_SOFTCORE, DEFAULT_ONLY_ALLOW_SOFTCORE);
            }
        }

        public bool OnlyAllowMediumcore
        {
            get
            {
                return getValue(ONLY_ALLOW_MEDIUMCORE, DEFAULT_ONLY_ALLOW_MEDIUMCORE);
            }
        }

        public bool OnlyAllowHardcore
        {
            get
            {
                return getValue(ONLY_ALLOW_HARDCORE, DEFAULT_ONLY_ALLOW_HARDCORE);
            }
        }

        public String InventoryMessage
        {
            get
            {
                return getValue(INVENTORY_MESSAGE, DEFAULT_INVENTORY_MESSAGE);
            }
        }

        public String SoftcoreMessage
        {
            get
            {
                return getValue(SOFTCORE_MESSAGE, DEFAULT_SOFTCORE_MESSAGE);
            }
        }

        public String MediumcoreMessage
        {
            get
            {
                return getValue(MEDIUMCORE_MESSAGE, DEFAULT_MEDIUMCORE_MESSAGE);
            }
        }

        public String HardcoreMessage
        {
            get
            {
                return getValue(HARDCORE_MESSAGE, DEFAULT_HARDCORE_MESSAGE);
            }
        }
    }
}
