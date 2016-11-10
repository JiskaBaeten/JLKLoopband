using UnityEngine;
using System.Collections;

public class MicInput : MonoBehaviour {
    AudioSource audioSource;
    AudioSource audioSource2;
    public float updateStep = 0.1f;
    public int sampleDataLength = 1024;
    private float timeBetweenStartAndEnd;
    private float TimeStarted;
    private float currentUpdateTime = 0f;
    private float currentTime = 0f;
    bool isRecording1;
    bool isRecording2;
    public float clipLoudness;
    private float[] clipSampleData;

    // Use this for initialization
    void Start() { 
    
        timeBetweenStartAndEnd = 1;
        audioSource = GetComponent<AudioSource>();
        audioSource2 = GetComponent<AudioSource>();
     //  audioSource.clip = Microphone.Start(null, true, 10, 44100);
        isRecording1 = false;
       // audioSource.Play();
        if (!audioSource)
        {
            Debug.LogError(GetType() + ".Awake: there was no audioSource set.");
        }
        clipSampleData = new float[sampleDataLength];

    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime >= TimeStarted + timeBetweenStartAndEnd && isRecording2)
        {
            Microphone.End(null);
            isRecording1 = true;
            Debug.Log("starting mic" + TimeStarted);
            TimeStarted = currentTime;
            audioSource.clip = Microphone.Start(null, true, 2, 44100);
        }
        
        else if (currentTime >= TimeStarted + timeBetweenStartAndEnd && isRecording1) 
        {
            
            Debug.Log("end mic");
            isRecording1 = false;
            Microphone.End(null);
          //  audioSource.mute = true;
            audioSource.Play(); // Play the audio source!
            audioSource2.clip = Microphone.Start(null, true, 2, 44100);
            isRecording2 = true;
        }
        
        currentUpdateTime += Time.deltaTime;
        currentTime += Time.deltaTime;
        if (currentUpdateTime >= updateStep)
        {
            currentUpdateTime = 0f;
            audioSource.clip.GetData(clipSampleData, audioSource.timeSamples); //I read 1024 samples, which is about 80 ms on a 44khz stereo clip, beginning at the current sample position of the clip.
            clipLoudness = 0f;
            foreach (var sample in clipSampleData)
            {
                clipLoudness += Mathf.Abs(sample);
            }
            clipLoudness /= sampleDataLength; //clipLoudness is what you are looking for
        }

    }



    /* public static float MicLoudness;
     private string _device;

     //mic initialization
     void Start()
     {
         InitMic();
         _isInitialized = true;
     }
     void InitMic()
     {
         if (_device == null) _device = Microphone.devices[0];
         _clipRecord = Microphone.Start(_device, true, 999, 44100);

     }

     void StopMicrophone()
     {
         Microphone.End(_device);
     }


     AudioClip _clipRecord = new AudioClip();
     int _sampleWindow = 128;

     //get data from microphone into audioclip
     float LevelMax()
     {
         float levelMax = 0;
         float[] waveData = new float[_sampleWindow];
         int micPosition = Microphone.GetPosition(null) - (_sampleWindow + 1); // null means the first microphone
         Debug.Log(micPosition);
         if (micPosition < 0) return 0;
         _clipRecord.GetData(waveData, micPosition);
         // Getting a peak on the last 128 samples
         Debug.Log("maxing");
         for (int i = 0; i < _sampleWindow; i++)
         {
             float wavePeak = waveData[i] * waveData[i];
             Debug.Log(levelMax + "wavepeak");
             if (levelMax < wavePeak)
             {
                 levelMax = wavePeak;

             }
         }
         return levelMax;
     }



     void Update()
     {
         // levelMax equals to the highest normalized value power 2, a small number because < 1
         // pass the value to a static var so we can access it from anywhere
         MicLoudness = LevelMax();
         Debug.Log(MicLoudness);
     }

     bool _isInitialized;
     // start mic when scene starts
     void OnEnable()
     {
         InitMic();
         _isInitialized = true;

     }

     //stop mic when loading a new level or quit application
     void OnDisable()
     {
         StopMicrophone();
     }

     void OnDestroy()
     {
         StopMicrophone();
     }


     // make sure the mic gets started & stopped when application gets focused
    /* void OnApplicationFocus(bool focus)
     {
         if (focus)
         {
             //Debug.Log("Focus");

             if (!_isInitialized)
             {
                 //Debug.Log("Init Mic");
                 InitMic();
                 _isInitialized = true;
             }
         }
         if (!focus)
         {
             //Debug.Log("Pause");
             StopMicrophone();
             //Debug.Log("Stop Mic");
             _isInitialized = false;

         }
     }*/
}
