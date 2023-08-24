# Pool Chess (Working title)

Chess with pool cues. A Reinforcement Learning game implemented in Unity. 

TODO:
* find existing (open source) implementation of pool/billiard game for unity, to inspire the control mechanism 
-> Resources Pool game Unity:
https://github.com/fgrehm/pucrs-unity3d-pool
https://www.youtube.com/watch?v=2nHh9vzTW_Y

* find existing (open source) implementation of (3D) chessboard, to inspire the assets
-> Resources Chess game Unity:
https://www.youtube.com/watch?v=FtGy7J8XD90
https://assetstore.unity.com/packages/3d/props/chess-pieces-board-70092
https://github.com/SacuL/3D-Chess-Unity
https://www.cgtrader.com/free-3d-models/chess

* decide on a chess engine -> is there any free chess engine?
* Consider how to train the agent, how to build the physic simulation 
* test the learning
* Decide on a game name: Pool Chess, Cue Chess, Chess Billiard, Chess Billiards, Chess Pool? -> I suggest Pool Chess
* Design creative chess figures (optional depending on time)
* Implement animations
* Add extra pool areas where we place the ball on. Reference: https://play.google.com/store/apps/details?id=com.SAGlobalLLC.ChessPool&hl=gsw&gl=US&pli=1

Take-away from presentation:
* Using the chess engine should be only done if the Agent actually needs to do anything. (For testing it would be nice to just use the engine to see if it can learn the physical task to move a figure across the board)
* Maybe use a pool ball additionally, to add the complexity to the game
* Test how hard it is for a user, the agent can do the physical task way easier, maybe add a target mark for users
* Something else?

# Setup
Install Unity (https://docs.unity3d.com/hub/manual/InstallHub.html#install-hub-linux)
Add the correct packages (https://unity-technologies.github.io/ml-agents/com.unity.ml-agents/)
'''
conda create -n chessbrawl python=3.10.8
conda activate chessbrawl
pip install mlagents
'''