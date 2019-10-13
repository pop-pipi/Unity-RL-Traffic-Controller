using UnityEngine;

[System.Serializable]
public class TrafficConfiguration : MonoBehaviour
{
    public TrafficLight[] trafficLights;

    public TrafficConfiguration(TrafficLight[] trafficLights)
    {
        this.trafficLights = trafficLights;
    }
}
