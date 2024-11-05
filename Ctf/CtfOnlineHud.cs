using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainMeadow.Ctf
{
    internal class CtfOnlineHud : OnlineHUD
    {
        public CtfOnlineHud(HUD.HUD hud, RoomCamera camera, OnlineGameMode onlineGameMode) : base(hud, camera, onlineGameMode)
        {
        }

        public override void UpdatePlayers()
        {
            // TODO

            var clientSettings = OnlineManager.lobby.clientSettings.Values.OfType<ClientSettings>();

            SlugTeam team = (OnlineManager.lobby.gameMode as CTFGameMode).ctfClientSettings.team;
            
                // indicators.Select(i => i.clientSettings.);

            var currentSettings = indicators.Select(i => i.clientSettings).ToList();

            clientSettings.Except(currentSettings).Do(PlayerAdded);
            currentSettings.Except(clientSettings).Do(PlayerRemoved);
        }
    }
}
