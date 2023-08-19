namespace Lobby.API
{
    using PluginAPI.Core.Zones;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public static class LobbyLocationHandler
    {
        public static GameObject Point;

        public static Dictionary<LobbyLocationType, LocationData> LocationDatas = new Dictionary<LobbyLocationType, LocationData>()
        {
            { LobbyLocationType.Tower_1, new CustomLocationData() { PositionX = 162.893f, PositionY = 1019.470f, PositionZ = -13.430f, RotationX = 0, RotationY = 0, RotationZ = 0 } },
            { LobbyLocationType.Tower_2, new CustomLocationData() { PositionX = 107.698f, PositionY = 1014.048f, PositionZ = -12.555f, RotationX = 0, RotationY = 0, RotationZ = 0 } },
            { LobbyLocationType.Tower_3, new CustomLocationData() { PositionX = 39.262f, PositionY = 1014.112f, PositionZ = -31.844f, RotationX = 0, RotationY = 0, RotationZ = 0 } },
            { LobbyLocationType.Tower_4, new CustomLocationData() { PositionX = -15.854f, PositionY = 1014.461f, PositionZ = -31.543f, RotationX = 0, RotationY = 0, RotationZ = 0 } },
            { LobbyLocationType.Tower_5, new CustomLocationData() { PositionX = 130.483f, PositionY = 993.366f, PositionZ = 20.601f, RotationX = 0, RotationY = 0, RotationZ = 0 } },
            { LobbyLocationType.Intercom, new CustomRoomLocationData() { RoomName = "EZ_Intercom",  OffsetX = -4.16f, OffsetY = -3.860f, OffsetZ = -2.113f, RotationX = 0, RotationY = 180, RotationZ = 0 } },
            { LobbyLocationType.GR18, new CustomRoomLocationData() { RoomName = "LCZ_372",  OffsetX = 4.8f, OffsetY = 1f, OffsetZ = 2.3f, RotationX = 0, RotationY = 180, RotationZ = 0 } },
            { LobbyLocationType.SCP173, new CustomRoomLocationData() { RoomName = "LCZ_173",  OffsetX = 17f, OffsetY = 13f, OffsetZ = 8f, RotationX = 0, RotationY = -90, RotationZ = 0 } },
        };

        public static void SetLocation(LocationData locationData)
        {
            if (locationData is CustomRoomLocationData customRoomLocation)
            {
                FacilityRoom Room;

                if (string.IsNullOrEmpty(customRoomLocation.RoomName) || !customRoomLocation.RoomName.Contains("LCZ_") && !customRoomLocation.RoomName.Contains("HCZ_") && !customRoomLocation.RoomName.Contains("EZ_"))
                    customRoomLocation = (CustomRoomLocationData)LocationDatas[LobbyLocationType.GR18];

                switch (customRoomLocation.RoomName.Split('_')[0])
                {
                    case "LCZ":
                        Room = LightZone.Rooms.First(x => x.GameObject.name.Contains(customRoomLocation.RoomName));
                        break;
                    case "HCZ":
                        Room = HeavyZone.Rooms.First(x => x.GameObject.name.Contains(customRoomLocation.RoomName));
                        break;
                    case "EZ":
                        Room = EntranceZone.Rooms.First(x => x.GameObject.name.Contains(customRoomLocation.RoomName));
                        break;
                    default:
                        Room = null;
                        break;
                }

                if (customRoomLocation.RoomName.Contains("Intercom"))
                    EventHandlers.IsIntercom = true;

                if (Room == null)
                {
                    customRoomLocation = (CustomRoomLocationData)LocationDatas[LobbyLocationType.GR18];
                    Room = LightZone.Rooms.First(x => x.GameObject.name.Contains(customRoomLocation.RoomName));
                }

                Point.transform.SetParent(Room.Transform);
                Point.transform.localPosition = new Vector3(customRoomLocation.OffsetX, customRoomLocation.OffsetY, customRoomLocation.OffsetZ);
                Point.transform.localRotation = Quaternion.Euler(customRoomLocation.RotationX, customRoomLocation.RotationY, customRoomLocation.RotationZ);
            }
            else if (locationData is CustomLocationData customLocation)
            {
                Point.transform.localPosition = new Vector3(customLocation.PositionX, customLocation.PositionY, customLocation.PositionZ);
                Point.transform.localRotation = Quaternion.Euler(customLocation.RotationX, customLocation.RotationY, customLocation.RotationZ);
            }
        }
    }
}
