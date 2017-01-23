using UnityEngine;
using System.Collections;

public class headRotationSetRig : MonoBehaviour {
  public Transform mainCamVR;
    float cameraToTurn; 
	// Use this for initialization
	/*void Start () {
        Debug.LogError("setting rotation");
        transform.rotation = Quaternion.Euler(0, 0, 0);

        cameraToTurn = 360 - mainCamVR.transform.localRotation.eulerAngles.y;
        transform.localRotation = Quaternion.Euler(0, cameraToTurn, 0);
    }*/

    void Awake()
    {
        Debug.LogError("setting rotation");
        transform.rotation = Quaternion.Euler(0, 0, 0);

        cameraToTurn = 360 - mainCamVR.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, cameraToTurn, 0);
    }


}
