using UnityEngine;
using System.Collections;

public class cameraMove : MonoBehaviour {
    GameObject dog;
	// Use this for initialization
	void Start () {
        dog = GameObject.FindGameObjectWithTag("Dog");
        
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.Lerp(transform.position, new Vector3( dog.transform.position.x, transform.position.y, dog.transform.position.z), 0.05f);
            if (Input.GetKey(KeyCode.D))
             {
                 gameObject.transform.Rotate(0, 1, 0);
             }
             if (Input.GetKey(KeyCode.Q))
             {
                 gameObject.transform.Rotate(0, -1, 0);
             }
             if (Input.GetKey(KeyCode.S))
             {
                 gameObject.transform.Rotate(1, 0, 0);
             }
             if (Input.GetKey(KeyCode.Z))
             {
                 gameObject.transform.Rotate(-1, 0, 0);
             }

       

    }
}
