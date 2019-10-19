using UnityEngine;
using MLAgents;

public class IntersectionAgent : Agent
{
    public int episodeLength;
    public IntersectionController intersection;
    public CarSpawner carSpawner;

    private int phaseNumber;

    // Start is called before the first frame update
    void Start()
    {
        intersection = GetComponent<IntersectionController>();
        carSpawner = intersection.carSpawner;
        RequestDecision();
    }

    public override void CollectObservations()
    {
        // Debug printing included
        foreach (WireLoopSensor wireLoop in intersection.wireLoops)
        {
            
            if (wireLoop.GetTimeActivated() == 0.0)
            {
                AddVectorObs(0);
            } else
            {
                AddVectorObs(1);
            } 

            //AddVectorObs((float)wireLoop.GetTimeActivated());
        }


        /*
        // No cars per lane, normalized from max cars

        float totalNumCars = 0;
        foreach (TrafficPath path in intersection.paths)
        {
            totalNumCars += path.noCars;
        }

        foreach (TrafficPath path in intersection.paths)
        {
            float nvalue = 0;
            if (System.Math.Abs(totalNumCars) > 0)
            {
                nvalue = path.noCars / totalNumCars;
            }
            AddVectorObs(nvalue);
        } */

        // Current traffic configuration
        //AddVectorObs(intersection.currentConfig);
        //AddVectorObs(intersection.getTimeInCurrentConfig());
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        if (phaseNumber < episodeLength)
        {
            // Actions
            intersection.SwitchTrafficConfiguration((int)vectorAction[0]); // Chosen configuration
            float timeGreen = vectorAction[1]; // Chosen length of configuration
            Debug.Log("Decision made: " + (int)vectorAction[0] + ", over time: " + timeGreen);

            // Reward
            Invoke("FinishAction", timeGreen);
            phaseNumber += 1;
        } else
        {
            Done();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        AddReward(0.05f);
    }

    private void FinishAction()
    {
        float totalWait = 0;
        foreach (CarController car in carSpawner.cars)
        {
            totalWait += car.TimeSinceStop();
        }
        float avgWait = totalWait / carSpawner.cars.Count;
        if (float.IsNaN(avgWait))
        {
            avgWait = 0;
        }
        AddReward((float)(-avgWait * 0.0005));
        Debug.Log("Reward for step: " + GetReward());
        RequestDecision();
    }

    public override void AgentReset()
    {
        Debug.Log("Reward for episode: " + GetCumulativeReward());
        Debug.Log("Agent Reset");
        phaseNumber = 0;
        RequestDecision();
    }
}
