#LOGIN

This plugin allows servers to restrict logins in various ways. The three simplest ways are restricting logins to Softcore, Mediumcore, or Hardcore players. Each of these has a corresponding properties file setting (true/false). If more than one of these are set to true, then players can be any of the "true" difficulties.

The other way this can be used is to restrict logins to new players. The way it does this is to check for empty inventory on initial login. This is to prevent having to keep up with any client upgrades and/or mods which alter starting inventory for a character. Players with inventory can either be immediately kicked or stay on the server but with no permission to build/use items until an OP validates them manually. Invalid players on a server can't (or shouldn't be able to anyway) build/break/use anything. This is so they can't grief the server, but can talk to an OP to become valid if there's a good reason. Max life and Max mana checking is also available.

Once a new player has logged on, they are given starting equipment. By default that is: an iron shortsword, an iron pickaxe, an iron axe, 100 wood and 10 torches. Starting equipment can be changed via the properties file. When a player logs off, their current inventory is saved. When they try to log back on, their current inventory is checked against what was saved and if it doesn't match, they're kicked. So basically this setting attempts to assure that items in a player's inventory were only collected from one world. I realize that part of the appeal of Terraria is using characters across servers, but I've had some requests like this so I thought I'd work on it.

This plugin is pretty raw, but I felt I needed a little feedback to move ahead. 

Some considerations:
If the server is closed in an unclean way (power outage, CTRL-C), players who are currently logged on will probably get invalid inventory messages upon logging back in because they won't have been saved correctly. Need a workaround for this (perhaps players can cause a save to happen themselves?). A rough workaround for this is to allow invalid players in, and then manually validate them (OP or console).
Want to allow setting of properties at runtime rather that just in the properties file.

##COMMANDS
* **/invalidate &lt;player&gt; (force)** - Manually invalidate a player (also sends them to the LobbyPoint if they're logged on). Deletes their XML file.
* **/setpoint &lt;lobby or validated&gt;** - The next block you attempt to break will become either the LobbyPoint or the ValidatedPoint respectively. If these points are set to something other than 0,0, players will be teleported to those points when they become invalid (LobbyPoint) or valid (ValidatedPoint).
* **/testpoint &lt;lobby or validated&gt;** - Sends player who ran the command to either the LobbyPoint or the ValidatedPoint. Used to make sure they're set properly.
* **/validate &lt;player&gt;** - Manually validates a player (also sends them to the ValidatedPoint).

##PROPERTIES
* **KickIfHasInventory** - Kick the player if they have invalid inventory if set to true, allow them in otherwise
* **KickIfDifferentLifeMana** - Kick the player if they have invalid life or mana if set to true, allow them in otherwise
* **OnlyAllowNewPlayers** - Don't let people in who haven't been in before and don't have empty inventory
* **ForceSoftcore** - Force softcore players
* **ForceMediumcore** - Force mediumcore players
* **ForceHardcore** - Force hardcore players
* **InventoryMessage** - Message displayed to players kicked for non-empty inventory
* **DifferentInventoryMessage** - Message displayed to players kicked because their inventory doesn't match.
* **ValidatedMessage** - Message displayed to players when they become valid
* **MaxLifeMessage** - Message displayed to players when their max life is different
* **MaxManaMessage** - Message displayed to players when their max mana is different
* **CheckMaxLife** - Whether to check for discrepancies in the max life stat
* **CheckMaxMana** - Whether to check for discrepancies in the max mana stat
* **BanPiggyBanks** - If true, players cannot place piggybanks
* **BanSafes** - If true, players cannot place safes.
* **StartingEquipment** - See Below
* **LobbyPoint** - A set of coordinates (0,0 for example) where a player will be teleported to upon login if they are invalid
* **ValidatedPoint** - A set of coordinates (0,0 for example) where a player will be teleported to upon being validated

The Starting Equipment property may look a little strange. You define it in name-value pairs separated by a colon (:). So, for example, if you wanted the starting equipment for a player to be 1 iron shortsword and 5 torches, you'd assign the property like this:
startingequipment=ironshortsword-1:torch:5

##SOURCE
https://github.com/amarriner/Login

##DOWNLOAD
http://awbw.amarriner.com/terraria/Login.zip

##CHANGELOG

**0.2.3**

* Updated plugin for TDSM Build 33

**0.2.2**

* Updated plugin for TDSM Build 32

**0.2.1**

* Updated how the *core checking works, allowing for setting more than one to true (changed some properties)
* Now optionally checking life and mana in addition to inventory
* Changed "jail" to "lobby"
* Added force option to invalidate command
* Invalidate now deletes player's xml file
* Another pass at banning piggy banks and safes properly

**0.2**

* Added several properties (BanPiggyBanks, BanSafes, StartingEquipment, JailPoint, ValidatedPoint)
* Added several commands (invalidate, setpoint, testpoint, validate)
* Fleshed out some of the functionality and refactored some code

**0.1**

* Initial Release