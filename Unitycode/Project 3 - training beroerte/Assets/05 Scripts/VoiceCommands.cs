using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;


public class continuousRecognition : MonoBehaviour {
    KeywordRecognizer keywordRecognizer;
    Dictionary<string, System.Action> keywords;
    // Use this for initialization
    void Start () {
        keywords = new Dictionary<string, System.Action>();
        keywords.Add("how are you", () => {Debug.Log("marvelous");});
        keywords.Add("bark", () => {Debug.Log("woof");});
        keywords.Add("activate", () => {Debug.Log("activated"); });
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
	
	}
}
