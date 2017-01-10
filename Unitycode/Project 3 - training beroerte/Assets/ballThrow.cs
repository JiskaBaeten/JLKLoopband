using UnityEngine;
using System.Collections;

public class ballThrow : MonoBehaviour
{
    Vector3 velocity;
    void Update() { 
    if(Input.GetMouseButton(0)){
            Debug.Log("test");
    transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition); ;
     Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    velocity = new Vector3(touchPosition.x, touchPosition.y, transform.position.z) - transform.position;
    }
        else
        {
            Debug.Log("test nop");
            transform.position += velocity;
        }
       
    }
}

