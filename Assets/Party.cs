using System.Collections.Generic;
using UnityEngine;

public class Party : MonoBehaviour
{
    public List<AudioClip> mp3List; // List of MP3 files
    private int currentMp3Index = 0;

    public AudioSource audioSource;
    public Light directionalLight;
    public float minIntensity = 1f;
    public float maxIntensity = 5f;
    public float intensityMultiplier = 10f;
    public alcanScript alcanScript;
    public GameObject canvas;

    private float originalRotationSpeed;

    // Parameters for BPM calculation
    private const int SampleSize = 1024;
    private const float PeakThreshold = 0.1f;

    private float[] samples;
    public float lastPeakTime;
    public float bpm;
    public float rotationMultiplier;
    public bool isPartyOn;
    
    // Smoothing parameters
    public float rotationSpeedSmoothing = 5f;

    void Start()
    {

        alcanScript = GameObject.FindGameObjectWithTag("Molecule").GetComponent<alcanScript>();
        originalRotationSpeed = alcanScript.rotationSpeed;

        samples = new float[SampleSize];
        alcanScript.speedBasedOnSlider = true;
        // Initialize audio source with the first MP3 in the list
        isPartyOn = false;
        if (mp3List.Count > 0)
        {
            audioSource.clip = mp3List[0];
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || isPartyOn)
        {
            canvas.SetActive(false);
            if (!audioSource.isPlaying)
            {
                // Choose the next MP3 in the list
                currentMp3Index = Random.RandomRange(0,mp3List.Count);

                // Set the chosen MP3 as the audio clip
                audioSource.clip = mp3List[currentMp3Index];

                // Play the audio
                audioSource.Play();

                Debug.Log("Playing MP3: " + audioSource.clip.name);

                alcanScript.speedBasedOnSlider = false;
                // Perform your desired actions here when starting a new MP3

                // ... (other actions you want to perform when playing a new MP3)
            }
            else
            {
                // If audio is already playing, stop it
                audioSource.Stop();
                Debug.Log("Stopping current MP3 playback");
            }
            isPartyOn = false;
        }

        CalculateBPM();
        AdjustRotationSpeed();

        if (audioSource.isPlaying)
        {
            float[] spectrumData = new float[256];
            audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);
            float averageAmplitude = 0f;

            for (int i = 0; i < spectrumData.Length; i++)
            {
                averageAmplitude += spectrumData[i];
            }

            averageAmplitude /= spectrumData.Length;

            float mappedIntensity = Mathf.Lerp(minIntensity, maxIntensity, averageAmplitude) * intensityMultiplier;
            directionalLight.intensity = mappedIntensity;

            Color newColor = new Color(spectrumData[64] * 200, spectrumData[128] * 200, spectrumData[192] * 200);
            directionalLight.color = newColor;
        }
    }

    void CalculateBPM()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);

        float currentPeak = 0f;
        int currentPeakIndex = 0;

        for (int i = 0; i < SampleSize; i++)
        {
            if (samples[i] > currentPeak && samples[i] > PeakThreshold)
            {
                currentPeak = samples[i];
                currentPeakIndex = i;
            }
        }

        float timeSinceLastPeak = Time.time - lastPeakTime;

        if (currentPeakIndex > 0 && timeSinceLastPeak > 0.1f)
        {
            bpm = 60f / timeSinceLastPeak;
            lastPeakTime = Time.time;
        }
    }

    void AdjustRotationSpeed()
    {
        float loudness = audioSource.volume;
        float targetRotationSpeed = originalRotationSpeed + (bpm * loudness) * rotationMultiplier;
        alcanScript.rotateEnabled = true;
        alcanScript.speedBasedOnSlider = false;
        alcanScript.rotationSpeed = Mathf.Lerp(alcanScript.rotationSpeed, targetRotationSpeed, Time.deltaTime * rotationSpeedSmoothing);
    }
    public void StartParty()
    {
        isPartyOn = true;
    }
}
