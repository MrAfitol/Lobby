﻿using MapGeneration;
using PlayerRoles;
using System.Collections.Generic;
using System.ComponentModel;

namespace Lobby
{
    public class Config
    {
        [Description("Main text ({seconds} - Either it shows how much is left until the start, or the server status is \"Server is suspended\", \"Round starting\", <rainbow> - Change the next text a rainbow color, </rainbow> - Close a rainbow color tag)")]
        public string TitleText { get; set; } = "<color=#F0FF00><b>Waiting for players, {seconds}</b></color>";

        [Description("Text showing the number of players ({players} - Text with the number of players, <rainbow> - Change the next text a rainbow color, </rainbow> - Close a rainbow color tag)")]
        public string PlayerCountText { get; set; } = "<color=#FFA600><i>{players}</i></color>";

        [Description("What will be written if the lobby is locked?")]
        public string ServerPauseText { get; set; } = "Server is suspended";

        [Description("What will be written when there is a second left?")]
        public string SecondLeftText { get; set; } = "{seconds} second left";

        [Description("What will be written when there is more than a second left?")]
        public string SecondsLeftText { get; set; } = "{seconds} seconds left";

        [Description("What will be written when the round starts?")]
        public string RoundStartText { get; set; } = "Round starting";

        [Description("What will be written when there is only one player on the server?")]
        public string PlayerJoinText { get; set; } = "player joined";

        [Description("What will be written when there is more than one player on the server?")]
        public string PlayersJoinText { get; set; } = "players joined";

        [Description("Vertical text position.")]
        public int VerticalPos { get; set; } = 25;

        [Description("Top text size")]
        public int TopTextSize { get; set; } = 50;

        [Description("Bottom text size")]
        public int BottomTextSize { get; set; } = 40;

        [Description("Top text size in intercom")]
        public int TopTextIcomSize { get; set; } = 150;

        [Description("Bottom text size in intercom")]
        public int BottomTextIcomSize { get; set; } = 140;

        [Description("Enable the movement boost effect?")]
        public bool EnableMovementBoost { get; set; } = true;

        [Description("What is the movement boost intensity? (Max 255)")]
        public byte MovementBoostIntensity { get; set; } = 50;

        [Description("Will infinity stamina be enabled for people in the lobby?")]
        public bool InfinityStamina { get; set; } = true;

        [Description("What role will people play in the lobby?")]
        public RoleTypeId LobbyPlayerRole { get; set; } = RoleTypeId.Tutorial;

        [Description("Allow people to talk over the intercom?")]
        public bool AllowIcom { get; set; } = true;

        [Description("Display text on Intercom? (Works only when lobby Intercom type)")]
        public bool DisplayInIcom { get; set; } = true;

        [Description("What size will the text be in the Intercom? (The larger the value, the smaller it will be)")]
        public int IcomTextSize { get; set; } = 20;

        [Description("What items will be given when spawning a player in the lobby? (Leave blank to keep inventory empty)")]
        public List<ItemType> LobbyInventory { get; set; } = new List<ItemType>()
        {
            ItemType.Coin
        };

        [Description("In what locations can people spawn? (If this parameter is empty, one of the custom locations (or custom room locations) will be selected)")]
        public List<LobbyLocationType> LobbyLocation { get; set; } = new List<LobbyLocationType>()
        {
            LobbyLocationType.Tower_1,
            LobbyLocationType.Tower_2,
            LobbyLocationType.Tower_3,
            LobbyLocationType.Tower_4,
            LobbyLocationType.Tower_5,
            LobbyLocationType.Intercom,
            LobbyLocationType.GR18,
            LobbyLocationType.SCP173
        };

        [Description("This option is for a custom lobby location")]
        public List<CustomRoomLocationData> CustomRoomLocations { get; set; } = new List<CustomRoomLocationData>()
        {
            new CustomRoomLocationData()
            {
                RoomNameType = RoomName.EzGateA.ToString(),
                OffsetX = 0,
                OffsetY = 1,
                OffsetZ = 0,
                RotationX = 0,
                RotationY = 0,
                RotationZ = 0,
            },
        };

        [Description("This option is for a custom lobby location")]
        public List<CustomLocationData> CustomLocations { get; set; } = new List<CustomLocationData>()
        {
            new CustomLocationData()
            {
                PositionX = 39.262f,
                PositionY = 315f,
                PositionZ = -31.844f,
                RotationX = 0,
                RotationY = 0,
                RotationZ = 0,
            },
        };
    }

    public class LocationData
    {
    }

    public class CustomRoomLocationData : LocationData
    {
        public string RoomNameType { get; set; }
        public float OffsetX { get; set; }
        public float OffsetY { get; set; }
        public float OffsetZ { get; set; }
        public float RotationX { get; set; }
        public float RotationY { get; set; }
        public float RotationZ { get; set; }
    }

    public class CustomLocationData : LocationData
    {
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
        public float RotationX { get; set; }
        public float RotationY { get; set; }
        public float RotationZ { get; set; }
    }
}