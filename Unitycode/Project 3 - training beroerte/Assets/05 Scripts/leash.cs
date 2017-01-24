using UnityEngine;
using System.Collections;

//using lineRenderer for leash
public class leash : MonoBehaviour {
    public GameObject hand;
    LineRenderer lineRendererObject;
    public GameObject dogLeash;
    GameObject dog;
    float dogLeashWidth;
    Animator dogAnimationController;
    // Use this for initialization
    void Start () {
        dog = GameObject.FindWithTag("Dog");
        dogLeash = GameObject.FindWithTag("dogLeash");
        dogAnimationController = dog.GetComponent<Animator>();
        lineRendererObject = GetComponent<LineRenderer>();
        dogLeashWidth = 0.02f;
        lineRendererObject.SetWidth(dogLeashWidth, dogLeashWidth);
        GetComponent<Renderer>().material.color = Color.red;
        lineRendererObject.SetColors(Color.red, Color.red);
    }
	
	
	void Update () {
        if (!dogAnimationController.GetBool("dogIsLoose"))
        {
            lineRendererObject.enabled = true;
            lineRendererObject.SetPosition(0, dogLeash.transform.position);
            lineRendererObject.SetPosition(1, hand.transform.position);
        }
        else
        {
            lineRendererObject.enabled = false;
        }

    }

    private void OnDestroy()
    {
        Destroy(GetComponent<Renderer>().material);
    }
}
