using UnityEngine;
using MLAgents;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class IntersectionAgent : Agent
{
    public bool exportData;
    public int episodeLength;
    public IntersectionController intersection;
    public CarSpawner carSpawner;
    private List<string[]> carThroughPutData = new List<string[]>();
    private List<string[]> carTravelTimeData = new List<string[]>();

    private int phaseNumber;

    // Start is called before the first frame update
    void Start()
    {
        if (exportData)
        {
            CreateHeaderRow();
        }

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

    private void OnTriggerExit(Collider other)
    {
        if(exportData == false)
        {
            return;
        }
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
        rowDataTemp[0] = "" + Time.time; // time stamp
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
        if (exportData == false)
        {
            return;
        }
        saveCarThroughPutData();
        saveCarTravelTimeData();
    }

    string GetCarThroughputPath()
    {
        return Application.dataPath + "/comparisons/" + "ml_car_throughput.csv";
    }

    string GetCarTravelTimePath()
    {
        return Application.dataPath + "/comparisons/" + "ml_car_travel_time.csv";
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
