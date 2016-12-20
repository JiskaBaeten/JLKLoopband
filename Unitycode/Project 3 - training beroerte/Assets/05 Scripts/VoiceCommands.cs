using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;


public class VoiceCommands : MonoBehaviour {
    KeywordRecognizer keywordRecognizer;
    Dictionary<string, System.Action> keywords;
    GameObject dog;
    Animator animationControllerDog;
    // Use this for initialization
    void Start () {
        dog = GameObject.FindWithTag("Dog");
        animationControllerDog = dog.GetComponent<Animator>();
        keywords = new Dictionary<string, System.Action>();
        keywords.Add("bark", () => {Debug.Log("woof");});
        keywords.Add("come", () => { Debug.Log("coming"); animationControllerDog.SetTrigger("dogCalled"); });
        keywords.Add("kom", () => { Debug.Log("ik kom eraan!"); animationControllerDog.SetTrigger("dogCalled"); });
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
            animationControllerDog.SetTrigger("dogCalled");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            animationControllerDog.SetBool("dogIsLoose", true);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            animationControllerDog.SetBool("dogCalled", false);
        }
	}
}
