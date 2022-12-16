namespace Lobby
{
    using CustomPlayerEffects;
    using MEC;
    using PlayerRoles;
    using PluginAPI.Core;
    using PluginAPI.Core.Attributes;
    using PluginAPI.Enums;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class EventHandlers
    {
        private Vector3 LobbiPos;

        private CoroutineHandle lobbyTimer;

        private string text;

        public static bool IsLobby = true;

        [PluginEvent(ServerEventType.WaitingForPlayers)]
        public void OnWaitingForPlayers()
        {
            IsLobby = true;

            SpawnManager();

            GameObject.Find("StartRound").transform.localScale = Vector3.zero;

            if (lobbyTimer.IsRunning)
            {
                Timing.KillCoroutines(lobbyTimer);
            }

            lobbyTimer = Timing.RunCoroutine(LobbyTimer());
        }

        [PluginEvent(ServerEventType.PlayerJoined)]
        public void OnPlayerJoin(Player player)
        {
            if (IsLobby && (GameCore.RoundStart.singleton.NetworkTimer > 1 || GameCore.RoundStart.singleton.NetworkTimer == -2))
            {
                Timing.CallDelayed(0.1f, () =>
                {
                    player.SetRole(Lobby.Instance.Config.LobbyPlayerRole);

                    player.IsGodModeEnabled = true;
                });

                Timing.CallDelayed(0.5f, () =>
                {

                    player.Position = LobbiPos;

                    player.EffectsManager.EnableEffect<MovementBoost>();
                    player.EffectsManager.ChangeState<MovementBoost>(Lobby.Instance.Config.MovementBoostIntensity);
                });
            }
        }

        public void SpawnManager()
        {
            int rndRoom = Random.Range(1, 6);

            switch (rndRoom)
            {
                case 1: LobbiPos = new Vector3(162.893f, 1019.470f, -13.430f); break;
                case 2: LobbiPos = new Vector3(107.698f, 1014.048f, -12.555f); break;
                case 3: LobbiPos = new Vector3(39.262f, 1014.112f, -31.844f); break;
                case 4: LobbiPos = new Vector3(-15.854f, 1014.461f, -31.543f); break;
                case 5: LobbiPos = new Vector3(130.483f, 993.366f, 20.601f); break;
                default: LobbiPos = new Vector3(39.262f, 1014.112f, -31.844f); break;
            }
        }

        [PluginEvent(ServerEventType.RoundStart)]
        public void OnRoundStarted()
        {
            foreach (var item in Player.GetPlayers())
            {
                item.SetRole(RoleTypeId.None);
            }

            IsLobby = false;
            Timing.CallDelayed(0.25f, () =>
            {
                foreach (Player ply in Player.GetPlayers())
                {

                    ply.IsGodModeEnabled = false;
                    ply.EffectsManager.DisableEffect<MovementBoost>();
                }
            });
        }

        private IEnumerator<float> LobbyTimer()
        {
            while (!Round.IsRoundStarted)
            {
                text = string.Empty;

                if (25 != 0 && 25 < 0)
                {
                    for (int i = 25; i < 0; i++)
                    {
                        text += "\n";
                    }
                }

                text += Lobby.Instance.Config.TitleText;

                text += "\n" + Lobby.Instance.Config.PlayerCountText;

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

                if (25 != 0 && 25 > 0)
                {
                    for (int i = 0; i < 25; i++)
                    {
                        text += "\n";
                    }
                }

                foreach (Player ply in Player.GetPlayers())
                {
                    ply.ReceiveHint(text.ToString(), 1f);
                }

                yield return Timing.WaitForSeconds(1f);
            }
        }
    }
}
