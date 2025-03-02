﻿namespace RainMeadow
{
    public static class PlayerMovementOverride
    {
        public static void StopPlayerMovement(Player p)
        {
            if (p != null)
            {
                p.input[0].x = 0;
                p.input[0].y = 0;
                p.input[0].analogueDir *= 0f;
                p.input[0].jmp = false;
                p.input[0].thrw = false;
                p.input[0].pckp = false;
                p.input[0].mp = false;
            } else
            {
                RainMeadow.Debug("Player is null while trying to stop movement");
            }
        }


        public static void HoldFire(Player p)
        {
            p.input[0].thrw = false;


        }

        public static void StopSpecialSkill(Player p)
        {
            if (p.wantToJump > 0 && p.input[0].pckp)
            {
                p.input[0].pckp = false;
            }

        }

    }
}
