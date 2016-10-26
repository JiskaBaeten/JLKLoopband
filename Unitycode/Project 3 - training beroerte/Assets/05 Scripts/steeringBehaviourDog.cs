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
    Vector3[] waypointsCurrentPath;
    List<Path> allPaths;
    public Path currentPath;
    public byte nextPathNumber;
    bool nextPathIsChosen;

    CharacterController controller;//this GO's CharacterController
                                   // Use this for initialization
    void Start()
    {

        allPaths = new List<Path>();
        controller = GetComponent<CharacterController>();//this GO's CharacterController
        wanderJitter = UnityEngine.Random.Range(wanderJitterMin, wanderJitterMax);//for the first time use.
        waypointPathsContainer = GameObject.FindGameObjectsWithTag("WayPoints");
        foreach (GameObject pathToAdd in waypointPathsContainer)
        {
            allPaths.Add(new Path(pathToAdd));
        }
        nextPathNumber = 0;
        
        waypointsCurrentPath = selectPath();
    }

    // Update is called once per frame
    void Update()
    {
        steerForce = FollowPath(waypointsCurrentPath);
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
        if (!nextPathIsChosen)
        {
            findNextPath();
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
    public Vector3[] selectPath()
    {
        foreach (Path pathToCheck in allPaths)
        {
            if (pathToCheck.PathNumber == nextPathNumber)
            {
                //next path number??
                nextPathIsChosen = false;
                currentPath = pathToCheck;
                Debug.Log(pathToCheck.PathNumber);
                currentPathPoint = pathToCheck.WaypointsFromPath[0];
                return pathToCheck.WaypointsFromPath;
            }
        }
        return allPaths[0].WaypointsFromPath;
    }

    public void findNextPath()
    {

        if (Input.GetKey(KeyCode.LeftArrow)) //go left
        {
            nextPathNumber = currentPath.NextPathNumberLeft;
            nextPathIsChosen = true;
            Debug.Log("path chosen left" + currentPath.NextPathNumberLeft);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            nextPathNumber = currentPath.NextPathNumberRight;
            nextPathIsChosen = true;
            Debug.Log("path chosen right" + currentPath.NextPathNumberRight);
        }
    }
    public Vector3 FollowPath(Vector3[] myPath)
    {
        
        //if no currentPathPoint is selected, pick closest one
        if (currentPathPoint == Vector3.zero)//no currentPathPoint selected, find closest one
        {
            Debug.Log("waypoints " + myPath[0]);
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
            if (indexOfCurrentPathPoint == myPath.Length)
            {
                waypointsCurrentPath = selectPath();
               
                indexOfCurrentPathPoint = 0; 
            }
            currentPathPoint = waypointsCurrentPath[indexOfCurrentPathPoint];//pick the next one
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
    public Path(GameObject path)
    {
        initializePathName(path);
        initializePathWaypoints(path);
    }

    private void initializePathWaypoints(GameObject path)
    {
        waypointsPath = new Vector3[path.transform.childCount];
        for (int i = 0; i < path.transform.childCount; i++)
        {
            waypointsPath[i] = path.transform.GetChild(i).position;
        }

    }

    private void initializePathName(GameObject path)
    {
        switch (path.name)
        {
            case "Path (0)":

                pathNumber = 0;
                pathBeforeLeft = 7;
                pathBeforeRight = 1;
                pathAfterLeft = 16;
                pathAfterRight = 16;
                break;
            case "Path (1)":

                pathNumber = 1;
                pathBeforeLeft = 5;
                pathBeforeRight = 4;
                pathAfterLeft = 0;
                pathAfterRight = 7;
                break;
            case "Path (2)":

                pathNumber = 2;
                pathBeforeLeft = 17;
                pathBeforeRight = 3;
                pathAfterLeft = 4;
                pathAfterRight = 6;
                break;
            case "Path (3)":

                pathNumber = 3;
                pathBeforeLeft = 10;
                pathBeforeRight = 10;
                pathAfterLeft = 2;
                pathAfterRight = 17;
                break;

            case "Path (4)":

                pathNumber = 4;
                pathBeforeLeft = 2;
                pathBeforeRight = 2;
                pathAfterLeft = 1;
                pathAfterRight = 1;
                break;

            case "Path (5)":
                pathNumber = 5;
                pathBeforeLeft = 7;
                pathBeforeRight = 7;
                pathAfterLeft = 1;
                pathAfterRight = 1;
                break;
            case "Path (6)":
                pathNumber = 6;
                pathBeforeLeft = 2;
                pathBeforeRight = 2;
                pathAfterLeft = 7;
                pathAfterRight = 7;
                break;

            case "Path (7)":
                pathNumber = 7;
                pathBeforeLeft = 6;
                pathBeforeRight = 5;
                pathAfterLeft = 0;
                pathAfterRight = 0;
                break;

            case "Path (8)":
                pathNumber = 8;
                pathBeforeLeft = 0;
                pathBeforeRight = 0;
                pathAfterLeft = 21;
                pathAfterRight = 9;
                break;

            case "Path (9)":
                pathNumber = 9;
                pathBeforeLeft = 8;
                pathBeforeRight = 3;
                pathAfterLeft = 11;
                pathAfterRight = 14;
                break;

            case "Path (10)":
                pathNumber = 10;
                pathBeforeLeft = 8;
                pathBeforeRight = 3;
                pathAfterLeft = 18;
                pathAfterRight = 20;
                break;

            case "Path (11)":
                pathNumber = 11;
                pathBeforeLeft = 15;
                pathBeforeRight = 15;
                pathAfterLeft = 9;
                pathAfterRight = 9;
                break;

            case "Path (12)":
                pathNumber = 12;
                pathBeforeLeft = 13;
                pathBeforeRight = 13;
                pathAfterLeft = 15;
                pathAfterRight = 15;
                break;

            case "Path (13)":
                pathNumber = 13;
                pathBeforeLeft = 16;
                pathBeforeRight = 16;
                pathAfterLeft = 12;
                pathAfterRight = 14;
                break;

            case "Path (14)":
                pathNumber = 14;
                pathBeforeLeft = 13;
                pathBeforeRight = 13;
                pathAfterLeft = 9;
                pathAfterRight = 9;
                break;

            case "Path (15)":

                pathNumber = 15;
                pathBeforeLeft = 16;
                pathBeforeRight = 16;
                pathAfterLeft = 12;
                pathAfterRight = 11;
                break;
            case "Path (16)":

                pathNumber = 16;
                pathBeforeLeft = 0;
                pathBeforeRight = 0;
                pathAfterLeft = 13;
                pathAfterRight = 15;
                break;
            case "Path (17)":

                pathNumber = 17;
                pathBeforeLeft = 19;
                pathBeforeRight = 18;
                pathAfterLeft = 3;
                pathAfterRight = 3;
                break;
            case "Path (18)":

                pathNumber = 18;
                pathBeforeLeft = 10;
                pathBeforeRight = 10;
                pathAfterLeft = 17;
                pathAfterRight = 17;
                break;
            case "Path (19)":

                pathNumber = 19;
                pathBeforeLeft = 21;
                pathBeforeRight = 21;
                pathAfterLeft = 17;
                pathAfterRight = 17;
                break;
            case "Path (20)":

                pathNumber = 20;
                pathBeforeLeft = 21;
                pathBeforeRight = 21;
                pathAfterLeft = 10;
                pathAfterRight = 10;
                break;
            case "Path (21)":

                pathNumber = 21;
                pathBeforeLeft = 8;
                pathBeforeRight = 8;
                pathAfterLeft = 20;
                pathAfterRight = 19;
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
    public byte NextPathNumberLeft //nog aanpassen!!!
    {
        get { return pathBeforeLeft; }
    }
    public byte NextPathNumberRight //nog aanpassen!!!
    {
        get { return pathBeforeRight; }
    }

    private void reversePath()
    {
        isReversed = !isReversed;
        Array.Reverse(waypointsPath);
    }

}
