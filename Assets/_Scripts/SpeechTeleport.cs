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

    [SerializeField]
    string[] PossibleMoveCommands;


    private bool FoundMoveCommand;


    void Start() {
        PossibleMoveCommands = new string[] { "move", "go", "teleport", "drive" };
        // Register this class to get notified, when the user entered a message
        SpeechDecoder.CommandTransmitter += TeleportPlayer;
        
    }

    void TeleportPlayer(string command) {
        FoundMoveCommand = SpeechDecoder.FindCommand(command, PossibleMoveCommands);

        if (FoundMoveCommand /* || go */) {
            SpeechDecoder.CommandWasFound = true;
            if (RayCaster.hitBottom) {
                sentHit = RayCaster.hit;
                posToMove = new Vector3(sentHit.point.x, transform.position.y, sentHit.point.z);
                teleporting = true;
            }

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
