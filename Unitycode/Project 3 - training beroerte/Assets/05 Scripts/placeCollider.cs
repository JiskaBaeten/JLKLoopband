using UnityEngine;
using System.Collections;

//this script is used to decide is the player is pointing left or right from the view
public class placeCollider : MonoBehaviour
{
  GameObject viveCam;
  Vector3 colliderLeftPosition;
  Vector3 colliderRightPosition;
  float leftZpos = 0.6f;
  float rightZpos = 1.4f;
  string cameraTag = "MainCamera";
  string controllerColliderLeftTag = "controllerColliderLeft";
  string controllerColliderRightTag = "controllerColliderRight";

  // Use this for initialization
  void Start()
  {
    colliderLeftPosition = new Vector3(0, 0, leftZpos);
    colliderRightPosition = new Vector3(0, 0, rightZpos);
    viveCam = GameObject.FindWithTag(cameraTag);
    if (tag == controllerColliderLeftTag) //collider left
    {
      transform.position = viveCam.transform.position + colliderLeftPosition;
    }
    else if (tag == controllerColliderRightTag)
    {
      transform.position = viveCam.transform.position - colliderRightPosition;
    }

  }
}
