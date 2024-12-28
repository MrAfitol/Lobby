using CommandSystem;
using Lobby.Extensions;
using PluginAPI.Core;
using System;

namespace Lobby.Command.ControlCommands
{
    public class RemoveLocationCommand : ICommand
    {
        public string Command => "removeloc";

        public string[] Aliases { get; } = { "remloc", "rloc", "rl" };

        public string Description => "Remove a new lobby location.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player playerSender = Player.Get(sender);

            if (!playerSender.IsAllowCommand())
            {
                response = $"You don't have permission to use this command!";
                return false;
            }

            if (arguments.Count != 2)
            {
                response = "Incorrect command, use: \nlobby removeloc room (index)\nlobby removeloc static (index)";
                return false;
            }

            if (!Int32.TryParse(arguments.At(1), out int index))
            {
                response = "The index must be a number!";
                return false;
            }

            var handler = PluginHandler.Get(Lobby.Instance);

            switch (arguments.At(0))
            {
                case "room":
                    if (Lobby.Config.CustomRoomLocations == null || Lobby.Config.CustomRoomLocations?.Count - 1 < index)
                    {
                        response = $"Custom location at index {index} was not found.";
                        return false;
                    }

                    Lobby.Config.CustomRoomLocations.RemoveAt(index);

                    handler.SaveConfig(Lobby.Instance, nameof(Lobby.Config));
                    response = $"Custom location at index {index} has been removed.";
                    return true;
                case "static":
                    if (Lobby.Config.CustomLocations == null || Lobby.Config.CustomLocations?.Count - 1 < index)
                    {
                        response = $"Custom location at index {index} was not found.";
                        return false;
                    }

                    Lobby.Config.CustomLocations.RemoveAt(index);

                    handler.SaveConfig(Lobby.Instance, nameof(Lobby.Config));
                    response = $"Custom location at index {index} has been removed.";
                    return true;
                default:
                    response = "Incorrect command, use: \nlobby removeloc room (index)\nlobby removeloc static (index)";
                    return false;
            }
        }
    }
}
