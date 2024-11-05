using Menu;
using UnityEngine;
using System.Collections.Generic;
using Steamworks;
using System.Linq;

namespace RainMeadow
{
    public class CTFLobbyMenu : CustomLobbyMenu
    {
        CTFGameMode gamemode;

        private SimplerButton playButton;

        public override MenuScene.SceneID GetScene => MenuScene.SceneID.Landscape_CC;
        public CTFLobbyMenu(ProcessManager manager) : base(manager, RainMeadow.Ext_ProcessID.CtfLobbyMenu)
        {
            gamemode = (OnlineManager.lobby.gameMode as CTFGameMode);
            
            playButton = new SimplerButton(this, mainPage, "START", new Vector2(1056f, 50f), new Vector2(110f, 30f));
            playButton.OnClick += StartGame;
            mainPage.subObjects.Add(playButton);
        }

        public override void Update()
        {
            base.Update();
            if (!OnlineManager.lobby.isOwner)
                playButton.buttonBehav.greyedOut = !gamemode.isInGame;
        }

        private void StartGame(SimplerButton button)
        {
            RainMeadow.DebugMe();
            if (OnlineManager.lobby == null || !OnlineManager.lobby.isActive) 
                return;

            manager.arenaSitting = null;
            manager.rainWorld.progression.ClearOutSaveStateFromMemory();
            manager.rainWorld.progression.miscProgressionData.currentlySelectedSinglePlayerSlugcat = SlugcatStats.Name.Red;
            manager.menuSetup.startGameCondition = ProcessManager.MenuSetup.StoryGameInitCondition.RegionSelect;
            manager.menuSetup.regionSelectRoom = CTFRoundHandler.ShelterSpawn(gamemode.ctfClientSettings);
            manager.RequestMainProcessSwitch(ProcessManager.ProcessID.Game);
        }

        public override void ShutDownProcess()
        {
            RainMeadow.DebugMe();
            if (manager.upcomingProcess != ProcessManager.ProcessID.Game)
            {
                OnlineManager.LeaveLobby();
            }
            base.ShutDownProcess();
        }
    }
}
