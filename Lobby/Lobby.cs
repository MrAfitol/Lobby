namespace Lobby
{
    using PluginAPI.Core.Attributes;
    using PluginAPI.Core;
    using PluginAPI.Enums;
    using PluginAPI.Events;

    public class Lobby
    {
        public static Lobby Instance { get; private set; }

        [PluginConfig("configs/lobby.yml")]
        public Config Config;

        [PluginPriority(LoadPriority.Highest)]
        [PluginEntryPoint("Lobby", "1.0.3", "A plugin that adds a lobby when waiting for players.", "MrAfitol")]
        void LoadPlugin()
        {
            Instance = this;
            EventManager.RegisterEvents<EventHandlers>(this);

            var handler = PluginHandler.Get(this);
            handler.SaveConfig(this, nameof(Config));
        }
    }
}