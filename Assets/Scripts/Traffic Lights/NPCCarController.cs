using UnityEngine;
using System.Collections.Generic;

public class NPCCarController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float turnSpeed = 5f;
    public float stoppingDistance = 6f;
    public float nodeReachedThreshold = 0.8f;

    [Header("Pathfinding")]
    public NodeController startNode;

    [Header("Detection Settings")]
    public string playerTag = "Player"; // Tag of your player car GameObject
    public float playerDetectionRadius = 10f;

    [Tooltip("Distance at which the car will detect and stop for other NPC cars in front.")]
    public float carDetectionDistance = 8f;
    [Tooltip("Layer where other NPC cars are placed.")]
    public LayerMask npcCarLayer;
    [Tooltip("Tag for other NPC car GameObjects.")] // New: Tag for other NPCs
    public string npcTag = "NPC"; // New: Tag for other NPCs

    private bool playerIsNear = false;
    private bool otherNpcIsAhead = false;
    private Rigidbody rb;

    private List<NodeController> currentPath = new List<NodeController>();
    private int currentNodeIndex = 0;
    private bool isStopped = false;
    private float stopTimer = 0f;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("NPCCarController requires a Rigidbody component!", this);
            enabled = false;
        }
    }

    void Start()
    {
        GenerateSimplePathFromStartNode();
    }

    void Update()
    {
        if (currentPath.Count == 0 || currentNodeIndex >= currentPath.Count)
        {
            GenerateSimplePathFromStartNode();
            if (currentPath.Count == 0) return;
        }

        NodeController targetNode = currentPath[currentNodeIndex];
        Vector3 directionToNode = (targetNode.transform.position - transform.position).normalized;
        float distanceToNode = Vector3.Distance(transform.position, targetNode.transform.position);

        // --- Update Detection Flags ---
        otherNpcIsAhead = DetectCarInFront();

        // --- Determine Final Car Stop State ---
        bool shouldStopForExternalReason = playerIsNear || otherNpcIsAhead;

        if (shouldStopForExternalReason)
        {
            isStopped = true;
            stopTimer = 0f;
        }
        else
        {
            if (distanceToNode <= stoppingDistance)
            {
                switch (targetNode.tag)
                {
                    case "StopNode":
                        HandleStopNode(targetNode);
                        break;
                    case "TrafficLightNode":
                        HandleTrafficLightNode(targetNode);
                        break;
                    case "YieldNode":
                        HandleYieldNode(targetNode);
                        break;
                    default:
                        isStopped = false;
                        stopTimer = 0f;
                        break;
                }
            }
            else
            {
                if (isStopped && stopTimer <= 0)
                {
                    isStopped = false;
                }
                else if (!isStopped)
                {
                    isStopped = false;
                }
            }
        }

        // --- Movement Execution ---
        if (!isStopped)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToNode);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            rb.linearVelocity = transform.forward * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, Time.deltaTime * 5f);
            if (stopTimer > 0)
            {
                stopTimer -= Time.deltaTime;
            }
        }

        if (targetNode.CompareTag("Waypoint") && distanceToNode <= nodeReachedThreshold && !isStopped)
        {
            MoveToNextNode();
        }
    }

    void OnTriggerStay(Collider other)
    {
        // This method specifically handles the Player tag
        if (other.CompareTag(playerTag))
        {
            float dist = Vector3.Distance(transform.position, other.transform.position);
            if (dist <= playerDetectionRadius)
            {
                playerIsNear = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerIsNear = false;
        }
    }

    // Detects other NPC cars in front using Raycast AND Tag Check
    private bool DetectCarInFront()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        Vector3 rayDirection = transform.forward;

        Debug.DrawRay(rayOrigin, rayDirection * carDetectionDistance, Color.yellow);

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, carDetectionDistance, npcCarLayer))
        {
            // Ensure the hit object is not THIS car itself
            // AND check if it has the specified NPC tag
            if (hit.collider.gameObject != gameObject && hit.collider.CompareTag(npcTag)) // Added CompareTag
            {
                return true;
            }
        }
        return false;
    }

    // Helper Methods (no changes to these based on this request)
    void MoveToNextNode()
    {
        currentNodeIndex++;
        if (currentNodeIndex < currentPath.Count)
        {
            isStopped = false;
            stopTimer = 0f;
        }
    }

    void HandleStopNode(NodeController node)
    {
        if (!isStopped && stopTimer <= 0)
        {
            isStopped = true;
            stopTimer = node.stopDuration;
            Debug.Log($"NPC {gameObject.name} stopping at Stop Node for {node.stopDuration} seconds.");
        }
        if (isStopped && stopTimer <= 0 && !playerIsNear && !otherNpcIsAhead)
        {
            MoveToNextNode();
            isStopped = false;
            Debug.Log($"NPC {gameObject.name} proceeding from Stop Node.");
        }
    }

    void HandleTrafficLightNode(NodeController node)
    {
        if (node.trafficLightManager == null)
        {
            Debug.LogWarning($"TrafficLightNode '{node.name}' has no TrafficLightManager assigned! NPC {gameObject.name} will treat it as a normal waypoint.", node);
            if (Vector3.Distance(transform.position, node.transform.position) <= nodeReachedThreshold)
            {
                MoveToNextNode();
            }
            return;
        }

        TrafficLightManager.LightState currentLightState = node.trafficLightManager.GetLightStateForApproach(node.approachDirection);

        if (currentLightState == TrafficLightManager.LightState.Red || currentLightState == TrafficLightManager.LightState.Yellow)
        {
            if (!isStopped || (isStopped && currentLightState == TrafficLightManager.LightState.Red))
            {
                isStopped = true;
                Debug.Log($"NPC {gameObject.name} stopping at {currentLightState} light.");
            }
        }
        else if (currentLightState == TrafficLightManager.LightState.Green)
        {
            if (isStopped && !playerIsNear && !otherNpcIsAhead)
            {
                isStopped = false;
                Debug.Log($"NPC {gameObject.name} proceeding on Green light.");
            }
            if (Vector3.Distance(transform.position, node.transform.position) <= nodeReachedThreshold && !isStopped)
            {
                MoveToNextNode();
            }
        }
    }

    void HandleYieldNode(NodeController node)
    {
        float currentMagnitude = rb.linearVelocity.magnitude;
        if (currentMagnitude > (moveSpeed * 0.5f))
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, transform.forward * (moveSpeed * 0.5f), Time.deltaTime * 3f);
        }

        if (!isStopped && stopTimer <= 0)
        {
            isStopped = true;
            stopTimer = 0.5f;
            Debug.Log($"NPC {gameObject.name} yielding.");
        }
        else if (stopTimer <= 0 && !playerIsNear && !otherNpcIsAhead)
        {
            isStopped = false;
            MoveToNextNode();
            Debug.Log($"NPC {gameObject.name} proceeding from Yield Node.");
        }
    }

    void GenerateSimplePathFromStartNode()
    {
        currentPath.Clear();
        currentNodeIndex = 0;

        if (startNode == null)
        {
            Debug.LogError($"NPC {gameObject.name}: Start Node is not assigned! Cannot generate path.", this);
            return;
        }

        NodeController currentNode = startNode;
        for (int i = 0; i < 50; i++)
        {
            if (currentNode == null) break;
            currentPath.Add(currentNode);

            if (currentNode.nextNodes != null && currentNode.nextNodes.Count > 0)
            {
                currentNode = currentNode.nextNodes[Random.Range(0, currentNode.nextNodes.Count)];
            }
            else
            {
                break;
            }
        }

        if (currentPath.Count == 0)
        {
            Debug.LogWarning($"NPC {gameObject.name}: Could not generate a path from start node {startNode.name}. Check node connections or assign a different start node.", this);
        }
        else
        {
            Debug.Log($"NPC {gameObject.name} generated a path with {currentPath.Count} nodes, starting at {startNode.name}.");
        }
    }

    void OnDrawGizmos()
    {
        // ... (Existing Path and Player/Stopping Gizmos) ...

        // Draw the car detection ray
        Gizmos.color = Color.yellow;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        Gizmos.DrawLine(rayOrigin, rayOrigin + transform.forward * carDetectionDistance);
    }
}