using UnityEngine;

/// <summary>
/// DestroyOnTrigger.cs
/// This script destroys a GameObject when another GameObject enters its trigger collider.
/// It can be configured to destroy the entering object or the trigger object itself.
/// </summary>
[RequireComponent(typeof(Collider))] // Ensures there's always a Collider component
public class DestroyOnTrigger : MonoBehaviour
{
    [Header("Destruction Settings")]
    [Tooltip("If true, the object that enters the trigger will be destroyed.")]
    public bool destroyEnteringObject = true;

    [Tooltip("If true, this GameObject (the one with this script) will be destroyed.")]
    public bool destroySelf = false;

    void Start()
    {
        // Ensure the collider is set to 'Is Trigger'
        Collider col = GetComponent<Collider>();
        if (col != null && !col.isTrigger)
        {
            Debug.LogWarning($"Collider on {gameObject.name} is not set to 'Is Trigger'. " +
                             "This script requires an 'Is Trigger' collider to function correctly.", this);
            // It's better practice for the user to set it in the Inspector.
        }

        // Basic validation
        if (!destroyEnteringObject && !destroySelf)
        {
            Debug.LogWarning("DestroyOnTrigger: Neither 'Destroy Entering Object' nor 'Destroy Self' is checked. " +
                             "This script will not destroy anything.", this);
            enabled = false; // Disable if no destruction action is specified
        }
    }

    /// <summary>
    /// Called when another collider enters this trigger collider.
    /// </summary>
    /// <param name="other">The Collider that entered the trigger.</param>
    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger entered by: {other.name}", other.gameObject);

        // Destroy the entering object if configured
        if (destroyEnteringObject)
        {
            Debug.Log($"Destroying entering object: {other.gameObject.name}", other.gameObject);
            Destroy(other.gameObject);
        }

        // Destroy this object (the trigger) if configured
        if (destroySelf)
        {
            Debug.Log($"Destroying self: {gameObject.name}", this);
            Destroy(gameObject);
        }
    }
}
