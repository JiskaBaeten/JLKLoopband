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
    List<WaypointPath> allPaths;
    public WaypointPath currentPath;
    public int nextPathNumber;
    bool nextPathIsChosen;
    bool pathDirectionIsReversed;
    CharacterController controller;//this GO's CharacterController
    bool nextPathShouldBeReversed = false;
    int numberToCheckIfReversed = 100;


    void Start()
    {

        allPaths = new List<WaypointPath>();
        controller = GetComponent<CharacterController>();//this GO's CharacterController
        wanderJitter = UnityEngine.Random.Range(wanderJitterMin, wanderJitterMax);//for the first time use.
        waypointPathsContainer = GameObject.FindGameObjectsWithTag("WayPoints");
        foreach (GameObject pathToAdd in waypointPathsContainer)
        {
            allPaths.Add(new WaypointPath(pathToAdd));
        }
        nextPathNumber = 0;
        pathDirectionIsReversed = false;
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
        if (nextPathNumber >= numberToCheckIfReversed) //if the number is bigger than 100, the path should be reversed
        {
            nextPathShouldBeReversed = true;
            nextPathNumber = nextPathNumber - numberToCheckIfReversed; 
        }
        else
        {
            nextPathShouldBeReversed = false;
        }
        foreach (WaypointPath pathToCheck in allPaths)
        {
            
            if (pathToCheck.PathNumber == nextPathNumber)
            {
                
                //next path number??
                nextPathIsChosen = false;
                pathToCheck.reversePath(nextPathShouldBeReversed);
                currentPath = pathToCheck;
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
            if (currentPath.pathIsReversed)
            {
                nextPathNumber = currentPath.NextPathNumberLeftBehind;

            }
            else
            {
                nextPathNumber = currentPath.NextPathNumberLeftBefore;
            }
            nextPathIsChosen = true;
            Debug.Log("path chosen left" + nextPathNumber);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (currentPath.pathIsReversed)
            {
                nextPathNumber = currentPath.NextPathNumberRightBehind;
            }
            else
            {
                nextPathNumber = currentPath.NextPathNumberRightBefore; 
            }
            nextPathIsChosen = true;
            Debug.Log("path chosen right" + nextPathNumber);
        }
    }

    public void chooseNextRandomPath()
    {
      float rndPathChoice = UnityEngine.Random.Range(0, 2);//for the first time use.
        if (rndPathChoice == 0)
        {
            if (currentPath.pathIsReversed)
            {
                nextPathNumber = currentPath.NextPathNumberLeftBehind;
            }
            else
            {
                nextPathNumber = currentPath.NextPathNumberLeftBefore;
            }
            nextPathIsChosen = true;
        }
        else
        {
            if (currentPath.pathIsReversed)
            {
                nextPathNumber = currentPath.NextPathNumberRightBehind;
            }
            else
            {
                nextPathNumber = currentPath.NextPathNumberRightBefore;
            }
            nextPathIsChosen = true;
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
                if (!nextPathIsChosen)
                {
                    chooseNextRandomPath();
                }
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

public class WaypointPath
{
    byte pathNumber;
    int pathBeforeLeft;
    int pathBeforeRight;
    int pathBehindLeft;
    int pathBehindRight;
    bool isReversed;

    Vector3[] waypointsPath;
    public WaypointPath() { }
    public WaypointPath(GameObject path)
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
                pathBehindLeft = 116; //path 16
                pathBehindRight = 116;
                break;

            case "Path (1)":

                pathNumber = 1;
                pathBeforeLeft = 5;
                pathBeforeRight = 4;
                pathBehindLeft = 100; //path 0
                pathBehindRight = 100;
                break;

            case "Path (2)":

                pathNumber = 2;
                pathBeforeLeft = 17;
                pathBeforeRight = 3;
                pathBehindLeft = 104;
                pathBehindRight = 6;
                break;

            case "Path (3)":

                pathNumber = 3;
                pathBeforeLeft = 110; //path 10
                pathBeforeRight = 110;
                pathBehindLeft = 102;
                pathBehindRight = 17; 
                break;

            case "Path (4)":

                pathNumber = 4;
                pathBeforeLeft = 2;
                pathBeforeRight = 2;
                pathBehindLeft = 101;
                pathBehindRight = 101;
                break;

            case "Path (5)":
                pathNumber = 5;
                pathBeforeLeft = 107;
                pathBeforeRight = 107;
                pathBehindLeft = 101;
                pathBehindRight = 101;

                break;
            case "Path (6)":
                pathNumber = 6;
                pathBeforeLeft = 107;
                pathBeforeRight = 107;
                pathBehindLeft = 2;
                pathBehindRight = 2;
                break;

            case "Path (7)":
                pathNumber = 7;
                pathBeforeLeft = 106;
                pathBeforeRight = 5;
                pathBehindLeft = 100;
                pathBehindRight = 100;
                break;

            case "Path (8)":
                pathNumber = 8;
                pathBeforeLeft = 121;
                pathBeforeRight = 9;
                pathBehindLeft = 110;
                pathBehindRight = 110;
                break;

            case "Path (9)":
                pathNumber = 9;
                pathBeforeLeft = 11;
                pathBeforeRight = 14;
                pathBehindLeft = 108;
                pathBehindRight = 121;
                break;

            case "Path (10)":
                pathNumber = 10;
                pathBeforeLeft = 8;
                pathBeforeRight = 103;
                pathBehindLeft = 18;
                pathBehindRight = 20;
                break;

            case "Path (11)":
                pathNumber = 11;
                pathBeforeLeft = 15;
                pathBeforeRight = 15;
                pathBehindLeft = 109;
                pathBehindRight = 109;
                break;

            case "Path (12)":
                pathNumber = 12;
                pathBeforeLeft = 13;
                pathBeforeRight = 13;
                pathBehindLeft = 15;
                pathBehindRight = 15;
                break;

            case "Path (13)":
                pathNumber = 13;
                pathBeforeLeft = 16;
                pathBeforeRight = 16;
                pathBehindLeft = 114;
                pathBehindRight = 112;
                break;

            case "Path (14)":
                pathNumber = 14;
                pathBeforeLeft = 13;
                pathBeforeRight = 13;
                pathBehindLeft = 109;
                pathBehindRight = 109;
                break;

            case "Path (15)":

                pathNumber = 15;
                pathBeforeLeft = 16;
                pathBeforeRight = 16;
                pathBehindLeft = 12;
                pathBehindRight = 111;
                break;
            case "Path (16)":

                pathNumber = 16;
                pathBeforeLeft = 0;
                pathBeforeRight = 0;
                pathBehindLeft = 113;
                pathBehindRight = 115;
                break;
            case "Path (17)":

                pathNumber = 17;
                pathBeforeLeft = 19;
                pathBeforeRight = 118;
                pathBehindLeft = 103;
                pathBehindRight = 103;
                break;
            case "Path (18)":

                pathNumber = 18;
                pathBeforeLeft = 117;
                pathBeforeRight = 117;
                pathBehindLeft = 110;
                pathBehindRight = 110;
                break;
            case "Path (19)":

                pathNumber = 19;
                pathBeforeLeft = 21;
                pathBeforeRight = 21;
                pathBehindLeft = 117;
                pathBehindRight = 117;
                break;
            case "Path (20)":

                pathNumber = 20;
                pathBeforeLeft = 21;
                pathBeforeRight = 21;
                pathBehindLeft = 10;
                pathBehindRight = 10;
                break;
            case "Path (21)":

                pathNumber = 21;
                pathBeforeLeft = 108;
                pathBeforeRight = 108;
                pathBehindLeft = 120;
                pathBehindRight = 119;
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

    public int NextPathNumberLeftBefore //nog aanpassen!!!
    {
        get { return pathBeforeLeft; }
    }
    public int NextPathNumberRightBefore //nog aanpassen!!!
    {
        get { return pathBeforeRight; }
    }
    public int NextPathNumberLeftBehind //nog aanpassen!!!
    {
        get { return pathBehindLeft; }
    }
    public int NextPathNumberRightBehind //nog aanpassen!!!
    {
        get { return pathBehindRight; }
    }
    public bool pathIsReversed
    {
        get { return isReversed; }
    }

    public void reversePath(bool pathShouldBeReversed)
    {
        if (pathShouldBeReversed != isReversed)
        {
            isReversed = !isReversed;
            Array.Reverse(waypointsPath);
        }
    }

}
