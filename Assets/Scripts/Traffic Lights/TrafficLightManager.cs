using UnityEngine;
using System.Collections;
using System.Collections.Generic; // Required for List and Dictionary

public class TrafficLightManager : MonoBehaviour
{
    // Define the approach directions for this specific intersection
    public enum ApproachDirection { North, East, South, West }

    // Enum to represent the state of an individual light (Red, Yellow, Green)
    public enum LightState { Red, Yellow, Green }

    // A serializable class to group the GameObjects for each light (red, yellow, green)
    // for a specific approach direction. You'll set these in the Inspector.
    [System.Serializable]
    public class LightSet
    {
        public ApproachDirection direction;
        public GameObject redLightGO;
        public GameObject yellowLightGO;
        public GameObject greenLightGO;
    }

    public List<LightSet> lightSets; // List of all light sets controlled by this manager

    // A serializable class to define the state of a single light within a phase
    [System.Serializable]
    public class LightStateSetting
    {
        public ApproachDirection direction;
        public LightState state;
    }

    // A serializable class to define a complete traffic light phase (duration and what lights are active)
    [System.Serializable]
    public class TrafficPhase
    {
        public string phaseName; // For debugging/identification
        public float duration; // How long this phase lasts
        public List<LightStateSetting> lightSettings; // States for all relevant lights in this phase
    }

    [Header("Traffic Light Cycle Phases")]
    public List<TrafficPhase> trafficPhases; // The sequence of phases for this intersection

    private int currentPhaseIndex = 0;
    // Dictionary to store the current light state for each approach, for quick lookup by NPCs
    private Dictionary<ApproachDirection, LightState> currentApproachLightStates = new Dictionary<ApproachDirection, LightState>();

    void Start()
    {
        // Initialize all approach directions to Red by default
        foreach (ApproachDirection dir in System.Enum.GetValues(typeof(ApproachDirection)))
        {
            currentApproachLightStates[dir] = LightState.Red;
        }

        // Start the main traffic light cycle coroutine
        StartCoroutine(TrafficCycle());
    }

    IEnumerator TrafficCycle()
    {
        while (true)
        {
            TrafficPhase currentPhase = trafficPhases[currentPhaseIndex];
            Debug.Log($"Traffic Light Manager: Starting Phase: {currentPhase.phaseName}");

            // Apply the light settings for the current phase
            foreach (LightSet lightSet in lightSets)
            {
                // Find the specific setting for this lightSet's direction within the current phase
                LightStateSetting setting = currentPhase.lightSettings.Find(s => s.direction == lightSet.direction);

                if (setting != null)
                {
                    SetLightGOs(lightSet, setting.state);
                    currentApproachLightStates[lightSet.direction] = setting.state; // Update internal state for querying
                }
                else
                {
                    // If a direction isn't explicitly set in this phase, default its light to Red
                    SetLightGOs(lightSet, LightState.Red);
                    currentApproachLightStates[lightSet.direction] = LightState.Red;
                }
            }

            // Wait for the duration of the current phase
            yield return new WaitForSeconds(currentPhase.duration);

            // Move to the next phase in the sequence
            currentPhaseIndex = (currentPhaseIndex + 1) % trafficPhases.Count;
        }
    }

    // Helper method to activate/deactivate the appropriate light GameObjects
    void SetLightGOs(LightSet lightSet, LightState state)
    {
        // Deactivate all lights in the set first
        if (lightSet.redLightGO != null) lightSet.redLightGO.SetActive(false);
        if (lightSet.yellowLightGO != null) lightSet.yellowLightGO.SetActive(false);
        if (lightSet.greenLightGO != null) lightSet.greenLightGO.SetActive(false);

        // Activate the correct light GameObject
        switch (state)
        {
            case LightState.Red:
                if (lightSet.redLightGO != null) lightSet.redLightGO.SetActive(true);
                break;
            case LightState.Yellow:
                if (lightSet.yellowLightGO != null) lightSet.yellowLightGO.SetActive(true);
                break;
            case LightState.Green:
                if (lightSet.greenLightGO != null) lightSet.greenLightGO.SetActive(true);
                break;
        }
    }

    // Public method for NPCCarController to query the light state for a specific approach
    public LightState GetLightStateForApproach(ApproachDirection direction)
    {
        if (currentApproachLightStates.ContainsKey(direction))
        {
            return currentApproachLightStates[direction];
        }
        // Fallback: If for some reason the direction isn't tracked, assume red for safety
        Debug.LogWarning($"TrafficLightManager: Attempted to get light state for untracked direction {direction}. Defaulting to Red.");
        return LightState.Red;
    }
}