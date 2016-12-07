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
    // Use this for initialization
    void Start()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        dog = GameObject.FindWithTag("Dog");
       dogAnimationController =  dog.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
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
        if (triggerButtonDown)
        {
            if (viveCam.transform.position.x - transform.position.x > 0)
            {
                Debug.LogError("right");
                
            }
            else if (viveCam.transform.position.x - transform.position.x < 0)
            {
                Debug.LogError("left");
            }
            
        }
        if (triggerButtonUp)
        {
            Debug.Log("Trigger Button was just unpressed");
        }
    }
}


//camera x = 5  hand x = 4 --> if cam - hand = 5 - 4 --> 1 links
//camera x = 4 hand x = 5 --> if cam - hand = 4-5 --> -1 rechts
