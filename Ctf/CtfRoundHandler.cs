using RainMeadow.Properties;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainMeadow
{
    internal class CTFRoundHandler
    {
        public CTFRoundHandler()
        {
            // hmm
        }

        static void InitRound()
        {

        }

        public static void SelectTeam()
        {
            // iterators would do this whole thing in only two lines >:(
            int blueCount = 0;
            int redCount = 0;
            foreach (ClientSettings settings in OnlineManager.lobby.clientSettings.Values)
            {
                if (settings is CTFClientSettings)
                {
                    CTFClientSettings ctfSettings = settings as CTFClientSettings;
                    switch (ctfSettings.team)
                    {
                        case CTFClientSettings.SlugTeam.Blue:
                            blueCount++;
                            break;
                        case CTFClientSettings.SlugTeam.Red:
                            redCount++;
                            break;
                    }
                }
            }

            CTFClientSettings.SlugTeam team = blueCount > redCount ? CTFClientSettings.SlugTeam.Red : CTFClientSettings.SlugTeam.Blue;
            (OnlineManager.lobby.gameMode as CTFGameMode).ctfClientSettings.team = team;
            RainMeadow.Debug("Selected team is " + team);
        }

        public static WorldCoordinate SelectRespawnRoom(CTFClientSettings settings)
        {
            string spawnShelterRoomStr = settings.team == CTFClientSettings.SlugTeam.Red ? "HI_A20" : "HI_S05";

            int spawnShelterRoom = RainWorld.roomNameToIndex.TryGetValue(spawnShelterRoomStr, out var val) ? val : -1;
            RainMeadow.Debug("spawnShelterRoom is " + spawnShelterRoom);
            return new WorldCoordinate(spawnShelterRoom, -1, -1, 0);
        }

        // desync 100% this is so bad -_-
        public static void RespawnSlugCat(RainWorldGame game, AbstractCreature absPlayer)
        {
            // WorldSession session = OnlineManager.lobby.worldSessions.First().Value;

            Player player = absPlayer.realizedCreature as Player;
            if (player == null)
            {
                absPlayer.RealizeInRoom();
                player = absPlayer.realizedCreature as Player;
            }

            WorldCoordinate worldPos = SelectRespawnRoom((OnlineManager.lobby.gameMode as CTFGameMode).ctfClientSettings);

            AbstractRoom newRoom = game.overWorld.activeWorld.GetAbstractRoom(worldPos);
            Room playerRoom = player.room;

            PlayerState state = (absPlayer.state as PlayerState);
            state.permaDead = false;
            state.alive = true;
            state.permanentDamageTracking = 0.1;
            player.dead = false;
            player.killTag = null;
            player.killTagCounter = 0;

            if (player.grasps[0] != null)
            {
                player.ReleaseGrasp(0);
            }
            if (player.grasps[0] != null)
            {
                player.ReleaseGrasp(1);
            }

            // player.ReleaseObject(0, true);
            // player.ReleaseObject(1, true);

            if (playerRoom != null && newRoom.name == absPlayer.Room.name)
            {
                return;
            }

  
            game.world.ActivateRoom(newRoom);

            if (playerRoom != null)
            {
                playerRoom.RemoveObject(player);
            }
            else
            {
                player.RemoveFromRoom();
            }

            game.cameras[0].MoveCamera(newRoom.realizedRoom, 0);

            newRoom.AddEntity(absPlayer);
            newRoom.realizedRoom.AddObject(player);
            absPlayer.pos = newRoom.realizedRoom.LocalCoordinateOfNode(0);
        }
    }
}
