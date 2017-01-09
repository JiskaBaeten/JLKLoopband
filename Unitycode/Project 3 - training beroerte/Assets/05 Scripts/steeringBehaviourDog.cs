using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class steeringBehaviourDog : MonoBehaviour
{
    //character control parameters
    public int maxForce = 150;
    public float mass = 100;
    public float gravity = 9.81f;
    public int maxRunningSpeed = 1;
    public float rotateSpeed = 0.3f;

    //steer forces
    public Vector3 velocity;
    public Vector3 acceleration;
    public Vector3 steerForce;

    //path 
    public float minDistToPathPoint = 1;
    public float minDistanceToWait = 0.5f;
    public float dogWaitForPlayerDistance = 2;
    public Vector3 currentPathPointDog;
    CharacterController controller;//this GO's CharacterController


    //runningParameters
    public float maxRunningDistance;
    public float wanderDist;
    public float wanderRadius;
    float tmrDogFree;
    float tmrDogLook;
    float maxLookTime;
    float maxWanderTime = 5;
    public Vector3 PathPointDogRandom;
    public bool dogCalledInScript;
    public bool dogLookingForCall;
    public byte minDistanceBetweenDogAndMan;

    //obstacle avoidance
    public float ObstacleAvoidanceDistance;
    public cameraSteeringBehaviour cameraMoveScript;
    GameObject cameraPlayer;
    public float ObstacleAvoidanceForce;

    public Animator animationController;
    void Start()
    {
        minDistanceBetweenDogAndMan = 2;
        maxLookTime = 4;
        dogLookingForCall = false;
        dogCalledInScript = false;
        controller = GetComponent<CharacterController>();
        animationController = GetComponent<Animator>();
        cameraPlayer = GameObject.FindWithTag("cameraTopObject");
        currentPathPointDog = cameraMoveScript.currentPathPoint;
       
        //animationController.SetBool("dogIsLoose", true);
    }

    void Update()
    {
        Debug.Log(cameraMoveScript.currentPathPoint + "pathpoint");
        dogWaitForOwner();
        tmrDogLook += Time.deltaTime;
        tmrDogFree += Time.deltaTime;
        chooseSteeringBehaviour();


        if (tmrDogLook > maxLookTime && dogLookingForCall)
        {
            animationController.SetBool("dogIsWaiting", false);
            dogLookingForCall = false;
        }
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

    public void resetTimerDogLooking()
    {
        tmrDogLook = 0;
    }
    public void dogWaitForOwner()
    {
        if (currentPathPointDog == Vector3.zero)
        {
            currentPathPointDog = cameraMoveScript.nextPathPoint;
        }
        if (Vector3.Distance(transform.position, currentPathPointDog) < minDistToPathPoint)//if close enough pick next one
        {
            dogLookingForCall = false;
            if (currentPathPointDog == cameraMoveScript.nextPathPoint) //dog waiting for player?
            {  
                   // animationController.SetTrigger("dogCalled");
                    animationController.SetBool("dogIsWaiting", true);        
            }
            else
            {
                dogCalledInScript = false;
                animationController.SetBool("dogIsWaiting", false);
                currentPathPointDog = cameraMoveScript.nextPathPoint;
            }
        }
       else if (Vector3.Distance(transform.position, currentPathPointDog) > minDistToPathPoint && Vector3.Distance(transform.position, cameraPlayer.transform.position) < minDistanceBetweenDogAndMan)
        {
            animationController.SetBool("dogIsWaiting", false);
            dogLookingForCall = false;
            animationController.SetTrigger("dogMoveAway");
            dogCalledInScript = false;
        }
    }

    public void chooseSteeringBehaviour()
    {
        Debug.Log("looking" + dogLookingForCall);
        if (animationController.GetBool("dogIsWaiting") || dogLookingForCall) //dog is called
        {
            maxRunningSpeed = 0;
            rotateSpeed = 0;
        }
        else //dog is walking/running
        {
            if (animationController.GetBool("dogIsLoose") && !dogCalledInScript) // dog is running free,not called
            {
                maxRunningSpeed = 2;
                rotateSpeed = 2;
                steerForce = dogLooseBehaviour();
                steerForce += ObstacleAvoidance();
            }
            else if (animationController.GetBool("dogIsLoose") && dogCalledInScript) //dog is running free, called so coming to owner
            {
                maxRunningSpeed = 2;
                rotateSpeed = 2;
                currentPathPointDog = cameraMoveScript.currentPathPoint;
                steerForce = Seek(currentPathPointDog);
                steerForce += ObstacleAvoidance();
            }
            else //dog is on leash
            {
                rotateSpeed = 0.5f;
                maxRunningSpeed = 1;
                steerForce = Seek(currentPathPointDog);
            }
        }
    }
    public Vector3 Seek(Vector3 seekPosition)
    {
        Vector3 mySteeringForce = (seekPosition - transform.position).normalized * maxForce;//look at target direction, normalized and scaled
        Debug.DrawLine(transform.position, seekPosition, Color.cyan);
        return mySteeringForce;
    }

    public Vector3 dogLooseBehaviour()
    {
       
        tmrDogFree += Time.deltaTime;
        if (tmrDogFree > maxWanderTime)
        {
            tmrDogFree = 0;
            currentPathPointDog = cameraMoveScript.currentPathPoint;
            if (Vector3.Distance(cameraPlayer.transform.position, transform.position) > 10)
            {
                return Seek(currentPathPointDog);
            }

            else
            {
                if (UnityEngine.Random.Range(0, 3) > 1f)
                {

                    PathPointDogRandom = new Vector3((PathPointDogRandom.x - UnityEngine.Random.Range(0, 5)), currentPathPointDog.y, (PathPointDogRandom.z - UnityEngine.Random.Range(0, 5)));
                }
                else
                {

                    PathPointDogRandom = new Vector3((UnityEngine.Random.Range(0, 5) + currentPathPointDog.x), currentPathPointDog.y, (UnityEngine.Random.Range(0, 5) + currentPathPointDog.z));

                }
            }
            return Seek(PathPointDogRandom);
        }
        else
        {
            return steerForce;
        }
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
                Debug.Log("narrow");
                return Vector3.zero;//keep on going 
            }
            else
            {//full force
             //   Debug.Log("avoiding");
                tmrDogFree = maxWanderTime;
                return ObstacleAvoidanceForce * (mySteeringForceL + mySteeringForceR +
                           mySteeringForceM);//just return the sum of all three
            }
        }
        return Vector3.zero;//no steering force  because no obstacle was detected}
    }

    private bool DetectObstacle(Vector3 myDirection, out RaycastHit myHit, float myObstacleAvoidanceDistance)
    {
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
}

