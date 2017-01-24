using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class homeSteeringBehaviourDog : MonoBehaviour
{
    //character control parameters
    public int maxForce = 150;
    public float mass = 100;
    public float gravity = 9.81f;
    int maxRunningSpeed = 1;
    float rotateSpeed = 0.5f;
    float tmrDogFree;
    float maxWanderTime = 3;
    float wanderDist;
    float wanderRadius;

    //steer forces
    public Vector3 velocity;
    public Vector3 acceleration;
    public Vector3 steerForce;
    public Vector3 eindpos;

    //timing for tricks
    byte ignoreBallTime = 3;
    float tmrDogIgnoreBall = 0;
    float dogTrickTime = 4f;
    float tmrDogTrick = 6;
    float dogPickUpBallTime = 1f;
    float dogWaitForBallTime = 1.5f;
    float dogFetchTime = 3f;

    //obstacle avoidance
    public float ObstacleAvoidanceDistance;
    public cameraSteeringBehaviour cameraMoveScript;
    GameObject cameraPlayer;
    public float ObstacleAvoidanceForce;

    //animation + sound
    CharacterController controller;
    public Animator animationController;
    public AudioSource audioDogBarkShort;
    public AudioSource audioDogBarkLong;

    //dog behaviour
    public bool dogPlayingFetch = false;
    public bool dogReturningBall = false;
    GameObject ballToFetch;
    GameObject carpet;
    float dogYPos;

    //dog fetch vars
    float fetchDistance = 0.8f;
    byte fetchBringBackDistance = 3;
    public GameObject dogMouthBone;
    public float ballFetchHeight = 1f;
    float fleeForce = 150;

    //used for fetch, waypoint avoidance
    List<Vector3> avoidancePointsWithFetch;
    GameObject[] avoidancePointsGameObjects;
    Vector3 seekPointForAvoidance;
    float smallestDistanceToBall;
    Vector3 currentObstacleAvoidancePathPointChosen;
    bool goToBallWithAvoidancePath;
    bool avoidancePathFinished;
    public GameObject nullAvoidance;
    float minDistanceBeforeSeeBall = 3f;
    float minDistanceToAvoidancePoint = 1f;
    RaycastHit hitinfo;

    //make sure dog can reach ball
    float minDistanceToBall = 2.5f;
    float minDistanceToObjectBallStuck = 2;
    float ballOnObjectHeight = 0.5f;

    //raycasts
    RaycastHit[] allRaycastHits;
    RaycastHit[] raycastHitsSeek;
    RaycastHit[] allRaycastHitsToAvoidancePoint;
    Vector3 rayCastDirAvoidance;


    void Start()
    {
        dogYPos = transform.position.y;
        wanderDist = 2;
        wanderRadius = 2;
        dogMouthBone = GameObject.FindWithTag("dogMouth");
        controller = GetComponent<CharacterController>();
        animationController = GetComponent<Animator>();
        cameraPlayer = GameObject.FindWithTag("cameraTopObject");
        eindpos = transform.position + transform.forward * wanderDist + Random.onUnitSphere * wanderRadius;
        eindpos.y = transform.position.y;
        ballToFetch = GameObject.FindWithTag("Ball");
        carpet = GameObject.FindWithTag("carpet");
        avoidancePointsWithFetch = new List<Vector3>();
        avoidancePointsGameObjects = GameObject.FindGameObjectsWithTag("collisionAvoidancePoint");
        foreach (GameObject avoidancePointToAdd in avoidancePointsGameObjects)
        {
            avoidancePointsWithFetch.Add(new Vector3(avoidancePointToAdd.transform.position.x, transform.position.y, avoidancePointToAdd.transform.position.z));
        }

        goToBallWithAvoidancePath = false;
        Physics.IgnoreCollision(ballToFetch.GetComponent<Collider>(), GetComponent<Collider>());
    }
    // Update is called once per frame
    void Update()
    {
        //for testing without vive
        if (Input.GetKeyDown(KeyCode.F))
        {
            dogCatch();
        }

        //timers increase
        tmrDogTrick += Time.deltaTime;
        tmrDogIgnoreBall += Time.deltaTime;

        //calc movement
        setSpeed();

        if (dogPlayingFetch && !(tmrDogTrick < dogFetchTime)) //if dog is not already fetching/ doing a trick)
        {
            if (Vector3.Distance(transform.position, ballToFetch.transform.position) > fetchDistance) // if dog is far away from ball, go near
            {
                dogFetchBehaviour(ballToFetch.transform.position);
            }
            else //if dog is near enough, pick up ball
            {
                animationController.SetTrigger("dogPickUpBall");
                dogPlayingFetch = false;
                dogReturningBall = true;
                tmrDogTrick = 0;
            }
        }
        else if (dogReturningBall) //dog got ball, brings back to mat
        {
            steerForce = dogReturnBallBehaviour();
        }

        else
        {
            steerForce = dogLooseBehaviour();
        }
        if (Vector3.Distance(transform.position, ballToFetch.transform.position) > minDistanceToBall)
        {
            steerForce += ObstacleAvoidance();
        }


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
    private void setSpeed()
    {
        if (dogReturningBall && tmrDogTrick < dogWaitForBallTime) // if the dog is picking up the ball
        {
            maxRunningSpeed = 0;
            rotateSpeed = 0;
        }
        else if (dogReturningBall && tmrDogTrick > dogWaitForBallTime) // if the dog is returning the ball after picking it up
        {
            maxRunningSpeed = 1;
            rotateSpeed = 1f;

        }
        else if ((!dogReturningBall && tmrDogTrick < dogTrickTime) || animationController.GetBool("ballStuck")) //if the dog is doing a trick or he can't reach the ball
        {
            maxRunningSpeed = 0;
            rotateSpeed = 0;
        }
        else
        {
            maxRunningSpeed = 1;
            rotateSpeed = 0.5f;
        }
    }

    public Vector3 Seek(Vector3 seekPosition)
    {
        Vector3 mySteeringForce = (seekPosition - transform.position).normalized * maxForce;//look at target direction, normalized and scaled
        Debug.DrawLine(transform.position, seekPosition, Color.cyan);
        return mySteeringForce;
    }

    public Vector3 Flee(Vector3 eindpos)
    {
        float distance = Vector3.Distance(transform.position, eindpos);
        //distance to target ==> afstand tussen deze twee
        Vector3 mySteeringForce = Vector3.zero;
        Debug.DrawLine(transform.position, eindpos, Color.green);
        // (beginpos - eindpos).normalized * fleeforce / distance
        mySteeringForce = (transform.position - eindpos).normalized * fleeForce / distance;
        //look away from target direction, normalized and scaled

        return mySteeringForce;
    }

    public void dogCatch() //if the dog is playing catch
    {
        animationController.SetTrigger("ballThrow");
        dogPlayingFetch = true;
        animationController.SetTrigger("ballUnstuck");
        animationController.SetBool("ballStuck", false);
        tmrDogIgnoreBall = 0f;
    }

    public Vector3 dogFetchBehaviour(Vector3 endPosition)
    {
        //code for avoidance 
        Vector3 raycastDirBall = endPosition - transform.position;

        //Still add if dog see the point just switch
        allRaycastHits = Physics.RaycastAll(transform.position, raycastDirBall);

        Debug.DrawRay(transform.position, raycastDirBall, Color.red);

        if (ballToFetch.transform.position.y > ballOnObjectHeight && Vector3.Distance(transform.position, ballToFetch.transform.position) < minDistanceToObjectBallStuck) //ball is stuck on an object
        {
            if (!animationController.GetBool("ballStuck") && !dogReturningBall && tmrDogIgnoreBall > ignoreBallTime)
            {
                animationController.SetBool("ballStuck", true);
                audioDogBarkLong.Play();
                dogPlayingFetch = false;
                dogReturningBall = false;
            }


        }
        if (Physics.Raycast(transform.position, raycastDirBall, out hitinfo, Vector3.Distance(transform.position, endPosition))) //if the dog cant see the ball, he has to go around an obstacle, bigger than one, first is always the room
        {

            Debug.Log("not seeing ball" + hitinfo.collider.name);
            if (avoidancePathFinished) //if the dog is near the point, he can find a new one
            {
                goToBallWithAvoidancePath = false;

            }
            if (!goToBallWithAvoidancePath) //if the dog cant see ball (code above) and chosen not to go with avoidance path, check again
            {
                currentObstacleAvoidancePathPointChosen = Vector3.zero;
                smallestDistanceToBall = 0;
                foreach (Vector3 pathpointToCheck in avoidancePointsWithFetch) //checking all points for avoiding obstacle
                {
                    rayCastDirAvoidance = pathpointToCheck - transform.position;
                    Debug.DrawRay(transform.position, rayCastDirAvoidance, Color.blue);
                    if (!Physics.Raycast(transform.position, rayCastDirAvoidance, out hitinfo, Vector3.Distance(transform.position, pathpointToCheck))) //if the dog can see the point (otherwise no use in going to this point)
                    {
                        if (Vector3.Distance(endPosition, pathpointToCheck) < smallestDistanceToBall || smallestDistanceToBall == 0)//check if distance is smaller than last distance
                        {
                            if (Vector3.Distance(transform.position, pathpointToCheck) > minDistanceToAvoidancePoint)
                            {
                                avoidancePathFinished = false;
                                smallestDistanceToBall = Vector3.Distance(endPosition, pathpointToCheck); //if distance is smaller, go to this point
                                currentObstacleAvoidancePathPointChosen = pathpointToCheck;
                                goToBallWithAvoidancePath = true;
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("obstacle" + hitinfo.collider.name);
                    }
                }
            }
            if (currentObstacleAvoidancePathPointChosen != Vector3.zero && !avoidancePathFinished) // dog is still using avoidance
            {
                if (Vector3.Distance(currentObstacleAvoidancePathPointChosen, transform.position) < minDistanceToAvoidancePoint)
                {
                    //if the dog finished the path, seek the ball
                    avoidancePathFinished = true;
                    currentObstacleAvoidancePathPointChosen = Vector3.zero;
                    goToBallWithAvoidancePath = false;
                    return steerForce = Seek(endPosition);
                }
                else //if the dog is still avoiding
                {
                    return steerForce = Seek(currentObstacleAvoidancePathPointChosen);
                }
            }
            else //if no point was found, go straight to ball, let old obstacle avoidance work
            {
                Debug.Log("no point found");
                return steerForce = Seek(endPosition);
            }
        }

        else //if dog can see the ball, go to the ball
        {
            Debug.Log("see ball");
            Debug.Log("go for ball");
            avoidancePathFinished = true;
            goToBallWithAvoidancePath = false;
            return steerForce = Seek(endPosition);
        }
    }


    public Vector3 dogReturnBallBehaviour()
    {
        if (tmrDogTrick > dogPickUpBallTime) // dog pickup ball
        {
            ballToFetch.GetComponent<Collider>().isTrigger = true;
            ballToFetch.GetComponent<Rigidbody>().useGravity = false;
            ballToFetch.transform.position = dogMouthBone.transform.position;
        }

        if (Vector3.Distance(transform.position, carpet.transform.position) > fetchBringBackDistance) //dog brings ball to carpet
        {
            return steerForce = dogFetchBehaviour(carpet.transform.position);
        }
        else //if dog is near enough, drop ball en leave
        {
            ballToFetch.GetComponent<Collider>().isTrigger = false;
            ballToFetch.GetComponent<Rigidbody>().useGravity = true;
            dogReturningBall = false;
            return steerForce = Flee(ballToFetch.transform.position); //get away from ball
        }
    }

    public Vector3 dogLooseBehaviour()
    {

        tmrDogFree += Time.deltaTime;
        if (tmrDogFree > maxWanderTime || Vector3.Distance(eindpos, transform.position) < 1)
        {
            eindpos = Vector3.zero;
            tmrDogFree = 0;

            //heading = velocity.normalized;//where we're going                  //where we are + forward + random         
            eindpos = transform.position + transform.forward * wanderDist + Random.onUnitSphere * wanderRadius;
            eindpos.y = transform.position.y;//same height     



        }
        return Seek(eindpos);
    }

    public Vector3 ObstacleAvoidance()
    {
        //two ray system, like antenna's
        //one extra ray for the middle
        Vector3 mySteeringForceL = Vector3.zero;
        Vector3 mySteeringForceR = Vector3.zero;
        Vector3 mySteeringForceM = Vector3.zero;
        Vector3 directionL = 2 * transform.forward - transform.right;
        Vector3 directionR = 2 * transform.forward + transform.right;
        Vector3 directionM = transform.forward;
        RaycastHit hitR;
        RaycastHit hitL;
        RaycastHit hitM;
        //check for obstacle
        bool hitObstacleOnTheLeft = DetectObstacle(directionL, out hitL,
                                    ObstacleAvoidanceDistance);
        bool hitObstacleOnTheRight = DetectObstacle(directionR, out hitR,
                                     ObstacleAvoidanceDistance);
        bool hitObstacleInTheMiddle = DetectObstacle(directionM, out hitM,
                                      ObstacleAvoidanceDistance);
        //obstacle found
        if (hitObstacleOnTheLeft || hitObstacleOnTheRight || hitObstacleInTheMiddle)
        {
            //calc forces for each direction
            if (hitObstacleOnTheLeft) mySteeringForceL = CalcAvoidanceForce(hitL);
            if (hitObstacleOnTheRight) mySteeringForceR = CalcAvoidanceForce(hitR);
            if (hitObstacleInTheMiddle) mySteeringForceM = CalcAvoidanceForce(hitM);

            //sum them
            if (mySteeringForceL != Vector3.zero &&
                mySteeringForceR != Vector3.zero &&
                mySteeringForceM == Vector3.zero)
            {//possible narrow pathway
                return Vector3.zero;//keep on going 
            }
            else
            {//full force
                tmrDogFree = maxWanderTime;
                return ObstacleAvoidanceForce * (mySteeringForceL + mySteeringForceR +
                           mySteeringForceM);//just return the sum of all three
            }
        }
        return Vector3.zero;//no steering force  because no obstacle was detected}
    }

    private bool DetectObstacle(Vector3 myDirection, out RaycastHit myHit, float myObstacleAvoidanceDistance)
    {
        Debug.DrawRay(transform.position, myDirection);

        if (Physics.Raycast(transform.position, myDirection, out myHit,
            ObstacleAvoidanceDistance))//raycast, if hit
        {//myHit is out since you need it elsewhere too
            return true;
        }
        return false;
    }

    private Vector3 CalcAvoidanceForce(RaycastHit myHit)
    {
        Vector3 mySteeringForce = Vector3.zero;//a zero force
                                               //eind - begin
        mySteeringForce += (transform.position - myHit.point).normalized *
                           ObstacleAvoidanceForce / myHit.distance;//calc force
        return mySteeringForce;
    }

    public bool dogTrick() //called from controllers script, gives dog the command to play fetch
    {
        if (tmrDogTrick > dogTrickTime)
        {
            audioDogBarkShort.Play();
            tmrDogTrick = 0;
            return true;
        }
        else
        {
            return false;
        }

    }

}

