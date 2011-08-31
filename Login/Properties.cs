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

        private const String KICK_IF_HAS_INVENTORY = "kickifhasinventory";
        private const String ONLY_ALLOW_NEW_PLAYERS = "onlyallownewplayers";
        private const String ONLY_ALLOW_SOFTCORE = "onlyallowsoftcore";
        private const String ONLY_ALLOW_MEDIUMCORE = "onlyallowmediumcore";
        private const String ONLY_ALLOW_HARDCORE = "onlyallowhardcore";

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
    }
}
