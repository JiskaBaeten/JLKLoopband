using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class audio_input : MonoBehaviour
{
    public float sensitivity = 100;
    public float loudness = 0;
    AudioSource src;
    private AudioListener theSource;
    public float timer;
    void Start()
    {
        src = GetComponent<AudioSource>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        //Debug.Log(timer);
        /*   if (Input.GetKeyDown(KeyCode.F1)) //als f1 word ingedrukt, starten met opnemen
           {
              src.clip = Microphone.Start(null, true, 10, 44100);
              src.mute = true; //van in code 1
               src.Play(); //code 1, plays audio source without sound?
               loudness = getAveragedVolume() * sensitivity;
           }*/

      //  while (Input.GetKeyDown(KeyCode.F1))
        //{
        //    loudness = getAveragedVolume() * sensitivity;
        //}
     /*   if (Input.GetKeyDown(KeyCode.F2)) //als f2 word ingedrukt, stoppen met opnemen + audio afspelen
        {
            Microphone.End(null); 
            src.Play(); // Play the audio source!
        }*/
    }
  /*  float getAveragedVolume()
    {
        float[] data = new float[256];
        float a = 0;
        src.GetOutputData(data, 0);
        float[] samples = new float[1024];
        theSource.GetOutputData(samples, 0);

        foreach (float s in data)
        {
            Debug.Log(s + " data");
            a += Mathf.Abs(s);
        }
        Debug.Log(a / 256);
        return a / 256;
    }*/
}
