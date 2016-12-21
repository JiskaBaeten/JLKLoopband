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
    // Use this for initialization
    void Start () {
        distanceToDogForLeash = 5;
        dog = GameObject.FindWithTag("Dog");
        viveCam = GameObject.FindWithTag("cameraTopObject");
        dogSteeringBehaviourScript = dog.GetComponent<steeringBehaviourDog>();
        animationControllerDog = dog.GetComponent<Animator>();
        keywords = new Dictionary<string, System.Action>();
        keywords.Add("bark", () => {Debug.Log("woof");});

        keywords.Add("come", () => {
            Debug.LogError("coming call");
            if (animationControllerDog.GetBool("dogIsLoose"))
            {
                dogSteeringBehaviourScript.maxRunningSpeed = 0;
                animationControllerDog.SetTrigger("dogCalled");
                animationControllerDog.SetBool("dogIsWaiting", true);
                if (UnityEngine.Random.Range(0, 3) >= 1)
                {

                    animationControllerDog.SetTrigger("callToWalkingWithExitTime");
                    Debug.LogError("dog called and coming");
                    animationControllerDog.SetBool("dogIsLoose", false);
                }
                else
                {
                    Debug.LogError("dog called and not coming");

                    animationControllerDog.SetBool("dogIsLoose", true);
                }
                animationControllerDog.SetBool("dogIsWaiting", false);
            }

        });

        keywords.Add("kom", () => {
            Debug.LogError("coming call");
            if (animationControllerDog.GetBool("dogIsLoose"))
            {
                dogSteeringBehaviourScript.maxRunningSpeed = 0;
                animationControllerDog.SetTrigger("dogCalled");
                Debug.Log("waiting true");
                animationControllerDog.SetBool("dogIsWaiting", true);
                if (UnityEngine.Random.Range(0, 3) >= 1)
                {
                    animationControllerDog.SetTrigger("callToWalkingWithExitTime");
                    Debug.LogError("dog called and coming");
                    animationControllerDog.SetBool("dogIsWaiting", false);
                    //animationControllerDog.SetBool("dogIsLoose", false);
                    dogSteeringBehaviourScript.dogCalled = true;
                }
                else
                {
                    Debug.LogError("dog called and not coming");
                    dogSteeringBehaviourScript.dogCalled = false;
                    animationControllerDog.SetBool("dogIsWaiting", false);
                }
                animationControllerDog.SetBool("dogIsWaiting", false);
            }




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
                dogSteeringBehaviourScript.maxRunningSpeed = 0;
                animationControllerDog.SetTrigger("dogCalled");
                Debug.Log("waiting true");
                animationControllerDog.SetBool("dogIsWaiting", true);
                if (UnityEngine.Random.Range(0, 3) >= 1)
                {
                    animationControllerDog.SetTrigger("callToWalkingWithExitTime");
                    Debug.LogError("dog called and coming");
                    animationControllerDog.SetBool("dogIsWaiting", false);
                    //animationControllerDog.SetBool("dogIsLoose", false);
                    dogSteeringBehaviourScript.dogCalled = true;
                }
                else
                {
                    Debug.LogError("dog called and not coming");
                    dogSteeringBehaviourScript.dogCalled = false;
                    animationControllerDog.SetBool("dogIsWaiting", false);
                }
                animationControllerDog.SetBool("dogIsWaiting", false);
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
