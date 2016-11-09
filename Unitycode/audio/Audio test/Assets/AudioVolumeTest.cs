using UnityEngine;

public class AudioVolumeTest : MonoBehaviour
{
    public float RmsValue;
    public float DbValue;
    public float PitchValue;

    private const int QSamples = 1024;
    private const float RefValue = 0.1f;
    private const float Threshold = 0.02f;

    float[] _samples;
    private float[] _spectrum;
    private float _fSample;

    AudioSource src;
    
    void Start()
    {
        src = GetComponent<AudioSource>();
        _samples = new float[QSamples];
        _spectrum = new float[QSamples];
        _fSample = AudioSettings.outputSampleRate;
        src.clip = Microphone.Start(null, true, 10, 44100);
        // src.mute = true; //van in code 1
        //src.Play(); //code 1, plays audio source without sound?
        
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2)) //als f2 word ingedrukt, stoppen met opnemen + audio afspelen
        {
            Microphone.End(null);
            src.mute = true;
            src.Play(); // Play the audio source!
        }
       // Debug.Log(src.clip.frequency);
        AnalyzeSound();
    }

    void AnalyzeSound()
    {
        src.GetOutputData(_samples, 0); // fill array with samples
        int i;
        float sum = 0;
        for (i = 0; i < QSamples; i++)
        {
            sum += _samples[i] * _samples[i]; // sum squared samples
        }
        RmsValue = Mathf.Sqrt(sum / QSamples); // rms = square root of average
        DbValue = 20 * Mathf.Log10(RmsValue / RefValue); // calculate dB
        if (DbValue < -160) DbValue = -160; // clamp it to -160dB min
                                            // get sound spectrum
    }
}
