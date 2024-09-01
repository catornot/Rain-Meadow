﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace RainMeadow
{

    public abstract class MatchmakingManager
    {
        public static MatchmakingManager instance;
        public static string CLIENT_KEY = "client";
        public static string CLIENT_VAL = "Meadow_" + RainMeadow.MeadowVersionStr;
        public static string NAME_KEY = "name";
        public static string MODE_KEY = "mode";
        public static string PASSWORD_KEY = "password";
        public static int MAX_LOBBY = 4;

        public static void InitLobbyManager()
        {
#if LOCAL_P2P
            instance = new LocalMatchmakingManager();
#else
            instance = new SteamMatchmakingManager();
#endif
        }

        public enum LobbyVisibility
        {
            [Description("Public")]
            Public = 1,
            [Description("Friends Only")]
            FriendsOnly,
            [Description("Private")]
            Private
        }

        public abstract event LobbyListReceived_t OnLobbyListReceived;
        public abstract event PlayerListReceived_t OnPlayerListReceived;
        public abstract event LobbyJoined_t OnLobbyJoined;
        public delegate void LobbyListReceived_t(bool ok, LobbyInfo[] lobbies);
        public delegate void PlayerListReceived_t(PlayerInfo[] players);
        public delegate void LobbyJoined_t(bool ok, string error = "");

        public abstract void RequestLobbyList();

        public abstract void CreateLobby(LobbyVisibility visibility, string gameMode, string? password, int? maxPlayerCount);

        public abstract void RequestJoinLobby(LobbyInfo lobby, string? password);
        public abstract void JoinLobby(bool success);

        public abstract void LeaveLobby();

        public abstract OnlinePlayer GetLobbyOwner();

        public virtual OnlinePlayer GetPlayer(MeadowPlayerId id)
        {
            return OnlineManager.players.FirstOrDefault(p => p.id == id);
        }

        public virtual OnlinePlayer BestTransferCandidate(OnlineResource onlineResource, List<OnlinePlayer> subscribers)
        {
            if (onlineResource.isAvailable && subscribers.Contains(OnlineManager.mePlayer)) return OnlineManager.mePlayer;
            if (subscribers.Count < 1) return null;
            return subscribers.FirstOrDefault(p => !p.hasLeft);
        }

        public abstract MeadowPlayerId GetEmptyId();

        public abstract string GetLobbyID();
    }
}