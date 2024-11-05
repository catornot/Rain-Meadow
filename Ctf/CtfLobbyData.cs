using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RainMeadow.OnlineResource.ResourceData;
using static RainMeadow.OnlineState;

namespace RainMeadow
{
    internal class CtfLobbyData : OnlineResource.ResourceData
    {
        public List<OnlinePlayer> Militia;

        public List<OnlinePlayer> IMC;

        public CtfLobbyData() { }

        public override ResourceDataState MakeState(OnlineResource resource)
        {
            return new State(this, resource);
        }

        internal class State : ResourceDataState
        {
            [OnlineField]
            public bool isInGame;

            [OnlineField]
            public List<OnlinePlayer> Militia;

            [OnlineField]
            public List<OnlinePlayer> IMC;

            public State(CtfLobbyData ctfLobbyData, OnlineResource onlineResource)
            {
                CTFGameMode gamemode = (onlineResource as Lobby).gameMode as CTFGameMode;
                isInGame = RWCustom.Custom.rainWorld.processManager.currentMainLoop is RainWorldGame;
                Militia = ctfLobbyData.Militia;
                IMC = ctfLobbyData.IMC;
            }

            public override void ReadTo(OnlineResource.ResourceData res, OnlineResource resource)
            {
                CtfLobbyData data = (CtfLobbyData)res;
                CTFGameMode gamemode = (resource as Lobby).gameMode as CTFGameMode;
                gamemode.isInGame = isInGame;

                data.Militia = Militia;
                data.IMC = IMC;
            }

            public override Type GetDataType() => typeof(CtfLobbyData);
        }
    }
}
