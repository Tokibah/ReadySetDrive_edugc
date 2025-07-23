using UnityEngine;
using UnityEngine.UI; // Required for UI elements like Slider
using System.Collections; // Required for Coroutines
using System.Linq; // Required for LINQ operations like .Where()

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [Tooltip("AudioSource for background music.")]
    public AudioSource musicAudioSource;
    [Tooltip("AudioSource for the always-playing wave sound.")]
    public AudioSource waveAudioSource;
    [Tooltip("AudioSources for looping bird sounds. Assign all your BirdEmitter AudioSources here.")]
    public AudioSource[] birdLoopAudioSources; // Changed to an array for multiple bird emitters
    [Tooltip("AudioSource for looping wind sound.")]
    public AudioSource windLoopAudioSource;
    // Removed: [Tooltip("AudioSource for one-shot environmental sounds (like leaf swaying).")]
    // Removed: public AudioSource environmentOneShotAudioSource;
    [Tooltip("AudioSource for the always-playing noise sound (should be on Camera).")]
    public AudioSource noiseAudioSource;

    [Header("Audio Clips")]
    [Tooltip("Assign your bird chirp audio clips here (expecting 3).")]
    public AudioClip[] birdChirps; // Expecting 3 audio clips
    [Tooltip("Assign your wind sound audio clip here (expecting 1).")]
    public AudioClip windSound; // Expecting 1 audio clip
    [Tooltip("Assign your wave sound audio clip here (expecting 1).")]
    public AudioClip waveSound; // Expecting 1 audio clip
    // Removed: [Tooltip("Assign your leaf swaying audio clip here (expecting 1).")]
    // Removed: public AudioClip leafSwayingSound; // Expecting 1 audio clip
    [Tooltip("Assign your background music audio clip here (expecting 1).")]
    public AudioClip backgroundMusicClip; // Expecting 1 audio clip
    [Tooltip("Assign your noise sound audio clip here (expecting 1).")]
    public AudioClip noiseSound; // New AudioClip for noise

    [Header("Timing Settings")]
    [Tooltip("The minimum time (in seconds) between bird chirp loops.")]
    public float minBirdLoopDelay = 5f;
    [Tooltip("The maximum time (in seconds) between bird chirp loops.")]
    public float maxBirdLoopDelay = 15f;
    // Removed: [Tooltip("The minimum time (in seconds) between leaf swaying one-shots.")]
    // Removed: public float minLeafSwayingDelay = 3f;
    // Removed: [Tooltip("The maximum time (in seconds) between leaf swaying one-shots.")]
    // Removed: public float maxLeafSwayingDelay = 8f;
    [Tooltip("The minimum time (in seconds) for the gap between wind sound playback.")]
    public float minWindDelay = 10f; // New: Minimum delay for wind
    [Tooltip("The maximum time (in seconds) for the gap between wind sound playback.")]
    public float maxWindDelay = 20f; // New: Maximum delay for wind

    [Header("Volume Control Sliders")]
    [Tooltip("Assign your UI Slider for master volume control (environmental sounds).")]
    public Slider masterVolumeSlider;
    [Tooltip("Assign your UI Slider for background music volume control.")]
    public Slider musicVolumeSlider;

    [Header("Area Detection Settings")]
    [Tooltip("The tag of the player GameObject. Make sure your player has this tag.")]
    public string playerTag = "Player";
    [Tooltip("Check this if the player is currently in the 'House Area'.")]
    public bool isInHouseArea = false;
    [Tooltip("Check this if the player is currently in the 'Road Area'.")]
    public bool isInRoadArea = false;

    [Header("Noise Specific Settings")]
    [Tooltip("The base volume factor for the noise sound (0.0 to 1.0).")]
    [Range(0f, 1f)] // Restrict to 0-1 range in Inspector
    public float noiseBaseVolumeFactor = 1f;

    private Coroutine birdChirpLoopCoroutine;
    // Removed: private Coroutine leafSwayingCoroutine;
    private Coroutine windSoundLoopCoroutine; // New: Coroutine for wind sound loop
    private int currentBirdChirpClipIndex = 0; // Index for cycling through bird *clips*
    private float currentMasterVolume = 1f; // To store the current master slider value

    void Start()
    {
        // --- Initialize Audio Sources ---
        if (musicAudioSource == null) musicAudioSource = AddAudioSource("MusicSource");
        if (waveAudioSource == null) waveAudioSource = AddAudioSource("WaveSource");
        if (windLoopAudioSource == null) windLoopAudioSource = AddAudioSource("WindLoopSource");
        // Removed: if (environmentOneShotAudioSource == null) environmentOneShotAudioSource = AddAudioSource("OneShotSource");
        if (noiseAudioSource == null) noiseAudioSource = AddAudioSource("NoiseSource");

        // Validate birdLoopAudioSources array
        if (birdLoopAudioSources == null || birdLoopAudioSources.Length == 0)
        {
            Debug.LogError("No AudioSources assigned to birdLoopAudioSources array! Bird sounds will not play.");
        }
        else
        {
            foreach (AudioSource source in birdLoopAudioSources)
            {
                if (source != null) source.loop = false; // Birds will NOT loop individually, managed by script
            }
        }

        // --- Configure AudioSource Defaults ---
        windLoopAudioSource.loop = false; // Wind will NOT loop, managed by script
        waveAudioSource.loop = true;     // Waves will loop
        musicAudioSource.loop = true;    // Music will loop
        noiseAudioSource.loop = true;    // Noise will loop

        // --- Initialize Volume Sliders ---
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.minValue = 0f;
            masterVolumeSlider.maxValue = 1f;
            masterVolumeSlider.value = 1f;
            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
            currentMasterVolume = masterVolumeSlider.value;
        }
        else
        {
            Debug.LogWarning("Master Volume Slider not assigned. Master volume control will not be available via UI.");
            currentMasterVolume = 1f;
        }

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.minValue = 0f;
            musicVolumeSlider.maxValue = 1f;
            musicVolumeSlider.value = musicAudioSource.volume;
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        }
        else
        {
            Debug.LogWarning("Music Volume Slider not assigned. Music volume control will not be available via UI.");
        }

        // --- Start Core Audio Routines ---
        StartCoroutine(PlayBackgroundMusicRoutine());
        StartCoroutine(PlayWaveSoundRoutine());
        StartCoroutine(PlayNoiseSoundRoutine());
        StartCoroutine(ManageAreaDependentSounds()); // This will now manage starting/stopping wind and bird loops
        // Removed: leafSwayingCoroutine = StartCoroutine(PlayLeafSwayingRoutine());
        
        // Initial update of noise volume based on starting state
        UpdateNoiseVolume();
    }

    /// <summary>
    /// Helper to add an AudioSource if it's null.
    /// </summary>
    private AudioSource AddAudioSource(string name)
    {
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.name = name;
        Debug.LogWarning($"No {name} AudioSource assigned. Added one to {gameObject.name}");
        return newSource;
    }

    /// <summary>
    /// Sets the master volume for all environmental sounds.
    /// </summary>
    /// <param name="volume">The new volume value from the slider.</param>
    public void SetMasterVolume(float volume)
    {
        currentMasterVolume = volume;
        if (waveAudioSource != null) waveAudioSource.volume = volume;
        if (windLoopAudioSource != null) windLoopAudioSource.volume = volume;
        // Removed: if (environmentOneShotAudioSource != null) environmentOneShotAudioSource.volume = volume;
        
        // Apply volume to all bird audio sources
        if (birdLoopAudioSources != null)
        {
            foreach (AudioSource source in birdLoopAudioSources)
            {
                if (source != null) source.volume = volume;
            }
        }
        
        UpdateNoiseVolume();
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
    /// Calculates and sets the noise audio source volume based on master volume.
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
            Debug.LogWarning("No background music clip assigned to the AudioManager.");
            yield break;
        }
        musicAudioSource.clip = backgroundMusicClip;
        musicAudioSource.Play();
        yield break;
    }

    /// <summary>
    /// Coroutine to play the wave sound. It will always play and loop.
    /// </summary>
    IEnumerator PlayWaveSoundRoutine()
    {
        if (waveSound == null)
        {
            Debug.LogWarning("No wave sound clip assigned to the AudioManager.");
            yield break;
        }
        waveAudioSource.clip = waveSound;
        waveAudioSource.Play();
        yield break;
    }

    /// <summary>
    /// Coroutine to play the noise sound. It will always play and loop.
    /// </summary>
    IEnumerator PlayNoiseSoundRoutine()
    {
        if (noiseSound == null)
        {
            Debug.LogWarning("No noise sound clip assigned to the AudioManager.");
            yield break;
        }
        noiseAudioSource.clip = noiseSound;
        noiseAudioSource.Play();
        yield break;
    }

    /// <summary>
    /// Manages the playback of area-dependent looping sounds (birds, wind).
    /// </summary>
    IEnumerator ManageAreaDependentSounds()
    {
        while (true)
        {
            bool shouldPlayAreaSounds = isInHouseArea || isInRoadArea;

            // --- Manage Bird Sounds ---
            if (shouldPlayAreaSounds && birdChirps.Length > 0 && birdLoopAudioSources != null && birdLoopAudioSources.Length > 0)
            {
                if (birdChirpLoopCoroutine == null) // Start the bird cycle coroutine if not already running
                {
                    birdChirpLoopCoroutine = StartCoroutine(CycleBirdChirps());
                }
            }
            else
            {
                // Stop all bird loops if not in area or no clips/sources
                if (birdLoopAudioSources != null)
                {
                    foreach (AudioSource source in birdLoopAudioSources)
                    {
                        if (source != null && source.isPlaying)
                        {
                            source.Stop();
                        }
                    }
                }
                if (birdChirpLoopCoroutine != null)
                {
                    StopCoroutine(birdChirpLoopCoroutine);
                    birdChirpLoopCoroutine = null;
                }
            }

            // --- Manage Wind Sound ---
            if (shouldPlayAreaSounds && windSound != null)
            {
                if (windSoundLoopCoroutine == null) // Start the wind cycle coroutine if not already running
                {
                    windSoundLoopCoroutine = StartCoroutine(WindSoundLoopRoutine());
                }
            }
            else
            {
                if (windLoopAudioSource.isPlaying)
                {
                    windLoopAudioSource.Stop();
                }
                if (windSoundLoopCoroutine != null)
                {
                    StopCoroutine(windSoundLoopCoroutine);
                    windSoundLoopCoroutine = null;
                }
            }

            yield return null; // Check every frame
        }
    }

    /// <summary>
    /// Cycles through bird chirp clips with a random delay between each loop, playing on available sources.
    /// </summary>
    IEnumerator CycleBirdChirps()
    {
        while (true)
        {
            // Find an AudioSource that is not currently playing a bird chirp
            AudioSource availableSource = birdLoopAudioSources.FirstOrDefault(s => s != null && !s.isPlaying);

            if (availableSource != null)
            {
                // Play a random bird chirp clip on the available source
                AudioClip clipToPlay = birdChirps[Random.Range(0, birdChirps.Length)];
                availableSource.clip = clipToPlay;
                availableSource.Play();
            }
            // If no source is available, the chirp is skipped for this cycle.

            // Wait for a random delay before attempting the next chirp
            float delay = Random.Range(minBirdLoopDelay, maxBirdLoopDelay);
            yield return new WaitForSeconds(delay);
        }
    }

    /// <summary>
    /// Coroutine to play the wind sound with a random gap between each playback.
    /// </summary>
    IEnumerator WindSoundLoopRoutine()
    {
        if (windSound == null)
        {
            Debug.LogWarning("Wind sound clip is not assigned for WindSoundLoopRoutine.");
            yield break;
        }

        while (true)
        {
            if (!windLoopAudioSource.isPlaying) // Only play if not already playing
            {
                windLoopAudioSource.clip = windSound;
                windLoopAudioSource.Play();
            }
            
            // Wait for the clip to finish, then add the random gap
            yield return new WaitForSeconds(windSound.length);
            float delay = Random.Range(minWindDelay, maxWindDelay);
            yield return new WaitForSeconds(delay);
        }
    }

    // Removed: /// <summary>
    // Removed: /// Coroutine to play leaf swaying sounds periodically when wind is active.
    // Removed: /// </summary>
    // Removed: IEnumerator PlayLeafSwayingRoutine()
    // Removed: {
    // Removed:     while (true)
    // Removed:     {
    // Removed:         // Leaf swaying plays only if wind is actively playing (not just in the area)
    // Removed:         if (windLoopAudioSource.isPlaying && leafSwayingSound != null)
    // Removed:         {
    // Removed:             environmentOneShotAudioSource.PlayOneShot(leafSwayingSound);
    // Removed:         }
    // Removed:         // Wait for a random delay before checking again
    // Removed:         float delay = Random.Range(minLeafSwayingDelay, maxLeafSwayingDelay);
    // Removed:         yield return new WaitForSeconds(delay);
    // Removed:     }
    // Removed: }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            bool wasInHouseArea = isInHouseArea;
            
            if (gameObject.CompareTag("HouseAreaTrigger"))
            {
                isInHouseArea = true;
                Debug.Log("Player entered House Area.");
            }
            else if (gameObject.CompareTag("RoadAreaTrigger"))
            {
                isInRoadArea = true;
                Debug.Log("Player entered Road Area.");
            }

            if (wasInHouseArea != isInHouseArea)
            {
                UpdateNoiseVolume();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            bool wasInHouseArea = isInHouseArea;

            if (gameObject.CompareTag("HouseAreaTrigger"))
            {
                isInHouseArea = false;
                Debug.Log("Player exited House Area.");
            }
            else if (gameObject.CompareTag("RoadAreaTrigger"))
            {
                isInRoadArea = false;
                Debug.Log("Player exited Road Area.");
            }

            if (wasInHouseArea != isInHouseArea)
            {
                UpdateNoiseVolume();
            }
        }
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
