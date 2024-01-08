namespace Lobby.API
{
    using MapGeneration;
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
            { LobbyLocationType.Intercom, new CustomRoomLocationData() { RoomNameType = RoomName.EzIntercom,  OffsetX = -4.16f, OffsetY = -3.860f, OffsetZ = -2.113f, RotationX = 0, RotationY = 180, RotationZ = 0 } },
            { LobbyLocationType.GR18, new CustomRoomLocationData() { RoomNameType = RoomName.LczGlassroom,  OffsetX = 4.8f, OffsetY = 1f, OffsetZ = 2.3f, RotationX = 0, RotationY = 180, RotationZ = 0 } },
            { LobbyLocationType.SCP173, new CustomRoomLocationData() { RoomNameType = RoomName.Lcz173,  OffsetX = 17f, OffsetY = 13f, OffsetZ = 8f, RotationX = 0, RotationY = -90, RotationZ = 0 } },
        };

        public static void SetLocation(LocationData locationData)
        {
            if (locationData is CustomRoomLocationData customRoomLocation)
            {
                RoomIdentifier Room;

                Room = RoomIdentifier.AllRoomIdentifiers.First(x => x.Name == customRoomLocation.RoomNameType);

                if (customRoomLocation.RoomNameType == RoomName.EzIntercom)
                    EventHandlers.IsIntercom = true;

                if (Room == null)
                {
                    customRoomLocation = (CustomRoomLocationData)LocationDatas[LobbyLocationType.GR18];
                    Room = RoomIdentifier.AllRoomIdentifiers.First(x => x.Name == customRoomLocation.RoomNameType);
                }

                Point.transform.SetParent(Room.transform);
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
