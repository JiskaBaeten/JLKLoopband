﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ControllersVive : MonoBehaviour
{
    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    public bool gripButtonDown = false;
    public bool gripButtonUp = false;
    public bool gripButtonPressed = false;

    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    public bool triggerButtonDown = false;
    public bool triggerButtonUp = false;
    public bool triggerButtonPressed = false;


    public GameObject viveCam; //werken met camera rig of met head??
    private GameObject dog;
    public Animator dogAnimationController;
    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;
    private cameraSteeringBehaviour cameraSteeringScript;
    private steeringBehaviourDog dogSteeringBehaviourScript;
    private GameObject ballToThrow;
    public GameObject arrowLeft;
    public GameObject arrowRight;
    // Use this for initialization
    void Start()
    {
        viveCam = GameObject.FindWithTag("cameraTopObject");
        cameraSteeringScript = viveCam.GetComponent<cameraSteeringBehaviour>();
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        dog = GameObject.FindWithTag("Dog");
        dogAnimationController = dog.GetComponent<Animator>();
        dogSteeringBehaviourScript = dog.GetComponent<steeringBehaviourDog>();
        ballToThrow = GameObject.FindWithTag("Ball");
        arrowLeft = GameObject.FindWithTag("arrowLeft");
        arrowRight = GameObject.FindWithTag("arrowRight");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.LogError("left");
            cameraSteeringScript.findNextPath("left");
            dog.GetComponent<AudioSource>().Play();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            cameraSteeringScript.findNextPath("right");
            Debug.LogError("right");
            dog.GetComponent<AudioSource>().Play();
        }
        if (controller == null)
        {
            Debug.Log("Controller not initialized");
            return;
        }

        gripButtonDown = controller.GetPressDown(gripButton);
        gripButtonUp = controller.GetPressUp(gripButton);
        gripButtonPressed = controller.GetPress(gripButton);


        triggerButtonDown = controller.GetPressDown(triggerButton);
        triggerButtonUp = controller.GetPressUp(triggerButton);
        triggerButtonPressed = controller.GetPress(triggerButton);

        if (SceneManager.GetActiveScene().name == "scene_park")
        {
            if (gripButtonUp)
            {
                if (dogAnimationController.GetBool("dogIsLoose"))
                {
                    if (Vector3.Distance(dog.transform.position, viveCam.transform.position) < 3)
                    {
                        dogAnimationController.SetBool("dogIsLoose", false);
                        Debug.LogError("dog leash attach");
                        dogSteeringBehaviourScript.maxRunningSpeed = 1;
                        dog.GetComponent<AudioSource>().Play();
                    }
                }
                else
                {
                    dog.GetComponent<AudioSource>().Play();
                    dogSteeringBehaviourScript.dogCalledInScript = false;
                    dogSteeringBehaviourScript.maxRunningSpeed = 2;
                    dogAnimationController.SetBool("dogIsLoose", true);
                }
            }
        }
        else if (SceneManager.GetActiveScene().name == "scene_home")
        {
            if (triggerButtonPressed)
            {
                ballToThrow.transform.position = transform.position;
            }
        }
       


    }


    void OnTriggerStay(Collider col)
    {
        if (cameraSteeringScript.showArrows())
        {
            if (triggerButtonDown)
            {

                if (col.gameObject.tag == "controllerColliderLeft")
                {
                    Debug.LogError("left");
                    cameraSteeringScript.findNextPath("left");
                    dog.GetComponent<AudioSource>().Play();

                }
                else if (col.gameObject.tag == "controllerColliderRight")
                {
                    cameraSteeringScript.findNextPath("right");
                    Debug.LogError("right");
                    dog.GetComponent<AudioSource>().Play();
                }
            }
            else
            {
                if (col.gameObject.tag == "controllerColliderLeft")
                {
                    arrowLeft.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                    arrowRight.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                }
                else if (col.gameObject.tag == "controllerColliderRight")
                {
                    arrowRight.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                    arrowLeft.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                }
            }

        }
        else
        {
            arrowRight.transform.localScale = new Vector3(0, 0, 0);
            arrowLeft.transform.localScale = new Vector3(0, 0, 0);
        }
    }
}