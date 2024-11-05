﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace RainMeadow
{
    internal class CtfLobbyData : OnlineResource.ResourceData
    {
        public List<OnlinePlayer> Militia = new List<OnlinePlayer>(32);

        public List<OnlinePlayer> IMC = new List<OnlinePlayer>(32);

        public CtfLobbyData() { }

        public override ResourceDataState MakeState(OnlineResource resource)
        {
            return new State(this, resource);
        }

        internal class State : ResourceDataState
        {
            [OnlineField]
            public bool isInGame;

            [OnlineField(nullable = true)]
            public Generics.DynamicUnorderedUshorts Militia;

            [OnlineField(nullable = true)]
            public Generics.DynamicUnorderedUshorts IMC;

            public State() { }

            public State(CtfLobbyData ctfLobbyData, OnlineResource onlineResource)
            {
                CTFGameMode gamemode = (onlineResource as Lobby).gameMode as CTFGameMode;
                isInGame = RWCustom.Custom.rainWorld.processManager.currentMainLoop is RainWorldGame;
                Militia = new(ctfLobbyData.Militia.Select(p => p.inLobbyId).ToList());
                IMC = new(ctfLobbyData.IMC.Select(p => p.inLobbyId).ToList());
            }

            public override void ReadTo(OnlineResource.ResourceData res, OnlineResource resource)
            {
                CtfLobbyData data = (CtfLobbyData)res;
                CTFGameMode gamemode = (resource as Lobby).gameMode as CTFGameMode;
                gamemode.isInGame = isInGame;

                data.Militia = Militia.list.Select(i => OnlineManager.lobby.PlayerFromId(i)).Where(p => p != null).ToList();
                data.IMC = IMC.list.Select(i => OnlineManager.lobby.PlayerFromId(i)).Where(p => p != null).ToList();
            }

            public override Type GetDataType() => typeof(CtfLobbyData);
        }
    }
}
