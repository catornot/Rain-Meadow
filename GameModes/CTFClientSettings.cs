using System;
using UnityEngine;

namespace RainMeadow
{
    public class CTFClientSettings : ClientSettings
    {
        public class Definition : ClientSettings.Definition
        {
            public Definition() { }
            public Definition(ClientSettings clientSettings, OnlineResource inResource) : base(clientSettings, inResource) { }

            public override OnlineEntity MakeEntity(OnlineResource inResource, EntityState initialState)
            {
                return new CTFClientSettings(this, inResource, (State)initialState);
            }
        }

        public enum SlugTeam
        {
            Unassigned,
            Red,
            Blue,
        }

        public Color bodyColor;
        public Color eyeColor;
        public bool readyForWin;
        public string? myLastDenPos = null;
        public bool isDead;
        public SlugTeam team = SlugTeam.Unassigned;

        public CTFClientSettings(Definition entityDefinition, OnlineResource inResource, State initialState) : base(entityDefinition, inResource, initialState)
        {
            bodyColor = team == SlugTeam.Red ? Color.red : Color.blue;
            eyeColor = PlayerGraphics.DefaultSlugcatColor(team == SlugTeam.Red ? SlugcatStats.Name.White : SlugcatStats.Name.Red);
        }

        public CTFClientSettings(EntityId id, OnlinePlayer owner) : base(id, owner)
        {
            bodyColor = team == SlugTeam.Red ? Color.red : Color.blue;
            eyeColor = PlayerGraphics.DefaultSlugcatColor(team == SlugTeam.Red ? SlugcatStats.Name.White : SlugcatStats.Name.Red);
        }

        internal override EntityDefinition MakeDefinition(OnlineResource onlineResource)
        {
            return new Definition(this, onlineResource);
        }

        internal override AvatarCustomization MakeCustomization()
        {
            RainMeadow.Debug("slugcat is getting colored?");
            return new SlugcatCustomization(this);
        }

        protected override EntityState MakeState(uint tick, OnlineResource inResource)
        {
            return new State(this, inResource, tick);
        }

        internal Color SlugcatColor()
        {
            return bodyColor;
        }

        public class State : ClientSettings.State
        {
            [OnlineFieldColorRgb]
            public Color bodyColor;
            [OnlineFieldColorRgb]
            public Color eyeColor;
            [OnlineField(group = "game")]
            public bool readyForWin;
            [OnlineField(group = "game")]
            public bool isDead;
            [OnlineField(group = "game")]
            public int team;

            public State() { }
            public State(CTFClientSettings onlineEntity, OnlineResource inResource, uint ts) : base(onlineEntity, inResource, ts)
            {
                bodyColor = onlineEntity.team == SlugTeam.Red ? Color.red : Color.blue;
                eyeColor = PlayerGraphics.DefaultSlugcatColor(onlineEntity.team == SlugTeam.Red ? SlugcatStats.Name.White : SlugcatStats.Name.Red);
                readyForWin = onlineEntity.readyForWin;
                isDead = onlineEntity.isDead;
                team = (int)onlineEntity.team;
            }

            public override void ReadTo(OnlineEntity onlineEntity)
            {
                base.ReadTo(onlineEntity);
                var avatarSettings = (CTFClientSettings)onlineEntity;
                avatarSettings.bodyColor = bodyColor;
                avatarSettings.eyeColor = eyeColor;
                avatarSettings.readyForWin = readyForWin;
                avatarSettings.isDead = isDead;
                avatarSettings.team = (SlugTeam)team;
            }
        }
        public class SlugcatCustomization : AvatarCustomization
        {
            public readonly CTFClientSettings settings;

            public SlugcatCustomization(CTFClientSettings slugcatAvatarSettings)
            {
                this.settings = slugcatAvatarSettings;
                RainMeadow.Debug(this.settings.bodyColor);
                RainMeadow.Debug(this.settings.eyeColor);
            }

            internal override void ModifyBodyColor(ref Color bodyColor)
            {
                bodyColor = new Color(Mathf.Clamp(settings.bodyColor.r, 0.004f, 0.996f), Mathf.Clamp(settings.bodyColor.g, 0.004f, 0.996f), Mathf.Clamp(settings.bodyColor.b, 0.004f, 0.996f));
            }

            internal override void ModifyEyeColor(ref Color eyeColor)
            {
                eyeColor = new Color(Mathf.Clamp(settings.eyeColor.r, 0.004f, 0.996f), Mathf.Clamp(settings.eyeColor.g, 0.004f, 0.996f), Mathf.Clamp(settings.eyeColor.b, 0.004f, 0.996f));
            }

            internal override Color GetBodyColor()
            {
                return settings.bodyColor;
            }
        }
    }
}