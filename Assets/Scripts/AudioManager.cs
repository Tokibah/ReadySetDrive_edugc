using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixer gameAudioMixer;

    [Header("Audio Sources")]
    public AudioSource musicAudioSource;
    public AudioSource waveAudioSource;
    public AudioSource[] birdLoopAudioSources;
    public AudioSource windLoopAudioSource;
    public AudioSource noiseAudioSource;

    [Header("Audio Clips")]
    public AudioClip[] birdChirps;
    public AudioClip windSound;
    public AudioClip waveSound;
    public AudioClip backgroundMusicClip;
    public AudioClip noiseSound;

    [Header("Timing Settings")]
    public float minBirdLoopDelay = 5f;
    public float maxBirdLoopDelay = 15f;
    public float minWindDelay = 10f;
    public float maxWindDelay = 20f;

    [Header("Volume Control UI Sliders")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;

    // No need for initial volume fields anymore as the mixer handles this
    // The sliders will initialize to their values and call the set methods
    
    private Coroutine birdChirpLoopCoroutine;
    private Coroutine windSoundLoopCoroutine;

    void Start()
    {
        // Set up sliders and their listeners to the new mixer methods
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.minValue = 0.0001f; // Avoid log(0) issues
            masterVolumeSlider.maxValue = 1f;
            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
            // Get initial mixer value to set the slider
            float currentMasterVolume;
            gameAudioMixer.GetFloat("MasterVolume", out currentMasterVolume);
            masterVolumeSlider.value = Mathf.Pow(10, currentMasterVolume / 20);
        }

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.minValue = 0.0001f;
            musicVolumeSlider.maxValue = 1f;
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
            // Get initial mixer value to set the slider
            float currentMusicVolume;
            gameAudioMixer.GetFloat("MusicVolume", out currentMusicVolume);
            musicVolumeSlider.value = Mathf.Pow(10, currentMusicVolume / 20);
        }

        // Start background audio
        if (backgroundMusicClip != null)
        {
            musicAudioSource.clip = backgroundMusicClip;
            musicAudioSource.loop = true; // BGM is typically a loop
            musicAudioSource.Play();
        }

        if (waveSound != null)
        {
            waveAudioSource.clip = waveSound;
            waveAudioSource.loop = true; // Environmental sounds are loops
            waveAudioSource.Play();
        }

        if (noiseSound != null)
        {
            noiseAudioSource.clip = noiseSound;
            noiseAudioSource.loop = true; // Environmental sounds are loops
            noiseAudioSource.Play();
        }

        // Start coroutines
        birdChirpLoopCoroutine = StartCoroutine(CycleBirdChirps());
        windSoundLoopCoroutine = StartCoroutine(WindSoundLoopRoutine());
    }

   public void SetMasterVolume(float sliderValue)
{
    // If the slider is at 0, explicitly mute it to -80 dB
    if (sliderValue == 0)
    {
        gameAudioMixer.SetFloat("EnvVolume", -80f);
    }
    else
    {
        // Otherwise, use the logarithmic scale
        float volume = Mathf.Log10(sliderValue) * 20;
        gameAudioMixer.SetFloat("MasterVolume", volume);
    }
}

public void SetMusicVolume(float sliderValue)
{
    // If the slider is at 0, explicitly mute it to -80 dB
    if (sliderValue == 0)
    {
        gameAudioMixer.SetFloat("MusicVolume", -80f);
    }
    else
    {
        // Otherwise, use the logarithmic scale
        float volume = Mathf.Log10(sliderValue) * 20;
        gameAudioMixer.SetFloat("MusicVolume", volume);
    }
}

    // Coroutines remain the same as they deal with timing, not volume
    IEnumerator CycleBirdChirps()
    {
        while (true)
        {
            var availableSource = birdLoopAudioSources.FirstOrDefault(s => s && !s.isPlaying);
            if (availableSource != null && birdChirps.Length > 0)
            {
                var clipToPlay = birdChirps[Random.Range(0, birdChirps.Length)];
                availableSource.clip = clipToPlay;
                availableSource.Play();
            }
            yield return new WaitForSeconds(Random.Range(minBirdLoopDelay, maxBirdLoopDelay));
        }
    }

    IEnumerator WindSoundLoopRoutine()
    {
        while (true)
        {
            if (windSound != null && !windLoopAudioSource.isPlaying)
            {
                windLoopAudioSource.clip = windSound;
                windLoopAudioSource.Play();
                yield return new WaitForSeconds(windSound.length);
            }
            yield return new WaitForSeconds(Random.Range(minWindDelay, maxWindDelay));
        }
    }

    void OnDestroy()
    {
        if (masterVolumeSlider != null)
            masterVolumeSlider.onValueChanged.RemoveListener(SetMasterVolume);
        if (musicVolumeSlider != null)
            musicVolumeSlider.onValueChanged.RemoveListener(SetMusicVolume);
    }
}