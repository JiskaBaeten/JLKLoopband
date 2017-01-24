using UnityEngine;
using System.Collections;

public class HomeCamera : MonoBehaviour {

	//script for looking around in house, without vive
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
