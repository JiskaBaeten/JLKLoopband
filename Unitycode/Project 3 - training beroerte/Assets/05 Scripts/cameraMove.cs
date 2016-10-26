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
        transform.position = Vector3.Lerp(transform.position, new Vector3( dog.transform.position.x + 1, transform.position.y, dog.transform.position.z + 1), 0.1f);
        /*      if (Input.GetKey(KeyCode.RightArrow))
             {
                 gameObject.transform.Rotate(0, 1, 0);
             }
             if (Input.GetKey(KeyCode.LeftArrow))
             {
                 gameObject.transform.Rotate(0, -1, 0);
             }
             if (Input.GetKey(KeyCode.DownArrow))
             {
                 gameObject.transform.Rotate(1, 0, 0);
             }
             if (Input.GetKey(KeyCode.UpArrow))
             {
                 gameObject.transform.Rotate(-1, 0, 0);
             }

         if (Input.GetKey(KeyCode.Z))
         {
             gameObject.transform.position += new Vector3( 0,0, -0.5f);
         }
         if (Input.GetKey(KeyCode.S))
         {
             gameObject.transform.position += new Vector3(0, 0, 0.5f);
         }
         if (Input.GetKey(KeyCode.D))
         {
             gameObject.transform.position += new Vector3(-0.5f, 0, 0);
         }
         if (Input.GetKey(KeyCode.Q))
         {
             gameObject.transform.position += new Vector3(0.5f, 0, 0);
         }
         */

    }
}
