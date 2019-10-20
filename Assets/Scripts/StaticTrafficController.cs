using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class StaticTrafficController : MonoBehaviour
{
    public float yellowSignalTimer;
    public float allRedSignalTimer;
    public float[] trafficConfigTimers;
    private List<string[]> rowData = new List<string[]>();

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

    private void OnTriggerExit(Collider other)
    {
        AppendToCSV();
    }

    void CreateHeaderRow()
    {
        string[] rowDataTemp = new string[1];
        rowDataTemp[0] = "Timestamp";
        rowData.Add(rowDataTemp);

    }

    void AppendToCSV()
    {
        string[] rowDataTemp = new string[1];
        rowDataTemp[0] = ""+Time.time; // time stamp
        rowData.Add(rowDataTemp);
    }

    void OnApplicationQuit()
    {
        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));


        string filePath = GetCSVPath();

        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }

    string GetCSVPath()
    {
        return Application.dataPath + "/comparisons/" + "static.csv";
    }
}
