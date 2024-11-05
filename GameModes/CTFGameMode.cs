using RainMeadow.Ctf;
using System.Collections.Generic;
using System.Linq;

namespace RainMeadow
{
    public class CTFGameMode : OnlineGameMode
    {
        private int ImcTeamAmount = 0;
        private int MilitiaTeamAmount = 0;
        public bool isInGame = true;

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
            RainMeadow.Debug("Init CTF gamemode");

            avatarSettings = new SlugcatCustomization() { nickname = OnlineManager.mePlayer.id.name };
            ctfClientSettings = new CTFClientSettings();
            ctfClientSettings.playingAs = SlugcatStats.Name.White;
        }


        public override bool AllowedInMode(PlacedObject item)
        {
            return !ItemBlackList.Contains(item.type);
        }
        public override bool ShouldSpawnRoomItems(RainWorldGame game, RoomSession roomSession)
        {
            return roomSession.isOwner;
        }
        public override bool ShouldLoadCreatures(RainWorldGame game, WorldSession worldSession)
        {
            //return worldSession.owner == null || worldSession.isOwner;
            return worldSession.isOwner;
        }

        public override SlugcatStats.Name LoadWorldAs(RainWorldGame game)
        {
            game.manager.rainWorld.options.friendlyFire = true;
            game.manager.rainWorld.options.friendlySteal = true;
            game.manager.rainWorld.options.fpsCap = 120;
            game.manager.rainWorld.options.dlcTutorialShown = false;
            game.manager.rainWorld.options.friendlyLizards = false;
            return SlugcatStats.Name.White;
        }

        public override ProcessManager.ProcessID MenuProcessId()
        {
            return RainMeadow.Ext_ProcessID.CtfLobbyMenu;
        }

        public override void NewEntity(OnlineEntity oe, OnlineResource inResource)
        {
            base.NewEntity(oe, inResource);
        }

        public override void ResourceAvailable(OnlineResource onlineResource)
        {
            base.ResourceAvailable(onlineResource);

            if (onlineResource is Lobby lobby)
            {
                lobby.AddData(new CtfLobbyData());
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

            RainMeadow.Debug("New Player [" + player.id.name + "] joined, team selected");
            if (ImcTeamAmount > MilitiaTeamAmount)
            {
                MilitiaTeamAmount++;
                player.InvokeRPC(CtfRPCs.SetTeam, (int)SlugTeam.Militia, ImcTeamAmount + MilitiaTeamAmount);
            }
            else
            {
                ImcTeamAmount++;
                player.InvokeRPC(CtfRPCs.SetTeam, (int)SlugTeam.IMC, ImcTeamAmount + MilitiaTeamAmount);
            }
        }

        public override void AddClientData()
        {
            clientSettings.AddData(ctfClientSettings);
        }

        public override void ConfigureAvatar(OnlineCreature onlineCreature)
        {
            RainMeadow.Debug("why ? " + avatarSettings);
            onlineCreature.AddData(avatarSettings);
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
