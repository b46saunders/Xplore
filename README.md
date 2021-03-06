# Xplore

Aim of this project is to learn about game development by creating a simple 2d space shooter with MonoGame.

I plan to use kenney's great set of free game assets for the project available at www.kenney.nl

# Helpful Resources
Understanding Steering Behaviours -
  http://gamedevelopment.tutsplus.com/series/understanding-steering-behaviors--gamedev-12732

  Math for Game Developers -
  https://www.youtube.com/playlist?list=PLW3Zl3wyJwWOpdhYedlD-yCB7WQoHf-My
  
  2d collision detection
  https://developer.mozilla.org/en-US/docs/Games/Techniques/2D_collision_detection

#Progress

Current WIP (Refactor)
  - Cleanup ScreenManager.cs and screen management in general
  - Define entities that need rendering/updates with interfaces (DONE)

Stage 1
  - Basic movement (DONE)
  - Camera follow player center(DONE)
  - Collisions (Boundary done)
  
Stage 2
  - Particles for exhaust (DONE)
  - Zooming camera out on speed increase (DONE)
  - Acceleration physics (Using Lerp currently...)

Stage 3
  - Collision Detection and Resolution
    - AABB Collision Detection and Resolution (DONE)
    - Sphere-Sphere Collision Detection and Resolution (DONE)
    - Sphere-Rectangle Collision Detection and Resolution
    - Bullet Collision Detection ,Sphere-Sphere (DONE)
    - Create nice API for this stuff for future use in other projects?

Stage 4
  - Basic enemy behaviour
    - Wander (DONE)
    - Seek (DONE)
    - Flee (DONE)
    - Bullets do damage to enemy ships (DONE)
    - Enemy ship explosions when Heal == 0 (DONE)
    - Collision Evasion - Not path finding but using steering vectors
  
Stage 5
  - Boulders
   - Random sizes
   - Random speeds (DONE)
   - Other random stuff to make them more fun
   - Boulder collision physics with ship(DONE) and bullets
    - Shooting them gives player score?
    - Make them explode!
    
  - Planets
    - Find/Create game art for planets
    - Create class that can generate random planets based on radius and mass
    - Random planets
    - Types of planets
    - Sphere of influence - gravity??

Stage 6
  - Further AI
    - Navigate round boulders and planets
    - Grouping movement
    - Advanced bullet leading
