using RainMeadow.Ctf;
using System.Collections.Generic;
using System.Linq;

namespace RainMeadow
{
    public class CTFGameMode : OnlineGameMode
    {
        private int ImcTeamAmount = 0;
        private int MilitiaTeamAmount = 0;
        public bool IsInGame = true;

        private static HashSet<PlacedObject.Type> ItemBlackList = new HashSet<PlacedObject.Type>
        {
            PlacedObject.Type.DangleFruit,
            PlacedObject.Type.DeadHazer,
            PlacedObject.Type.VultureGrub,
            PlacedObject.Type.DeadVultureGrub,
            PlacedObject.Type.KarmaFlower,
            PlacedObject.Type.Mushroom,
        };

        public CTFClientSettings ctfClientSettings;
        public SlugcatCustomization avatarSettings;

        public CTFGameMode(Lobby lobby) : base(lobby)
        {
            avatarSettings = new SlugcatCustomization() { nickname = OnlineManager.mePlayer.id.name };
            ctfClientSettings = new CTFClientSettings();
            ctfClientSettings.playingAs = SlugcatStats.Name.White;
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

        public virtual bool ShouldRegisterAPO(OnlineResource resource, AbstractPhysicalObject apo)
        {
            return true;
        }

        public virtual bool ShouldSyncAPOInWorld(WorldSession ws, AbstractPhysicalObject apo)
        {
            return true;
        }

        public virtual bool ShouldSyncAPOInRoom(RoomSession rs, AbstractPhysicalObject apo)
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
            game.manager.rainWorld.options.friendlyFire = true;
            game.manager.rainWorld.options.friendlySteal = true;
            game.manager.rainWorld.options.fpsCap = 120;
            game.manager.rainWorld.options.dlcTutorialShown = false;
            game.manager.rainWorld.options.friendlyLizards = false;
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

        public override void NewEntity(OnlineEntity oe, OnlineResource inResource)
        {
            base.NewEntity(oe, inResource);
        }

        public override void ConfigureAvatar(OnlineCreature onlineCreature)
        {
            RainMeadow.Debug(onlineCreature);
            onlineCreature.AddData(avatarSettings);
        }

        public override void ResourceAvailable(OnlineResource onlineResource)
        {
            base.ResourceAvailable(onlineResource);
        }

        public override void ResourceActive(OnlineResource onlineResource)
        {
            base.ResourceActive(onlineResource);
        }

        public override bool PlayerCanOwnResource(OnlinePlayer from, OnlineResource onlineResource)
        {
            if (onlineResource is WorldSession)
            {
                return lobby.owner == from;
            }
            return true;
        }

        public override void PlayerLeftLobby(OnlinePlayer player)
        {
            base.PlayerLeftLobby(player);
            if (player == lobby.owner)
            {
                OnlineManager.instance.manager.RequestMainProcessSwitch(ProcessManager.ProcessID.MainMenu);
            }
        }

        public override void NewPlayerInLobby(OnlinePlayer player)
        {
            if (!lobby.isOwner)
                return;

            if (ImcTeamAmount > MilitiaTeamAmount)
            {
                MilitiaTeamAmount++;
                player.InvokeOnceRPC(CtfRPCs.SetTeam, (int)SlugTeam.Militia);
            }
            else
            {
                ImcTeamAmount++;
                player.InvokeOnceRPC(CtfRPCs.SetTeam, (int)SlugTeam.IMC);
            }
        }

        public override void AddClientData()
        {
            base.AddClientData();
        }

        public override void LobbyTick(uint tick)
        {
            base.LobbyTick(tick);
        }

        public override void Customize(Creature creature, OnlineCreature oc)
        {
            if (oc.TryGetData<SlugcatCustomization>(out var data))
            {
                RainMeadow.Debug(oc);
                RainMeadow.creatureCustomizations.GetValue(creature, (c) => data);
            }
        }
    }
}
