using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpeechTeleport : MonoBehaviour
{

    public bool go;

    private bool teleporting;

    private RaycastHit sentHit;
    private Vector3 posToMove;

    public float teleportSpeed = 50;


    void Start() {
        // Registrate this class to get notified, when the user entered a message
        SpeechDecoder.InteractionEvent.AddListener(TeleportPlayer);
        
    }

    void TeleportPlayer() {

        if (SpeechDecoder.FoundMoveCommand /* || go */) {

            

            if (RayCaster.hitBottom) {

                sentHit = RayCaster.hit;
                posToMove = new Vector3(sentHit.point.x, transform.position.y, sentHit.point.z);
                teleporting = true;

                // Debug
                //GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                //sphere.transform.position = sentHit.point;
            }
        }
    }

    private void Update() {
        // Debug
        //if (go) {
        //    TeleportPlayer();
        //    go = false;
        //}
        if (teleporting) {
            Vector3 maxDistanceDelta = Vector3.MoveTowards(transform.position, posToMove, Time.deltaTime * teleportSpeed);
            transform.position = maxDistanceDelta;
            if (maxDistanceDelta == posToMove) {
                teleporting = false;

            }
        }
    }





    }
