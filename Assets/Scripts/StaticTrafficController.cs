using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class StaticTrafficController : MonoBehaviour
{
    public float yellowSignalTimer;
    public float allRedSignalTimer;
    public float[] trafficConfigTimers;
    private List<string[]> carThroughPutData = new List<string[]>();
    private List<string[]> carTravelTimeData = new List<string[]>();

    public WireLoopSensor[] woodlandsRightWireLoops;
    public WireLoopSensor[] napierRightWireLoops;
    public WireLoopSensor[] napierStraightWireLoops;
    public WireLoopSensor[] woodlandsStraightWireLoops;

    private IntersectionController intersection;
    private int currentConfigSelection = 0;

    // Use this for initialization
    void Start()
    {
        CreateHeaderRow();

        intersection = GetComponent<IntersectionController>();

        int index = currentConfigSelection;
        currentConfigSelection += 1;
        float timeGreen = trafficConfigTimers[index];
        intersection.SwitchTrafficConfiguration(index);
        Invoke("SwitchConfig", timeGreen + yellowSignalTimer + allRedSignalTimer);
    }

    void SwitchConfig()
    {
        Debug.Log("Switching Config");
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

        if (currentConfigSelection >= trafficConfigTimers.Length)
        {
            currentConfigSelection = 0;
        }

        // If not active, skip this config to the next section

        if (skipConfig)
        {
            SwitchConfig();
            return;
        }
        intersection.SwitchTrafficConfiguration(index);

        Invoke("SwitchConfig", timeGreen + yellowSignalTimer + allRedSignalTimer);
    }

    private void OnTriggerExit(Collider other)
    {
        AppendToCarThroughPut();
    }

    void CreateHeaderRow()
    {
        string[] rowDataTemp = new string[1];
        rowDataTemp[0] = "Timestamp";
        carThroughPutData.Add(rowDataTemp);

        string[] rowDataTemp2 = new string[1];
        rowDataTemp2[0] = "Total Travel Time";
        carTravelTimeData.Add(rowDataTemp2);

    }

    void AppendToCarThroughPut()
    {
        string[] rowDataTemp = new string[1];
        rowDataTemp[0] = ""+Time.time; // time stamp
        carThroughPutData.Add(rowDataTemp);
    }

    public void AppendToCarTravelTime(float time)
    {
        string[] rowDataTemp = new string[1];
        rowDataTemp[0] = "" + time; // time travelled
        carTravelTimeData.Add(rowDataTemp);
    }

    void OnApplicationQuit()
    {
        saveCarThroughPutData();
        saveCarTravelTimeData();
    }

    string GetCarThroughputPath()
    {
        return Application.dataPath + "/comparisons/" + "static_car_throughput.csv";
    }

    string GetCarTravelTimePath()
    {
        return Application.dataPath + "/comparisons/" + "static_car_travel_time.csv";
    }

    void saveCarTravelTimeData()
    {
        string[][] output = new string[carTravelTimeData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = carTravelTimeData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));


        string filePath = GetCarTravelTimePath();

        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }

    void saveCarThroughPutData()
    {
        string[][] output = new string[carThroughPutData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = carThroughPutData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));


        string filePath = GetCarThroughputPath();

        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }
}
