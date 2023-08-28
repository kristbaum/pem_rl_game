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
- Consider how to implement the behaviors of agents to kick the chess pieces
- Consider how to implement the chess pieces to move forward or kick other chess pieces
- Consider which information needed to feed the agents to solve desired tasks
- Implement the setup of the environment for a new episode e.g. OnEpisodeBegin() in C#
- Implement observing behaviors of agents e.g. CollectObservations(VectorSensor sensor) in C#
- Implement the actions of agents e.g. OnActionReceived(ActionBuffers actionBuffers) in C#
- Implement the collisions of the agents with walls, other chess pieces, goals
- Implement the rewards for agents
- Implement the human player mode
- Study self-play strategy for multi agents in adversarial games
- Implement self-play mode in .yaml and C#
- Study and test different hyperparameters for training the model
- Implement the policy and optimizer for the trainer in Python (PPO, SAC, POCA or self-made trainer)
- Set up corresponding configuration files .yaml
- Train the agents
- Test the learning with inference model
- Test the learning with heuristic mode
- Design creative chess figures, agents 
- Add sound, visual effects, animations


How to train the model/agents on Windows: (can be done in a separated environment/project)
1. Make sure ml-agents is installed, a virtual environment is activated
2. Navigate to the downloaded folder ml-agents -> `mlagents-learn <path to configuration file .yaml> <optional run-id>`
3. Wait until a port is listening
4. Navigate to the unity project -> click on start button

How to stop the training:
- a. Click on stop button in unity, can start the training again when clicking on play button again
- b. Strg C 

How to test the model/agents on Windows: (can be done in a separated environment/project)
- a. Test the behaviors (for human play mode)
  - Navigate to agents objects in unity project -> Behavior Parameters -> Behavior Type -> Heuristic Only -> use key board or mouse to test the agents
- b. Test the trained model:
  - Navigate to agents objects in unity project -> Behavior Parameters -> Behavior Type -> Inference Only
  - Navigate to /results -> find the trained model -> drag it into the unity project -> add it as model in Behavior Parameters -> click on play button 


Take-away from presentation:
* Using the chess engine should be only done if the Agent actually needs to do anything. (For testing it would be nice to just use the engine to see if it can learn the physical task to move a figure across the board)
* Maybe use a pool ball additionally, to add the complexity to the game
* Test how hard it is for a user, the agent can do the physical task way easier, maybe add a target mark for users
* Something else?

# Setup
* Install Unity (https://docs.unity3d.com/hub/manual/InstallHub.html#install-hub-linux) in Version 2021.3 LTS
* Add the correct ML_Agent package to Unity (https://unity-technologies.github.io/ml-agents/com.unity.ml-agents/) (Probably Version 2.0.1)
* Download ML agents for locally
'''
conda create -n chessbrawl python=3.8
conda activate chessbrawl
pip install mlagents
'''

Reset of all boards if agent dies.

# Result board

https://docs.google.com/spreadsheets/d/1vLPF9WEUuqI6n34Wo41OAbFHOeJACiuNkI61odF_tic/edit

# Trainble Values

| Name  | Description | User interaction equivalent |
| ------------- | ------------- | ------------- |
| x-axis  | for agent movement  | arrow key |
| y-axis  | for agent movement  | arrow key |
| speed | for agent movement  | none |
| kickforce |  applied to kicked objects  | none |

# Reward parameters

| Name  | Description | Idea/Learning |
| ------------- | ------------- | ------------- |
| Collision Penalty Wall | (Rule) Large penalty for throwing the agent off the board (the wall is the boundary) | Obvious rule to prevent the agent from falling |
| CP Touching Other Agent  | (Rule) Large penalty for touching  | Rule of the game |
| CP Touching opponent piece | (Rule) Large penalty; Agents should not touch opposing pieces | Rule of the game |
| CP Kicking of own piece |  Apply penalty for doing an action that kicks of a piece of your own  | Disincentivise the agents from losing pieces through own actions  |
| Collision Reward Touching own piece | Reward for touching one of your own chess pieces  | Intended to incentivise movement and interaction of the agent |
| CR Kick Motion (Chain reation)  | Calculates an extra reward for a kick that leads to a sucessful kick off of an oponent piece | Incentivise actions that directly lead to kicking of oponent pieces |
| CR Kicking Off Oponent Piece (Globally) | Reward for a oponent piece falling off the board | intended as a global reward similar to the human score for removing pieces from the board |

# Ideas to try

* Additional physical boundaries, some wall in the middel or to the sides
* Make agent King or Queen instead of external pawn
* How to control speed and kickforce as human?
* Introduce timelimit for training (in epoch steps) and interactive game (as time?)
* Third person camera (already implemented)
* Score board for humans (already implemented)

# DEMO video on Friday as part of presentation:

120 seconds
16:9 MP4

# Afterwards
Project abstract, which kind of training etc, user interactions etc
Statment of contribution of each project member
