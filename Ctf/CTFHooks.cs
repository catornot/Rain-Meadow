﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static MonoMod.InlineRT.MonoModRule;

namespace RainMeadow
{
    public partial class RainMeadow
    {
        private void CTFHooks()
        {
            On.Spear.HitSomething += Spear_HitSomething;
            On.Weapon.HitThisObject += Weapon_HitThisObject;
            On.RegionGate.ctor += ctf_RegionGate_ctor;
        }

        bool Spear_HitSomething(On.Spear.orig_HitSomething orig, Spear self, SharedPhysics.CollisionResult result, bool eu)
        {
            bool origResult = orig(self, result, eu);
            RainMeadow.Debug("tried hitting obj" + result.obj + " with " + (self is Spear) + " got " + origResult);
            if (!origResult && result.hitSomething
                && OnlineManager.lobby != null
                && OnlineManager.lobby.gameMode is CTFGameMode
                && result.obj != null
                && result.obj is Player playerHit
                && self is Spear 
                && self.thrownBy != null
                && self.thrownBy is Player
                && playerHit.SpearStick(self, Mathf.Lerp(0.55f, 0.62f, UnityEngine.Random.value), result.chunk, result.onAppendagePos, self.firstChunk.vel)
            )
            {
                RainMeadow.Debug("hit pls");
                self.room.PlaySound(SoundID.Spear_Stick_In_Creature, self.firstChunk);
                self.LodgeInCreature(result, eu);
                self.abstractPhysicalObject.world.game.GetArenaGameSession.PlayerLandSpear(self.thrownBy as Player, self.stuckInObject as Creature);
                playerHit.Die(); // everyone dies in one hit!!!!!!!
                return true;
            }

            RainMeadow.Debug("hit 1 " + self.thrownBy != null && self.thrownBy is Player);
            RainMeadow.Debug("hit 2 " + result.obj != null && result.obj is Player);
            RainMeadow.Debug("hit 3 " + result.hitSomething);

            return origResult;
        }

        bool Weapon_HitThisObject(On.Weapon.orig_HitThisObject orig, Weapon self, PhysicalObject obj)
        {
            return (OnlineManager.lobby != null && OnlineManager.lobby.gameMode is CTFGameMode) || orig(self, obj);
            // TODO: add team based collision
            bool yes = (OnlineManager.lobby.gameMode is CTFGameMode && obj is Player && self is Spear && self.thrownBy != null && self.thrownBy is Player) || orig(self, obj);
            RainMeadow.Debug("tried hitting " + obj + " with " + self + " got " + yes);

            return yes;
        }

        public void ctf_RegionGate_ctor(On.RegionGate.orig_ctor orig, RegionGate self, Room room)
        {
            orig(self, room);
            if (OnlineManager.lobby != null && OnlineManager.lobby.gameMode is CTFGameMode)
            {
                self.mode = RegionGate.Mode.Broken;
                self.unlocked = false;
            }
        }
    }
}
