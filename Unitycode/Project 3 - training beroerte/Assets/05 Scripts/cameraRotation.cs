using UnityEngine;
using System.Collections;

public class cameraRotation : MonoBehaviour {
    Vector3 currentCameraRotation;
   public Vector3 rotationCameraLeft;
   public Vector3 rotationCameraRight;
    public GameObject cameraEye;
    public float rotateCameraStrength;
    public GameObject dog;
    public steeringBehaviourDog scriptDogMove;
    Vector3 lookAtWaypoint;
    Quaternion targetRotationCamera;
    Vector3 cameraPosition;
    float lockPos;
    void Start () {
        currentCameraRotation = cameraEye.transform.position;
        rotateCameraStrength = 0.1f;
        rotationCameraLeft = new Vector3(0, -rotateCameraStrength, 0);
        rotationCameraRight = new Vector3(0, rotateCameraStrength, 0);
        lockPos = 0;
	}
	
	void Update () {
        cameraPosition = transform.position;
        lookAtWaypoint = scriptDogMove.currentPathPoint;
        Debug.Log("rotate" + lookAtWaypoint);

        Debug.DrawRay(cameraPosition, dog.transform.position, Color.red);
        //transform.LookAt(scriptDogMove.currentPathPoint);
        //transform.rotation = Quaternion.LookRotation( Vector3.RotateTowards(transform.localPosition, dog.transform.position,10*Time.deltaTime,0));
        transform.LookAt(dog.transform.position);
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, lockPos);
        //Debug.DrawRay(transform.position,scriptDogMove.currentPathPoint, Color.red);
        // transform.rotation = Quaternion.Slerp(transform.rotation,targetRotationCamera, 0.02f );


        /* if (cameraEye.transform.position.z < currentCameraRotation.z)
        {
            Vector3.RotateTowards(transform.localPosition, )
            transform.Rotate(rotationCameraLeft);
        }
        else if (cameraEye.transform.position.z > currentCameraRotation.z)
        {
            transform.Rotate(rotationCameraRight);
        }
        currentCameraRotation = cameraEye.transform.position;*/
    }
}
