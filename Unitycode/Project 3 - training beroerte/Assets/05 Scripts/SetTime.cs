using UnityEngine;
using System.Collections;

public class SetTime : MonoBehaviour {
//shows time on watch by collection the system's current time

	void LateUpdate () {
    GetComponent<TextMesh>().text = System.DateTime.Now.ToString("HH:mm");
  }
}
