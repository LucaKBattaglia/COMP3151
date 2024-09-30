> # COMP3151 - Progress Report 1
> Dylan Wijesinghe - 47292105 <br> Gian-Luca Battaglia - 47416084 <br> Zion Xiaoxi Su - 46340661 <br> William Dixon - 47391146

> # Current Progress
> We have made significant progress throughout the development process. At this point, we have developed all the core mechanics for the game.
>
> ## Movement
> 
> https://github.com/user-attachments/assets/d72d37a8-e420-4e20-a1b6-7219c6af63b5
>
> https://github.com/user-attachments/assets/cad42375-6943-4b62-916c-7770e19bff7f
> 
> https://github.com/user-attachments/assets/aa28ec26-be47-465a-b961-e279ca0a7e32
> 
> Movement in Swing City has been fully implemented with basic controls such as walking (WASD), sprinting (Shift + WASD), and jumping (Space). Since we also wanted the movement to be smooth rather than sudden, we gave the player a slightly slippery effect when walking and stopping. Jumping also has air drag, meaning the longer the player stays in the air, the more the horizontal velocity decreases over time.
> ## Sliding / Crouching
>
> Sliding and Crouching in Swing City has been implemented with the Control key. By holding Control, the player shrinks in half, allowing them to crawl under small spaces. A nice addition to crouching is when moving before they crouch, they can slide. Sliding is similar to crouching, but it maintains player speed for a short time before reverting to crawling speed.
> ## Wallrunning
> 
> https://github.com/user-attachments/assets/808c3b0b-b83a-4388-9f60-1d3134653e88
> 
> Wallrunning in Swing City has been implemented with a working camera tilt. When the player runs along the wall, the camera tilts smoothly to give the effect of running almost sideways. Wallrunning lasts for a short time, but players can jump away from the wall during that time, which leads to wall-jumping.
> 
> ## Swinging / Grappling
> 
> https://github.com/user-attachments/assets/ce823c3c-8c31-403e-8a04-7a60caaa6b9d
>
> https://github.com/user-attachments/assets/42f2cbab-218c-4c9d-95c5-c52d34fce715
> 
> Swinging and grappling in Swing City now work fluently with their purpose in levels. They are also highly adjustable and currently show differing colours for distinction. They are controlled by mouse clicks (left click for swinging, and right click for grappling). Swinging currently uses joints while Grappling uses trajectories.
> ## UI
> 
> https://github.com/user-attachments/assets/eef690c5-4821-41ce-b1bd-ba46e0151689
> 
> Swing City has a working UI for many kinds of menu systems such as the main menu, pause menu, and settings menu. While the options menu is empty since it does not have any adjustable settings yet, the controls menu now shows the visual implementation of which controls refer to which mechanic.
> ## Respawning
> 
> https://github.com/user-attachments/assets/ace63c4e-d531-47f1-a060-791a44e6760d
> 
> The respawn system has been implemented with working checkpoints. While there are hardly any checkpoints placed in the scene, the respawn is coded to teleport the player back to the previous checkpoint (or back to the start). There is a fade in and out segment in respawning but due to its buggy nature, we did not show it in the Alpha Build, but rather made the player respawn instantly. Despite the fade segment still being worked on, the respawn and checkpoint system is working fine.
> ## Speed / Jump Boosts
> 
> https://github.com/user-attachments/assets/525dbe94-97cb-4eef-82ea-53061b9b8795
> 
> https://github.com/user-attachments/assets/254a4c2e-337b-42d6-8267-abbf65dd0316
> 
> Speed and Jump Boost Pads in Swing City have been implemented with working functionality. Speed Boost Pads are coded to give the player a pushing force in the given direction, and a temporary speed boost. As for Jump Boost Pads, they have a simpler functionality by multiplying the player’s jump force. Both pads are coloured and drawn for distinction.
>
> ## Summary
> With these given mechanics, we were able to fully develop levels such as Tutorial, Hub World, and Level 1. However, we are still in the prototype phase as a lot of the assets lack design and instead use greyboxing elements in ProBuilder. Within the level design process, we are hoping to find art assets to use for Swing City and develop themes/biomes. 
>
> https://github.com/user-attachments/assets/e12e9eaa-6a10-4b2c-bf44-d4d8edd0b1dd
>
> https://github.com/user-attachments/assets/39a8ea4e-8c60-4eeb-9b20-08a0e5895046

---------------------------------------------------------------------------
 
> # Reflection
> Throughout project development, we have encountered numerous roadblocks that have impacted our game’s development and required changes to rectify. These roadblocks range from being small to quite significant to our development cycle.<br>
>
> One such instance was during the development of the first-person camera, where two camera systems were developed at the same time that worked quite differently from one another. This meant that when it came time to implement the camera with the player, there was confusion over which one to keep and which to discard. Eventually, both systems had to be merged, as there were some features that we desired for the player that could only work well in one of the camera systems, but the second system did other things more efficiently. This situation could have been avoided or lessened if collaboration between the team was more frequent and covered the work we were doing more comprehensively. <br>
>
> Another instance of this was during the designing of maps for the game, where it seemed that everyone was working on a different scene for a while, which meant that making changes to certain aspects was difficult. These maps also included many objects, making modifications of them and changing each object’s layer to reflect whether it could be grappled or wall run, etc, was cumbersome. In the end, we managed to make most things that needed to be in multiple scenes into prefabs and began using certain tools such as ProBuilder to make developing new maps easier and more frictionless, though this could have been avoided had we done these things in the first place.<br>
>
> Ultimately, while we believe we have made good progress with the development of our game, there have been numerous challenges throughout the semester that we have had to recognise and overcome. After reflecting on them, it is clear that significant changes in development have been and will continue to be made to get our project done promptly and with a level of quality that we can be proud of. 

---

> # Changes in Development
> The changes in development that have been made for our project have been quite frequent, which we had expected due to how quickly we have had to come up with a new game idea to design mechanics for and build upon over 6 weeks. However, some changes have been made after reflection and feedback that have fundamentally affected the design of our game, which we will go into more detail about in this section. <br>
>
> Firstly, the roles that we initially assigned ourselves have been subject to change depending on what needs to be done at that moment. For instance, Luca was originally supposed to be the lead playtester for our project, but since the beginning of the semester, he has been more involved with the programming side due to the amount of work that needed to be done. Our approach to playtesting has also been quite spread out over the team, with each person getting others to play their branch’s version and making changes based on feedback. This shows that consideration of priorities has been taken into account across the team.<br>
> 
> Another change made was the way that we would design levels. At first, we were planning on implementing non-linear levels that had larger places to explore, but upon feedback, we decided that we should make more linear levels to play to the strengths of our game rather than having a larger scope that would be harder to meet. This demonstrates that our team can respond to feedback quickly to improve the end experience. <br>
>
> These changes have affected our production plan by altering the scope and completion timeline of our project. A smaller scope concerning levels and level content should make it much easier for us to hit our design goals and milestones over the rest of our semester, resulting in our completion timeline being more spread out. They have also affected how view certain aspects of our game during design and development, ensuring we are more open to feedback and alterations. In the end, we believe that the changes that we have made have served to positively impact both the development experience and the final product. <br>
>
---
> # Playtesting
> ### Playtesting Approach
> Throughout the development of the project, we adopted an iterative playtesting approach, where team members developed separate mechanics and tested them using a basic player controller. As the project progressed, these mechanics were further tested with a more advanced player controller that included features such as wall running and sliding.<br>
>
> ### Playtesting Influence on Design <br>
> By using this approach, it significantly influenced how the initial development of mechanics was handled, as it allowed us to rapidly prototype and integrate new features within the group without the need for external playtesters for the first half of the semester.<br>
>
> During development team members worked on their mechanics in separate branches, which resulted in individual playtests in each member's branch. This consequently led to the creation of 4 separate versions of the mechanics' parameters like player walk speed and jump height to be created for each player's branch. Due to this method of playtesting being highly individualised, it naturally led to highly integrable and adjustable mechanics, ensuring that the team did not need to constantly reset values when updating scenes.<br>
>
> Through this process of having 4 separate versions of mechanics parameters, we were also able to determine an optimal range for player controls, such as swinging and grappling early on, enabling the creation of a single, refined version of control input values for the player. This also fits the requirements for all the other mechanics dependent on it such as wall running requiring the player to reach over a specific threshold of momentum provided by the player's movement to execute. <br>
>
> ---
