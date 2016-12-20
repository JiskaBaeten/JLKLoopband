using UnityEngine;
using System.Collections;

public class tempControlsSteering : MonoBehaviour {

    public GameObject viveCam; //werken met camera rig of met head??
    private GameObject dog;
    public Animator dogAnimationController;
    private cameraSteeringBehaviour cameraSteeringScript;
    // Use this for initialization
    void Start()
    {
        viveCam = GameObject.FindWithTag("cameraTopObject");
        cameraSteeringScript = viveCam.GetComponent<cameraSteeringBehaviour>();
        dog = GameObject.FindWithTag("Dog");
        dogAnimationController = dog.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("left");
            cameraSteeringScript.findNextPath("left");
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("right");
            cameraSteeringScript.findNextPath("right");
        }
    }
}
