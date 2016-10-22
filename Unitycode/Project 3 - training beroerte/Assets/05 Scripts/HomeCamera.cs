using UnityEngine;
using System.Collections;

public class HomeCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            gameObject.transform.Rotate(0,1,0);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            gameObject.transform.Rotate(0, -1, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            gameObject.transform.Rotate(1,0,0);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            gameObject.transform.Rotate(-1, 0, 0);
        }
	}
}
