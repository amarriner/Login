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
        private const bool DEFAULT_FORCE_SOFTCORE = false;
        private const bool DEFAULT_FORCE_MEDIUMCORE = false;
        private const bool DEFAULT_FORCE_HARDCORE = false;
        private const String DEFAULT_INVENTORY_MESSAGE = "Server only allows empty inventory";
        private const String DEFAULT_DIFFERENT_INVENTORY_MESSAGE = "You have different inventory";
        private const String DEFAULT_VALIDATED_MESSAGE = "You've been validated";
        private const bool DEFAULT_BAN_PIGGY_BANKS = true;
        private const bool DEFAULT_BAN_SAFES = true;
        private const String DEFAULT_STARTING_EQUIPMENT = "ironshortsword-1:ironpickaxe-1:ironaxe-1:wood-100:torch-10";
        private const String DEFAULT_LOBBY_POINT = "0,0";
        private const String DEFAULT_VALIDATED_POINT = "0,0";

        private const String KICK_IF_HAS_INVENTORY = "kickifhasinventory";
        private const String ONLY_ALLOW_NEW_PLAYERS = "onlyallownewplayers";
        private const String FORCE_SOFTCORE = "forcesoftcore";
        private const String FORCE_MEDIUMCORE = "forcemediumcore";
        private const String FORCE_HARDCORE = "forcehardcore";
        private const String INVENTORY_MESSAGE = "inventorymessage";
        private const String DIFFERENT_INVENTORY_MESSAGE = "differentinventorymessage";
        private const String VALIDATED_MESSAGE = "validated_message";
        private const String BAN_PIGGY_BANKS = "banpiggybanks";
        private const String BAN_SAFES = "bansafes";
        private const String STARTING_EQUIPMENT = "startingequipment";
        private const String LOBBY_POINT = "lobbypoint";
        private const String VALIDATED_POINT = "validatedpoint";

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

        public bool ForceSoftcore
        {
            get
            {
                return getValue(FORCE_SOFTCORE, DEFAULT_FORCE_SOFTCORE);
            }
        }

        public bool ForceMediumcore
        {
            get
            {
                return getValue(FORCE_MEDIUMCORE, DEFAULT_FORCE_MEDIUMCORE);
            }
        }

        public bool ForceHardcore
        {
            get
            {
                return getValue(FORCE_HARDCORE, DEFAULT_FORCE_HARDCORE);
            }
        }

        public String InventoryMessage
        {
            get
            {
                return getValue(INVENTORY_MESSAGE, DEFAULT_INVENTORY_MESSAGE);
            }
        }

        public String DifferentInventoryMessage
        {
            get
            {
                return getValue(DIFFERENT_INVENTORY_MESSAGE, DEFAULT_DIFFERENT_INVENTORY_MESSAGE);
            }
        }

        public String ValidatedMessage
        {
            get
            {
                return getValue(VALIDATED_MESSAGE, DEFAULT_VALIDATED_MESSAGE);
            }
        }

        public bool BanPiggyBanks
        {
            get
            {
                return getValue(BAN_PIGGY_BANKS, DEFAULT_BAN_PIGGY_BANKS);
            }
        }

        public bool BanSafes
        {
            get
            {
                return getValue(BAN_SAFES, DEFAULT_BAN_SAFES);
            }
        }

        public String StartingEquipment
        {
            get
            {
                return getValue(STARTING_EQUIPMENT, DEFAULT_STARTING_EQUIPMENT);
            }
        }

        public String LobbyPoint
        {
            get
            {
                return getValue(LOBBY_POINT, DEFAULT_LOBBY_POINT);
            }
            set
            {
                setValue(LOBBY_POINT, value);
            }
        }

        public String ValidatedPoint
        {
            get
            {
                return getValue(VALIDATED_POINT, DEFAULT_VALIDATED_POINT);
            }
            set
            {
                setValue(VALIDATED_POINT, value);
            }
        }
    }
}
