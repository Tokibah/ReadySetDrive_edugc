using UnityEngine;
using System.Collections.Generic;

public class NodeController : MonoBehaviour
{
    // The next possible nodes the car can go to from this node.
    // Drag and drop the next Node GameObjects here in the Inspector.
    public List<NodeController> nextNodes;

    [Header("Node Behavior Settings")]
    public float stopDuration = 2f; // For StopNode: How long the car should stop.

    // For TrafficLightNode: Reference to the central TrafficLightManager for this intersection.
    // Drag the Traffic Light Manager GameObject (with TrafficLightManager script) here.
    public TrafficLightManager trafficLightManager;
    // For TrafficLightNode: The direction this node's approach corresponds to (e.g., North, South).
    public TrafficLightManager.ApproachDirection approachDirection;

    // Add any other specific data needed for your node types here (e.g., speed limit for a zone).

    // --- Editor-only Visuals for Debugging ---
    void OnDrawGizmos()
    {
        // Draw a sphere at the node's position to make it visible in the editor.
        Gizmos.color = Color.cyan; // Light blue color for nodes
        Gizmos.DrawSphere(transform.position, 0.5f); // Adjust size as needed

        // Draw lines to next nodes to visualize the path connections.
        if (nextNodes != null)
        {
            Gizmos.color = Color.blue; // Blue color for connections
            foreach (NodeController nextNode in nextNodes)
            {
                if (nextNode != null)
                {
                    Gizmos.DrawLine(transform.position, nextNode.transform.position);
                }
            }
        }
    }
}