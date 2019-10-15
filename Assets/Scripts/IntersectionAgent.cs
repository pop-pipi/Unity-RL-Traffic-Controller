using UnityEngine;
using MLAgents;

public class IntersectionAgent : Agent
{
    IntersectionController intersection;
    CarSpawner carSpawner;

    // Start is called before the first frame update
    void Start()
    {
        intersection = GetComponent<IntersectionController>();
        carSpawner = intersection.carSpawner;
        RequestDecision();
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
            float nvalue = 0;
            if (System.Math.Abs(totalNumCars) > 0)
            {
                nvalue = path.noCars / totalNumCars;
            }
            AddVectorObs(nvalue);
        }
        // Current traffic configuration
        AddVectorObs(intersection.currentConfig);
        AddVectorObs(intersection.getTimeInCurrentConfig());
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        // Actions
        intersection.SwitchTrafficConfiguration((int)vectorAction[0]); // Chosen configuration
        float timeOnConfig = vectorAction[1]; // Chosen length of configuration
        Invoke("RequestDecision", timeOnConfig);
        Debug.Log("Decision made: " + (int)vectorAction[0] + ", over time: "+timeOnConfig);

        // Reward
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
    }

    private void OnTriggerExit(Collider other)
    {
        AddReward(0.05f);
    }

    public override void AgentOnDone()
    {
        Debug.Log("Agent Done");
        intersection.resetIntersection();
    }
}
