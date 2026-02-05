using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio; // Required for AudioMixer
using System.Collections;
using System.Linq;

public class AudioDrivingCenter : MonoBehaviour
{
    // The AudioMixer to control. Assign this in the Inspector.
    [Header("Audio Mixer")]
    [Tooltip("Assign your AudioMixer asset here.")]
    public AudioMixer gameAudioMixer;
    public string masterVolumeExposer = "MasterVolume"; // Name of the exposed master volume parameter
    public string musicVolumeExposer = "MusicVolume";   // Name of the exposed music volume parameter

    [Header("Audio Sources")]
    [Tooltip("AudioSource for the always-playing noise sound.")]
    public AudioSource noiseAudioSource;
    [Tooltip("AudioSources for looping bird sounds.")]
    public AudioSource[] birdAudioSources;
    [Tooltip("AudioSource for the first car's engine noise.")]
    public AudioSource car1AudioSource;
    [Tooltip("AudioSource for the second car's engine noise.")]
    public AudioSource car2AudioSource;
    [Tooltip("AudioSource for background music.")]
    public AudioSource musicAudioSource;

    [Header("Audio Clips")]
    [Tooltip("Assign your noise sound audio clip here.")]
    public AudioClip noiseSound;
    [Tooltip("Assign your bird chirp audio clips here.")]
    public AudioClip[] birdChirps;
    [Tooltip("Assign your car engine audio clip here.")]
    public AudioClip carEngineSound;
    [Tooltip("Assign your background music audio clip here.")]
    public AudioClip backgroundMusicClip;

    [Header("Timing Settings")]
    [Tooltip("The minimum time (in seconds) for the gap between bird chirp playback.")]
    public float minBirdDelay = 5f;
    [Tooltip("The maximum time (in seconds) for the gap between bird chirp playback.")]
    public float maxBirdDelay = 15f;

    [Header("Volume Control")]
    [Tooltip("Assign your UI Slider for master volume control.")]
    public Slider masterVolumeSlider;
    [Tooltip("Assign your UI Slider for background music volume control.")]
    public Slider musicVolumeSlider;

    [Header("Noise Specific Settings")]
    [Tooltip("The base volume factor for the noise sound (0.0 to 1.0).")]
    [Range(0f, 1f)]
    public float noiseBaseVolumeFactor = 0.05f;

    private Coroutine birdSoundLoopCoroutine;
    
    void Start()
    {
        // --- Initialization and Validation ---
        if (gameAudioMixer == null) Debug.LogError("AudioMixer not assigned! Volume control will not work.");
        if (noiseAudioSource == null) Debug.LogWarning("Noise AudioSource not assigned!");
        if (car1AudioSource == null) Debug.LogWarning("Car 1 AudioSource not assigned!");
        if (car2AudioSource == null) Debug.LogWarning("Car 2 AudioSource not assigned!");
        if (musicAudioSource == null) Debug.LogWarning("Music AudioSource not assigned!");

        if (birdAudioSources == null || birdAudioSources.Length == 0)
        {
            Debug.LogError("No AudioSources assigned to birdAudioSources array! Bird sounds will not play.");
        }
        else
        {
            foreach (AudioSource source in birdAudioSources)
            {
                if (source != null) source.loop = false;
            }
        }
        
        if (noiseAudioSource != null) noiseAudioSource.loop = true;
        if (car1AudioSource != null) car1AudioSource.loop = true;
        if (car2AudioSource != null) car2AudioSource.loop = true;
        if (musicAudioSource != null) musicAudioSource.loop = true;

        // --- Initialize Volume Sliders ---
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.minValue = 0.0001f;
            masterVolumeSlider.maxValue = 1f;

            // Get the current master volume from the AudioMixer and set the slider's value accordingly.
            float currentMasterVolumeDb;
            gameAudioMixer.GetFloat(masterVolumeExposer, out currentMasterVolumeDb);
            masterVolumeSlider.value = DecibelToLinear(currentMasterVolumeDb);

            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
            SetMasterVolume(masterVolumeSlider.value); // Apply initial volume to mixer
        }
        else
        {
            Debug.LogWarning("Master Volume Slider not assigned. Master volume control will not be available via UI.");
        }

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.minValue = 0.0001f;
            musicVolumeSlider.maxValue = 1f;
            
            // Get the current music volume from the AudioMixer and set the slider's value accordingly.
            float currentMusicVolumeDb;
            gameAudioMixer.GetFloat(musicVolumeExposer, out currentMusicVolumeDb);
            musicVolumeSlider.value = DecibelToLinear(currentMusicVolumeDb);

            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
            SetMusicVolume(musicVolumeSlider.value); // Apply initial volume to mixer
        }
        else
        {
            Debug.LogWarning("Music Volume Slider not assigned. Music volume control will not be available via UI.");
        }

        // --- Start Core Audio Routines ---
        StartCoroutine(PlayNoiseSoundRoutine());
        StartCoroutine(PlayCarEngineSoundRoutine(car1AudioSource));
        StartCoroutine(PlayCarEngineSoundRoutine(car2AudioSource));
        birdSoundLoopCoroutine = StartCoroutine(BirdSoundLoopRoutine());
        StartCoroutine(PlayBackgroundMusicRoutine());
    }

    /// <summary>
    /// Sets the master volume for the game using the AudioMixer.
    /// </summary>
    public void SetMasterVolume(float volume)
    {
        if (gameAudioMixer != null)
        {
            gameAudioMixer.SetFloat(masterVolumeExposer, LinearToDecibel(volume));
            UpdateNoiseVolume();
        }
    }

    /// <summary>
    /// Sets the volume for background music using the AudioMixer.
    /// </summary>
    public void SetMusicVolume(float volume)
    {
        if (gameAudioMixer != null)
        {
            gameAudioMixer.SetFloat(musicVolumeExposer, LinearToDecibel(volume));
        }
    }

    /// <summary>
    /// Calculates and sets the noise audio source volume based on master volume and a base factor.
    /// </summary>
    private void UpdateNoiseVolume()
    {
        if (noiseAudioSource != null && masterVolumeSlider != null)
        {
            float targetVolume = masterVolumeSlider.value * noiseBaseVolumeFactor;
            noiseAudioSource.volume = targetVolume;
        }
    }
    
    // Helper function to convert a linear volume (0-1) to decibels (-80 to 0).
    private float LinearToDecibel(float linear)
    {
        if (linear <= 0.0001f) // Treat values near zero as a mute
            return -80f;
        else
            return Mathf.Log10(linear) * 20f;
    }

    // Helper function to convert decibels (-80 to 0) to a linear volume (0-1).
    private float DecibelToLinear(float db)
    {
        return Mathf.Pow(10f, db / 20f);
    }
    
    IEnumerator PlayBackgroundMusicRoutine()
    {
        if (backgroundMusicClip == null)
        {
            Debug.LogWarning("No background music clip assigned.");
            yield break;
        }
        if (musicAudioSource != null)
        {
            musicAudioSource.clip = backgroundMusicClip;
            musicAudioSource.Play();
        }
        yield break;
    }

    IEnumerator PlayNoiseSoundRoutine()
    {
        if (noiseSound == null)
        {
            Debug.LogWarning("No noise sound clip assigned.");
            yield break;
        }
        if (noiseAudioSource != null)
        {
            noiseAudioSource.clip = noiseSound;
            noiseAudioSource.Play();
        }
        yield break;
    }

    IEnumerator BirdSoundLoopRoutine()
    {
        if (birdChirps == null || birdChirps.Length == 0)
        {
            Debug.LogWarning("Bird chirp sound clips are not assigned.");
            yield break;
        }
        if (birdAudioSources == null || birdAudioSources.Length == 0)
        {
            Debug.LogWarning("Bird AudioSources are not assigned.");
            yield break;
        }

        while (true)
        {
            float delay = Random.Range(minBirdDelay, maxBirdDelay);
            yield return new WaitForSeconds(delay);
            AudioSource availableSource = birdAudioSources.FirstOrDefault(s => s != null && !s.isPlaying);
            if (availableSource != null)
            {
                AudioClip clipToPlay = birdChirps[Random.Range(0, birdChirps.Length)];
                availableSource.clip = clipToPlay;
                availableSource.Play();
            }
        }
    }

    IEnumerator PlayCarEngineSoundRoutine(AudioSource carSource)
    {
        if (carEngineSound == null)
        {
            Debug.LogWarning("Car engine sound clip is not assigned.");
            yield break;
        }
        if (carSource == null)
        {
            Debug.LogWarning("Car AudioSource is not assigned.");
            yield break;
        }

        carSource.clip = carEngineSound;
        carSource.Play();
        yield break;
    }

    void OnDestroy()
    {
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.onValueChanged.RemoveListener(SetMasterVolume);
        }
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.onValueChanged.RemoveListener(SetMusicVolume);
        }
    }
}