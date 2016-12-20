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
    Vector3 currentPathPointDog;
    CharacterController controller;//this GO's CharacterController


    //runningParameters
    public float maxRunningDistance;
    public float wanderDist;
    public float wanderRadius;
    float tmrDogFree;
    float maxWanderTime = 3;
    public Vector3 PathPointDogRandom;


    //obstacle avoidance
   public  float ObstacleAvoidanceDistance;
    public cameraSteeringBehaviour cameraMoveScript;
    GameObject cameraPlayer;
  public  float ObstacleAvoidanceForce;

    public Animator animationController;
    void Start()
    {
        currentPathPointDog = cameraMoveScript.currentPathPoint;
        controller = GetComponent<CharacterController>();
        animationController = GetComponent<Animator>();
        cameraPlayer = GameObject.FindWithTag("cameraTopObject");
        //animationController.SetBool("dogIsLoose", true);
    }

    void Update()
    {
      
        if (Vector3.Distance(transform.position, currentPathPointDog) < minDistToPathPoint)//if close enough pick next one
        {
            currentPathPointDog = cameraMoveScript.nextPathPoint;
        }

        if (animationController.GetBool("dogIsLoose"))
        {
            steerForce = dogLooseBehaviour();
        }
        else
        {
            steerForce = Seek(currentPathPointDog);
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



    public Vector3 Seek(Vector3 seekPosition)
    {
        Vector3 mySteeringForce = (seekPosition - transform.position).normalized * maxForce;//look at target direction, normalized and scaled
        Debug.DrawLine(transform.position, seekPosition, Color.cyan);
        return mySteeringForce;
    }

    public Vector3 dogLooseBehaviour()
    {
        ObstacleAvoidance();
        tmrDogFree += Time.deltaTime;
        if (tmrDogFree > maxWanderTime)
        {
            tmrDogFree = 0;

            if (Vector3.Distance(cameraPlayer.transform.position, transform.position) > 10)
            {
                maxRunningSpeed = 2;
                rotateSpeed = 2;
                return Seek(currentPathPointDog);
            }
            else
            {
                maxRunningSpeed = 2;
                rotateSpeed = 2;

                if (UnityEngine.Random.Range(0, 3) > 1f)
                {
                    Debug.Log("minus");
                    PathPointDogRandom = new Vector3((PathPointDogRandom.x - UnityEngine.Random.Range(0, 5)), currentPathPointDog.y, (PathPointDogRandom.z - UnityEngine.Random.Range(0, 5)));

                }
                else
                {
                    PathPointDogRandom = new Vector3((UnityEngine.Random.Range(0, 5) + PathPointDogRandom.x), currentPathPointDog.y, (UnityEngine.Random.Range(0, 5) + PathPointDogRandom.z));

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
            Debug.Log("avoiding");
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

