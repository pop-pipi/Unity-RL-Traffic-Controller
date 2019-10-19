using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireLoopSensor : MonoBehaviour
{
    private double timeActivated;

    private void OnTriggerEnter(Collider other)
    {
        timeActivated = Time.time;
    }

    private void OnTriggerExit(Collider other)
	{
        timeActivated = 0.0;
	}

    public double GetTimeActivated()
    {
        if(timeActivated == 0.0)
        {
            return timeActivated;
        }
        return Time.time - timeActivated;
    }
}
