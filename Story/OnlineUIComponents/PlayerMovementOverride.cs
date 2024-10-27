﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainMeadow.Story.OnlineUIComponents
{
    internal static class PlayerMovementOverride
    {

        public static void StopPlayerMovement(Player p)
        {

            p.input[0].x = 0;
            p.input[0].y = 0;
            p.input[0].analogueDir *= 0f;
            p.input[0].jmp = false;
            p.input[0].thrw = false;
            p.input[0].pckp = false;

        }

    }
}
