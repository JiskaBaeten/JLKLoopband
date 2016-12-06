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

    public cameraSteeringBehaviour cameraMoveScript;


    void Start()
    {
        currentPathPointDog = cameraMoveScript.currentPathPoint;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, cameraMoveScript.currentPathPoint) < minDistToPathPoint)//if close enough pick next one
        {
            currentPathPointDog = cameraMoveScript.nextPathPoint;
        }


        steerForce = Seek(currentPathPointDog);
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
}

    
