using UnityEngine;

public class IntersectionController : MonoBehaviour
{

    public TrafficConfiguration[] configurations;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(configurations[0].trafficLights[0].transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}