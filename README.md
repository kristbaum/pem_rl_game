# ChessBrawl

TODOs (to be updated)
- A: Study the suitable hyperparameters in .yaml for training the model
- B: Implement the behaviors of agents in C# scripts
- C: Design the environments/scenes in Unity
- D: Add visual, sound effects and animations

Preferences:
- Van: 1A, 2B, 2C, 3D
- Martin:
- Ludwig:
- Max:

Subtasks:
- Design a simple scene with 1 chessboard, 2 agents, and chess pieces
- Consider how to implement the behaviors of agents kick the chess pieces
- Consider hot to implement the chess pieces to move forward or kick other chess pieces
- Consider which information needed to feed the agents to solve desired tasks
- Implement the OnEpisodeBegin() in C#
- Implement CollectObservations(VectorSensor sensor) in C#
- Implement OnActionReceived(ActionBuffers actionBuffers) in C#
- Implement the human player mode
- Study and test different hyperparameters for training the model
- Set up configuration file .yaml
- Train the agents
- Test the learning with inference model
- Test the learning with heuristic mode
- Design creative chess figures 
- Add sound, visual effects, animations






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
