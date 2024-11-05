using RainMeadow.Ctf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
        public static void SetTeam(int team, int colorOffset)
        {
            SlugTeam sTeam = (SlugTeam)team;
            CTFGameMode gamemode = (OnlineManager.lobby.gameMode as CTFGameMode);

            RainMeadow.Debug("team assigned: " + sTeam);

            gamemode.ctfClientSettings.team = sTeam;
            gamemode.avatarSettings.bodyColor = new Color(
                (0.5F + colorOffset / 100F) * (sTeam == SlugTeam.IMC ? 1.0F : 0.0F),
                (0.5F + colorOffset / 100F) * (sTeam == SlugTeam.Militia ? 1.0F : 0.0F),
                0.02F,
                0.98F
            );
        }
    }
}
