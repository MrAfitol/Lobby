namespace Lobby
{
    using CustomPlayerEffects;
    using Interactables.Interobjects.DoorUtils;
    using InventorySystem.Items;
    using InventorySystem.Items.Pickups;
    using MEC;
    using PlayerRoles;
    using PlayerRoles.Voice;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class EventHandlers
    {
        private CoroutineHandle lobbyTimer;

        private string text;

        private LobbyLocationType curLobbyLocationType;

        public static bool IsLobby = true;

        [PluginEvent(ServerEventType.WaitingForPlayers)]
        public void OnWaitingForPlayers()
        {
            try
            {
                LobbyLocationHandler.Point = new GameObject("LobbyPoint");
                IsLobby = true;
                SpawnManager();

                Timing.CallDelayed(0.1f, () => {
                    GameObject.Find("StartRound").transform.localScale = Vector3.zero;

                    if (lobbyTimer.IsRunning)
                    {
                        Timing.KillCoroutines(lobbyTimer);
                    }

                    if (curLobbyLocationType == LobbyLocationType.Intercom && Lobby.Instance.Config.DisplayInIcom) lobbyTimer = Timing.RunCoroutine(LobbyIcomTimer());
                    else lobbyTimer = Timing.RunCoroutine(LobbyTimer());
                });
            }
            catch (Exception e)
            {
                Log.Error("[Lobby] [Event: OnWaitingForPlayers] " + e.ToString());
            }
        }

        [PluginEvent(ServerEventType.PlayerJoined)]
        public void OnPlayerJoin(Player player)
        {
            try
            {
                if (IsLobby && (GameCore.RoundStart.singleton.NetworkTimer > 1 || GameCore.RoundStart.singleton.NetworkTimer == -2))
                {
                    Timing.CallDelayed(1f, () =>
                    {
                        player.SetRole(Lobby.Instance.Config.LobbyPlayerRole);

                        player.IsGodModeEnabled = true;

                        if (Lobby.Instance.Config.LobbyInventory.Count > 0)
                        {
                            foreach (var item in Lobby.Instance.Config.LobbyInventory)
                            {
                                player.AddItem(item);
                            }
                        }

                        Timing.CallDelayed(0.1f, () =>
                        {
                            player.Position = LobbyLocationHandler.Point.transform.position;
                            player.Rotation = LobbyLocationHandler.Point.transform.rotation.eulerAngles;

                            if (Lobby.Instance.Config.EnableMovementBoost)
                            {
                                player.EffectsManager.EnableEffect<MovementBoost>();
                                player.EffectsManager.ChangeState<MovementBoost>(Lobby.Instance.Config.MovementBoostIntensity);
                            }
                            if (Lobby.Instance.Config.InfinityStamina) player.EffectsManager.EnableEffect<Invigorated>();
                        });
                    });
                }
            }
            catch (Exception e)
            {
                Log.Error("[Lobby] [Event: OnPlayerJoin] " + e.ToString());
            }
        }

        public void SpawnManager()
        {
            try
            {
                if (Lobby.Instance.Config.LobbyLocation.Count <= 0)
                {
                    LobbyLocationHandler.TowerLocation();
                    return;
                }

                curLobbyLocationType = Lobby.Instance.Config.LobbyLocation.RandomItem();

                switch (curLobbyLocationType)
                {
                    case LobbyLocationType.Tower:
                        LobbyLocationHandler.TowerLocation();
                        break;
                    case LobbyLocationType.Intercom:
                        LobbyLocationHandler.IntercomLocation();
                        break;
                    case LobbyLocationType.GR18:
                        LobbyLocationHandler.GRLocation();
                        break;
                    case LobbyLocationType.SCP173:
                        LobbyLocationHandler.SCP173Location();
                        break;
                    default:
                        LobbyLocationHandler.TowerLocation();
                        break;
                }
            }
            catch(Exception e)
            {
                Log.Error("[Lobby] [Method: SpawnManager] " + e.ToString());
            }
        }

        [PluginEvent(ServerEventType.RoundStart)]
        public void OnRoundStarted()
        {
            try
            {
                IsLobby = false;

                if (!string.IsNullOrEmpty(IntercomDisplay._singleton.Network_overrideText)) IntercomDisplay._singleton.Network_overrideText = "";

                foreach (var player in Player.GetPlayers())
                {
                    if (player.Role != RoleTypeId.Overwatch) player.SetRole(RoleTypeId.Spectator);

                    Timing.CallDelayed(0.1f, () =>
                    {
                        player.IsGodModeEnabled = false;
                        if (Lobby.Instance.Config.EnableMovementBoost) player.EffectsManager.DisableEffect<MovementBoost>();
                        if (Lobby.Instance.Config.InfinityStamina) player.EffectsManager.DisableEffect<Invigorated>();
                    });
                }
            }
            catch (Exception e)
            {
                Log.Error("[Lobby] [Event: OnRoundStarted] " + e.ToString());
            }
        }

        [PluginEvent(ServerEventType.PlayerInteractDoor)]
        public bool OnPlayerInteractDoor(Player ply, DoorVariant door, bool canOpen)
        {
            if (IsLobby)
            {
                canOpen = false;
                return false;
            }

            return true;
        }

        [PluginEvent(ServerEventType.PlayerSearchPickup)]
        public bool OnSearchPickup(Player player, ItemPickupBase pickup)
        {
            if (IsLobby)
            {
                return false;
            }

            return true;
        }

        [PluginEvent(ServerEventType.PlayerDropItem)]
        public bool OnPlayerDroppedItem(Player player, ItemBase item)
        {
            if (IsLobby)
            {
                return false;
            }

            return true;
        }

        [PluginEvent(ServerEventType.PlayerThrowItem)]
        public bool OnThrowItem(Player player, ItemBase item, Rigidbody rb)
        {
            if (IsLobby)
            {
                return false;
            }

            return true;
        }

        [PluginEvent(ServerEventType.PlayerUsingIntercom)]
        public bool OnPlayerUsingIntercom(Player player, IntercomState state)
        {
            if (IsLobby && !Lobby.Instance.Config.AllowIcom) return false;
            return true;
        }

        private IEnumerator<float> LobbyTimer()
        {
            while (!Round.IsRoundStarted)
            {
                text = string.Empty;

                if (Lobby.Instance.Config.VerticalPos < 0)
                    for (int i = 0; i < ~Lobby.Instance.Config.VerticalPos; i++)
                        text += "\n";

                text += $"<size={Lobby.Instance.Config.TopTextSize}>" + Lobby.Instance.Config.TitleText + "</size>";

                text += "\n" + $"<size={Lobby.Instance.Config.BottomTextSize}>" + Lobby.Instance.Config.PlayerCountText + "</size>";

                short NetworkTimer = GameCore.RoundStart.singleton.NetworkTimer;

                switch (NetworkTimer)
                {
                    case -2: text = text.Replace("{seconds}", Lobby.Instance.Config.ServerPauseText); break;

                    case -1: text = text.Replace("{seconds}", Lobby.Instance.Config.RoundStartText); break;

                    case 1: text = text.Replace("{seconds}", Lobby.Instance.Config.SecondLeftText.Replace("{seconds}", NetworkTimer.ToString())); break;

                    case 0: text = text.Replace("{seconds}", Lobby.Instance.Config.RoundStartText); break;

                    default: text = text.Replace("{seconds}", Lobby.Instance.Config.SecondsLeftText.Replace("{seconds}", NetworkTimer.ToString())); break;
                }

                if (Player.GetPlayers().Count() == 1)
                {
                    text = text.Replace("{players}", $"{Player.GetPlayers().Count()} " + Lobby.Instance.Config.PlayerJoinText);
                }
                else
                {
                    text = text.Replace("{players}", $"{Player.GetPlayers().Count()} " + Lobby.Instance.Config.PlayersJoinText);
                }

                if (!Lobby.Instance.Config.UseBC && Lobby.Instance.Config.VerticalPos >= 0)
                    for (int i = 0; i < Lobby.Instance.Config.VerticalPos; i++)
                        text += "\n";

                foreach (Player ply in Player.GetPlayers())
                {
                    if (Lobby.Instance.Config.UseBC) ply.SendBroadcast(text.ToString(), 1, Broadcast.BroadcastFlags.Normal, Lobby.Instance.Config.ClearPrevBC);
                    else ply.ReceiveHint(text.ToString(), 1f);
                }

                yield return Timing.WaitForSeconds(1f);
            }
        }

        private IEnumerator<float> LobbyIcomTimer()
        {
            while (!Round.IsRoundStarted)
            {
                text = string.Empty;

                text += $"<size={Lobby.Instance.Config.TopTextIcomSize}>" + Lobby.Instance.Config.TitleText + "</size>";

                text += "\n" + $"<size={Lobby.Instance.Config.BottomTextIcomSize}>" + Lobby.Instance.Config.PlayerCountText + "</size>";

                short NetworkTimer = GameCore.RoundStart.singleton.NetworkTimer;

                switch (NetworkTimer)
                {
                    case -2: text = text.Replace("{seconds}", Lobby.Instance.Config.ServerPauseText); break;

                    case -1: text = text.Replace("{seconds}", Lobby.Instance.Config.RoundStartText); break;

                    case 1: text = text.Replace("{seconds}", Lobby.Instance.Config.SecondLeftText.Replace("{seconds}", NetworkTimer.ToString())); break;

                    case 0: text = text.Replace("{seconds}", Lobby.Instance.Config.RoundStartText); break;

                    default: text = text.Replace("{seconds}", Lobby.Instance.Config.SecondsLeftText.Replace("{seconds}", NetworkTimer.ToString())); break;
                }

                if (Player.GetPlayers().Count() == 1)
                {
                    text = text.Replace("{players}", $"{Player.GetPlayers().Count()} " + Lobby.Instance.Config.PlayerJoinText);
                }
                else
                {
                    text = text.Replace("{players}", $"{Player.GetPlayers().Count()} " + Lobby.Instance.Config.PlayersJoinText);
                }

                for (int i = 0; i < 25; i++)
                {
                    text += "\n";
                }

                IntercomDisplay._singleton.Network_overrideText = $"<size={Lobby.Instance.Config.IcomTextSize}>" + text + "</size>";

                yield return Timing.WaitForSeconds(1f);
            }
        }
    }
}
