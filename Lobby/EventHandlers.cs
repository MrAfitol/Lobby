using CentralAuth;
using CustomPlayerEffects;
using Lobby.API;
using MEC;
using PlayerRoles;
using PlayerRoles.Voice;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Lobby
{
    public class EventHandlers
    {
        private CoroutineHandle lobbyTimer;

        private CoroutineHandle rainbowColor;

        private string text;

        public static bool IsIntercom = false;

        public static bool IsLobby = true;

        private int r = 255, g = 0, b = 0;

        [PluginEvent]
        public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
        {
            try
            {
                LobbyLocationHandler.Point = new GameObject("LobbyPoint");
                IsLobby = true;
                Lobby.Instance.Harmony.PatchAll();
                SpawnManager();

                Timing.CallDelayed(0.1f, () =>
                {
                    GameObject.Find("StartRound").transform.localScale = Vector3.zero;

                    if (lobbyTimer.IsRunning)
                        Timing.KillCoroutines(lobbyTimer);
                    if (rainbowColor.IsRunning)
                        Timing.KillCoroutines(rainbowColor);

                    if (Lobby.Config.TitleText.Contains("<rainbow>") || Lobby.Config.PlayerCountText.Contains("<rainbow>"))
                        rainbowColor = Timing.RunCoroutine(RainbowColor());

                    lobbyTimer = Timing.RunCoroutine(LobbyTimer());
                });
            }
            catch (Exception e)
            {
                Log.Error("[Lobby] [Event: OnWaitingForPlayers] " + e.ToString());
            }
        }

        [PluginEvent]
        public void OnPlayerJoin(PlayerJoinedEvent ev)
        {
            try
            {
                if (IsLobby && (GameCore.RoundStart.singleton.NetworkTimer > 1 || GameCore.RoundStart.singleton.NetworkTimer == -2))
                {
                    Timing.CallDelayed(1f, () =>
                    {
                        if (!ev.Player.IsOverwatchEnabled)
                        {
                            ev.Player.SetRole(Lobby.Config.LobbyPlayerRole);

                            ev.Player.IsGodModeEnabled = true;

                            if (Lobby.Config.LobbyInventory.Count > 0)
                            {
                                foreach (var item in Lobby.Config.LobbyInventory)
                                {
                                    ev.Player.AddItem(item);
                                }
                            }

                            Timing.CallDelayed(0.1f, () =>
                            {
                                ev.Player.Position = LobbyLocationHandler.Point.transform.position;
                                ev.Player.Rotation = LobbyLocationHandler.Point.transform.rotation.eulerAngles;

                                if (Lobby.Config.EnableMovementBoost)
                                {
                                    ev.Player.EffectsManager.EnableEffect<MovementBoost>();
                                    ev.Player.EffectsManager.ChangeState<MovementBoost>(Lobby.Config.MovementBoostIntensity);
                                }
                            });
                        }
                    });
                }
            }
            catch (Exception e)
            {
                Log.Error("[Lobby] [Event: OnPlayerJoin] " + e.ToString());
            }
        }

        [PluginEvent]
        public void OnRoundStarted(RoundStartEvent ev)
        {
            try
            {
                IsLobby = false;

                if (!string.IsNullOrEmpty(IntercomDisplay._singleton.Network_overrideText)) IntercomDisplay._singleton.Network_overrideText = "";

                foreach (var player in Player.GetPlayers().Where(x => x.Role != RoleTypeId.Overwatch))
                {
                    player.ClearInventory();
                    player.SetRole(RoleTypeId.Spectator);

                    Timing.CallDelayed(0.1f, () =>
                    {
                        player.IsGodModeEnabled = false;
                        if (Lobby.Config.EnableMovementBoost) player.EffectsManager.DisableEffect<MovementBoost>();
                    });
                }

                Timing.CallDelayed(1f, () =>
                {
                    if (lobbyTimer.IsRunning)
                        Timing.KillCoroutines(lobbyTimer);
                    if (rainbowColor.IsRunning)
                        Timing.KillCoroutines(rainbowColor);
                });

                Lobby.Instance.Harmony.UnpatchAll("lobby.scp.sl");
            }
            catch (Exception e)
            {
                Log.Error("[Lobby] [Event: OnRoundStarted] " + e.ToString());
            }
        }

        [PluginEvent]
        public bool OnPlayerInteractDoor(PlayerInteractDoorEvent ev)
        {
            if (IsLobby)
            {
                ev.CanOpen = false;
                return false;
            }

            return true;
        }

        [PluginEvent]
        public bool OnPlayerInteractElevator(PlayerInteractElevatorEvent ev)
        {
            if (IsLobby)
                return false;

            return true;
        }

        [PluginEvent]
        public bool OnSearchPickup(PlayerSearchPickupEvent ev)
        {
            if (IsLobby)
                return false;
            return true;
        }

        [PluginEvent]
        public bool OnPlayerDropItem(PlayerDropItemEvent ev)
        {
            if (IsLobby)
                return false;

            return true;
        }

        [PluginEvent]
        public bool OnPlayerDropAmmo(PlayerDropAmmoEvent ev)
        {
            if (IsLobby)
                return false;

            return true;
        }

        [PluginEvent]
        public bool OnThrowItem(PlayerThrowItemEvent ev)
        {
            if (IsLobby)
                return false;

            return true;
        }

        [PluginEvent]
        public bool OnPlayerUsingIntercom(PlayerUsingIntercomEvent ev)
        {
            if (IsLobby && !Lobby.Config.AllowIcom)
                return false;
            return true;
        }

        public void SpawnManager()
        {
            try
            {
                List<LocationData> locationList = new List<LocationData>();

                if (Lobby.Config.LobbyLocation?.Count > 0)
                    foreach (var item in Lobby.Config.LobbyLocation)
                        locationList.Add(LobbyLocationHandler.LocationDatas[item]);

                if (Lobby.Config.CustomRoomLocations?.Count > 0)
                    foreach (var item in Lobby.Config.CustomRoomLocations)
                        locationList.Add(item);

                if (Lobby.Config.CustomLocations?.Count > 0)
                    foreach (var item in Lobby.Config.CustomLocations)
                        locationList.Add(item);

                if (locationList.Count <= 0)
                    locationList.Add(LobbyLocationHandler.LocationDatas.ElementAt(Random.Range(0, LobbyLocationHandler.LocationDatas.Count - 1)).Value);

                LobbyLocationHandler.SetLocation(locationList.RandomItem());
            }
            catch (Exception e)
            {
                Log.Error("[Lobby] [Method: SpawnManager] " + e.ToString());
            }
        }

        private IEnumerator<float> RainbowColor()
        {
            r = 255; g = 0; b = 0;

            while (!Round.IsRoundStarted)
            {
                if (r > 0 && b == 0)
                {
                    r -= 2;
                    g += 2;
                }

                if (g > 0 && r == 0)
                {
                    g -= 2;
                    b += 2;
                }

                if (b > 0 && g == 0)
                {
                    b -= 2;
                    r += 2;
                }

                r = Mathf.Clamp(r, 0, 255);
                g = Mathf.Clamp(g, 0, 255);
                b = Mathf.Clamp(b, 0, 255);

                yield return Timing.WaitForSeconds(0.4f);
            }
        }

        private IEnumerator<float> LobbyTimer()
        {
            while (!Round.IsRoundStarted)
            {
                text = string.Empty;

                if (Lobby.Config.VerticalPos < 0)
                    for (int i = 0; i < ~Lobby.Config.VerticalPos; i++)
                        text += "\n";

                text += $"<size={(IsIntercom && Lobby.Config.DisplayInIcom ? Lobby.Config.TopTextIcomSize : Lobby.Config.TopTextSize)}>" + Lobby.Config.TitleText + "</size>";

                text += "\n" + $"<size={(IsIntercom && Lobby.Config.DisplayInIcom ? Lobby.Config.BottomTextIcomSize : Lobby.Config.BottomTextSize)}>" + Lobby.Config.PlayerCountText + "</size>";

                short NetworkTimer = GameCore.RoundStart.singleton.NetworkTimer;

                switch (NetworkTimer)
                {
                    case -2: text = text.Replace("{seconds}", Lobby.Config.ServerPauseText); break;

                    case -1: text = text.Replace("{seconds}", Lobby.Config.RoundStartText); break;

                    case 1: text = text.Replace("{seconds}", Lobby.Config.SecondLeftText.Replace("{seconds}", NetworkTimer.ToString())); break;

                    case 0: text = text.Replace("{seconds}", Lobby.Config.RoundStartText); break;

                    default: text = text.Replace("{seconds}", Lobby.Config.SecondsLeftText.Replace("{seconds}", NetworkTimer.ToString())); break;
                }

                if (Player.GetPlayers().Count() == 1)
                {
                    text = text.Replace("{players}", $"{Player.GetPlayers().Count()} " + Lobby.Config.PlayerJoinText);
                }
                else
                {
                    text = text.Replace("{players}", $"{Player.GetPlayers().Count()} " + Lobby.Config.PlayersJoinText);
                }

                string hex = $"{r:X2}{g:X2}{b:X2}";
                text = text.Replace("<rainbow>", $"<color=#{hex}>");
                text = text.Replace("</rainbow>", "</color>");

                if (Lobby.Config.VerticalPos >= 0)
                    for (int i = 0; i < Lobby.Config.VerticalPos; i++)
                        text += "\n";

                if (!IsIntercom || !Lobby.Config.DisplayInIcom)
                {
                    foreach (Player ply in Player.GetPlayers())
                    {
                        if (ply.ReferenceHub.Mode != ClientInstanceMode.Unverified && ply != null)
                        {
                            ply.ReceiveHint(text, 1.1f);
                        }
                    }
                }
                else
                {
                    IntercomDisplay._singleton.Network_overrideText = $"<size={Lobby.Config.IcomTextSize}>" + text + "</size>";
                }

                yield return Timing.WaitForSeconds(1f);
            }
        }
    }
}