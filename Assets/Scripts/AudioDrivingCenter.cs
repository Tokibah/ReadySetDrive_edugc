using UnityEngine;
using UnityEngine.UI; // Required for UI elements like Slider
using System.Collections; // Required for Coroutines
using System.Linq; // Required for LINQ operations

public class AudioDrivingCenter : MonoBehaviour
{
    [Header("Audio Sources")]
    [Tooltip("AudioSource for the always-playing noise sound (should be on Camera).")]
    public AudioSource noiseAudioSource;
    [Tooltip("AudioSources for looping bird sounds (on BirdEmitter GameObjects). Assign all your BirdEmitter AudioSources here.")]
    public AudioSource[] birdAudioSources; // Changed to an array for multiple bird emitters
    [Tooltip("AudioSource for the first car's engine noise (on Car1 GameObject).")]
    public AudioSource car1AudioSource;
    [Tooltip("AudioSource for the second car's engine noise (on Car2 GameObject).")]
    public AudioSource car2AudioSource;
    [Tooltip("AudioSource for background music (should be on Camera or a dedicated BGM object).")]
    public AudioSource musicAudioSource;

    [Header("Audio Clips")]
    [Tooltip("Assign your noise sound audio clip here.")]
    public AudioClip noiseSound;
    [Tooltip("Assign your bird chirp audio clips here (expecting 2).")]
    public AudioClip[] birdChirps; // Changed to an array for multiple bird chirp sounds
    [Tooltip("Assign your car engine audio clip here (will be used for both cars).")]
    public AudioClip carEngineSound; // Single engine sound for both cars
    [Tooltip("Assign your background music audio clip here.")]
    public AudioClip backgroundMusicClip;

    [Header("Timing Settings")]
    [Tooltip("The minimum time (in seconds) for the gap between bird chirp playback.")]
    public float minBirdDelay = 5f;
    [Tooltip("The maximum time (in seconds) for the gap between bird chirp playback.")]
    public float maxBirdDelay = 15f;

    [Header("Volume Control")]
    [Tooltip("Assign your UI Slider for master volume control (affects noise, birds, cars).")]
    public Slider masterVolumeSlider;
    [Tooltip("Assign your UI Slider for background music volume control.")]
    public Slider musicVolumeSlider;

    [Header("Noise Specific Settings")]
    [Tooltip("The base volume factor for the noise sound (0.0 to 1.0).")]
    [Range(0f, 1f)]
    public float noiseBaseVolumeFactor = 0.05f; // Default to a faint volume

    private float currentMasterVolume = 1f; // To store the current master slider value
    private Coroutine birdSoundLoopCoroutine; // To manage bird sound timing

    void Start()
    {
        // --- Initialize Audio Sources ---
        // These will ideally be assigned in the Inspector. Add a warning if not.
        if (noiseAudioSource == null) Debug.LogWarning("Noise AudioSource not assigned!");
        if (car1AudioSource == null) Debug.LogWarning("Car 1 AudioSource not assigned!");
        if (car2AudioSource == null) Debug.LogWarning("Car 2 AudioSource not assigned!");
        if (musicAudioSource == null) Debug.LogWarning("Music AudioSource not assigned!");

        // Validate birdAudioSources array
        if (birdAudioSources == null || birdAudioSources.Length == 0)
        {
            Debug.LogError("No AudioSources assigned to birdAudioSources array! Bird sounds will not play.");
        }
        else
        {
            foreach (AudioSource source in birdAudioSources)
            {
                if (source != null) source.loop = false; // Birds will NOT loop individually, managed by coroutine
            }
        }

        // --- Configure AudioSource Defaults ---
        if (noiseAudioSource != null) noiseAudioSource.loop = true;
        if (car1AudioSource != null) car1AudioSource.loop = true; // Car engine sounds will loop
        if (car2AudioSource != null) car2AudioSource.loop = true; // Car engine sounds will loop
        if (musicAudioSource != null) musicAudioSource.loop = true;

        // --- Initialize Volume Sliders ---
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.minValue = 0f;
            masterVolumeSlider.maxValue = 1f;
            masterVolumeSlider.value = 1f; // Start master slider at full
            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
            currentMasterVolume = masterVolumeSlider.value; // Initialize stored master volume
        }
        else
        {
            Debug.LogWarning("Master Volume Slider not assigned. Master volume control will not be available via UI.");
            currentMasterVolume = 1f; // Default to full volume if no slider
        }

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.minValue = 0f;
            musicVolumeSlider.maxValue = 1f;
            musicVolumeSlider.value = 0.5f; // Default BGM volume
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        }
        else
        {
            Debug.LogWarning("Music Volume Slider not assigned. Music volume control will not be available via UI.");
        }

        // --- Start Core Audio Routines ---
        StartCoroutine(PlayNoiseSoundRoutine());
        StartCoroutine(PlayCarEngineSoundRoutine(car1AudioSource)); // Start car 1 engine
        StartCoroutine(PlayCarEngineSoundRoutine(car2AudioSource)); // Start car 2 engine
        birdSoundLoopCoroutine = StartCoroutine(BirdSoundLoopRoutine()); // Start bird sound with gaps
        StartCoroutine(PlayBackgroundMusicRoutine());

        // Initial volume update (applies to master-controlled sounds)
        SetMasterVolume(currentMasterVolume);
        // Initial BGM volume update (applies to BGM only)
        if (musicVolumeSlider != null) SetMusicVolume(musicVolumeSlider.value);
    }

    /// <summary>
    /// Sets the master volume for all sounds in this scene except background music.
    /// </summary>
    /// <param name="volume">The new volume value from the slider.</param>
    public void SetMasterVolume(float volume)
    {
        currentMasterVolume = volume; // Update the stored master volume

        // Apply volume to all relevant audio sources (excluding music)
        if (birdAudioSources != null) // Apply to all bird sources
        {
            foreach (AudioSource source in birdAudioSources)
            {
                if (source != null) source.volume = volume;
            }
        }
        if (car1AudioSource != null) car1AudioSource.volume = volume;
        if (car2AudioSource != null) car2AudioSource.volume = volume;
        
        UpdateNoiseVolume(); // Noise volume has its own factor
    }

    /// <summary>
    /// Sets the volume for background music.
    /// </summary>
    /// <param name="volume">The new volume value from the slider.</param>
    public void SetMusicVolume(float volume)
    {
        if (musicAudioSource != null)
        {
            musicAudioSource.volume = volume;
        }
    }

    /// <summary>
    /// Calculates and sets the noise audio source volume based on master volume and base factor.
    /// </summary>
    private void UpdateNoiseVolume()
    {
        if (noiseAudioSource != null)
        {
            float targetVolume = currentMasterVolume * noiseBaseVolumeFactor;
            noiseAudioSource.volume = targetVolume;
        }
    }

    /// <summary>
    /// Coroutine to play background music. It will loop the assigned music clip.
    /// </summary>
    IEnumerator PlayBackgroundMusicRoutine()
    {
        if (backgroundMusicClip == null)
        {
            Debug.LogWarning("No background music clip assigned to the AudioManagerSimpleScene.");
            yield break;
        }
        if (musicAudioSource != null)
        {
            musicAudioSource.clip = backgroundMusicClip;
            musicAudioSource.Play();
        }
        yield break; // This coroutine only needs to run once to start the loop
    }

    /// <summary>
    /// Coroutine to play the noise sound. It will always play and loop.
    /// </summary>
    IEnumerator PlayNoiseSoundRoutine()
    {
        if (noiseSound == null)
        {
            Debug.LogWarning("No noise sound clip assigned to the AudioManagerSimpleScene.");
            yield break;
        }
        if (noiseAudioSource != null)
        {
            noiseAudioSource.clip = noiseSound;
            noiseAudioSource.Play();
        }
        yield break; // This coroutine only needs to run once to start the loop
    }

    /// <summary>
    /// Coroutine to play bird chirps with random gaps, using available AudioSources.
    /// </summary>
    IEnumerator BirdSoundLoopRoutine()
    {
        if (birdChirps == null || birdChirps.Length == 0)
        {
            Debug.LogWarning("Bird chirp sound clips are not assigned for BirdSoundLoopRoutine.");
            yield break;
        }
        if (birdAudioSources == null || birdAudioSources.Length == 0)
        {
            Debug.LogWarning("Bird AudioSources are not assigned for BirdSoundLoopRoutine.");
            yield break;
        }

        while (true)
        {
            // Wait for a random delay before attempting to play the next chirp
            float delay = Random.Range(minBirdDelay, maxBirdDelay);
            yield return new WaitForSeconds(delay);

            // Find an AudioSource that is not currently playing a bird chirp
            AudioSource availableSource = birdAudioSources.FirstOrDefault(s => s != null && !s.isPlaying);

            if (availableSource != null)
            {
                // Play a random bird chirp clip on the available source
                AudioClip clipToPlay = birdChirps[Random.Range(0, birdChirps.Length)];
                availableSource.clip = clipToPlay;
                availableSource.Play();
            }
            // If no source is available, the chirp is skipped for this cycle.
        }
    }

    /// <summary>
    /// Coroutine to play a car engine sound on a given AudioSource. It will loop continuously.
    /// NOTE: For truly seamless looping, ensure the 'carEngineSound' AudioClip itself is
    /// designed to loop perfectly (e.g., no clicks or pops at the loop point).
    /// </summary>
    IEnumerator PlayCarEngineSoundRoutine(AudioSource carSource)
    {
        if (carEngineSound == null)
        {
            Debug.LogWarning("Car engine sound clip is not assigned for car engine routine.");
            yield break;
        }
        if (carSource == null)
        {
            Debug.LogWarning("Car AudioSource is not assigned for car engine routine.");
            yield break;
        }

        carSource.clip = carEngineSound;
        carSource.Play();
        // Since carSource.loop is true, it will loop automatically.
        yield break; // This coroutine only needs to run once to start the loop
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
