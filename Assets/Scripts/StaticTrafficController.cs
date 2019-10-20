using UnityEngine;

public class StaticTrafficController : MonoBehaviour
{
    public float yellowSignalTimer;
    public float allRedSignalTimer;
    public float[] trafficConfigTimers;

    public WireLoopSensor[] woodlandsRightWireLoops;
    public WireLoopSensor[] napierRightWireLoops;
    public WireLoopSensor[] napierStraightWireLoops;
    public WireLoopSensor[] woodlandsStraightWireLoops;

    private IntersectionController intersection;
    private int currentConfigSelection = 0;

    // Use this for initialization
    void Start()
    {
        intersection = GetComponent<IntersectionController>();

        int index = currentConfigSelection;
        currentConfigSelection += 1;
        float timeGreen = trafficConfigTimers[index];
        intersection.SwitchTrafficConfiguration(index);
        Invoke("SwitchConfig", timeGreen + yellowSignalTimer + allRedSignalTimer);
    }

    void SwitchConfig()
    {
        int index = currentConfigSelection;
        currentConfigSelection += 1;
        float timeGreen = trafficConfigTimers[index];
        bool skipConfig = true;
        WireLoopSensor[] relevantConfigSensors = new WireLoopSensor[0];

        // Check to see if relevant wire loop sensors are currently active
        switch (index)
        {
            case 0:
                relevantConfigSensors = woodlandsRightWireLoops;
                break;
            case 1:
                relevantConfigSensors = woodlandsStraightWireLoops;
                break;
            case 2:
                relevantConfigSensors = napierRightWireLoops;
                break;
            case 3:
                relevantConfigSensors = napierStraightWireLoops;
                break;
        }

        foreach (WireLoopSensor sensor in relevantConfigSensors)
        {
            if (sensor.GetTimeActivated() != 0.0)
            {
                skipConfig = false;
            }
        }

        // If not active, skip this config to the next section
        if (skipConfig)
        {
            SwitchConfig();
            return;
        }

        intersection.SwitchTrafficConfiguration(index);
        if(currentConfigSelection >= trafficConfigTimers.Length)
        {
            currentConfigSelection = 0;
        }
        Invoke("SwitchConfig", timeGreen + yellowSignalTimer + allRedSignalTimer);
    }
}
