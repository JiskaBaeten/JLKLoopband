using UnityEngine;
using System.Collections;

public class cameraRotation : MonoBehaviour {
    Vector3 currentCameraRotation;
   public Vector3 rotationCameraLeft;
   public Vector3 rotationCameraRight;
	// Use this for initialization
	void Start () {
        currentCameraRotation = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.x < currentCameraRotation.x)
        {
            transform.Rotate(rotationCameraLeft);
        }
        else if (transform.position.x > currentCameraRotation.x)
        {
            transform.Rotate(rotationCameraRight);
        }
        currentCameraRotation = transform.position;
	}
}
