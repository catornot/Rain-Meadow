using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            // I don't think this works as I wanted :/
            return (OnlineManager.lobby.gameMode is CTFGameMode && result.obj is Player) || orig(self, result, eu);
        }

        bool Weapon_HitThisObject(On.Weapon.orig_HitThisObject orig, Weapon self, PhysicalObject obj)
        {
            // TODO: add team based collision
            return (OnlineManager.lobby.gameMode is CTFGameMode && obj is Player && self is Spear && self.thrownBy != null && self.thrownBy is Player) || orig(self, obj);
        }

        public void ctf_RegionGate_ctor(On.RegionGate.orig_ctor orig, RegionGate self, Room room)
        {
            if (OnlineManager.lobby.gameMode is CTFGameMode)
            {
                self.mode = RegionGate.Mode.Broken;
                self.unlocked = false;
            }
            orig(self, room);
        }
    }
}
