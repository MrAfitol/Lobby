using CentralAuth;
using CustomPlayerEffects;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using LabApi.Features.Wrappers;
using Lobby.API;
using MEC;
using PlayerRoles;
using PlayerRoles.Voice;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Logger = LabApi.Features.Console.Logger;
using Random = UnityEngine.Random;

namespace Lobby
{
    public class EventsHandler
    {
        public static bool IsIntercom { get; set; } = false;
        public static bool IsLobby { get; set; } = true;

        private CoroutineHandle lobbyTimer, rainbowColor;
        private int r = 255, g = 0, b = 0;
        private string text;

        #region Events

        public void OnWaitingForPlayers()
        {
            try
            {
                LobbyLocationHandler.Point = new GameObject("LobbyPoint");
                IsLobby = true;
                IsIntercom = false;
                Lobby.Instance.Harmony.PatchAll();
                RegisterHandlers();
                SpawnManager();

                Timing.CallDelayed(0.1f, () =>
                {
                    GameObject.Find("StartRound").transform.localScale = Vector3.zero;

                    if (lobbyTimer.IsRunning)
                        Timing.KillCoroutines(lobbyTimer);
                    if (rainbowColor.IsRunning)
                        Timing.KillCoroutines(rainbowColor);

                    if (Lobby.Instance.Config.TitleText.Contains("<rainbow>") || Lobby.Instance.Config.PlayerCountText.Contains("<rainbow>"))
                        rainbowColor = Timing.RunCoroutine(RainbowColor());

                    lobbyTimer = Timing.RunCoroutine(LobbyTimer());
                });
            }
            catch (Exception e)
            {
                Logger.Error("[Event: OnWaitingForPlayers] " + e.ToString());
            }
        }

        public void OnPlayerJoined(PlayerJoinedEventArgs ev)
        {
            try
            {
                if (IsLobby && (GameCore.RoundStart.singleton.NetworkTimer > 1 || GameCore.RoundStart.singleton.NetworkTimer == -2))
                {
                    Timing.CallDelayed(1f, () =>
                    {
                        if (!ev.Player.IsOverwatchEnabled)
                        {
                            ev.Player.SetRole(Lobby.Instance.Config.LobbyPlayerRole);

                            ev.Player.IsGodModeEnabled = true;

                            if (Lobby.Instance.Config.LobbyInventory.Count > 0)
                            {
                                foreach (var item in Lobby.Instance.Config.LobbyInventory)
                                {
                                    ev.Player.AddItem(item);
                                }
                            }

                            Timing.CallDelayed(0.1f, () =>
                            {
                                ev.Player.Position = LobbyLocationHandler.Point.transform.position;
                                ev.Player.Rotation = LobbyLocationHandler.Point.transform.rotation;

                                if (Lobby.Instance.Config.EnableMovementBoost)
                                {
                                    ev.Player.EnableEffect<MovementBoost>(Lobby.Instance.Config.MovementBoostIntensity);
                                }
                            });
                        }
                    });
                }
            }
            catch (Exception e)
            {
                Logger.Error("[Event: OnPlayerJoined] " + e.ToString());
            }
        }

        public void OnRoundStarted()
        {
            try
            {
                UnregisterHandlers();
                IsLobby = false;

                if (!string.IsNullOrEmpty(IntercomDisplay._singleton.Network_overrideText)) IntercomDisplay._singleton.Network_overrideText = "";

                foreach (var player in Player.List.Where(x => x.Role != RoleTypeId.Overwatch))
                {
                    player.SetRole(RoleTypeId.Spectator);

                    Timing.CallDelayed(0.1f, () =>
                    {
                        player.IsGodModeEnabled = false;
                        if (Lobby.Instance.Config.EnableMovementBoost) player.DisableEffect<MovementBoost>();
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
                Logger.Error("[Event: OnRoundStarted] " + e.ToString());
            }
        }

        #endregion

        #region Methods

        public void RegisterHandlers()
        {
            try
            {
                PlayerEvents.InteractingDoor += Lobby.Instance.RestrictionsHandler.OnPlayerInteractingDoor;
                PlayerEvents.InteractingElevator += Lobby.Instance.RestrictionsHandler.OnPlayerInteractingElevator;
                PlayerEvents.SearchingPickup += Lobby.Instance.RestrictionsHandler.OnPlayerSearchingPickup;
                PlayerEvents.DroppingItem += Lobby.Instance.RestrictionsHandler.OnPlayerDroppingItem;
                PlayerEvents.DroppingAmmo += Lobby.Instance.RestrictionsHandler.OnPlayerDroppingAmmo;
                PlayerEvents.ThrowingItem += Lobby.Instance.RestrictionsHandler.OnPlayerThrowingItem;
                PlayerEvents.UsingIntercom += Lobby.Instance.RestrictionsHandler.OnPlayerUsingIntercom;
                PlayerEvents.Joined += OnPlayerJoined;
            }
            catch (Exception e)
            {
                Logger.Error("[Lobby] [Method: RegisterHandlers] " + e.ToString());
            }
        }

        public void UnregisterHandlers()
        {
            try
            {
                PlayerEvents.InteractingDoor -= Lobby.Instance.RestrictionsHandler.OnPlayerInteractingDoor;
                PlayerEvents.InteractingElevator -= Lobby.Instance.RestrictionsHandler.OnPlayerInteractingElevator;
                PlayerEvents.SearchingPickup -= Lobby.Instance.RestrictionsHandler.OnPlayerSearchingPickup;
                PlayerEvents.DroppingItem += Lobby.Instance.RestrictionsHandler.OnPlayerDroppingItem;
                PlayerEvents.DroppingAmmo -= Lobby.Instance.RestrictionsHandler.OnPlayerDroppingAmmo;
                PlayerEvents.ThrowingItem -= Lobby.Instance.RestrictionsHandler.OnPlayerThrowingItem;
                PlayerEvents.UsingIntercom -= Lobby.Instance.RestrictionsHandler.OnPlayerUsingIntercom;
                PlayerEvents.Joined -= OnPlayerJoined;
            }
            catch (Exception e)
            {
                Logger.Error("[Lobby] [Method: UnregisterHandlers] " + e.ToString());
            }
        }

        private void SpawnManager()
        {
            try
            {
                List<LocationData> locationList = new List<LocationData>();

                if (Lobby.Instance.Config.LobbyLocation?.Count > 0)
                    locationList.AddRange(Lobby.Instance.Config.LobbyLocation
                        .Where(x => LobbyLocationHandler.LocationDatas.ContainsKey(x))
                        .Select(x => LobbyLocationHandler.LocationDatas[x]));

                if (Lobby.Instance.Config.CustomLocations?.Count > 0)
                    locationList.AddRange(Lobby.Instance.Config.CustomLocations);

                if (Lobby.Instance.Config.CustomRoomLocations?.Count > 0)
                    locationList.AddRange(Lobby.Instance.Config.CustomRoomLocations);

                if (locationList.Count <= 0)
                    locationList.Add(LobbyLocationHandler.LocationDatas.ElementAt(Random.Range(0, LobbyLocationHandler.LocationDatas.Count - 1)).Value);
                LobbyLocationHandler.SetLocation(locationList.RandomItem());
            }
            catch (Exception e)
            {
                Logger.Error("[Lobby] [Method: SpawnManager] " + e.ToString());
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

                if (Lobby.Instance.Config.VerticalPos < 0)
                    for (int i = 0; i < ~Lobby.Instance.Config.VerticalPos; i++)
                        text += "\n";

                text += $"<size={(IsIntercom && Lobby.Instance.Config.DisplayInIcom ? Lobby.Instance.Config.TopTextIcomSize : Lobby.Instance.Config.TopTextSize)}>" + Lobby.Instance.Config.TitleText + "</size>";

                text += "\n" + $"<size={(IsIntercom && Lobby.Instance.Config.DisplayInIcom ? Lobby.Instance.Config.BottomTextIcomSize : Lobby.Instance.Config.BottomTextSize)}>" + Lobby.Instance.Config.PlayerCountText + "</size>";

                short NetworkTimer = GameCore.RoundStart.singleton.NetworkTimer;

                switch (NetworkTimer)
                {
                    case -2: text = text.Replace("{seconds}", Lobby.Instance.Config.ServerPauseText); break;
                    case -1: text = text.Replace("{seconds}", Lobby.Instance.Config.RoundStartText); break;
                    case 1: text = text.Replace("{seconds}", Lobby.Instance.Config.SecondLeftText.Replace("{seconds}", NetworkTimer.ToString())); break;
                    case 0: text = text.Replace("{seconds}", Lobby.Instance.Config.RoundStartText); break;
                    default: text = text.Replace("{seconds}", Lobby.Instance.Config.SecondsLeftText.Replace("{seconds}", NetworkTimer.ToString())); break;
                }

                if (Player.Count == 1)
                    text = text.Replace("{players}", $"{Player.Count} " + Lobby.Instance.Config.PlayerJoinText);
                else
                    text = text.Replace("{players}", $"{Player.Count} " + Lobby.Instance.Config.PlayersJoinText);

                string hex = $"{r:X2}{g:X2}{b:X2}";
                text = text.Replace("<rainbow>", $"<color=#{hex}>");
                text = text.Replace("</rainbow>", "</color>");

                if (Lobby.Instance.Config.VerticalPos >= 0)
                    for (int i = 0; i < Lobby.Instance.Config.VerticalPos; i++)
                        text += "\n";

                if (!IsIntercom || !Lobby.Instance.Config.DisplayInIcom)
                {
                    foreach (Player ply in Player.List)
                        if (ply.ReferenceHub.Mode != ClientInstanceMode.Unverified && ply.ReferenceHub.Mode != ClientInstanceMode.DedicatedServer && ply != null)
                            ply.SendHint(text, 1.05f);
                }
                else IntercomDisplay._singleton.Network_overrideText = $"<size={Lobby.Instance.Config.IcomTextSize}>" + text + "</size>";

                yield return Timing.WaitForSeconds(1f);
            }
        }

        #endregion
    }
}
