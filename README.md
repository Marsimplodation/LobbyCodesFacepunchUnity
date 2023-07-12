# LobbyCodesFacepunchUnity
This script generates 6 letter codes as an alternative to these way to long steam lobby ids

# How to use it
First step is to setup Facepunch and get it working, after that you can simpy adjust yours scripts
After you generate your Lobby (lets call it lobby) on the host, simply get the string with:
```csharp
string inviteCode = ShorterLobbyCodes.convertLobbyToInviteCode(lobby.Id);
```
to join a lobby you just use this
```
string inviteCode = ""; //wherever you get this from, probably ui input
SteamId lobbyID = ShorterLobbyCodes.convertInviteCodeToLobby(inviteCode);
SteamMatchmaking.JoinLobbyAsync(lobbyID);
```

# How it works
The normal lobby IDs from steam are numerical codes and rely on our digits from 0-9, The system I use instead uses 62 possible digits (0-9, a-z, A-Z) which shortens the length immensly.
In addition to that a lot of the information in the steamID is not unique, so it can just get taken out and get calculated back in, with some binary magic.
