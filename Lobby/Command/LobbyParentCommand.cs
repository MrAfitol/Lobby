using CommandSystem;
using Lobby.Command.ControlCommands;
using Lobby.Command.DebugCommands;
using System;

namespace Lobby.Command
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class LobbyParentCommand : ParentCommand
    {
        public LobbyParentCommand() => LoadGeneratedCommands();

        public override string Command => "lobby";

        public override string[] Aliases { get; } = { "lb", "ly" };

        public override string Description => "Lobby parent command.";

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new AddLocationCommand());
            RegisterCommand(new RemoveLocationCommand());
            RegisterCommand(new LocationListCommand());
            RegisterCommand(new TpLocationCommand());
            RegisterCommand(new GetLocalDataCommand());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "Incorrect command, use: \nlobby addloc\nlobby removeloc\nlobby loclist\nlobby tploc\nlobby getlocaldata";
            return false;
        }
    }
}
