# Lobby
[![Version](https://img.shields.io/github/v/release/MrAfitol/Lobby?sort=semver&style=flat-square&color=blue&label=Version)](https://github.com/MrAfitol/Lobby/releases)
[![Downloads](https://img.shields.io/github/downloads/MrAfitol/Lobby/total?style=flat-square&color=yellow&label=Downloads)](https://github.com/MrAfitol/Lobby/releases)


A plugin that adds a lobby when waiting for players

The idea is taken from the [plugin](https://github.com/Michal78900/WaitAndChillReborn)
## How to download?
   - *1. Find the SCP SL server config folder*
   
   *("C:\Users\(user name)\AppData\Roaming\SCP Secret Laboratory\" for windows, "/home/(user name)/.config/SCP Secret Laboratory/" for Linux)*
  
   - *2. Find the "PluginAPI" folder there, it contains the "plugins" folder.*
  
   - *3. Select either the port of your server to install the same on that server or the "global" folder to install the plugin for all servers*
  
  ***Or***
  
   - *Run the command in console `p install MrAfitol/Lobby`*
  
## View
https://user-images.githubusercontent.com/76150070/208076431-7e7a98e3-d1b3-4365-a989-a09e7fa7f639.mp4


## Config
```yml
# Main text ({seconds} - Either it shows how much is left until the start, or the server status is "Server is suspended", "Round starting", <rainbow> - Change the next text a rainbow color, </rainbow> - Close a rainbow color tag)
title_text: <color=#F0FF00><b>Waiting for players, {seconds}</b></color>
# Text showing the number of players ({players} - Text with the number of players, <rainbow> - Change the next text a rainbow color, </rainbow> - Close a rainbow color tag)
player_count_text: <color=#FFA600><i>{players}</i></color>
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
# Vertical text position.
vertical_pos: 25
# Top text size
top_text_size: 50
# Bottom text size
bottom_text_size: 40
# Top text size in intercom
top_text_icom_size: 150
# Bottom text size in intercom
bottom_text_icom_size: 140
# Enable the movement boost effect?
enable_movement_boost: true
# What is the movement boost intensity? (Max 255)
movement_boost_intensity: 50
# Will infinity stamina be enabled for people in the lobby?
infinity_stamina: true
# What role will people play in the lobby?
lobby_player_role: Tutorial
# Allow people to talk over the intercom?
allow_icom: true
# Display text on Intercom? (Works only when lobby Intercom type)
display_in_icom: true
# What size will the text be in the Intercom? (The larger the value, the smaller it will be)
icom_text_size: 20
# What items will be given when spawning a player in the lobby? (Leave blank to keep inventory empty)
lobby_inventory:
- Coin
# In what locations can people spawn? (If this parameter is empty, one of the custom locations (or custom room locations) will be selected)
lobby_location:
- Tower_1
- Tower_2
- Tower_3
- Tower_4
- Tower_5
- Intercom
- GR18
- SCP173
# This option is for a custom lobby location
custom_room_locations:
- room_name_type: EzGateA
  offset_x: 0
  offset_y: 1
  offset_z: 0
  rotation_x: 0
  rotation_y: 0
  rotation_z: 0
# This option is for a custom lobby location
custom_locations:
- position_x: 39.262001
  position_y: 1014.112
  position_z: -31.8439999
  rotation_x: 0
  rotation_y: 0
  rotation_z: 0
# The name of the role that can use commands for Lobby.
allowed_rank:
- owner
# User ID that can use commands for the Lobby.
allowed_user_i_d:
- SomeOtherSteamId64@steam
```

## Wiki
**Be sure to check out the [Wiki](https://github.com/MrAfitol/Lobby/wiki)**
