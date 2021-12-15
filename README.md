# WWYT
Senior Project Code Upload
This is the C# code for a Unity proof of concept, presenting the mechanics and gameplay for a 
possible short text-based game about the decisions and consequences in our daily lives.

The "DialogueCode" folder holds all the necessary scripts to handle the dialogue portion of the game.
They handle the text display for objects and side characters, as the logic for any changes that dialogue-based objects
may effect on the player. This folder also includes the "DialogueTreeNode", a Unity Scriptable Object, which is used to 
generate the branching dialogues for the side character, and it may hold other variables for any effects that dialogue may 
have, such as changing the current hint.

The "GameTools" folder holds all the scripts that handle any logic or mechanics not directly related to player inptus
or to the dialogue tree mechanics. There are scripts to handle the minigame display and controls, as well as scripts for 
on-screen buttons, and game- and scene-transitions.

The "PlayerCode" folder holds all the scripts that handle the players input, as well as scripts directly related to them, 
namely the scripts for player dialogue choices. The "Player" script handles player inputs, and controls the logic for changing 
the effects of player-inputs in game, while the "UIMove" script is for main-menu navigation.
