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
    Quaternion targetRotationCamera;
    void Start () {
        currentCameraRotation = cameraEye.transform.position;
        rotateCameraStrength = 0.1f;
        rotationCameraLeft = new Vector3(0, -rotateCameraStrength, 0);
        rotationCameraRight = new Vector3(0, rotateCameraStrength, 0);
       
	}
	
	void Update () {

        //transform.LookAt(scriptDogMove.currentPathPoint);
        //transform.rotation = Quaternion.LookRotation( Vector3.RotateTowards(transform.localPosition, dog.transform.position,10*Time.deltaTime,0));
        targetRotationCamera = Quaternion.LookRotation(scriptDogMove.currentPathPoint);
        Debug.Log("before" +targetRotationCamera);
       // targetRotationCamera.y -= 22.037f;
        Debug.Log("afteré" + targetRotationCamera);
        transform.rotation = Quaternion.Lerp(transform.rotation,targetRotationCamera,0.5f );


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
