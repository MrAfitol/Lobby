namespace Lobby
{
    using HarmonyLib;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using PluginAPI.Events;

    public class Lobby
    {
        public static Lobby Instance { get; private set; }

        [PluginConfig("configs/lobby.yml")]
        public static Config Config;

        public Harmony Harmony { get; private set; }

        [PluginPriority(LoadPriority.Highest)]
        [PluginEntryPoint("Lobby", "1.3.1", "A plugin that adds a lobby when waiting for players.", "MrAfitol")]
        public void LoadPlugin()
        {
            Instance = this;
            Harmony = new Harmony("lobby.scp.sl");
            EventManager.RegisterEvents<EventHandlers>(this);
        }
    }
}