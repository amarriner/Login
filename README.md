LOGIN

This plugin allows servers to restrict logins in various ways. The three simplest ways are restricting logins to Softcore, Mediumcore, or Hardcore players. Each of these has a corresponding properties file setting (true/false). They each also have an additional property which is the text that will be displayed to the player when they're kicked. I hadn't intended this initially, but you could set more than one to true at the same time and, in theory, restrict to 2 out of the 3 difficulties. Messaging to kicked users might be a bit wonky/misleading, however.

The other way this can be used is to restrict logins to new players. The way it does this is to check for empty inventory on initial login. This is to prevent having to keep up with any client upgrades and/or mods which alter starting inventory for a character. Players with inventory can either be immediately kicked or stay on the server but with no permission to build/use items until an OP validates them manually. Invalid players on a server can't (or shouldn't be able to anyway) build/break/use anything. This is so they can't grief the server, but can talk to an OP to become valid if there's a good reason.

Once a new player has logged on, they are given starting equipment. By default that is: an iron shortsword, an iron pickaxe, an iron axe, 100 wood and 10 torches. Starting equipment can be changed via the properties file. When a player logs off, their current inventory is saved. When they try to log back on, their current inventory is checked against what was saved and if it doesn't match, they're kicked. So basically this setting attempts to assure that items in a player's inventory were only collected from one world. I realize that part of the appeal of Terraria is using characters across servers, but I've had some requests like this so I thought I'd work on it.

This plugin is pretty raw, but I felt I needed a little feedback to move ahead. 

Some considerations:
If the server is closed in an unclean way (power outage, CTRL-C), players who are currently logged on will probably get invalid inventory messages upon logging back in because they won't have been saved correctly. Need a workaround for this (perhaps players can cause a save to happen themselves?). A rough workaround for this is to allow invalid players in, and then manually validate them (OP or console).
Want to allow setting of properties at runtime rather that just in the properties file.

There are a few commands available to OPs (or console for two of them):
/invalidate <player> - Manually invalidate a player (also sends them to the JailPoint).
/setpoint <jail or validated> - The next block you attempt to break will become either the JailPoint or the ValidatedPoint respectively. If these points are set to something other than 0,0, players will be teleported to those points when they become invalid (JailPoint) or valid (ValidatedPoint).
/testpoint <jail or validated> - Sends player who ran the command to either the JailPoint or the ValidatedPoint. Used to make sure they're set properly.
/validate <player> - Manually validates a player (also sends them to the ValidatedPoint).

There are several properties that can be set in a properties file:
KickIfHasInventory - Kick the player if they're invalid if set to true, allow them in otherwise
OnlyAllowNewPlayers - Don't let people in who haven't been in before and don't have empty inventory
OnlyAllowSoftcore - Only allow softcore players
OnlyAllowMediumcore - Only allow mediumcore players
OnlyAllowHardcore - Only allow hardcore players
InventoryMessage - Message displayed to players kicked for inventory issues
SoftcoreMessage - Message displayed to players kicked because they aren't softcore
MediumcoreMessage - Message displayed to players kicked because they aren't mediumcore
HardcoreMessage - Message displayed to players kicked because they aren't hardcore
BanPiggyBanks - If true, players cannot place piggybanks
BanSafes - If true, players cannot place safes.
StartingEquipment - See Below
JailPoint - A set of coordinates (0,0 for example) where a player will be teleported to upon login if they are invalid
ValidatedPoint - A set of coordinates (0,0 for example) where a player will be teleported to upon being validated

The Starting Equipment property may look a little strange. You define it in name-value pairs separated by a colon (:). So, for example, if you wanted the starting equipment for a player to be 1 iron shortsword and 5 torches, you'd assign the property like this:
startingequipment=ironshortsword-1:torch:5

SOURCE
https://github.com/amarriner/Login

DOWNLOAD
http://awbw.amarriner.com/terraria/Login.zip

CHANGELOG
0.2
Added several properties (BanPiggyBanks, BanSafes, StartingEquipment, JailPoint, ValidatedPoint)
Added several commands (invalidate, setpoint, testpoint, validate)
Fleshed out some of the functionality and refactored some code

0.1
Initial Release