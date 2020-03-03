# Crystal-Reign

![alt text](https://user-images.githubusercontent.com/23744531/74228774-6bd8f080-4cc1-11ea-84a7-bc9256a21b84.png)

## Overwiev

Crystal Reign is a TPS game based on matches versus AI. Game mechanics allow environment destruction and partial control of the destruction process. When a bullet hits an object made of fragile material, it breaks apart into debris. Shortly after that, player can "freeze" falling debris, i.e. stop their movement and suspend them in the air, which may serve as a shield or platform for climbing.  

## Gameplay
The goal is to survive and defeat as many bots as possible. Current build includes two kinds of bots (static, soaring) and 
one scene containing objects applied with one of materials:
* Indestructible: doesn't interact with bullets
* Fully destructible: breaks into debris, which may be "freezed"
* Partly destructible: only a fragment of the object breaks into "freezable" pieces; the rest remains unaffected and doesn't changle the position

There are 2 types of weapons: fast (weaker) and powerful (slower). Powerful bullets destroy appropriate objects and create and expanding sphere, which represents the area possible to freeze. The sphere fades shortly after collision. It doesn't affect player nor bots.

## Download
The game is in CrystalReign.zip. Extract the archive and run build.exe.

## Controls
* WASD - move
* E - freeze debris inside sphere
* C - show cursor
* Scroll - change type of weapon
* LMB - shoot
* RMB - zoom in

![alt-text](https://user-images.githubusercontent.com/23744531/74230145-1225f580-4cc4-11ea-8ffd-20a340f1095b.jpg)

<!-- Gallery ? -->
