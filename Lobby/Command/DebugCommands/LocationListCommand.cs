using CommandSystem;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using System;

namespace Lobby.Command.DebugCommands
{
    public class LocationListCommand : ICommand
    {
        public string Command => "loclist";

        public string[] Aliases { get; } = { "llist", "ll" };

        public string Description => "Display a list of custom locations in the console.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player playerSender = Player.Get(sender);

            if (!playerSender.HasAnyPermission("lobby.*", "lobby.debug.*", "lobby.debug.loclist"))
            {
                response = $"You don't have permission to use this command!";
                return false;
            }

            if (arguments.Count != 1)
            {
                response = "Incorrect command, use: \nlobby loclist all\nlobby loclist room\nlobby loclist static";
                return false;
            }

            string resString = string.Empty;

            switch (arguments.At(0))
            {
                case "all":
                    resString += "Room locations:\n";
                    if (Lobby.Instance.Config.CustomRoomLocations?.Count > 0)
                    {
                        foreach (var item in Lobby.Instance.Config.CustomRoomLocations)
                            resString += item.RoomNameType + " " + $"({item.OffsetX}, {item.OffsetY}, {item.OffsetZ})\n";
                    }

                    resString += "\nStatic locations:\n";
                    if (Lobby.Instance.Config.CustomLocations?.Count > 0)
                    {
                        foreach (var item in Lobby.Instance.Config.CustomLocations)
                            resString += $"({item.PositionX}, {item.PositionY}, {item.PositionZ})\n";
                    }

                    response = resString;
                    return true;
                case "room":
                    resString += "Room locations:\n";
                    if (Lobby.Instance.Config.CustomRoomLocations?.Count > 0)
                    {
                        for (int i = 0; i < Lobby.Instance.Config.CustomRoomLocations.Count; i++)
                        {
                            CustomRoomLocationData data = Lobby.Instance.Config.CustomRoomLocations[i];
                            resString += $"({i}) " + data.RoomNameType + " " + $"({data.OffsetX}, {data.OffsetY}, {data.OffsetZ})\n";
                        }
                    }

                    response = resString;
                    return true;
                case "static":
                    resString += "Static locations:\n";
                    if (Lobby.Instance.Config.CustomLocations?.Count > 0)
                    {
                        for (int i = 0; i < Lobby.Instance.Config.CustomLocations.Count; i++)
                        {
                            CustomLocationData data = Lobby.Instance.Config.CustomLocations[i];
                            resString += $"({i}) " + $"({data.PositionX}, {data.PositionY}, {data.PositionZ})\n";
                        }
                    }

                    response = resString;
                    return true;
                default:
                    response = "Incorrect command, use: \nlobby loclist all\nlobby loclist room\nlobby loclist static";
                    return false;
            }
        }
    }
}