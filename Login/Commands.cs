using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Terraria_Server;
using System.Threading;
using Terraria_Server.Collections;
using Terraria_Server.Commands;
using Terraria_Server.Misc;
using Terraria_Server.Logging;
using Terraria_Server.RemoteConsole;
using Terraria_Server.WorldMod;
using Terraria_Server.Definitions;
using Terraria_Server.Plugin;

namespace Login.Commands
{
    public class Commands
    {
        public static void GetEquip(Server server, ISender sender, ArgumentList args)
        {
            if (Login.plugin.PlayerHasNullInventory(server.GetPlayerByName(sender.Name)))
            {
                ArgumentList giveArgs = new ArgumentList(server);

                // Iron Shortsword
                giveArgs.Add(sender.Name);
                giveArgs.Add("1");
                giveArgs.Add("6");
                Terraria_Server.Commands.Commands.Give(server, sender, giveArgs);

                // Iron Pickaxe
                giveArgs[2] = "ironpickaxe";
                Terraria_Server.Commands.Commands.Give(server, sender, giveArgs);

                // Iron Axe
                giveArgs[2] = "10";
                Terraria_Server.Commands.Commands.Give(server, sender, giveArgs);

                // Wood
                giveArgs[1] = "100";
                giveArgs[2] = "9";
                Terraria_Server.Commands.Commands.Give(server, sender, giveArgs);

                // Torches
                giveArgs[1] = "10";
                giveArgs[2] = "8";
                Terraria_Server.Commands.Commands.Give(server, sender, giveArgs);
            }
            else
                server.GetPlayerByName(sender.Name).sendMessage("You cannot use this command if you have inventory items!", Login.plugin.chatColor);
        }
    }
}