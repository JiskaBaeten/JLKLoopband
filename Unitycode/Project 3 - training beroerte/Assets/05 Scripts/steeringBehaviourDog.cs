using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class steeringBehaviourDog : MonoBehaviour
{
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
    public float minDistToPathPoint = 3;
    private int indexOfCurrentPathPoint = 0;
    private float tmrTimeSpentOnPathPoint = 0;
    public float MaxTimeToSpendOnPathPoint = 8;
    private float randomWanderTime;
    public float randomTimeToWanderMin = 2;
    public float randomTimeToWanderMax = 5;
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

    GameObject[] waypointPathsContainer;
    Vector3 currentPathPoint;
    Vector3[] Path;
    List<Path> allPaths;
    public byte currentPath;
    public byte nextPathNumber;

    CharacterController controller;//this GO's CharacterController
                                   // Use this for initialization
    void Start()
    {

        currentPath = 0;
        controller = GetComponent<CharacterController>();//this GO's CharacterController
        behaviour = defaultBehaviour;
        wanderJitter = UnityEngine.Random.Range(wanderJitterMin, wanderJitterMax);//for the first time use.
        waypointPathsContainer = GameObject.FindGameObjectsWithTag("WayPoints");
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            allPaths.Add(new Path(waypointPathsContainer[i].transform));
        }
        Path = choosePath();
        randomWanderTime = UnityEngine.Random.Range(randomTimeToWanderMin, randomTimeToWanderMax);//for the first time use
    }

    // Update is called once per frame
    void Update()
    {
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
    public Vector3[] choosePath()
    {
        Vector3[] chosenPath;

        Debug.Log(waypointPathsContainer[currentPath].name);
        foreach (Path pathToCheck in allPaths)
        {
            if (pathToCheck.PathNumber == nextPathNumber)
            {
                nextPathNumber = pathToCheck.NextPathNumber;
                return pathToCheck.WaypointsFromPath;
            }
        }
        return null;
        /* for (int i = 0; i < waypointPathsContainer[currentPath].transform.childCount; i++)//fill in the locations in the Path array
         {
             chosenPath[i] = waypointPathsContainer[currentPath].transform.GetChild(i).transform.position;
         }
         */
    }
    public Vector3 FollowPath(Vector3[] myPath)
    {

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
        }
        else if (Vector3.Distance(transform.position, currentPathPoint) < minDistToPathPoint)//if close enough pick next one
        {

            indexOfCurrentPathPoint++;//increase index
            if (indexOfCurrentPathPoint == myPath.Length)//set to the first one if out of bounds
            {
                choosePath();
            }
            currentPathPoint = myPath[indexOfCurrentPathPoint];//pick the next one
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

public class Path
{
    byte pathNumber;
    byte pathBeforeLeft;
    byte pathBeforeRight;
    byte pathAfterLeft;
    byte pathAfterRight;
    bool isReversed;
    Vector3[] waypointsPath;
   public Path() { }
    public Path(Transform path)
    {
        initializePathName(path);
        
    }

    private void initializePathWaypoints(Transform path)
    {
        for (int i = 0; i < path.childCount; i++)
        {
            waypointsPath[i] = path.GetChild(i).position;
        }
    }

    private void initializePathName(Transform path)
    {
        switch (path.name)
        {
            case "Path(0)":

                pathNumber = 0;
                pathBeforeLeft = 0; //change
                pathAfterLeft = 7;
                pathAfterRight = 1;
                break;
            case "Path(1)":

                pathNumber = 1;
                pathBeforeLeft = 0; //change
                pathAfterLeft = 5;
                pathAfterRight = 4;
                break;
            case "Path(2)":

                pathNumber = 2;
                pathBeforeLeft = 0; //change
                pathAfterLeft = 17;
                pathAfterRight = 3;
                break;
            case "Path(3)":

                pathNumber = 3;
                pathBeforeLeft = 0; //change
                pathAfterLeft = 7;
                pathAfterRight = 1;
                break;
            case "Path(4)":

                pathNumber = 4;
                pathBeforeLeft = 0; //change
                pathAfterLeft = 7;
                pathAfterRight = 1;
                break;
            case "Path(5)":

                pathNumber = 5;
                pathBeforeLeft = 0; //change
                pathAfterLeft = 7;
                pathAfterRight = 1;
                break;
            case "Path(6)":

                pathNumber = 6;
                pathBeforeLeft = 0; //change
                pathAfterLeft = 7;
                pathAfterRight = 1;
                break;
            case "Path(7)":

                pathNumber = 7;
                pathBeforeLeft = 0; //change
                pathAfterLeft = 7;
                pathAfterRight = 1;
                break;
            case "Path(8)":

                pathNumber = 8;
                pathBeforeLeft = 0; //change
                pathAfterLeft = 7;
                pathAfterRight = 1;
                break;
            case "Path(9)":

                pathNumber = 9;
                pathBeforeLeft = 0; //change
                pathAfterLeft = 7;
                pathAfterRight = 1;
                break;
            case "Path(10)":

                pathNumber = 10;
                pathBeforeLeft = 0; //change
                pathAfterLeft = 7;
                pathAfterRight = 1;
                break;
            case "Path(11)":

                pathNumber = 11;
                pathBeforeLeft = 0; //change
                pathAfterLeft = 7;
                pathAfterRight = 1;
                break;
            case "Path(12)":

                pathNumber = 12;
                pathBeforeLeft = 0; //change
                pathAfterLeft = 7;
                pathAfterRight = 1;
                break;
            case "Path(13)":

                pathNumber = 13;
                pathBeforeLeft = 0; //change
                pathAfterLeft = 7;
                pathAfterRight = 1;
                break;
            case "Path(14)":

                pathNumber = 14;
                pathBeforeLeft = 0; //change
                pathAfterLeft = 7;
                pathAfterRight = 1;
                break;
            case "Path(15)":

                pathNumber = 15;
                pathBeforeLeft = 0; //change
                pathAfterLeft = 7;
                pathAfterRight = 1;
                break;
            case "Path(16)":

                pathNumber = 16;
                pathBeforeLeft = 0; //change
                pathAfterLeft = 7;
                pathAfterRight = 1;
                break;
            case "Path(17)":

                pathNumber = 17;
                pathBeforeLeft = 0; //change
                pathAfterLeft = 7;
                pathAfterRight = 1;
                break;
            case "Path(18)":

                pathNumber = 18;
                pathBeforeLeft = 0; //change
                pathAfterLeft = 7;
                pathAfterRight = 1;
                break;
            case "Path(19)":

                pathNumber = 19;
                pathBeforeLeft = 0; //change
                pathAfterLeft = 7;
                pathAfterRight = 1;
                break;
            case "Path(20)":

                pathNumber = 20;
                pathBeforeLeft = 0; //change
                pathAfterLeft = 7;
                pathAfterRight = 1;
                break;
            case "Path(21)":

                pathNumber = 21;
                pathBeforeLeft = 0; //change
                pathAfterLeft = 7;
                pathAfterRight = 1;
                break;
        }
    }

    public byte PathNumber
    {
        get { return pathNumber; }
    }
    public Vector3[] WaypointsFromPath
    {
        get { return waypointsPath; }
    }
    public byte NextPathNumber //nog aanpassen!!!
    {
        get { return pathAfterLeft; }
    }

    private void reversePath()
    {
        isReversed = !isReversed;
        Array.Reverse(waypointsPath);
    }

}
