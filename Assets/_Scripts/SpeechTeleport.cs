using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpeechTeleport : MonoBehaviour
{

    // For testing with the inspector
    public bool go;

    private bool teleporting;

    private RaycastHit sentHit;
    private Vector3 posToMove;

    public float teleportSpeed = 50;
    public float maxDistanceBottom = 15;

    [SerializeField]
    string[] PossibleMoveCommands;


    private bool FoundMoveCommand;

    private bool hitBottom;
    

    void Start() {
        PossibleMoveCommands = new string[] { "move", "go", "teleport", "drive" };
        // Register this class to get notified, when the user entered a message
        SpeechDecoder.speechDecoder.CommandTransmitter += TeleportPlayer;

    }

    void TeleportPlayer(string command) {
        FoundMoveCommand = SpeechDecoder.speechDecoder.FindCommand(command, PossibleMoveCommands);
        
        if (FoundMoveCommand && Raycaster.rayCaster.hitSomething) {

            sentHit = Raycaster.rayCaster.hit;
            hitBottom = sentHit.transform.CompareTag("Bottom") && sentHit.distance <= maxDistanceBottom;
            if (hitBottom && sentHit.transform.GetComponent<InteractableOnPoint>() != null) {
                SpeechDecoder.speechDecoder.CommandWasFound = true;
                posToMove = new Vector3(sentHit.point.x, transform.position.y, sentHit.point.z);
                teleporting = true;
            }
            hitBottom = false;

        }

    }

    private void Update() {
        if (teleporting) {
            Vector3 maxDistanceDelta = Vector3.MoveTowards(transform.position, posToMove, Time.deltaTime * teleportSpeed);
            transform.position = maxDistanceDelta;
            if (maxDistanceDelta == posToMove) {
                teleporting = false;

            }
        }
    }


    }
