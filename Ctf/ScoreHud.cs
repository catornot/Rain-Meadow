using RainMeadow;
using System.Collections.Generic;
using UnityEngine;

namespace RainMeadow.Ctf
{
    public class ScoreHud : HUD.HudPart
    {
        private FLabel scoreLabel;
        private FLabel teamLabel;

        private Vector2 pos, lastPos;
        private float fade, lastFade;

        private CTFClientSettings settings;
        private CtfLobbyData ctfdata;
        private List<int> lastScore = new List<int>{ 0, 0, 0, 0 };

        private Player? player;

        public ScoreHud(HUD.HUD hud, FContainer fContainer, CTFGameMode ctf) : base(hud)
        {
            this.settings = ctf.ctfClientSettings;
            this.ctfdata = ctf.ctfdata;

            scoreLabel = new FLabel("font", "0-0")
            {
                scale = 2.4f,
                alignment = FLabelAlignment.Left
            };

            teamLabel = new FLabel("font", settings.team.ToString())
            {
                scale = 1.6f,
                alignment = FLabelAlignment.Left
            };

            pos = new Vector2(80f, hud.rainWorld.options.ScreenSize.y - 60f);
            lastPos = pos;
            scoreLabel.SetPosition(DrawPos(1f));
            teamLabel.SetPosition(DrawPos(1f) + new Vector2(135f, 0f));

            fContainer.AddChild(teamLabel);
            fContainer.AddChild(scoreLabel);
        }

        public Vector2 DrawPos(float timeStacker)
        {
            return Vector2.Lerp(lastPos, pos, timeStacker);
        }

        public override void Update()
        {
            base.Update();
            player = hud.owner as Player;
            if (player == null) return;

            // ummmmmm
        }

        public override void Draw(float timeStacker)
        {
            base.Draw(timeStacker);

            int teamIndex = (int)settings.team;
            int otherTeam = settings.team == SlugTeam.IMC ? (int)SlugTeam.Militia : (int)SlugTeam.IMC;

            if (ctfdata.score[teamIndex] != lastScore[teamIndex] || ctfdata.score[otherTeam] != lastScore[otherTeam])
            {
                scoreLabel.text = newScore(teamIndex, otherTeam);
                lastScore[teamIndex] = ctfdata.score[teamIndex];
                lastScore[otherTeam] = ctfdata.score[otherTeam];
            }

            teamLabel.text = settings.team.ToString();
        }

        public string newScore(int ourIndex, int theirIndex)
        {
            int us = ctfdata.score[ourIndex];
            int them = ctfdata.score[theirIndex];
            return $"{us}-{them}";
        }

        public override void ClearSprites()
        {
            base.ClearSprites();
            scoreLabel.RemoveFromContainer();
            teamLabel.RemoveFromContainer();
        }
    }
}