using UnityEngine;

public class IntersectionController : MonoBehaviour
{
    public float yellowSignalTimer;
    public float timeBetweenConfigs;
    public TrafficConfiguration[] configurations;
    public CarSpawner carSpawner;

    // TODO: Review OBS REFERENCES
    public TrafficPath[] paths;

    private bool changingConfigurations;
    public int currentConfig;

    private float startedCurrentConfig;

    // Set all traffic light yellow signal lengths
    void Start()
    {
        startedCurrentConfig = Time.time;

        for (int i = 0; i < configurations.Length; i++)
        {
            TrafficConfiguration config = configurations[i];
            for (int a = 0; a < config.trafficLights.Length; a++)
            {
                config.trafficLights[a].yellowSignalTimer = yellowSignalTimer;
            }
        }
    }

    // Change traffic config after max time
    void FixedUpdate()
    {
        /*
        if(Time.time - startedCurrentConfig > 10)
        {
            GetComponent<IntersectionAgent>().RequestDecision();
        } */
    }

    public void SwitchTrafficConfiguration(int index)
    {
        if (!changingConfigurations && currentConfig != index)
        {
            changingConfigurations = true;
            currentConfig = index;

            // Close traffic in all lanes
            foreach (TrafficConfiguration configuration in configurations)
            {
                foreach (TrafficLight trafficLight in configuration.trafficLights)
                {
                    trafficLight.CloseTraffic();
                }
            }

            // Open traffic in lanes selected by User
            foreach (TrafficLight trafficLight in configurations[index].trafficLights)
            {
                trafficLight.Invoke("OpenTraffic", timeBetweenConfigs);
                Invoke("FinishedChangingConfig", timeBetweenConfigs);
            }
            return;
        }
    }

    private void FinishedChangingConfig()
    {
        changingConfigurations = false;
        startedCurrentConfig = Time.time;
    }

    public float getTimeInCurrentConfig()
    {
        return Time.time - startedCurrentConfig;
    }

    public void resetIntersection()
    {
        // Close traffic in all lanes
        changingConfigurations = true;
        currentConfig = -1;
        foreach (TrafficConfiguration configuration in configurations)
        {
            foreach (TrafficLight trafficLight in configuration.trafficLights)
            {
                trafficLight.CloseTraffic();
            }
        }
        changingConfigurations = false;
    }
}
