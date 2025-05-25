using LabApi.Events.Arguments.PlayerEvents;

namespace Lobby
{
    public class RestrictionsHandler
    {
        private bool IsLobby => EventsHandler.IsLobby;

        public void OnPlayerInteractingDoor(PlayerInteractingDoorEventArgs ev) => ev.IsAllowed = !IsLobby;

        public void OnPlayerInteractingElevator(PlayerInteractingElevatorEventArgs ev)
        {
            if (IsLobby) ev.IsAllowed = false;
        }

        public void OnPlayerSearchingPickup(PlayerSearchingPickupEventArgs ev) => ev.IsAllowed = !IsLobby;

        public void OnPlayerDroppingItem(PlayerDroppingItemEventArgs ev) => ev.IsAllowed = !IsLobby;

        public void OnPlayerDroppingAmmo(PlayerDroppingAmmoEventArgs ev) => ev.IsAllowed = !IsLobby;

        public void OnPlayerThrowingItem(PlayerThrowingItemEventArgs ev) => ev.IsAllowed = !IsLobby;

        public void OnPlayerUsingIntercom(PlayerUsingIntercomEventArgs ev) => ev.IsAllowed = (IsLobby && !Lobby.Instance.Config.AllowIcom ? false : true);
    }
}