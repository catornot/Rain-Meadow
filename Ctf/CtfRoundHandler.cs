using RainMeadow.Ctf;
using RainMeadow.Properties;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RainMeadow.RainMeadow;

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

        public static string ShelterSpawnForTeam(SlugTeam team)
        {
            return team == SlugTeam.IMC ? "HI_S01" : "HI_S05";
        }

        public static WorldCoordinate SelectRespawnRoom(CTFClientSettings settings)
        {
            string spawnShelterRoomStr = ShelterSpawnForTeam(settings.team);

            int spawnShelterRoom = RainWorld.roomNameToIndex.TryGetValue(spawnShelterRoomStr, out var val) ? val : -1;
            RainMeadow.Debug("spawnShelterRoom is " + spawnShelterRoom);
            return new WorldCoordinate(spawnShelterRoom, -1, -1, 0);
        }

        public static void RespawnSlugCat(RainWorldGame game, AbstractCreature absPlayer)
        {
            // WorldSession session = OnlineManager.lobby.worldSessions.First().Value;

            CTFClientSettings settings = (OnlineManager.lobby.gameMode as CTFGameMode).ctfClientSettings;
            WorldCoordinate worldPos = SelectRespawnRoom(settings);

            AbstractRoom newRoom = game.overWorld.activeWorld.GetAbstractRoom(worldPos);

            var flag = new AbstractCtfFlag(game.world, Ext_PhysicalObjectType.CtfFlag, null, worldPos, game.GetNewID(), settings.team);
            newRoom.AddEntity(flag);
            if (flag.realizedObject == null)
            {
                flag.Realize();
            }

            // realize room if it's nto realized
            if (newRoom.realizedRoom == null)
            {
                newRoom.RealizeRoom(game.world, game);
            }

            PlayerState state = (absPlayer.state as PlayerState);
            Player player = absPlayer.realizedCreature as Player;
            if (player == null)
            {
                newRoom.AddEntity(absPlayer);
                absPlayer.Move(worldPos);
                absPlayer.realizedCreature.PlaceInRoom(newRoom.realizedRoom);
                player = absPlayer.realizedCreature as Player;
                state.permaDead = true; // using this has a flag lol
            }

            Room playerRoom = player.room;

            // just do everything to revive the player

            state.alive = true;
            state.permanentDamageTracking = 0.1;
            player.dead = false;
            player.killTag = null;
            player.killTagCounter = 0;
            player.SubtractFood(player.CurrentFood); // must remain hungry

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

            /*
            if (playerRoom != null)
            {
                if (!state.permaDead)
                    playerRoom.RemoveObject(player);

                List<AbstractPhysicalObject> allConnectedObjects = absPlayer.GetAllConnectedObjects();
                for (int i = 0; i < allConnectedObjects.Count; i++)
                {
                    if (allConnectedObjects[i].realizedObject != null)
                        player.room.RemoveObject(allConnectedObjects[i].realizedObject);
                }
            }

            game.cameras[0].virtualMicrophone.AllQuiet();
            game.cameras[0].MoveCamera(newRoom.realizedRoom, 0);

            newRoom.AddEntity(absPlayer);
            newRoom.realizedRoom.AddObject(player);
            absPlayer.pos = newRoom.realizedRoom.LocalCoordinateOfNode(0);
            */

            // I give up
            JollyCoop.JollyCustom.WarpAndRevivePlayer(absPlayer, newRoom, worldPos);

            // set perma death last
            state.permaDead = false;
        }
    }
}