# Lobby
[![GitHub release](https://flat.badgen.net/github/release/MrAfitol/Lobby)](https://github.com/MrAfitol/Lobby/releases/)
![GitHub downloads](https://flat.badgen.net/github/assets-dl/MrAfitol/Lobby)


A plugin that adds a lobby when waiting for players
## How download ?
  *1. Find the SCP SL server config folder*
  
  *("C:\Users\(user name)\AppData\Roaming\SCP Secret Laboratory\" for windows, "/home/(user name)/.config/SCP Secret Laboratory/" for linux)*
  
  *2. Find the "PluginAPI" folder there, it contains the "plugins" folder.*
  
  *3. Select either the port of your server to install the same on that server or the "global" folder to install the plugin for all servers*
## View
https://user-images.githubusercontent.com/76150070/208076431-7e7a98e3-d1b3-4365-a989-a09e7fa7f639.mp4


## Config
```yml
# Main text ({seconds} - Either it shows how much is left until the start, or the server status is "Server is suspended", "Round starting")
title_text: <size=50><color=#F0FF00><b>Waiting for players, {seconds}</b></color></size>
# Text showing the number of players ({players} - Text with the number of players)
player_count_text: <size=40><color=#FFA600><i>{players}</i></color></size>
# What will be written if the lobby is locked?
server_pause_text: Server is suspended
# What will be written when there is a second left?
second_left_text: '{seconds} second left'
# What will be written when there is more than a second left?
seconds_left_text: '{seconds} seconds left'
# What will be written when the round starts?
round_start_text: Round starting
# What will be written when there is only one player on the server?
player_join_text: player joined
# What will be written when there is more than one player on the server?
players_join_text: players joined
# What is the movement boost intensity?
movement_boost_intensity: 50
# What role will people play in the lobby?
lobby_player_role: Tutorial
# What items will be given when spawning a player in the lobby? (Leave blank to keep inventory empty)
lobby_inventory:
- Coin
```
