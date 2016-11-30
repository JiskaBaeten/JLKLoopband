using UnityEngine;
using System.Collections;

public class cameraRotation : MonoBehaviour {
    Vector3 currentCameraRotation;
   public Vector3 rotationCameraLeft;
   public Vector3 rotationCameraRight;
    public GameObject cameraEye;
    public float rotateCameraStrength;
	// Use this for initialization
	void Start () {
        currentCameraRotation = cameraEye.transform.position;
        rotateCameraStrength = 10;
        rotationCameraLeft = new Vector3(0, -rotateCameraStrength, 0);
        rotationCameraRight = new Vector3(0, rotateCameraStrength, 0);
	}
	
	// Update is called once per frame
	void Update () {
        if (cameraEye.transform.position.x < currentCameraRotation.x)
        {
            transform.Rotate(rotationCameraLeft);
        }
        else if (cameraEye.transform.position.x > currentCameraRotation.x)
        {
            transform.Rotate(rotationCameraRight);
        }
        currentCameraRotation = cameraEye.transform.position;
	}
}
