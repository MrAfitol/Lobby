using CommandSystem;
using Lobby.Extensions;
using PluginAPI.Core;
using System;
using UnityEngine;

namespace Lobby.Command.DebugCommands
{
    public class GetLocalDataCommand : ICommand
    {
        public string Command => "getlocaldata";

        public string[] Aliases { get; } = { "getlocdat", "getld", "gld" };

        public string Description => "Shows the exact position and rotation in the room where the player is located.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player playerSender = Player.Get(sender);

            if (!playerSender.IsAllowCommand())
            {
                response = $"You don't have permission to use this command!";
                return false;
            }

            if (arguments.Count > 0)
            {
                response = "Incorrect command, use: \nlobby getlocalpos";
                return false;
            }

            if (playerSender.Room == null)
            {
                response = "An error occurred when getting the room you are in.";
                return false;
            }

            GameObject Point = new GameObject("Point");
            Point.transform.position = playerSender.Position;
            Point.transform.eulerAngles = playerSender.Rotation;
            Point.transform.SetParent(playerSender.Room.transform);

            response = $"Room name {playerSender.Room.name}; Local position: {Point.transform.localPosition.ToString()}; Local Rotation: {Point.transform.localEulerAngles.ToString()}.";
            return true;
        }
    }
}