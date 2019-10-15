using UnityEngine;
using System.Linq;

public class CarController : MonoBehaviour
{
    public CarSpawner parent;
    public TrafficPath path;
    public float maxSpeed; // distance units per second
    public float maxAcceleration; // change in velocity per second
    public float speed;
    public float acceleration;
    public float idealSpaceToCarAhead; // distance units between car to any obj infront

    public float timeStopped;

    private int nextWaypointIndex;

    void Start()
    {
        FaceInitialWaypoint();
        speed = IdealSpeed();
    }

    void FixedUpdate()
    {
        float idealSpeed = IdealSpeed();
        // If required, adjust speed to ideal speed
        if(speed < idealSpeed)
        {
            SpeedUp();
        } else if (speed > idealSpeed)
        {
            SlowDown();
        }

        GoToNextWaypoint();
    }

    // Retrieve closest relevant collider (Next vehicle or assigned traffic light)
    // returns empty RaycastHit struct if no obstacle found
    private RaycastHit GetReferenceToNextObstacle()
    {
        LayerMask mask = 1 << 9; //Raycast layer
        // Cast a ray infront of vehicle, to detect if possible collision may occur in future
        // Only consider two possible collisions; Against another vehicle, and against yellow/red traffic light
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, Mathf.Infinity, mask).OrderBy(h => h.distance).ToArray();
        RaycastHit hit = new RaycastHit();

        // Retrieve closest relevant collider (Next vehicle or assigned traffic light)
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject.CompareTag("Vehicle"))
            {
                hit = hits[i];
                break;

            }

            if (ReferenceEquals(hits[i].collider.gameObject, path.trafficLight.gameObject))
            {
                TrafficLight trafficLight = hits[i].collider.gameObject.GetComponent<TrafficLight>();
                if (trafficLight.GetSignal() == TrafficLight.TrafficSignal.Yellow)
                {
                    // If able to stop before signal then use this as next obstacle
                    // Otherwise ignore to skip and use the next object
                    // stopping distance = -(initial velocity)^2 / 2*acceleration
                    float stoppingDistance = (-Mathf.Pow(speed, 2)) / (2 * acceleration);
                    if (hits[i].distance > stoppingDistance)
                    {
                        hit = hits[i];
                        break;
                    }
                }
                else
                {
                    hit = hits[i];
                    break;
                }
            }
        }

        return hit;
    }

    // Calculates desired speed from distance to the next obstacle (minus ideal following
    // space)
    private float IdealSpeed()
    {
        RaycastHit hit = GetReferenceToNextObstacle();
        if(hit.collider != null)
        {
            float objectAheadSpeed = 0f;
            if (hit.collider.gameObject.CompareTag("Vehicle"))
            {
                objectAheadSpeed = hit.collider.gameObject.GetComponent<CarController>().speed;
            }

            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.magenta);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * (SafeFollowingDistance(objectAheadSpeed) + idealSpaceToCarAhead), Color.blue);

            // safe initial speed to stop within an allocated distance:
            // speed = sqroot(2 * acceleration * distance)
            float safeInitialSpeed = Mathf.Sqrt(2 * acceleration * Mathf.Max(0,hit.distance-idealSpaceToCarAhead));
            return Mathf.Min(safeInitialSpeed, maxSpeed);
        }

        return maxSpeed;
    }

    private float SafeFollowingDistance(float nextObstacleSpeed)
    {
        float distToStop = (float)-System.Math.Pow(speed, 2) / (2 * -acceleration);
        float carAheadDistToStop = (float)-System.Math.Pow(nextObstacleSpeed, 2) / (2 * -acceleration);
        return distToStop - carAheadDistToStop;
    }

    // Face first waypoint of path, while keeping rotation parallel with ground plane
    private void FaceInitialWaypoint()
    {
        Vector3 initialWaypointPos = path.waypoints[0].position;
        initialWaypointPos.y = transform.position.y;
        transform.LookAt(initialWaypointPos);

    }

	private void GoToNextWaypoint()
	{
        // Delete self if end of path
        if (nextWaypointIndex >= path.waypoints.Length)
		{
            parent.cars.Remove(this);
            Destroy(gameObject);
            path.noCars -= 1;
			return;
		}

        Transform target = path.waypoints[nextWaypointIndex];
        float step = speed * Time.deltaTime; // calculate distance to move

        // Move car position a step closer to the next waypoint
        Vector3 newPosition = Vector3.MoveTowards(transform.position, target.position, step);
        transform.position = newPosition;

        // Rotate car towards direction of movement
        Vector3 targetDir = target.position - transform.position;
        targetDir.y = 0; // lock rotation
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
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

        if (System.Math.Abs(timeStopped) > 0)
        {
            timeStopped = 0;
        }
    }

    private void SlowDown()
    {
        GetComponent<Renderer>().material.SetColor("_Color",Color.red);
        speed -= acceleration * Time.deltaTime;
        if (speed < 0)
        {
            timeStopped = Time.time;
            speed = 0;
        }
    }

    private void DidCrash()
    {
        Debug.Log("Car did crash");
    }

    public float TimeSinceStop()
    {
        if (System.Math.Abs(timeStopped) > 0)
        {
            return Time.time - timeStopped;
        } else
        {
            return 0;
        }
    }

}
