using UnityEngine;
using Steamworks;


public struct CustomSteamLobbyId {
    public int Universe;
    public int Type;
    public int Instance;
    public ulong LobbyId;

    public CustomSteamLobbyId(int u, int t, int i, ulong l) {this.Universe = u; this.Type = t; this.Instance = i; this.LobbyId = l;}

}

public class ShorterLobbyCodes {
    private static char[] base62Chars = {
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
        'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
        'U', 'V', 'W', 'X', 'Y', 'Z',
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
        'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
        'u', 'v', 'w', 'x', 'y', 'z'
    };

    public static string convertLobbyToInviteCode(SteamId id) {
        string code = "";
        CustomSteamLobbyId lobby = SplitSteamID(id);
        ulong lId = lobby.LobbyId;
        Debug.Log(lId);
        while(lId > 0) {
            ulong remainder = lId % (ulong)base62Chars.Length;
            lId = (lId - remainder) / (ulong)base62Chars.Length;
            code = base62Chars[remainder]+code;
        }
        return code;
    }
    private static ulong getIndex(char c) {
        for(ulong i = 0; i < (ulong)base62Chars.Length; i++) {
            if(base62Chars[i] == c) return i;
        }
        return 0;
    }
    public static SteamId convertInviteCodeToLobby(string code) {
        ulong lobbyId=0;
        foreach(char c in code) {
            lobbyId = lobbyId*(ulong)base62Chars.Length + getIndex(c);
        }

        //magic numbers for the steamid
        //EUniverse: 1-public
        //EAccountType: 8-lobby or groupchat
        //AccountInstance: magic number 393216u (https://kb.heathen.group/steam/csteamid)
        //lobbyId: the unique ID of the lobby
        return CreateSteamID(new CustomSteamLobbyId(1,8,393216,lobbyId));
    }

    private static CustomSteamLobbyId SplitSteamID(ulong steamID){
        CustomSteamLobbyId lobbySteamID;
        lobbySteamID.Universe = (int)((steamID >> 56) & 0xFF);
        lobbySteamID.Type = (int)((steamID >> 52) & 0xF);
        lobbySteamID.Instance = (int)((steamID >> 32) & 0xFFFFF);
        lobbySteamID.LobbyId = steamID & 0xFFFFFFFF;
        return lobbySteamID;
    }

    private static SteamId CreateSteamID(CustomSteamLobbyId lobbySteamID) {
        ulong SteamId = ((ulong)lobbySteamID.Universe << 56) |
                        ((ulong)lobbySteamID.Type << 52) |
                        ((ulong)lobbySteamID.Instance << 32) |
                        lobbySteamID.LobbyId;
        return SteamId;
    }
}
