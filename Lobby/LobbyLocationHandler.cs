namespace Lobby
{
    using PluginAPI.Core;
    using PluginAPI.Core.Zones;
    using PluginAPI.Core.Zones.Entrance;
    using System.Linq;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public static class LobbyLocationHandler
    {
        public static Vector3 LobbyPosition;
        public static Quaternion LobbyRotation;

        public static void TowerLocation()
        {
            int rndRoom = Random.Range(1, 6);

            switch (rndRoom)
            {
                case 1: LobbyPosition = new Vector3(162.893f, 1019.470f, -13.430f); break;
                case 2: LobbyPosition = new Vector3(107.698f, 1014.048f, -12.555f); break;
                case 3: LobbyPosition = new Vector3(39.262f, 1014.112f, -31.844f); break;
                case 4: LobbyPosition = new Vector3(-15.854f, 1014.461f, -31.543f); break;
                case 5: LobbyPosition = new Vector3(130.483f, 993.366f, 20.601f); break;
                default: LobbyPosition = new Vector3(39.262f, 1014.112f, -31.844f); break;
            }
        }

        public static void IntercomLocation()
        {
            var IcomRoom = EntranceZone.Rooms.FirstOrDefault(x => x.GameObject.name == "EZ_Intercom");

            LobbyPosition = IcomRoom.Transform.TransformPoint(new Vector3(-4.16f, -3.860f, -2.113f));
            LobbyRotation = Quaternion.Euler(IcomRoom.Rotation.eulerAngles.x, IcomRoom.Rotation.eulerAngles.y + 180, IcomRoom.Rotation.eulerAngles.z);
        }
    }
}
