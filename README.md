[![Open in Visual Studio Code](https://classroom.github.com/assets/open-in-vscode-f059dc9a6f8d3a56e377f745f24479a46679e63a5d9fe6f495e02850cd0d8118.svg)](https://classroom.github.com/online_ide?assignment_repo_id=455444&assignment_repo_type=GroupAssignmentRepo)


**The University of Melbourne**
# COMP30019 – Graphics and Interaction

Final Electronic Submission (project): **4pm, November 1**

Do not forget **One member** of your group must submit a text file to the LMS (Canvas) by the due date which includes the commit ID of your final submission.

You can add a link to your Gameplay Video here but you must have already submit it by **4pm, October 17**

# Project-2 README

You must modify this `README.md` that describes your application, specifically what it does, how to use it, and how you evaluated and improved it.

Remember that _"this document"_ should be `well written` and formatted **appropriately**. This is just an example of different formating tools available for you. For help with the format you can find a guide [here](https://docs.github.com/en/github/writing-on-github).


**Get ready to complete all the tasks:**

- [x] Read the handout for Project-2 carefully.

- [x] Brief explanation of the game.

- [x] How to use it (especially the user interface aspects).

- [x] How you designed objects and entities.

- [ ] How you handled the graphics pipeline and camera motion.

- [x] The procedural generation technique and/or algorithm used, including a high level description of the implementation details.

- [ ] Descriptions of how the custom shaders work (and which two should be marked).

- [ ] A description of the particle system you wish to be marked and how to locate it in your Unity project.

- [ ] Description of the querying and observational methods used, including a description of the participants (how many, demographics), description of the methodology (which techniques did you use, what did you have participants do, how did you record the data), and feedback gathered.

- [ ] Document the changes made to your game based on the information collected during the evaluation.

- [ ] References and external resources that you used.

- [ ] A description of the contributions made by each member of the group.

## Table of contents
* [Team Members](#team-members)
* [Explanation of the game](#explanation-of-the-game)
* [Technologies](#technologies)
* [Using Images](#using-images)
* [Code Snipets ](#code-snippets)

## Team Members

| Name | Task | State |
| :---         |     :---:      |          ---: |
| Student Name 1  | MainScene     |  Done |
| Student Name 2    | Shader      |  Testing |
| Student Name 3    | README Format      |  Amazing! |
| Joseph Leonardi    | README Format and bug fixing      |  In Progress... |

## Explanation of the game
Our game is a 3D dungeon crawler set in a never-ending hell created by the Greek gods Hades, Ares, and Artemis to punish a particular Spartan solider. By using a procedurally generated map that swaps between spaces inspired by the aesthetics and elements of each of these three gods, the player attempts to escape the maze only to find that each level becomes more difficult until the player dies. The challenge is therefore to get through as many levels as possible and to compete with yourself or your friends to get through the infinite challenge all whilst never really knowing if there is an end. 

## How to use it
Movement: W - A - S - D movement with SHIFT to sprint. 
Combat: SPACE to attack 
Equipment: Pickup equipment by colliding with it in game. E key opens an inventory and you can use the mouse to select the equipable items and to use the potions.
Misc: ESC to pause, access the controls list, and modify some of the settings. 
Level transitions: Find and collide with the door in the scene to transition to the next level.

## How we designed models and entities
All models are taken from free online resources such as the Unity asset store, SketchFab, or other similar resources. Small modifications were made to the colour of the objects for distiction purposes. 

## How you handled the graphics pipeline and camera motion.
@Jack
A random camera motion script was created for the StartScene, in order to display and highlight the map to the player upon booting up the game. The script uses 2 predetermined points as boundaries, and randomly picks a point within this boundary to move to, iteratively in order to showcase the procedurely generated level/map in the background of the main menu.

## The procedural generation technique and/or algorithm used, including a high level description of the implementation details.

The procedural generation technique that we used was Cellular Automation. First we generate a binary noise grid. This gives us the initial walls and floors. We then go through a prescribed amount of rounds of Cellular Automation wherein each cell is compared with its neighbours. If more of its neighbors are walls, it is made a wall, if more of its neighbors are floor tiles, it it becomes a floor tile. After making sure that the map is all connected and that each room is bigger than a certain size, the map is turned into a mesh using the marching squares algorithm. Much of this was created following a tutorial by Sebastian Langue here: https://www.youtube.com/watch?v=v7yyZZjF1z4&list=PLFt_AvWsXl0eZgMK_DT5_biRkWXftAOf9. 

Then we used a combination of random and semi-random methods to distribute objects throughout each of these levels. This creates three distict environments one based on each of the Gods. We distribute lights throughout the map evenly which are then spread and attached to the walls of the scene afterwards. 

## Descriptions of how the custom shaders work (and which two should be marked).

**Cel-shader** (to mark)

The cel-shader (ToonShader2.shader in the Shaders folder) is used on the player as well as all the enemies and most of the environmental objects to give a cel-shaded aesthetic ..

**Lava flow shader** (to mark)

The lava flow shader was written to give make the lava in the lavapools look as though they are flowing..


**Triplanar mapping shader**
 
This shader was written in order to map the texture to the walls of the map without the need for UV mapping as there were issues with UV mapping in the procedurally generated map.

## A description of the particle system you wish to be marked and how to locate it in your Unity project.

## Description of the querying and observational methods used, including a description of the participants (how many, demographics), description of the methodology (which techniques did you use, what did you have participants do, how did you record the data), and feedback gathered.

For the observational method we used a Think Aloud Study focusing on the ease of use of two features: 1) the inventory specifically finding and picking up items. 2) finding and transitioning to a new level. We had 5 participants of the study: 
- Participant A: Experienced gamer on both PC and console
- Participant B: Experienced gamer on PC
- Participant C: Novice gamer on PC 
- Participant D: Experienced gamer and game developer on both PC and console 
- Participant E: Experienced gamer on only console

We prompted the participants only to find the objects in the scene and transition to the next level. We left out all the combat elements and only left in the objects in the scene some of which were scenery and some of which were interactable. We used audio recording to record the data and as the task was simple enough, little prompting was needed by the experimenter to get the participants to talk freely and actively especially after the first few minutes. The feedback gathered was clear. Once the pattern was observed, participants found they were easily able to identify that the objects were pickup-able but did not know what the objects were as the light that we had used to identify the objects obscured the object itself. The door to transition to each level was easy to identify and obvious. All players did not need any prompting to identify, move towards, and transition to a new level. We also identified some feedback about the controls. PC gamers found the controls intuitive and easy to use but those who are new to gaming and PC gaming had intiuting the controls. 

@Joey the other method. queryingtechnique
For the querying method we invited players to play the game, and answer a Google form questionnaire. The questionnaire focused on the art style, and overall 'fun-ness' and difficulty of the game.

## Document the changes made to your game based on the information collected during the evaluation.
In response to the Think Aloud Study, we made a number of changes. 
- For each of the pickup-able objects, we created a new script that made the object bob up and down whilst turning slightly and included a light over its head instead of coming from the object's centre. This means that the objects are easier to see from further away and the object itself are more identifiable.
- In the pause screen we have included the controls to make it easier for new players.

@Joey describe what changes we made here. 

## References and external resources that you used.
Sebastian Lague tutorial series for the creation of a procedurally generated cave: https://www.youtube.com/watch?v=v7yyZZjF1z4&list=PLFt_AvWsXl0eZgMK_DT5_biRkWXftAOf9
Brackey's youtube channel assisted a lot in the template of our game: https://www.youtube.com/c/Brackeys
@All please add your references and other resources here. 

## A description of the contributions made by each member of the group.
@All please add your contributions here. 

Alex Gorbatov (996729) focused on the generation of the map and populating it with objects. The majority of the content within the scripts involved with spawning objects, populating the map, and generating the map are created by him. This included sourcing and building many of the prefab assets for the visuals and population of the game. In addition, he created the minimap, transitions between levels, and performed the observational method. 

Joseph Leonardi (1025351) focused on particle system effects, mob AI, player and mob combat, and UI elements. The majority of the content in scripts related to mob movement, combat, and menu UI were created by him. He created animation controllers and set the stats of each mob, and all the components in the mob prefabs besides the ones related to sounds. The StartScene was made entirely by him with the exception of the sample level that was randomly generated. Scene transitions were also made by him.

Zhen Yi Ang (1000033) focused on the shaders, the graphics and aesthetic style of the game, and the sound design. All of the shaders implemented in the game was written by him. A majority of the graphics and sound assets in the game were sourced by him. He managed most of the graphics pipeline including the scene lighting, the graphics settings as well as the post-processing. He also designed the DemoScene and recorded and edited the demo video using that scene.

## Technologies
Project is created with:
* Unity 2021.1.13f1
* Ipsum version: 2.33
* Ament library version: 999

## Using Images

You can use images/gif by adding them to a folder in your repo:

<p align="center">
  <img src="Gifs/Q1-1.gif"  width="300" >
</p>

To create a gif from a video you can follow this [link](https://ezgif.com/video-to-gif/ezgif-6-55f4b3b086d4.mov).

## Code Snippets 

You can include a code snippet here, but make sure to explain it! 
Do not just copy all your code, only explain the important parts.

```c#
public class firstPersonController : MonoBehaviour
{
    //This function run once when Unity is in Play
     void Start ()
    {
      standMotion();
    }
}
```




