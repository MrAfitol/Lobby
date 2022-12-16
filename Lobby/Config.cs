namespace Lobby
{
    using PlayerRoles;

    public class Config
    {
        public string TitleText { get; set; } = "<size=50><color=#F0FF00><b>Waiting for players, {seconds}</b></color></size>";

        public string PlayerCountText { get; set; } = "<size=40><color=#FFA600><i>{players}</i></color></size>";

        public string ServerPauseText { get; set; } = "Server is suspended";

        public string SecondLeftText { get; set; } = "{seconds} second left";

        public string SecondsLeftText { get; set; } = "{seconds} seconds left";

        public string RoundStartText { get; set; } = "Round starting";

        public string PlayerJoinText { get; set; } = "player joined";

        public string PlayersJoinText { get; set; } = "players joined";

        public byte MovementBoostIntensity { get; set; } = 50;

        public RoleTypeId LobbyPlayerRole { get; set; } = RoleTypeId.Tutorial;
    }
}
