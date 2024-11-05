using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RainMeadow.Ctf
{
    public class CtfFlag : ExplosiveSpear
    {
        SlugTeam team;
        public CtfFlag(AbstractPhysicalObject abstractPhysicalObject, World world, SlugTeam team) : base(abstractPhysicalObject, world)
        {
            RainMeadow.Debug("Creating CtfFlag");

            this.team = team;
            redColor = new Color(
                0.5F * (team == SlugTeam.IMC ? 1.0F : 0.0F),
                0.5F * (team == SlugTeam.Militia ? 1.0F : 0.0F),
                0.06F,
                0.98F
            );
        }

        public override void PlaceInRoom(Room placeRoom)
        {
            RainMeadow.Debug("CtfFlag placed into " + placeRoom.abstractRoom.name);

            base.PlaceInRoom(placeRoom);
            this.ResetRag();

            igniteCounter = 0;
            
            // check if we are in the other's team shelter
            if (room.abstractRoom.name == CTFRoundHandler.ShelterSpawnForTeam(team == SlugTeam.IMC ? SlugTeam.Militia : SlugTeam.IMC))
            {
                Lobby lobby = OnlineManager.lobby;
                lobby.owner.InvokeRPC(CtfRPCs.FlagCaptured, team, (lobby.gameMode as CTFGameMode).ctfdata.scoringIndex + 1);
            }

            base.Ignite();
        }

        public override void WeaponDeflect(Vector2 inbetweenPos, Vector2 deflectDir, float bounceSpeed)
        {
            // DO NOTHING
        }

        public override void HitByExplosion(float hitFac, Explosion explosion, int hitChunk)
        {
            // DO NOTHING
        }

        public override bool HitSomething(SharedPhysics.CollisionResult result, bool eu)
        {
            // DO NOTHING
            return false;
        }

        public override void HitByWeapon(Weapon weapon)
        {
            // DO NOTHING
        }
    }
}
