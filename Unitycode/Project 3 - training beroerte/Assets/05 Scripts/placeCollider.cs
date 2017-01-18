using UnityEngine;
using System.Collections;

public class placeCollider : MonoBehaviour {
    GameObject viveCam;
	// Use this for initialization
	void Start () {
        viveCam = GameObject.FindWithTag("MainCamera");
        if (tag == "controllerColliderLeft") //collider left
        {
            transform.position = viveCam.transform.position + new Vector3(0, 0,0.6f);
        }
        else if (tag == "controllerColliderRight")
        {
            transform.position = viveCam.transform.position - new Vector3(0, 0,1.4f);
        }

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
