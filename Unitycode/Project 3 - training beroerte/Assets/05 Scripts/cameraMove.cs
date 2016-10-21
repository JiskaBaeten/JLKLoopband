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
        transform.position = Vector3.Lerp(transform.position, dog.transform.position, 0.1f);
	}
}
