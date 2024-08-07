# Wizards and Goblins-1 
### Gameplay video

 If there are any wizards within range, a goblin will attack the weakest one (if the it can attack on this frame).
• Since wizards have an area-of-effect spell, if there are any goblins within range, a wizard will attack all of them (again, only if the wizard can attack on this frame).
• Whenever any entity is attacked, they are knocked backwards / pushed away from their attacker.
In addition to attacking:
• All goblins move in a random direction every single frame.
• Unlike goblins, wizards cannot attack and move in the same frame; if a wizard was not able to attack for this frame (either because its cooldown was not complete or because there were none in range), then it will move towards the closest goblin in the scene instead.
Once all wizards and goblins have processed their "turns" for the current tick/frame, any wizards or goblins that have been defeated (their HP has fallen to zero) should be removed from both of their respective ArrayLists.
Every so often, either side gets reinforcements. A new goblin should appear in the scene every 15 frames/ticks. A new wizard should appear every 50 frames/ticks. When they appear, a wizard/goblin will need to be inserted into both lists: when inserting into the all-entities list, make sure the list stays in sorted order.

https://github.com/user-attachments/assets/a4862cad-e9a7-46c8-b54c-47ef6bf4a02c

