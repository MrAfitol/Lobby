using PluginAPI.Core;
using System;
using System.Linq;

namespace Lobby.Extensions
{
    public static class PlayerExtensions
    {
        public static bool IsAllowCommand(this Player player) => player.IsAllowFromRank() || player.IsAllowFromUserID();

        public static bool IsAllowFromUserID(this Player player)
        {
            if (!string.IsNullOrEmpty(player.UserId))
                if (Lobby.Config.AllowedUserID?.Count > 0)
                    if (Lobby.Config.AllowedUserID.Contains(player.UserId))
                        return true;
            return false;
        }

        public static bool IsAllowFromRank(this Player player)
        {
            if (!string.IsNullOrEmpty(player.GetGroupName()))
                if (Lobby.Config.AllowedRank?.Count > 0)
                    if (Lobby.Config.AllowedRank.Contains(player.GetGroupName()))
                        return true;
            return false;
        }

        public static string GetGroupName(this Player player)
        {
            try
            {
                if (player.UserId == null) return string.Empty;

                if (ServerStatic.PermissionsHandler._members.ContainsKey(player.UserId))
                {
                    return ServerStatic.PermissionsHandler._members[player.UserId];
                }
                else
                {
                    return player.ReferenceHub.serverRoles.Group != null ? ServerStatic.GetPermissionsHandler()._groups.FirstOrDefault(g => EqualsTo(g.Value, player.ReferenceHub.serverRoles.Group)).Key : string.Empty;
                }
            }
            catch (Exception e)
            {
                Log.Error("[Lobby] [Event: GetPlayerGroupName] " + e.ToString());
                return string.Empty;
            }
        }

        private static bool EqualsTo(UserGroup check, UserGroup player)
            => check.BadgeColor == player.BadgeColor
               && check.BadgeText == player.BadgeText
               && check.Permissions == player.Permissions
               && check.Cover == player.Cover
               && check.HiddenByDefault == player.HiddenByDefault
               && check.Shared == player.Shared
               && check.KickPower == player.KickPower
               && check.RequiredKickPower == player.RequiredKickPower;
    }
}
