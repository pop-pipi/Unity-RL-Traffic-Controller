# Unity-RL-Traffic-Controller
Unity based traffic simulator. A learning environment for a reinforcement learning agent to control traffic lights at a single intersection, training to maximise traffic flow.

This repository provides two simlulation scenes and a pretrained agent 'IntersectionBrain.nn', which is used within the 'comparison_simulation'.

## Setup

### Requirements
* ML Agents - Required to train agent
* Git Large File Storage (Git LFS) (https://github.com/git-lfs/git-lfs/wiki/Installation)
* Optional - Virtual Environment (Or any package manager of your choice)
* Optional - Unity (Project requires version 2018.4.10f1 or above, required to run simulation scenes within the editor and change parameters)

### Installation
##### Assumes Git LFS is installed as well as Unity for optional steps
- Clone the project `git clone https://github.com/pop-pipi/Unity-RL-Traffic-Controller.git`
- Run git lfs install for project (refer to Contributing Notes) `git lfs install --local`
- install ml-agents package `pip3 install ml-agents` (This is required for training)
##### Optional Steps - Using the Unity Editor
- (For Unity Editor) Add the cloned Unity-RL-Traffic-Controller directory as a project in Unity
- (For Unity Editor) Add trainer scene OR comparison scene to view from the bottom Project navigation panel
  - Drag relevant scene from Assets > Scenes into view hierarchy
- Refer to the Running from Unity section below for more details


## Running application (Unity not required)

### Running the Comparison Scene
- Run the comparison_simulation from the project folder (double click)
- The machine learning agent controls the left environment, while the static controller is employed on the right. The timescale for the simulation is increased roughly by a factor of 10.
- On closing the simluator, four data sets are generated and saved into the Assets/comparisons/ folder, two for the static controller and two for the agent
    - Car travel time (Total length of time from spawn to path completion)
    - Car throughput (Each row hold a timestamp indicating when a car crossed the intersection)

### Running the Training Scene
- Run the training_simulation from the project folder `mlagents-learn config/sac_trainer_config.yaml --env="training_simluation" --run-id=<Name your training session> --train`
- This will begin training at a timescale increased roughly by a factor of 100. On completion of the simulation (or on cancel, Control+C) the trained model will be exported to models/<run-id>, with a tensorboard summary provided in summary/<run-id>
- To view the summary with tensorboard run `tensorboard --logdir=summaries` from the project folder and visit `localhost:6006` in your browser
- To use the new trained model in the comparison scene, refer to the steps below under 'Running the Comparison Scene' (Unity required)

## Running from Unity

### Running the Comparison Scene
- Drag the 'ComparisonScene', from the Assets view below (bottom middle panel), into the view Heirarchy. The scene is located in the Scenes folder
- Highlight 'IntersectionBrain.asset' from the assets view. Ensure the Model field is assigned the brain IntersectionBrain (The IntersectionBrain in the assets folder is a pretrained SAC RL agent, although this can be replaced with any trained model from the models/ folder in the project root)
- Highlight 'Academy' from the Hierarchy view (left panel), and ensure the 'Control' tickbox is unticked next to IntersecitonBrain
- Click the play button to run the scene
- The machine learning agent controls the left environment, while the static controller is employed on the right. The timescale for the simulation is increased roughly by a factor of 10.
- On stopping the simluator, four data sets are generated and saved into the Assets/comparisons/ folder, two for the static controller and two for the agent
    - Car travel time (Total length of time from spawn to path completion)
    - Car throughput (Each row holds a timestamp indicating when a car crossed the intersection)
    
### Running the Training Scene
- Drag the 'ComparisonScene', from the Assets view below (bottom middle panel), into the view Heirarchy. The scene is located in the Scenes folder
- Highlight 'IntersectionBrain.asset' from the assets view. Ensure the Model field is assigned the brain IntersectionBrain (The IntersectionBrain in the assets folder is a pretrained SAC RL agent, although this can be replaced with any trained model from the models/ folder in the project root)
- Highlight 'Academy' from the Hierarchy view (left panel), and ensure the 'Control' tickbox is unticked next to IntersecitonBrain
- Run the ml-agents learn command from the project folder while the editor is open in the background `mlagents-learn config/sac_trainer_config.yaml --run-id=<Name your training session> --train`
- When the Unity logo appears in your terminal, click the play button to run the scene
- This will begin training at a timescale increased roughly by a factor of 100. On completion of the simulation (or on cancel, Control+C) the trained model will be exported to models/<run-id>, with a tensorboard summary provided in summary/<run-id>
- To view the summary with tensorboard run `tensorboard --logdir=summaries` from the project folder and visit `localhost:6006` in your browser

## Notes
### Contributers
- Contributers must use Git LFS when commiting to this repo, running this command in the project directory `git lfs install --local` (note if you have installed Git LFS on your OS previously using `git lfs install` then this step is not required)

### Other Tips
- The powerpoint file included in the project was used to create the intersection image. Feel free to play around with it and create other intersections (Please note creating your own environment and adding Paths with waypoints, traffic lights, etc. is not documented - although there are minor directions in the source code. Refer to the provided scenes for how the different components should be laid out and referenced)
- The ml-agents documentation is quite thorough and it is highly recommended you read through it should you want to run simultaneous training sessions, etc. https://github.com/Unity-Technologies/ml-agents

### Limitations
- Cars do not check for a safe right turn, therefore in this simulation right turn traffic lights are always red unless explicity set to green by the traffic controller as part of safe traffic configuration
- Driver behaivour is modelled as simply as possible, as such some parameters are restricted, for example acceleration is a constant value

