using UnityEngine;
using System.Collections;

public class tempControlsSteering : MonoBehaviour
{

    public GameObject viveCam; //work with camera rig or head?
    private GameObject dog;
    public Animator dogAnimationController;
    private cameraSteeringBehaviour cameraSteeringScript;
    private steeringBehaviourDog dogSteeringBehaviourScript;
    // Use this for initialization
    void Start()
    {
        viveCam = GameObject.FindWithTag("cameraTopObject");
        cameraSteeringScript = viveCam.GetComponent<cameraSteeringBehaviour>();
        dog = GameObject.FindWithTag("Dog");
        dogAnimationController = dog.GetComponent<Animator>();
        dogSteeringBehaviourScript = dog.GetComponent<steeringBehaviourDog>();
    }

    // Update is called once per frame
    void Update()
    {
        /*   if (Input.GetKeyDown(KeyCode.LeftArrow))
           {
               Debug.Log("left");
               cameraSteeringScript.findNextPath("left");
           }
           else if (Input.GetKeyDown(KeyCode.RightArrow))
           {
               Debug.Log("right");
               cameraSteeringScript.findNextPath("right");
           }
           */

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (dogAnimationController.GetBool("dogIsLoose"))
            {
                dogSteeringBehaviourScript.maxRunningSpeed = 1;
                dogAnimationController.SetBool("dogIsLoose", false);
            }
            else
            {
                dogSteeringBehaviourScript.maxRunningSpeed = 2;
                dogAnimationController.SetBool("dogIsLoose", true);
            }
        }
    }
}
