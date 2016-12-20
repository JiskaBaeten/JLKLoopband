using UnityEngine;
using System.Collections;

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
    // Use this for initialization
    void Start()
    {
        viveCam = GameObject.FindWithTag("cameraTopObject");
        cameraSteeringScript = viveCam.GetComponent<cameraSteeringBehaviour>();
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        dog = GameObject.FindWithTag("Dog");
        dogAnimationController = dog.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("update");
        if (controller == null)
        {
            Debug.Log("Controller not initialized");
            return;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("left");
            cameraSteeringScript.findNextPath("left");
        }

        gripButtonDown = controller.GetPressDown(gripButton);
        gripButtonUp = controller.GetPressUp(gripButton);
        gripButtonPressed = controller.GetPress(gripButton);


        triggerButtonDown = controller.GetPressDown(triggerButton);
        triggerButtonUp = controller.GetPressUp(triggerButton);
        triggerButtonPressed = controller.GetPress(triggerButton);

        if (gripButtonDown)
        {
            if (dogAnimationController.GetBool("dogIsLoose"))
            {
                dogAnimationController.SetBool("dogIsLoose", false);
            }
            else
            {
                dogAnimationController.SetBool("dogIsLoose", true);
            }

        }
        if (gripButtonUp)
        {
            Debug.Log("Grip Button was just unpressed");
        }
    }


    void OnTriggerStay(Collider col)
    {
        if (triggerButtonDown)
        {
            if (col.gameObject.tag == "controllerColliderLeft")
            {
                Debug.LogError("left");
                cameraSteeringScript.findNextPath("left");
                
            }
            else if (col.gameObject.tag == "controllerColliderRight")
            {
                cameraSteeringScript.findNextPath("right");
                Debug.LogError("right");
            }
        }
    }
}