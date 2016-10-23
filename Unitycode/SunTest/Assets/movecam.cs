using UnityEngine;
using System.Collections;

public class movecam : MonoBehaviour {


  public float _distance = 20.0f;

  //Control the speed of zooming and dezooming.
  public float _zoomStep = 1.0f;

  //The speed of the camera. Control how fast the camera will rotate.
  public float _xSpeed = 1f;
  public float _ySpeed = 1f;

  //The position of the cursor on the screen. Used to rotate the camera.
  private float _x = 0.0f;
  private float _y = 0.0f;

  //Distance vector. 
  private Vector3 _distanceVector;

  /**
  * Move the camera to its initial position.
  */
  void Start()
  {
    _distanceVector = new Vector3(0.0f, 0.0f, -_distance);

    Vector2 angles = this.transform.localEulerAngles;
    _x = angles.x;
    _y = angles.y;

    this.Rotate(_x, _y);

  }

  void LateUpdate()
  {
      this.RotateControls();
  }

  /**
  * Rotate the camera when the first button of the mouse is pressed.
  * 
  */
  void RotateControls()
  {
    /**_x += Input.GetAxis("Mouse X") * _xSpeed;
    _y += Input.GetAxis("Mouse Y") * _xSpeed;*/

    _x += Input.GetAxis("Horizontal") * _xSpeed;
    _y += Input.GetAxis("Vertical") * _xSpeed;

    this.Rotate(_x, _y);
  }

  /**
  * Transform the cursor mouvement in rotation and in a new position
  * for the camera.
  */
  void Rotate(float x, float y)
  {
    //Transform angle in degree in quaternion form used by Unity for rotation.
    Quaternion rotation = Quaternion.Euler(y, x, 0.0f);

    Vector3 position = rotation * _distanceVector;

    //Update the rotation and position of the camera.
    transform.rotation = rotation;
    transform.position = position;
  }

} //End class