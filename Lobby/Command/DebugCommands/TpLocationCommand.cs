using CommandSystem;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using MapGeneration;
using System;
using System.Linq;
using UnityEngine;

namespace Lobby.Command.DebugCommands
{
    public class TpLocationCommand : ICommand
    {
        public string Command => "tploc";

        public string[] Aliases { get; } = { "tpl", "tl" };

        public string Description => "Teleport to a custom location.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player playerSender = Player.Get(sender);

            if (!playerSender.HasAnyPermission("lobby.*", "lobby.debug.*", "lobby.debug.tp"))
            {
                response = $"You don't have permission to use this command!";
                return false;
            }

            if (arguments.Count != 2)
            {
                response = "Incorrect command, use: \nlobby tploc room (index)\nlobby tploc static (index)";
                return false;
            }

            if (!Int32.TryParse(arguments.At(1), out int index))
            {
                response = "The index must be a number";
                return false;
            }

            GameObject Point = new GameObject("Point");

            switch (arguments.At(0))
            {
                case "room":
                    if (Lobby.Instance.Config.CustomRoomLocations == null || Lobby.Instance.Config.CustomRoomLocations?.Count - 1 < index)
                    {
                        response = $"Custom location at index {index} was not found.";
                        return false;
                    }

                    if (Enum.TryParse<RoomName>(Lobby.Instance.Config.CustomRoomLocations[index].RoomNameType, out RoomName roomName))
                        Point.transform.SetParent(RoomIdentifier.AllRoomIdentifiers.First(x => x.Name == roomName).transform);
                    else if (RoomIdentifier.AllRoomIdentifiers.Count(x => x.name.Contains(Lobby.Instance.Config.CustomRoomLocations[index].RoomNameType)) > 0)
                        Point.transform.SetParent(RoomIdentifier.AllRoomIdentifiers.First(x => x.name.Contains(Lobby.Instance.Config.CustomRoomLocations[index].RoomNameType)).transform);

                    Point.transform.localPosition = new Vector3(Lobby.Instance.Config.CustomRoomLocations[index].OffsetX, Lobby.Instance.Config.CustomRoomLocations[index].OffsetY, Lobby.Instance.Config.CustomRoomLocations[index].OffsetZ);
                    Point.transform.localEulerAngles = new Vector3(Lobby.Instance.Config.CustomRoomLocations[index].RotationX, Lobby.Instance.Config.CustomRoomLocations[index].RotationY, Lobby.Instance.Config.CustomRoomLocations[index].RotationZ);

                    playerSender.Position = Point.transform.position;
                    playerSender.Rotation = Point.transform.rotation;

                    GameObject.Destroy(Point);

                    response = $"You have successfully teleported to a custom location at index {index}.";
                    return true;
                case "static":
                    if (Lobby.Instance.Config.CustomLocations == null || Lobby.Instance.Config.CustomLocations?.Count - 1 < index)
                    {
                        response = $"Custom location at index {index} was not found.";
                        return false;
                    }

                    playerSender.Position = new Vector3(Lobby.Instance.Config.CustomLocations[index].PositionX, Lobby.Instance.Config.CustomLocations[index].PositionY, Lobby.Instance.Config.CustomLocations[index].PositionZ);
                    playerSender.Rotation = Quaternion.Euler(Lobby.Instance.Config.CustomLocations[index].RotationX, Lobby.Instance.Config.CustomLocations[index].RotationY, Lobby.Instance.Config.CustomLocations[index].RotationZ);

                    response = $"You have successfully teleported to a custom location at index {index}.";
                    return true;
                default:
                    response = "Incorrect command, use: \nlobby tploc room (index)\nlobby tploc static (index)";
                    return false;
            }
        }
    }
}