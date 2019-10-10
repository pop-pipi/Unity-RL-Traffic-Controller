# Unity-RL-Traffic-Controller
Unity based traffic simulator. A learning environment for a reinforcement learning agent to control traffic lights at a single intersection, training to maximise traffic flow.

### Requirements
* Unity (Project requires version 2018.4.10f1 or above)
* Git Large File Storage (Git LFS) (https://github.com/git-lfs/git-lfs/wiki/Installation)
* Optional - Virtual Environment (Highly Recommended) **Not required at this stage - ML-Agents yet to be implemented**

### Installation
##### Assumes Unity and Git LFS is installed
- Clone the project `git clone https://github.com/pop-pipi/Unity-RL-Traffic-Controller.git`
- Run git lfs install for project (refer to Contributing Notes) `git lfs install --local`
- Add the cloned Unity-RL-Traffic-Controller directory as a project in Unity
- Add sample scene to view from the bottom Project navigation panel
  - Drag 'SampleScene' from Assets > Scenes into Hierarchy
  
## Running the Scene (Player)
- Click the Play button at the top of the scene view
- This scene comes with 8 pre-defined traffic configurations, tap a number on your keyboard to flick through each one
- To change paramters such as car spawn rate, acceleration and max speed, you may edit these from the 'Car Spawner' asset located in the hierarchy. 

## Notes
### Contributers
- Contributers must use Git LFS when commiting to this repo, running this command in the project directory `git lfs install --local` (note if you have installed Git LFS on your OS previously using `git lfs install` then this step is not required)

### Other Tips
- The powerpoint file included in the project was used to create the intersection image. Feel free to play around with it and create other intersections (Please note adding Paths with waypoints, traffic lights, etc. is not documented - although there may be some minor direct in the source code. Refer to the Sample Scene for how the different components should be laid out)
