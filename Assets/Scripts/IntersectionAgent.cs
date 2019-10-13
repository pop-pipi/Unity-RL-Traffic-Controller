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
        float totalNumCars = 0;
        foreach (TrafficPath path in intersection.paths)
        {
            totalNumCars += path.noCars;
        }

        // No cars per lane, normalized from max cars
        foreach (TrafficPath path in intersection.paths)
        {

            float nvalue = path.noCars / totalNumCars;
            AddVectorObs(nvalue);
        }

        // Current traffic configuration
        //AddVectorObs(intersection.currentConfig);
        //AddVectorObs(intersection.getTimeInCurrentConfig());
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        // Actions, size = 1
        intersection.SwitchTrafficConfiguration((int)vectorAction[0]);

        // Reward
        /*
        float totalNumCars = 0;
        foreach (TrafficPath path in intersection.paths)
        {
            totalNumCars += path.noCars;
            /*
            if(totalNumCars<path.noCars)
            {
                totalNumCars = path.noCars;
            } 
        }
        AddReward(-totalNumCars);
        */
        //Debug.Log("Chosen action - " + vectorAction[0] + ", Current Num Cars - " + totalNumCars + ", Time in conifg - "+intersection.getTimeInCurrentConfig());
    }

    private void OnTriggerExit(Collider other)
    {
        AddReward(0.01f);
    }
}
