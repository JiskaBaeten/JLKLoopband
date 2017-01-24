using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

//script for linking controller input to right script
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
    private byte dogCatchDistance = 3;
    private byte dogWalkingSpeed = 1;
    private byte dogRunningSpeed = 2;
    public Animator dogAnimationController;
    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;
    private cameraSteeringBehaviour cameraSteeringScript;
    private steeringBehaviourDog dogSteeringBehaviourScript;
    private GameObject ballToThrow;


    //arrows 
    public GameObject arrowLeft;
    public GameObject arrowRight;
    private float scaleArrowBig = 0.75f;
    private float scaleArrowSmall = 0.5f;
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
                if (dogAnimationController.GetBool("dogIsLoose")) //if the dog is loose, catch the dog but only when within distance
                {
                    if (Vector3.Distance(dog.transform.position, viveCam.transform.position) < dogCatchDistance)
                    {
                        dogAnimationController.SetBool("dogIsLoose", false);
                        Debug.LogError("dog leash attach");
                        dogSteeringBehaviourScript.maxRunningSpeed = dogWalkingSpeed;
                        dog.GetComponent<AudioSource>().Play();
                    }
                }
                else // set the dog loose
                {
                    dog.GetComponent<AudioSource>().Play();
                    dogSteeringBehaviourScript.dogCalledInScript = false;
                    dogSteeringBehaviourScript.maxRunningSpeed = dogRunningSpeed;
                    dogAnimationController.SetBool("dogIsLoose", true);
                }
            }
        }
        else if (SceneManager.GetActiveScene().name == "scene_home") //pickup the ball
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

                if (col.gameObject.tag == "controllerColliderLeft") //player choosing left
                {
                    Debug.LogError("left");
                    cameraSteeringScript.findNextPath("left");
                    dog.GetComponent<AudioSource>().Play();

                }
                else if (col.gameObject.tag == "controllerColliderRight") //player choosing right
                {
                    cameraSteeringScript.findNextPath("right");
                    Debug.LogError("right");
                    dog.GetComponent<AudioSource>().Play();
                }
            }
            else
            {
                if (col.gameObject.tag == "controllerColliderLeft") //controller is on left side of player
                {
                    arrowLeft.transform.localScale = new Vector3(scaleArrowBig, scaleArrowBig, scaleArrowBig);
                    arrowRight.transform.localScale = new Vector3(scaleArrowSmall, scaleArrowSmall, scaleArrowSmall);
                }
                else if (col.gameObject.tag == "controllerColliderRight") // controller is on right side of player
                {
                    arrowRight.transform.localScale = new Vector3(scaleArrowBig, scaleArrowBig, scaleArrowBig);
                    arrowLeft.transform.localScale = new Vector3(scaleArrowSmall, scaleArrowSmall, scaleArrowSmall);
                }
            }

        }
        else
        {
            arrowRight.transform.localScale = Vector3.zero;
            arrowLeft.transform.localScale = Vector3.zero;
        }
    }
}