using RainMeadow.Ctf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainMeadow
{
    internal class CtfRPCs
    {
        [RPCMethod]
        public static void FlagCaptured(int team)
        {
            var game = (RWCustom.Custom.rainWorld.processManager.currentMainLoop as RainWorldGame);
            
        }

        [RPCMethod]
        public static void SetTeam(int team)
        {
            var game = (RWCustom.Custom.rainWorld.processManager.currentMainLoop as RainWorldGame);

            (OnlineManager.lobby.gameMode as CTFGameMode).ctfClientSettings.team = (SlugTeam)team;
        }
    }
}
