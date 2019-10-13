using UnityEngine;

public class IntersectionController : MonoBehaviour
{
    public float yellowSignalTimer;
    public float timeBetweenConfigs;
    public TrafficConfiguration[] configurations;

    // TODO: Review OBS REFERENCES
    public TrafficPath[] paths;

    private bool changingConfigurations;
    public int currentConfig;

    // Set all traffic light yellow signal lengths
    void Start()
    {
        for (int i = 0; i < configurations.Length; i++)
        {
            TrafficConfiguration config = configurations[i];
            for (int a = 0; a < config.trafficLights.Length; a++)
            {
                config.trafficLights[a].yellowSignalTimer = yellowSignalTimer;
            }
        }
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
                Invoke("FinishedChangingLanes", timeBetweenConfigs);
            }
            return;
        }
    }

    private void FinishedChangingLanes()
    {
        changingConfigurations = false;
    }
}