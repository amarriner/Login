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
        public static void InvalidatePlayer(Server server, ISender sender, ArgumentList args)
        {
            string playerName;
            Player player = server.GetPlayerByName(sender.Name);

            if (args.TryGetString(0, out playerName))
            {
                if (server.GetPlayerByName(playerName) != null)
                {
                    Login.plugin.SetPlayerInvalid(server.GetPlayerByName(playerName));
                    SendMessage(player, "You have invalidated " + playerName);
                }
                else
                    SendMessage(player, "There is no current player named " + playerName);
            }
            else
                SendMessage(player, "You must supply a player name");
        }

        private static void SendMessage(Player InPlayer, String message)
        {
            if (InPlayer != null)
                InPlayer.sendMessage(message, Login.plugin.chatColor);
            else
                Program.tConsole.WriteLine(message);
        }

        public static void SetPoint(Server server, ISender sender, ArgumentList args)
        {
            string param;
            Player player = server.GetPlayerByName(sender.Name);

            if (args.TryGetString(0, out param))
            {
                switch (param.ToUpper())
                {
                    case "LOBBY":
                        player.PluginData["lobby"] = true;
                        player.sendMessage("Hit a block where you want the lobby to be", Login.plugin.chatColor);
                        break;
                    case "VALIDATED":
                        player.PluginData["validated"] = true;
                        player.sendMessage("Hit a block where you want the validated point to be", Login.plugin.chatColor);
                        break;
                    default:
                        player.sendMessage("You must specify either lobby or validated", Login.plugin.chatColor);
                        break;
                }
            }
            else
                player.sendMessage("You must specify either lobby or validated", Login.plugin.chatColor);
        }

        public static void TestPoint(Server server, ISender sender, ArgumentList args)
        {
            string param;
            Player player = server.GetPlayerByName(sender.Name);

            if (args.TryGetString(0, out param))
            {
                switch (param.ToUpper())
                {
                    case "LOBBY":
                        player.sendMessage("Teleporting to the lobby", Login.plugin.chatColor);
                        Login.plugin.TeleportPlayerToPoint(player, Login.LOBBY);
                        break;
                    case "VALIDATED":
                        player.sendMessage("Teleporting to the validated point", Login.plugin.chatColor);
                        Login.plugin.TeleportPlayerToPoint(player, Login.VALIDATED);
                        break;
                    default:
                        player.sendMessage("You must specify either lobby or validated", Login.plugin.chatColor);
                        break;
                }
            }
            else
                player.sendMessage("You must specify either lobby or validated", Login.plugin.chatColor);
        }

        public static void ValidatePlayer(Server server, ISender sender, ArgumentList args)
        {
            string playerName;
            Player player = server.GetPlayerByName(sender.Name);

            if (args.TryGetString(0, out playerName))
            {
                if (server.GetPlayerByName(playerName) != null)
                {
                    Login.plugin.SetPlayerValid(server.GetPlayerByName(playerName), true);
                    SendMessage(player, "You have validated " + playerName);
                }
                else
                    SendMessage(player, "There is no current player named " + playerName);
            }
            else
                SendMessage(player, "You must supply a player name");
        }
    }
}