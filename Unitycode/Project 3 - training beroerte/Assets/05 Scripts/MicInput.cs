﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//used for yelling feedback
public class MicInput : MonoBehaviour
{
    public AudioSource audioSource;
    public float updateStep = 0.1f;
    public int sampleDataLength = 1024;
    public float timeBetweenStartAndEnd;
    private float timeSpoken;
    private float currentUpdateTime = 0f;
    private float currentTime = 0f;
    public float clipLoudness;
    private float[] clipSampleData;
    public float volume;
    public float isYelling;
    public GameObject dogYellingImage;


    public Transform targetPoint;
    public float holdGazeTimeInSeconds = 2;
    private float currentGazeTimeInSeconds = 0;
    public GameObject dog;
    Vector3 screenPoint;
    Renderer cameraRenderer;
    float tmrDogYellingImage = 3;
    byte showDogYellingImageTime = 2;
    
    // Use this for initialization
    void Start()
    {
        dogYellingImage.SetActive(false);
        dog = GameObject.FindWithTag("dogMesh");
        isYelling = 0.05f;
        timeBetweenStartAndEnd = 3;
        audioSource = GetComponent<AudioSource>();
        // audioSource2 = GetComponent<AudioSource>();
        audioSource.clip = Microphone.Start(null, true, 1, sampleDataLength);
        cameraRenderer = dog.GetComponent<Renderer>();
        if (!audioSource)
        {
            Debug.LogError(GetType() + ".Awake: there was no audioSource set.");
        }
        clipSampleData = new float[sampleDataLength];

    }

    // Update is called once per frame
    void Update()
    {
        currentUpdateTime += Time.deltaTime;
        currentTime += Time.deltaTime;
        tmrDogYellingImage += Time.deltaTime;
        if (currentUpdateTime >= updateStep)
        {
            currentUpdateTime = 0f;
            audioSource.clip.GetData(clipSampleData, audioSource.timeSamples); //I read 1024 samples, which is about 80 ms on a 44khz stereo clip, beginning at the current sample position of the clip.
            clipLoudness = 0f;
            foreach (var sample in clipSampleData) //get all data + calculate average
            {
                clipLoudness += Mathf.Abs(sample);
            }
            clipLoudness /= sampleDataLength; //clipLoudness is what you are looking for
            volume = clipLoudness;

            if (volume > isYelling && currentTime >= timeSpoken + timeBetweenStartAndEnd)
            {
                timeSpoken = currentTime;
                dogYellingImage.SetActive(true);
                Debug.LogError("yelling" + volume);
                tmrDogYellingImage = 0;
            }
            if (tmrDogYellingImage > showDogYellingImageTime) // if dogimage is on long enough, deactivate
            {
                dogYellingImage.SetActive(false);
            }    
        }
        
    }
}
