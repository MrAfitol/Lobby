namespace Lobby
{
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using PluginAPI.Events;

    public class Lobby
    {
        public static Lobby Instance { get; private set; }

        [PluginConfig("configs/lobby.yml")]
        public static Config Config;

        [PluginPriority(LoadPriority.Highest)]
        [PluginEntryPoint("Lobby", "1.2.4", "A plugin that adds a lobby when waiting for players.", "MrAfitol")]
        public void LoadPlugin()
        {
            Instance = this;
            EventManager.RegisterEvents<EventHandlers>(this);
        }
    }
}