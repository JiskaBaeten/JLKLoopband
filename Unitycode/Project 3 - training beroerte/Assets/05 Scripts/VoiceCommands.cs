using UnityEngine;
using System.Collections;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;


public class VoiceCommands : MonoBehaviour {
    KeywordRecognizer keywordRecognizer;
    Dictionary<string, System.Action> keywords;
    AudioSource dogBark; //playing bark
    // Use this for initialization
    void Start () {
        keywords = new Dictionary<string, System.Action>();
        dogBark = this.GetComponent<AudioSource>();
        dogBark.loop = false;
        keywords.Add("zit", () => {Debug.Log("aye aye cap");});
        keywords.Add("woof", () => {
            Debug.Log("woof");
            dogBark.Play();
        });
        keywords.Add("roll", () => {Debug.Log("they see me rolllliinnggg"); });
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
