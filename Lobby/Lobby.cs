using HarmonyLib;
using LabApi.Events.Handlers;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;
using System;

namespace Lobby
{
    public class Lobby : Plugin<Config>
    {
        public static Lobby Instance { get; private set; }

        public override string Name { get; } = "Lobby";

        public override string Description { get; } = "A plugin that adds a lobby when waiting for players.";

        public override string Author { get; } = "MrAfitol";

        public override Version Version { get; } = new Version(1, 6, 0);

        public override Version RequiredApiVersion { get; } = new Version(LabApiProperties.CompiledVersion);

        public Harmony Harmony { get; private set; }

        public EventsHandler EventsHandler { get; private set; }
        public RestrictionsHandler RestrictionsHandler { get; private set; }

        public override void Enable()
        {
            Instance = this;
            Harmony = new Harmony("lobby.scp.sl");
            EventsHandler = new EventsHandler();
            RestrictionsHandler = new RestrictionsHandler();
            ServerEvents.WaitingForPlayers += EventsHandler.OnWaitingForPlayers;
            ServerEvents.RoundStarted += EventsHandler.OnRoundStarted;
        }

        public override void Disable()
        {
            ServerEvents.WaitingForPlayers -= EventsHandler.OnWaitingForPlayers;
            ServerEvents.RoundStarted -= EventsHandler.OnRoundStarted;
            EventsHandler.UnregisterHandlers();
            RestrictionsHandler = null;
            EventsHandler = null;
            Harmony = null;
            Instance = null;
        }
    }
}