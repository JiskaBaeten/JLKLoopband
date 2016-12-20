using UnityEngine;
using System.Collections;

public class leash : MonoBehaviour {
    public GameObject hand;
    LineRenderer lineRendererObject;
    public GameObject dog;
    float dogLeashWidth;
    // Use this for initialization
    void Start () {
        dog = GameObject.FindWithTag("Dog");
        lineRendererObject = GetComponent<LineRenderer>();
        dogLeashWidth = 0.02f;
        lineRendererObject.SetWidth(dogLeashWidth, dogLeashWidth);
        //Material whiteDiffuseMat = new Material(Shader.Find("Unlit/Texture"));
        //lineRendererObject.material = whiteDiffuseMat;
        GetComponent<Renderer>().material.color = Color.red;
        lineRendererObject.SetColors(Color.red, Color.red);
    }
	
	// Update is called once per frame
	void Update () {
        // GameObject gameObjectLineRenderer = new GameObject();

        // GameObject hand = GameObject.Find("manArm");
        
        lineRendererObject.SetPosition(0, dog.transform.position);
        lineRendererObject.SetPosition(1, hand.transform.position);
    }

    private void OnDestroy()
    {
        Destroy(GetComponent<Renderer>().material);
    }
}
