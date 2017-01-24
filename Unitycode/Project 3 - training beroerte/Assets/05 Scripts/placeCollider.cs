using UnityEngine;
using System.Collections;

public class placeCollider : MonoBehaviour
{
    GameObject viveCam;
    Vector3 colliderLeftPosition;
    Vector3 colliderRightPosition;
    // Use this for initialization
    void Start()
    {
        colliderLeftPosition = new Vector3(0, 0, 0.6f);
        colliderRightPosition = new Vector3(0, 0, 1.4f);
        viveCam = GameObject.FindWithTag("MainCamera");
        if (tag == "controllerColliderLeft") //collider left
        {
            transform.position = viveCam.transform.position + colliderLeftPosition;
        }
        else if (tag == "controllerColliderRight")
        {
            transform.position = viveCam.transform.position - colliderRightPosition;
        }

    }
}
