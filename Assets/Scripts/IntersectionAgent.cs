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
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        float timeGreen = vectorAction[1]; // Chosen length of configuration

        if (phaseNumber < episodeLength)
        {
            if (intersection.currentConfig == (int)vectorAction[0])
            {
                Invoke("FinishAction", timeGreen);
            }
            else
            {
                Invoke("FinishAction", timeGreen + intersection.yellowSignalTimer + intersection.timeBetweenConfigs);
            }

            // Switch traffic config
            intersection.SwitchTrafficConfiguration((int)vectorAction[0]); // Chosen configuration
            Debug.Log("Decision made: " + (int)vectorAction[0] + ", over time: " + timeGreen);
            phaseNumber += 1;
        } else
        {
            Done();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        //AddReward(0.05f);
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
        //AddReward((float)(-avgWait * 0.0005));
        AddReward((float)(-avgWait * 0.01));
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
