using HarmonyLib;
using LabApi.Events.CustomHandlers;
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

        public override Version Version { get; } = new Version(1, 5, 1);

        public override Version RequiredApiVersion { get; } = new Version(LabApiProperties.CompiledVersion);

        public Harmony Harmony { get; private set; }

        public EventsHandler EventsHandler { get; private set; }

        public override void Enable()
        {
            Instance = this;
            Harmony = new Harmony("lobby.scp.sl");
            EventsHandler = new EventsHandler();
            CustomHandlersManager.RegisterEventsHandler(EventsHandler);
        }

        public override void Disable()
        {
            CustomHandlersManager.UnregisterEventsHandler(EventsHandler);
            EventsHandler = null;
            Harmony = null;
            Instance = null;
        }
    }
}