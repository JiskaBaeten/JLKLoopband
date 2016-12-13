using UnityEngine;
using System.Collections;

public class controllerLeftright : MonoBehaviour {

    public Transform child;

    private Transform thisTransform;
    private Vector3 childPositionOffset;
    private Quaternion childRotationOffset;
    private Quaternion originalRotation;

    private void Awake()
    {
        thisTransform = this.transform;

        childPositionOffset = child.position - thisTransform.position;
        childRotationOffset = child.rotation * Quaternion.Inverse(thisTransform.rotation);

        originalRotation = thisTransform.rotation;
    }

    private void Update()
    {
        Quaternion deltaRotation = thisTransform.rotation * Quaternion.Inverse(originalRotation);

       Debug.Log("pos" + thisTransform.position + deltaRotation * childPositionOffset);
    }
}
