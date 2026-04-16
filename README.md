# TopDownShooter
 RGP2/Assignment2
 
How to test 800+ entities in the gameplay scene
1. Open the Gameplay scene.
2. Find EnemySpawner in the Hierarchy.
3. Locate the Wave Settings list.
4. Drag the "Test" wave into the first slot.
5. Press Play.

How to test 1500+ entities in the testing scene
1. Open the Spatial Partitioning Test scene.
2. Find SPAWNER and SPAWNER SP in the Hierarchy.
3. To test entities using Unity’s physics:
   * Enable SPAWNER
   * Disable SPAWNER SP
5.  To test entities using custom physics with spatial partitioning:
   * Enable SPAWNER SP
   * Disable SPAWNER
