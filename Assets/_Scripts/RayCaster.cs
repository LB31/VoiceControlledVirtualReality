using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RayCaster : MonoBehaviour
{
    [Tooltip("Represents the position of the player's eyes. Typically the ReticlePointer GameObject")]
    [SerializeField]
    private Transform RayOrigin;

    // Triggers the ReticlePointer to grow, when hit
    private EventTrigger et;

    // Static to allow interaction classes to get information of the hit objects
    public static RaycastHit hit;
    public static bool hitBottom;
    public static bool hitItem;
    public static bool hitWall;
    public static Vector3 spawnPoint;

    void FixedUpdate() {
        Vector3 fwd = RayOrigin.TransformDirection(Vector3.forward);
        
        int maxDistanceBottom = 15; // for teleportation
        int maxDistanceItem = 2; // for interaction with items
        int maxDistanceCreate = 6; // for creating items

        // Creates a maxDistanceCreate long Ray to get the position at its end
        Ray ray = new Ray(transform.position, fwd);
        spawnPoint = ray.GetPoint(maxDistanceCreate);

        Debug.DrawRay(transform.position, fwd * maxDistanceBottom, Color.red);
        if (Physics.Raycast(RayOrigin.position, fwd, out hit, maxDistanceBottom)) {
            // When a wall was hit, create the object slightly in front of it
            hitWall = hit.transform.CompareTag("Wall");
            if (hitWall) {
                // Set the position for the object before the wall
                spawnPoint = hit.point - fwd;

            }
            if (hit.transform.GetComponent<InteractableOnPoint>() != null) { // If object is marked as interactable

                // Checks if specific interaction distances are met
                hitBottom = hit.transform.CompareTag("Bottom") && hit.distance <= maxDistanceBottom;
                hitItem = hit.transform.CompareTag("Item") && hit.distance <= maxDistanceItem;
                
                // Expands the ReticlePointer
                if (hitBottom || hitItem) {
                    et = hit.transform.GetComponent<InteractableOnPoint>();
                    et.enabled = true;        
                }
            }

        } else {
            // It's important to reset the bools if something specific was hit
            hitBottom = false;
            hitItem = false;
            if (et != null)
                et.enabled = false;
        }
    }
}
