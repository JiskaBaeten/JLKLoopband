using UnityEngine;
using System.Collections;

public class catBehaviour : MonoBehaviour {
    //character control parameters
    public int maxForce = 150;
    public float mass = 100;
    public float gravity = 9.81f;
    public int maxRunningSpeed = 2;
    float rotateSpeed = 0.5f;
    float tmrDogFree;
    float maxWanderTime = 3;
    float wanderDist;
    float wanderRadius;
    float distanceCatRunAway = 3;

    //steer forces
    public Vector3 velocity;
    public Vector3 acceleration;
    public Vector3 steerForce;
    public Vector3 eindpos;
    float fleeForce = 150;

    //dog object
    GameObject dog;

    //obstacle avoidance
    public float ObstacleAvoidanceDistance;
    public float ObstacleAvoidanceForce;

    CharacterController controller;
    public Animator animationController;
    AudioSource audioCat;
    // Use this for initialization
    void Start()
    {
        dog = GameObject.FindWithTag("Dog");
        wanderDist = 2;
        wanderRadius = 2;
        controller = GetComponent<CharacterController>();
        animationController = GetComponent<Animator>();
        eindpos = transform.position + transform.forward * wanderDist + Random.onUnitSphere * wanderRadius;
        eindpos.y = transform.position.y;
        audioCat = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        //calc movement + obstacle avoidance
        steerForce = catWanderBehaviour();
        //steerForce += ObstacleAvoidance();
       
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



    public Vector3 catWanderBehaviour()
    {
        if (Vector3.Distance(transform.position, dog.transform.position) < distanceCatRunAway)
        {
            audioCat.Play();
            dog.GetComponent<steeringBehaviourDog>().reactOnCat();
            return Flee(dog.transform.position);
        }

        else { 
        tmrDogFree += Time.deltaTime;
        if (tmrDogFree > maxWanderTime || Vector3.Distance(eindpos, transform.position) < 1)
        {
            eindpos = Vector3.zero;
            tmrDogFree = 0;

            //heading = velocity.normalized;//where we're going                  //where we are + forward + random         
            eindpos = transform.position + transform.forward * wanderDist + Random.onUnitSphere * wanderRadius;
            eindpos.y = transform.position.y;//same height     

            Debug.Log("eindpos change");


        }
        return Seek(eindpos);
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
        Debug.Log("collision avoid");
        //obstacle found
        if (hitObstacleOnTheLeft || hitObstacleOnTheRight || hitObstacleInTheMiddle)
        {
            Debug.Log("obstacle");
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
                Debug.Log("avoiding");
                tmrDogFree = maxWanderTime;
                return ObstacleAvoidanceForce * (mySteeringForceL + mySteeringForceR +
                           mySteeringForceM);//just return the sum of all three
            }
        }
        return Vector3.zero;//no steering force  because no obstacle was detected}
    }

    private bool DetectObstacle(Vector3 myDirection, out RaycastHit myHit, float myObstacleAvoidanceDistance)
    {
        // Debug.Log(Physics.Raycast(transform.position, myDirection, out myHit,  ObstacleAvoidanceDistance));
        Debug.DrawRay(transform.position, myDirection);

        if (Physics.Raycast(transform.position, myDirection, out myHit,
            ObstacleAvoidanceDistance))//raycast, if hit
        {//myHit is out since you need it elsewhere too
            Debug.Log("obstacle avoidance");
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

