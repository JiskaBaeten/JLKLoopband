using UnityEngine;
using System.Collections;

public class headRotationSetRig : MonoBehaviour {
  public Transform mainCamVR;
    float cameraToTurn; 
	// Use this for initialization
	void Start () {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        Debug.Log(mainCamVR.rotation.y);
        cameraToTurn = 360 - mainCamVR.transform.localRotation.eulerAngles.y;
        transform.localRotation = Quaternion.Euler(0, cameraToTurn, 0);
    }
	
}
