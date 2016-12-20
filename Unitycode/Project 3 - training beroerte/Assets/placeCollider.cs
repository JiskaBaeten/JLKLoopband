using UnityEngine;
using System.Collections;

public class placeCollider : MonoBehaviour {
    GameObject viveCam;
	// Use this for initialization
	void Start () {
        viveCam = GameObject.FindWithTag("MainCamera");
        if (tag == "controllerColliderLeft") //collider left
        {

            transform.position = viveCam.transform.position + new Vector3(0, 0,1);
        }
        else if (tag == "controllerColliderRight")
        {
            transform.position = viveCam.transform.position - new Vector3(0, 0, 1);
        }

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
