using UnityEngine;
using System.Collections;

public class steeringBehaviourDog : MonoBehaviour {
    public int maxForce = 150;
    public float mass = 100;
    public float gravity = 9.81f;
    public int maxRunningSpeed = 15;
    public int maxWalkSpeed = 5;
    public int rotateSpeed = 2;

    public string behaviour;//what this GO is are doing
    public string defaultBehaviour;//at startup
    public string behaviourAfterSomeRandomWandering;//what this GO is supposed to do but can't so wanders a bit instead

    public bool ObstacleAvoidanceOn;
    public bool SeparationOn;
    public bool CohesionOn;

    public float fleeForce = 100;
    public float fleeRadius = 25;
    public float arriveRadius = 5;
    public float arriveDamping = 6;
    public float wanderRadius = 10;
    public float wanderDistance = 1;
    private float wanderJitter;
    public float wanderJitterMin = 0.5f;
    public float wanderJitterMax = 2;
    private float tmrWander = 999;
    public float minDistToPathPoint = 3;
    private int indexOfCurrentPathPoint = 0;
    private float tmrTimeSpentOnPathPoint = 0;
    public float MaxTimeToSpendOnPathPoint = 8;
    private float tmrWanderSomeRandomTime = 999;
    private float randomWanderTime;
    public float randomTimeToWanderMin = 2;
    public float randomTimeToWanderMax = 5;
    private float tmrCheckForStuck = 0;
    public float checkForStuckTime = 5;
    public float separationDistance = 5;
    public float separationForce = 200;
    public float ObstacleAvoidanceDistance = 2;
    public float ObstacleAvoidanceForce = 200;
    public float ObstacleAvoidanceForceFactorDuringWander = 8;
    public float cohesionRadius = 30;
    public float cohesionForce = 200;
    public float minShadowDistance = 10;
    public float maxShadowDistance = 50;


    public Vector3 velocity;
    public Vector3 lastVelocity;
    public Vector3 acceleration;
    public Vector3 steerForce;
    public Vector3 heading;
    public Vector3 wandertargetPosition;

    public Vector3 targetPosition;//where to go

    Transform WayPointContainer;
    Vector3 currentPathPoint;
    Vector3[] Path;

    CharacterController controller;//this GO's CharacterController
                                   // Use this for initialization
    void Start () {
        controller = GetComponent<CharacterController>();//this GO's CharacterController
        behaviour = defaultBehaviour;
        wanderJitter = Random.Range(wanderJitterMin, wanderJitterMax);//for the first time use.
        Debug.Log(GameObject.FindWithTag("WayPoints"));
        WayPointContainer = GameObject.FindWithTag("WayPoints").transform;//for path finding, other paths also possible
        
        Path = new Vector3[WayPointContainer.childCount];//init Path
        for (int i = 0; i < WayPointContainer.childCount; i++)//fill in the locations in the Path array
        {
            Path[i] = WayPointContainer.GetChild(i).transform.position;
        }
        randomWanderTime = Random.Range(randomTimeToWanderMin, randomTimeToWanderMax);//for the first time use

    }

    // Update is called once per frame
    void Update () {
        steerForce = FollowPath(Path);
        //calc movement
        Truncate(ref steerForce, maxForce);// not > max
        acceleration = steerForce / mass;
        velocity += acceleration;//velocity = transform.TransformDirection(velocity);
        Truncate(ref velocity, maxRunningSpeed);
        if (controller.isGrounded)
        {
            controller.Move(velocity * Time.deltaTime);//move

        }
        else
        {
            controller.Move(new Vector3(0, -gravity * Time.deltaTime, 0));//fall down
        }

        //rotate
        if (new Vector3(velocity.x, 0, velocity.z) != Vector3.zero)//otherwise warning
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(velocity.x, 0, velocity.z)), rotateSpeed * Time.deltaTime);
        }
    }
    private void Truncate(ref Vector3 myVector, int myMax)//not above max
    {
        if (myVector.magnitude > myMax)
        {
            myVector.Normalize();// Vector3.normalized returns this vector with a magnitude of 1
            myVector *= myMax;//scale to max
        }
    }
    public Vector3 FollowPath(Vector3[] myPath)
    {
        tmrTimeSpentOnPathPoint += Time.deltaTime;//keep track of time
        //if no currentPathPoint is selected, pick closest one
        if (currentPathPoint == Vector3.zero)//no currentPathPoint selected, find closest one
        {
            indexOfCurrentPathPoint = 0;
            for (int i = 0; i < myPath.Length; i++)//go through all the waypoints
            {
                if (!Physics.Linecast(transform.position, myPath[i]))//returns NOT true if there is any collider intersecting the line between start and end
                {
                    float distanceToWayPoint = Vector3.Distance(transform.position, myPath[i]);//calc distance
                    if (distanceToWayPoint <= Vector3.Distance(transform.position, myPath[indexOfCurrentPathPoint]))//a closer waypoint found
                    {
                        currentPathPoint = myPath[i];//select the closer one
                        indexOfCurrentPathPoint = i;
                        tmrTimeSpentOnPathPoint = 0;//reset clock for this point
                    }
                }
            }
            if (currentPathPoint == Vector3.zero)//no currentPathPoint selected because none was visible => wander
            {
                behaviour = "WanderSomeRandomTime";//hopefully the GO will correct himself
                behaviourAfterSomeRandomWandering = "Follow Path";//remember this and switch back to this
            }
        }
        else if (Vector3.Distance(transform.position, currentPathPoint) < minDistToPathPoint)//if close enough pick next one
        {
            tmrTimeSpentOnPathPoint = 0;//reset clock for this point
            indexOfCurrentPathPoint++;//increase index
            if (indexOfCurrentPathPoint == myPath.Length)//set to the first one if out of bounds
            {
                indexOfCurrentPathPoint = 0;
            }
            currentPathPoint = myPath[indexOfCurrentPathPoint];//pick the next one
        }
        if (tmrTimeSpentOnPathPoint > MaxTimeToSpendOnPathPoint)//spend a lot of time on the current point, maybe stuck => wander around
        {
            tmrTimeSpentOnPathPoint = 0;//reset clock for this point
            behaviour = "WanderSomeRandomTime";//hopefully the GO will correct himself
            behaviourAfterSomeRandomWandering = "Follow Path";//remember this and switch back to this
        }
        //go to currentPathPoint like seek
        return Seek(currentPathPoint);
    }
    public Vector3 Seek(Vector3 seekPosition)
    {
        Vector3 mySteeringForce = (seekPosition - transform.position).normalized * maxForce;//look at target direction, normalized and scaled
        Debug.DrawLine(transform.position, seekPosition, Color.green);
        return mySteeringForce;
    }
}
