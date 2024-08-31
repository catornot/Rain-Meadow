﻿using System.Linq;
using System.Runtime.CompilerServices;

namespace RainMeadow
{
    public partial class RoomSession : OnlineResource
    {
        public AbstractRoom absroom;
        public bool abstractOnDeactivate;
        public static ConditionalWeakTable<AbstractRoom, RoomSession> map = new();

        public WorldSession worldSession => super as WorldSession;
        public World World => worldSession.world;

        public RoomSession(WorldSession ws, AbstractRoom absroom) : base(ws)
        {
            this.absroom = absroom;
            map.Add(absroom, this);
        }

        protected override void AvailableImpl()
        {
            if (isOwner)
            {
                foreach (var ent in absroom.entities.Concat(absroom.entitiesInDens))
                {
                    if (ent is AbstractPhysicalObject apo && OnlinePhysicalObject.map.TryGetValue(apo, out var oe)
                         && !oe.realized && !oe.isMine && oe.isTransferable && !oe.isPending)
                    {
                        oe.Request(); // I am realizing this entity, let me have it
                    }
                }
            }
        }

        protected override void ActivateImpl()
        {
            foreach (var ent in absroom.entities.Concat(absroom.entitiesInDens))
            {
                if (ent is AbstractPhysicalObject apo)
                {
                    worldSession.ApoEnteringWorld(apo);
                    ApoEnteringRoom(apo, apo.pos);
                }
            }
        }

        protected override void DeactivateImpl()
        {
            if (abstractOnDeactivate && absroom.realizedRoom != null)
            {
                foreach (AbstractWorldEntity? item in absroom.entities.Concat(absroom.entitiesInDens))
                {
                    if (item is AbstractPhysicalObject apo && OnlinePhysicalObject.map.TryGetValue(apo, out var ent))
                    {
                        ent.beingMoved = true;
                    }
                }
                absroom.world.loadingRooms.RemoveAll(rl => rl.room == absroom.realizedRoom);
                absroom.Abstractize();
                foreach (AbstractWorldEntity? item in absroom.entities.Concat(absroom.entitiesInDens))
                {
                    if (item is AbstractPhysicalObject apo && OnlinePhysicalObject.map.TryGetValue(apo, out var ent))
                    {
                        ent.beingMoved = false;
                    }
                }
                abstractOnDeactivate = false;
            }
        }

        public override string Id()
        {
            return super.Id() + absroom.name;
        }

        public override ushort ShortId()
        {
            return (ushort)absroom.index;
        }

        public override OnlineResource SubresourceFromShortId(ushort shortId)
        {
            return this.subresources[shortId];
        }

        protected override ResourceState MakeState(uint ts)
        {
            return new RoomState(this, ts);
        }

        public class RoomState : ResourceState
        {
            public RoomState() : base() { }
            public RoomState(RoomSession resource, uint ts) : base(resource, ts) { }
        }

        public override string ToString()
        {
            return "Room " + Id();
        }
    }
}
