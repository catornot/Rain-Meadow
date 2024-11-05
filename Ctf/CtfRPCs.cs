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
        public static void FlagCaptured(int team, int index)
        {
            Lobby lobby = OnlineManager.lobby;
            CTFGameMode gamemode = (lobby.gameMode as CTFGameMode);
            if (lobby.isOwner)
            {
                if (gamemode.ctfdata.scoringIndex <= index)
                    return;

                gamemode.ctfdata.score[team]++;
                gamemode.ctfdata.scoringIndex = index + 1;
            } else
            {
                lobby.owner.InvokeRPC(FlagCaptured, team, index);
            }
        }

        [RPCMethod]
        public static void FlagCapturedSound(int team, int index)
        {

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
