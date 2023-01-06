namespace Lobby
{
    using PluginAPI.Core;
    using PluginAPI.Core.Zones;
    using System.Linq;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public static class LobbyLocationHandler
    {
        public static GameObject Point = new GameObject("LobbyPoint");

        public static void TowerLocation()
        {
            int rndRoom = Random.Range(1, 6);

            Vector3 position;

            switch (rndRoom)
            {
                case 1: position = new Vector3(162.893f, 1019.470f, -13.430f); break;
                case 2: position = new Vector3(107.698f, 1014.048f, -12.555f); break;
                case 3: position = new Vector3(39.262f, 1014.112f, -31.844f); break;
                case 4: position = new Vector3(-15.854f, 1014.461f, -31.543f); break;
                case 5: position = new Vector3(130.483f, 993.366f, 20.601f); break;
                default: position = new Vector3(39.262f, 1014.112f, -31.844f); break;
            }

            Point.transform.position = position;
            Point.transform.rotation = Quaternion.identity;
        }

        public static void IntercomLocation()
        {
            var IcomRoom = EntranceZone.Rooms.FirstOrDefault(x => x.GameObject.name == "EZ_Intercom");

            Point.transform.parent = IcomRoom.Transform;
            Point.transform.localPosition = new Vector3(-4.16f, -3.860f, -2.113f);
            Point.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        }

        public static void GRLocation()
        {
            var GRRoom = LightZone.Rooms.FirstOrDefault(x => x.GameObject.name.Contains("LCZ_372"));

            Point.transform.parent = GRRoom.Transform;
            Point.transform.localPosition = new Vector3(4.8f, 1f, 2.3f);
            Point.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        }

        public static void SCP173Location()
        {
            var SCP173Room = LightZone.Rooms.FirstOrDefault(x => x.GameObject.name.Contains("LCZ_173"));

            Point.transform.parent = SCP173Room.Transform;
            Point.transform.localPosition = new Vector3(17f, 13f, 8f);
            Point.transform.localRotation = Quaternion.Euler(0f, -90f, 0f);
        }
    }
}
