using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RainMeadow.RainMeadow;

namespace RainMeadow.Ctf
{
    internal class AbstractCtfFlag : AbstractPhysicalObject
    {
        SlugTeam team;

        public AbstractCtfFlag(World world, AbstractObjectType type, PhysicalObject realizedObject, WorldCoordinate pos, EntityID ID, SlugTeam team) : base(world, Ext_PhysicalObjectType.CtfFlag, realizedObject, pos, ID)
        {
            RainMeadow.Debug("Creating abstract ctfflag for " + team);

            if (type != Ext_PhysicalObjectType.CtfFlag)
                throw new InvalidProgrammerException("hmmmm");

            type = Ext_PhysicalObjectType.CtfFlag;
            this.team = team;
        }

        public override void Realize()
        {
            RainMeadow.Debug("Relizing CtfFlag from abstract");
            base.Realize();
            //if (realizedObject == null)
                realizedObject = new CtfFlag(this, world, team);
        }

        public override void Destroy()
        {
            // TODO: custom logic for getting a new one possibly
            base.Destroy();
        }
    }
}
