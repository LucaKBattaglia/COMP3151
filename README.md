># COMP3151 - Progress Report 1
> Dylan Wijesinghe - 47292105 <br> Gian-Luca Battaglia - 47416084 <br> Zion Xiaoxi Su - 46340661 <br> William Dixon - 47391146
>
> ## Movement
>
> Movement in Swing City has been fully implemented with basic controls such as walking (WASD), sprinting (Hold Shift + WASD), and jumping (Space). Since we also wanted the movement to be smooth rather than sudden, we gave the player a slightly slippery effect when walking and stopping. Jumping also has air drag, meaning the longer the player stays in the air, the more the horizontal velocity decreases over time.
> ## Sliding / Crouching
>
> Sliding and Crouching in Swing City has been implemented with the Control key. By holding the Control key, the player shrinks by half the height, allowing them to crawl under small spaces. A nice addition to crouching is when moving before they crouch, they can slide. By sliding, it is the same as crouching, but it maintains the speed for a short time before reverting to crawling speed.
> ## Wallrunning
>
> Wallrunning in Swing City has been implemented with a working camera transition. When the player aligns and runs along the wall, the camera tilts smoothly to give the effect of running almost sideways. Wallrunning also lasts for a short time, but they can also jump away from the wall during that time, which leads to wall-jumping.
Swinging / Grappling
Swinging and Grappling in Swing City now work fluently with their purpose in levels. They are also highly adjustable and currently show differing colours for distinction. They are controlled by mouse clicks (left click for swinging, and right click for grappling). Swinging currently uses joints while Grappling uses trajectories.
> ## UI
>
> Swing City has a working UI for many kinds of menu systems such as the main menu, pause menu, and settings menu. While the options menu is empty since it does not have any adjustable settings yet, the controls menu now shows the visual implementation of which controls refer to which mechanic.
> ## Respawning
> 
> The respawn system has been implemented with working checkpoints. While there are hardly any checkpoints placed in the scene, the respawn is coded to teleport the player back to the previous checkpoint (or back to the start). There is a fade in and out segment in respawning but due to its buggy nature, we did not show it in the Alpha Build, but rather made the player respawn instantly. Despite the fade segment still being worked on, the respawn and checkpoint system is working fine.
> ## Speed / Jump Boosts
>
> Speed and Jump Boost Pads in Swing City have been implemented with working functionality. Speed Boost Pads are coded to give the player a pushing force in the given direction, and a temporary speed boost. As for Jump Boost Pads, they have a simpler functionality by multiplying the player’s jump force. Both pads are coloured and drawn for distinction.
>
> ## Summary
> With these given mechanics, we were able to fully develop levels such as Tutorial, Hub World, and Level 1. However, we are still in the prototype phase as a lot of the assets lack design and instead use greyboxing elements in ProBuilder. Within the level design process, we are hoping to find art assets to use for Swing City and develop themes/biomes.
>

---------------------------------------------------------------------------
 
> # Reflection
> Over the course of developing this project, we have encountered numerous roadblocks that have impacted our game’s development and required changes in order to rectify. These roadblocks range from being small in nature to quite significant to our development cycle. <br>
>
> One such instance was during the development of the first-person camera, where two camera systems were developed at the same time that worked quite differently from one another. This meant that when it came time to implement the camera with the player, there was confusion over which one to keep and which to discard. Eventually, both systems had to be merged together, as there were some features that we desired for the player that could only work well in one of the camera systems, but the second system did other things more efficiently. This situation could have been avoided or lessened if collaboration between the team was more frequent and covered the work we were doing more comprehensively. <br>
>
> Another instance of this was during the designing of maps for the game, where it seemed that everyone was working on a different scene for a while, which meant that making changes to certain aspects was difficult. These maps also included many objects, making modification of them and changing each object’s layer to reflect whether it could be grappled or wall runned, etc, was cumbersome. In the end, we managed to make most things that needed to be in multiple scenes into prefabs, and began using certain tools such as ProBuilder to make developing new maps easier and more frictionless, though this could have been avoided had we done these things in the first place. <br>
>
> Lastly, in a general sense, our relationship with the time we have to do certain tasks has been shaky, in part due to the fact that we are starting from scratch as a new team this semester. While not unexpected, there have been times where we have completed tasks very close to their due dates, which is risky, and though we have gotten better at managing this over the past few weeks, there is still room to improve. <br>
>
>Ultimately, while we believe we have made good progress with the development of our game, there have been numerous challenges throughout the semester that we have had to recognise and overcome. After reflecting on them, it is clear that significant changes in development have been and will continue to be made in order to get our project done in a timely manner with a level of quality that we can be proud of. <br>
>

---

> # Playtesting
> ### Playtesting Approach
> Throughout the development of the project we adopted an iterative playtesting approach, where team members developed separate mechanics and tested them using a basic player controller. As the project progressed, these mechanics were further tested with a more advanced player controller that included features such as wall running and sliding. <br>
>
> ### Playtesting Influence on Design <br>
> By using this approach, it significantly influenced the way in which the initial development of mechanics was handled, as it allowed us to rapidly prototype and integrate new features within the group without the need for external play testers for the first half of the semester. <br>
>
> During development team members worked on their mechanics in separate branches, which resulted in individual playtests in each member's branch.This consequently led to the creation of 4 separate versions of the mechanics parameters like player walk speed and jump height to be created for each player's branch. Due to this method of playtesting being highly individualised it naturally led to highly integrable and adjustable mechanics, ensuring that the team did not need to constantly reset values when updating scenes. <br>
>
> Through this process of having 4 separate versions of mechanics parameters, we were also able to determine an optimal range for player controls, such as swinging and grappling early on, enabling the creation of a single, refined version of control input values for the player. This also fits the requirements for all the other mechanics dependent on it such as: wall running requiring the player to reach over a specific threshold of momentum provided by the player movement in order to execute.

> 
>
>
> 
