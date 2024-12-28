using CommandSystem;
using Lobby.Extensions;
using PluginAPI.Core;
using System;
using UnityEngine;

namespace Lobby.Command.ControlCommands
{
    public class AddLocationCommand : ICommand
    {
        public string Command => "addloc";

        public string[] Aliases { get; } = { "add", "ad" };

        public string Description => "Add a new lobby location.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player playerSender = Player.Get(sender);

            if (!playerSender.IsAllowCommand())
            {
                response = $"You don't have permission to use this command!";
                return false;
            }

            if (arguments.Count != 1)
            {
                response = "Incorrect command, use: \nlobby addloc room\nlobby addloc static";
                return false;
            }

            var handler = PluginHandler.Get(Lobby.Instance);

            switch (arguments.At(0))
            {
                case "room":
                    GameObject Point = new GameObject("Point");
                    Point.transform.position = playerSender.Position;
                    Point.transform.eulerAngles = playerSender.Rotation;
                    Point.transform.SetParent(playerSender.Room.transform);

                    CustomRoomLocationData roomLocationData = new CustomRoomLocationData();
                    roomLocationData.RoomNameType = playerSender.Room.name;
                    roomLocationData.OffsetX = Point.transform.localPosition.x; roomLocationData.OffsetY = Point.transform.localPosition.y + 0.05f; roomLocationData.OffsetZ = Point.transform.localPosition.z;
                    roomLocationData.RotationX = Point.transform.localEulerAngles.x; roomLocationData.RotationY = Point.transform.localEulerAngles.y; roomLocationData.RotationZ = Point.transform.localEulerAngles.z;

                    Lobby.Config.CustomRoomLocations.Add(roomLocationData);
                    handler.SaveConfig(Lobby.Instance, nameof(Lobby.Config));
                    response = "New custom location added to the config.";
                    return true;
                case "static":
                    CustomLocationData locationData = new CustomLocationData();
                    locationData.PositionX = playerSender.Position.x; locationData.PositionY = playerSender.Position.y + 0.05f; locationData.PositionZ = playerSender.Position.z;
                    locationData.RotationX = playerSender.Rotation.x; locationData.RotationY = playerSender.Rotation.y; locationData.RotationZ = playerSender.Rotation.z;

                    Lobby.Config.CustomLocations.Add(locationData);
                    handler.SaveConfig(Lobby.Instance, nameof(Lobby.Config));
                    response = "New custom location added to the config.";
                    return true;
                default:
                    response = "Incorrect command, use: \nlobby addloc room\nlobby addloc static";
                    return false;
            }
        }


    }
}