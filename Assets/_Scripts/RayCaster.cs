using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Raycaster : MonoBehaviour
{

    public static Raycaster rayCaster;

    [Tooltip("Represents the position of the player's eyes. Typically the ReticlePointer GameObject")]
    [SerializeField]
    private Transform RayOrigin;


    private float MaximumRayDistance = 15;

    // Allow interaction classes to get information of this fields
    public RaycastHit hit;
    public bool hitSomething;
    public Vector3 fwd;

    private void Awake() {
            rayCaster = this;
    }

    void FixedUpdate() {
        fwd = RayOrigin.TransformDirection(Vector3.forward);
        
        Debug.DrawRay(RayOrigin.position, fwd * MaximumRayDistance, Color.red);

        if (Physics.Raycast(RayOrigin.position, fwd, out hit, MaximumRayDistance)) {
            hitSomething = true;
        } else {
            hitSomething = false;
        }
    }
}
