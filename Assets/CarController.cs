using UnityEngine;

public class CarController : MonoBehaviour
{
    public Path path;
    public float maxSpeed; // distance units per second
    public float maxAcceleration; // change in velocity per second
    public float speed;
    public float acceleration;
    public float idealSpaceToCarAhead; // distance units between car to any obj infront

    private int nextWaypointIndex;

    // Start is called before the first frame update
    void Start()
    {
        speed = maxSpeed;
    }

    void Update()
    {
        ReviewFrontSensorDistance();
        GoToNextWaypoint();
    }

    private void ReviewFrontSensorDistance()
    {
        LayerMask mask = 1 << 9; //Raycast layer
        // Cast a ray infront of vehicle, to detect if possible collision may occur in future
        // We care about two collisions; Against another vehicle, and against yellow/red traffic light
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), Mathf.Infinity, mask);
        RaycastHit hit = new RaycastHit();

        // Retrieve first relevant collider
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject.CompareTag("Vehicle"))
            {
                Debug.Log("Next object is car");
                hit = hits[i];
                break;
            }
            else if (ReferenceEquals(hits[i].collider.gameObject, path.trafficLight.gameObject))
            {
                Debug.Log("Next object is traffic light");
                hit = hits[i];
                break;
            }
        }

        if (hit.collider != null)
        {
            Debug.Log(hit.distance);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.magenta);

            // If vehicle ahead, retrieve it's speed
            // TODO: This doesn't account for direction, and assumes vehicle ahead
            // is travelling in the same direction, although this should have minimal,
            // if any, impact. This means we can still use the Unity method
            // Vector3.MoveTowards for a more succinct waypoint system.
            float distanceToObstacle = hit.distance;
            float objectAheadSpeed = 0f;
            if (hit.collider.gameObject.CompareTag("Vehicle"))
            {
                objectAheadSpeed = hit.collider.gameObject.GetComponent<CarController>().speed;
            }

            // Calc Safe Following Distance
            // Take difference of minimum stopping distances for self and vehicle ahead
            float distToStop = (float)-System.Math.Pow(speed, 2) / (2 * -acceleration);
            float carAheadDistToStop = (float)-System.Math.Pow(objectAheadSpeed, 2) / (2 * -acceleration);
            float safeFollowingDistance = distToStop - carAheadDistToStop;

            // Debug line
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * safeFollowingDistance, Color.blue);

            // Adjust speed to maintain, at minimum, a safe following distance,
            // accounting for ideal spacing between cars.
            if (distanceToObstacle < (safeFollowingDistance + idealSpaceToCarAhead))
            {
                SlowDown();
            }
            else
            {
                SpeedUp();
            }
        }
        else // Where nothing infront of vehicle, it is free to accelerate
        {
            SpeedUp();
        }
    }

	private void GoToNextWaypoint()
	{
		// Delete self if end of path
		if (nextWaypointIndex >= path.waypoints.Length)
		{
            Destroy(gameObject);
			return;
		}

        Transform target = path.waypoints[nextWaypointIndex];
        float step = speed * Time.deltaTime; // calculate distance to move

        // Move car position a step closer to the next waypoint
        Vector3 newPosition = Vector3.MoveTowards(transform.position, target.position, step);
        transform.position = newPosition;

        // Rotate car towards direction of movement
        Vector3 targetDir = target.position - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
        Debug.DrawRay(transform.position, newDir, Color.red);
        transform.rotation = Quaternion.LookRotation(newDir); 

        // If the position of the waypoint and car are approximately equal, increment waypoint
        if (Vector3.Distance(transform.position, target.position) < 0.001f)
		{
			nextWaypointIndex += 1;
		}
	}

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.name)
        {
            case "Car(Clone)":
                Destroy(gameObject);
                DidCrash();
                break;
            default:
                break;
        }
    }

    private void SpeedUp()
    {
        GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
        speed += acceleration * Time.deltaTime;
        if (speed > maxSpeed)
        {
            speed = maxSpeed;
        }
    }

    private void SlowDown()
    {
        GetComponent<Renderer>().material.SetColor("_Color",Color.red);
        speed -= acceleration * Time.deltaTime;
        if (speed < 0)
        {
            speed = 0;
        }
    }

    private void DidCrash()
    {
        Debug.Log("Car did crash");
    }

}
