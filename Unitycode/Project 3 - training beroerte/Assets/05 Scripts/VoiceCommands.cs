using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;


public class VoiceCommands : MonoBehaviour {
    KeywordRecognizer keywordRecognizer;
    Dictionary<string, System.Action> keywords;
    GameObject dog;
    Animator animationControllerDog;
    steeringBehaviourDog dogSteeringBehaviourScript;
    GameObject viveCam;
    byte distanceToDogForLeash;
    Renderer dogRenderer; 
    // Use this for initialization
    void Start () {
       dogRenderer =  dog.GetComponent<Renderer>();
        distanceToDogForLeash = 5;
        dog = GameObject.FindWithTag("Dog");
        viveCam = GameObject.FindWithTag("cameraTopObject");
        dogSteeringBehaviourScript = dog.GetComponent<steeringBehaviourDog>();
        animationControllerDog = dog.GetComponent<Animator>();
        keywords = new Dictionary<string, System.Action>();
        keywords.Add("bark", () => {Debug.Log("woof");});
        
        keywords.Add("come", () => {
            Debug.Log("coming");
            if (animationControllerDog.GetBool("dogIsLoose"))
            {
                if (dogRenderer.isVisible)
                {
                    Debug.Log("hond in sight");
                }
                dogSteeringBehaviourScript.dogLookingForCall = true;

                animationControllerDog.SetBool("dogIsWaiting", true);
                dogSteeringBehaviourScript.resetTimerDogLooking();
                if (UnityEngine.Random.Range(0, 3) >= 1)
                {
                    Debug.LogError("dog called and coming");
                    dogSteeringBehaviourScript.dogCalledInScript = true;

                }
                else
                {
                    Debug.LogError("dog called and not coming");
                    dogSteeringBehaviourScript.dogCalledInScript = false;
                }
            }
        });

        keywords.Add("kom", () => {
            if (animationControllerDog.GetBool("dogIsLoose"))
            {
                if (dogRenderer.isVisible)
                {
                    Debug.Log("hond in sight");
                }
                dogSteeringBehaviourScript.dogLookingForCall = true;

                animationControllerDog.SetBool("dogIsWaiting", true);
                dogSteeringBehaviourScript.resetTimerDogLooking();
                if (UnityEngine.Random.Range(0, 3) >= 1)
                {
                    Debug.LogError("dog called and coming");
                    dogSteeringBehaviourScript.dogCalledInScript = true;

                }
                else
                {
                    Debug.LogError("dog called and not coming");
                    dogSteeringBehaviourScript.dogCalledInScript = false;
                }
            }
            Debug.Log("ik kom");

        });
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
        
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        // if the keyword recognized is in our dictionary, call that Action.
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("a");
            if (animationControllerDog.GetBool("dogIsLoose"))
            {
                dogSteeringBehaviourScript.dogLookingForCall = true;
                
                animationControllerDog.SetBool("dogIsWaiting", true);
               dogSteeringBehaviourScript.resetTimerDogLooking();
                if (UnityEngine.Random.Range(0, 3) >= 1)
                {
                    Debug.LogError("dog called and coming");
                    dogSteeringBehaviourScript.dogCalledInScript = true;
                    
                }
                else
                {
                    Debug.LogError("dog called and not coming");
                    dogSteeringBehaviourScript.dogCalledInScript = false;
                }
            }

        }
        
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (animationControllerDog.GetBool("dogIsLoose"))
            {
                if (Vector3.Distance(dog.transform.position, viveCam.transform.position) < distanceToDogForLeash)
                {
                    animationControllerDog.SetBool("dogIsLoose", false);
                    Debug.LogError("dog leash attach");
                    dogSteeringBehaviourScript.maxRunningSpeed = 1;
                }


            }
            else
            {
                dogSteeringBehaviourScript.maxRunningSpeed = 2;
                animationControllerDog.SetBool("dogIsLoose", true);
            }
        }
       /* if (Input.GetKeyDown(KeyCode.C))
        {
            animationControllerDog.SetBool("dogCalled", false);
        }*/
    }
}
