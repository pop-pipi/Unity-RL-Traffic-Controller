using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public float yellowSignalTimer;
    public Material redLight;
    public Material greenLight;
    public Material yellowLight;

    private TrafficSignal signal = TrafficSignal.Red;

    private void Start()
    {
        signal = TrafficSignal.Red;
        GetComponent<Renderer>().material = redLight;
        gameObject.layer = 9; // Raycast layer
    }

    public void CloseTraffic()
    {
        if (signal == TrafficSignal.Red){
            return;
        }
        signal = TrafficSignal.Yellow;
        GetComponent<Renderer>().material = yellowLight;
        gameObject.layer = 9; // Raycast layer
        Invoke("SwitchToRed", yellowSignalTimer);
    }

    public void OpenTraffic()
    {
        signal = TrafficSignal.Green;
        GetComponent<Renderer>().material = greenLight;
        gameObject.layer = 2; // Ignore Raycast layer
    }

    private void SwitchToRed()
    {
        signal = TrafficSignal.Red;
        GetComponent<Renderer>().material = redLight;
    }

    public TrafficSignal GetSignal()
    {
        return signal;
    }

    public enum TrafficSignal
    {
        Green,
        Yellow,
        Red
    };

}
