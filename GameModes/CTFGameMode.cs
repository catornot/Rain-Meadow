using System.Collections.Generic;
using System.Linq;

namespace RainMeadow
{
    public class CTFGameMode : OnlineGameMode
    {
        private int redTeamAmount = 0;
        private int blueTeamAmount = 0;
        private static HashSet<PlacedObject.Type> ItemBlackList = new HashSet<PlacedObject.Type>
        {
            PlacedObject.Type.DangleFruit,
            PlacedObject.Type.DeadHazer,
            PlacedObject.Type.VultureGrub,
            PlacedObject.Type.DeadVultureGrub,
        };
        public CTFClientSettings ctfClientSettings => clientSettings as CTFClientSettings;

        public CTFGameMode(Lobby lobby) : base(lobby)
        {
        }


        public override bool AllowedInMode(PlacedObject item)
        {
            //return (
            //    OnlineGameModeHelpers.PlayerGrablableItems.Contains(item.type) ||
            //    OnlineGameModeHelpers.creatureRelatedItems.Contains(item.type) ||
            //    OnlineGameModeHelpers.cosmeticItems.Contains(item.type)
            //) && !ItemBlackList.Contains(item.type);
            return !ItemBlackList.Contains(item.type);
        }
        public override bool ShouldSpawnRoomItems(RainWorldGame game, RoomSession roomSession)
        {
            return roomSession.isOwner;
        }
        public override bool ShouldLoadCreatures(RainWorldGame game, WorldSession worldSession)
        {
            return worldSession.owner == null || worldSession.isOwner;
        }

        public override bool ShouldSyncObjectInWorld(WorldSession ws, AbstractPhysicalObject apo)
        {
            return true;
        }

        public override bool ShouldSyncObjectInRoom(RoomSession rs, AbstractPhysicalObject apo)
        {
            return true;
        }

        public override bool ShouldSpawnFly(FliesWorldAI self, int spawnRoom)
        {
            return false;
        }

        public override SlugcatStats.Name GetStorySessionPlayer(RainWorldGame self)
        {
            return RainMeadow.Ext_SlugcatStatsName.OnlineSessionPlayer;
        }

        public override SlugcatStats.Name LoadWorldAs(RainWorldGame game)
        {
            return SlugcatStats.Name.Red;
        }

        public override ProcessManager.ProcessID MenuProcessId()
        {
            return RainMeadow.Ext_ProcessID.CtfLobbyMenu;
        }

        public override AbstractCreature SpawnAvatar(RainWorldGame self, WorldCoordinate location)
        {
            return null; // game runs default code
        }

        internal override void NewEntity(OnlineEntity oe, OnlineResource inResource)
        {

        }

        internal override void AddAvatarSettings()
        {
            RainMeadow.Debug("Adding avatar settings for ctf!");
            clientSettings = new CTFClientSettings(new OnlineEntity.EntityId(OnlineManager.mePlayer.inLobbyId, OnlineEntity.EntityId.IdType.settings, 0), OnlineManager.mePlayer);
            clientSettings.EnterResource(lobby);
        }

        internal override void SetAvatar(OnlineCreature onlineCreature)
        {
            RainMeadow.Debug(onlineCreature);
            this.avatar = onlineCreature;
            this.clientSettings.avatarId = onlineCreature.id;
        }

        internal override void ResourceAvailable(OnlineResource onlineResource)
        {

        }

        internal override void ResourceActive(OnlineResource onlineResource)
        {
            if (onlineResource is Lobby)
            {
                AddAvatarSettings();
                OnlineManager.instance.manager.RequestMainProcessSwitch(MenuProcessId());
            }
        }

        public override bool PlayerCanOwnResource(OnlinePlayer from, OnlineResource onlineResource)
        {
            if (onlineResource is WorldSession)
            {
                return lobby.owner == from;
            }
            return true;
        }

        public override void LobbyReadyCheck()
        {

        }
        internal override void PlayerLeftLobby(OnlinePlayer player)
        {
            base.PlayerLeftLobby(player);
            if (player == lobby.owner)
            {
                OnlineManager.instance.manager.RequestMainProcessSwitch(ProcessManager.ProcessID.MainMenu);
            }
        }

        internal override void NewPlayerInLobby(OnlinePlayer player)
        {

        }

        internal override void LobbyTick(uint tick)
        {


        }

        //internal override void Customize(Creature creature, OnlineCreature oc)
        //{
        //    if (lobby.playerAvatars.Any(a => a.Value == oc.id))
        //        if (lobby.playerAvatars.Any(a => a.Value == oc.id))
        //        {
        //            RainMeadow.Debug($"Customizing avatar {creature} for {oc.owner}");
        //            var settings = lobby.activeEntities.First(em => em is ClientSettings avs && avs.avatarId == oc.id) as ClientSettings;

        //            // this adds the entry in the CWT
        //            var mcc = RainMeadow.creatureCustomizations.GetValue(creature, (c) => settings.MakeCustomization());

        //            // todo one day come back to making emote support universal
        //            //if (oc.TryGetData<MeadowCreatureData>(out var mcd))
        //            //{
        //            //    EmoteDisplayer.map.GetValue(creature, (c) => new EmoteDisplayer(creature, oc, mcd, mcc));
        //            //}
        //            //else
        //            //{
        //            //    RainMeadow.Error("missing mcd?? " + oc);
        //            //}
        //        }
        //}
    }
}