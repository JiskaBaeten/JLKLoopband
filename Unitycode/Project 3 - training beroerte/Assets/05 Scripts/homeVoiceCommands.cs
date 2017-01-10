using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;

public class homeVoiceCommands : MonoBehaviour
{

    KeywordRecognizer keywordRecognizer;
    Dictionary<string, System.Action> keywords;
    Animator animationControllerDog;
    homeSteeringBehaviourDog dogSteeringBehaviourScript;



    void Start()
    {
   
        dogSteeringBehaviourScript = GetComponent<homeSteeringBehaviourDog>();
        animationControllerDog = GetComponent<Animator>();
        keywords = new Dictionary<string, System.Action>();
        keywords.Add("bark", () => { Debug.Log("woof"); });

        keywords.Add("zit", () => {
            Debug.Log("zit");

                dogSteeringBehaviourScript.dogTrick();
                animationControllerDog.SetTrigger("triggerSit");
        });

        keywords.Add("sit", () => {
            Debug.Log("sit");

            dogSteeringBehaviourScript.dogTrick();
            animationControllerDog.SetTrigger("triggerSit");
            
        });

        keywords.Add("paw", () => {
            Debug.Log("paw");
  
            dogSteeringBehaviourScript.dogTrick();
            animationControllerDog.SetTrigger("triggerPaw");
            
        });

        keywords.Add("poot", () => {
            Debug.Log("poot");
  
            dogSteeringBehaviourScript.dogTrick();
            animationControllerDog.SetTrigger("triggerPaw");
            
        });

        keywords.Add("lig", () => {
            Debug.Log("lig");
      
            dogSteeringBehaviourScript.dogTrick();
            animationControllerDog.SetTrigger("triggerLay");
            
        });

        keywords.Add("lay", () => {
            Debug.Log("lay");
        
            dogSteeringBehaviourScript.dogTrick();
            animationControllerDog.SetTrigger("triggerLay");
            
        });

        keywords.Add("high", () => {
            Debug.Log("high");
        
            dogSteeringBehaviourScript.dogTrick();
            animationControllerDog.SetTrigger("triggerHigh");
            
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
}

