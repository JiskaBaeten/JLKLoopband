using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;

//voicecommands for park
public class VoiceCommands : MonoBehaviour {
    KeywordRecognizer keywordRecognizer;
    Dictionary<string, System.Action> keywords;
    GameObject dog;
    Animator animationControllerDog;
    steeringBehaviourDog dogSteeringBehaviourScript;
    GameObject viveCam;
    byte distanceToDogForLeash;
    public GameObject imageDogUnderstood;
    float tmrDogCalledUnderstood = 3;
    byte showDogUnderstoodImageTime = 3;
    // Use this for initialization
    void Start () {
        imageDogUnderstood.SetActive(false);
        distanceToDogForLeash = 5;
        dog = GameObject.FindWithTag("Dog");
        viveCam = GameObject.FindWithTag("cameraTopObject");
        dogSteeringBehaviourScript = dog.GetComponent<steeringBehaviourDog>();
        animationControllerDog = dog.GetComponent<Animator>();
        keywords = new Dictionary<string, System.Action>();
        //add keyword to listen to + reaction dog will give
        keywords.Add("come", () => {
            Debug.Log("coming");
            if (animationControllerDog.GetBool("dogIsLoose"))
            {

                dogSteeringBehaviourScript.dogLookingForCall = true;

                animationControllerDog.SetBool("dogIsWaiting", true);
                dogSteeringBehaviourScript.resetTimerDogLooking();
                    Debug.LogError("dog called and coming");
                    dogSteeringBehaviourScript.dogCalledInScript = true;
            }
        });

        keywords.Add("kom", () => {
            if (animationControllerDog.GetBool("dogIsLoose"))
            {
                //show image, dog heard you well, play timer
                imageDogUnderstood.SetActive(true);
                tmrDogCalledUnderstood = 0;

                dogSteeringBehaviourScript.dogLookingForCall = true;
                animationControllerDog.SetBool("dogIsWaiting", true);
                dogSteeringBehaviourScript.resetTimerDogLooking();

                    Debug.LogError("dog called and coming");
                    dogSteeringBehaviourScript.dogCalledInScript = true;
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
        tmrDogCalledUnderstood += Time.deltaTime;
        if ( tmrDogCalledUnderstood > showDogUnderstoodImageTime) //how long does image show?
        {
            imageDogUnderstood.SetActive(false);
        }
    }
}
