using RainMeadow.Ctf;
using System;
using UnityEngine;
using static RainMeadow.OnlineEntity.EntityData;
using static RainMeadow.OnlineState;

namespace RainMeadow
{
    public class CTFClientSettings : OnlineEntity.EntityData
    {
        public SlugcatStats.Name playingAs = SlugcatStats.Name.White;
        public SlugTeam team = SlugTeam.Unassigned;

        public override EntityDataState MakeState(OnlineEntity onlineEntity, OnlineResource inResource)
        {
            return new State(this);
        }

        public class State : EntityDataState
        {
            [OnlineField]
            public int team = 0;

            public State() { }
            public State(CTFClientSettings storyClient) : base()
            {
                team = (int)storyClient.team;
            }

            public override Type GetDataType()
            {
                return typeof(CTFClientSettings);
            }

            public override void ReadTo(OnlineEntity.EntityData data, OnlineEntity onlineEntity)
            {
                var storyClientData = (CTFClientSettings)data;
                storyClientData.team = (SlugTeam)team;
            }
        }
    }
}