using UnityEngine;
using MLAgents;

public class IntersectionAgent : Agent
{
    IntersectionController intersection;

    // Start is called before the first frame update
    void Start()
    {
        intersection = GetComponent<IntersectionController>();
    }

    public override void CollectObservations()
    {
        // No cars per lane
        foreach (TrafficPath path in intersection.paths)
        {
            AddVectorObs(path.noCars);
        }

        // Current traffic configuration
        AddVectorObs(intersection.currentConfig);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        // Actions, size = 1
        Debug.Log("Action value = " + vectorAction[0]);
        intersection.SwitchTrafficConfiguration((int)vectorAction[0]);

        // Reward
        float totalNumCars = 0;
        foreach (TrafficPath path in intersection.paths)
        {
            totalNumCars += path.noCars;
        }

        SetReward(-totalNumCars);
    }
}
