using UnityEngine;
using System.Collections;

public class cameraSetAtStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
       // Valve.VR.OpenVR.System.
        transform.localPosition = new Vector3(0, 0, 0);
       // transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
