using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechCreate : MonoBehaviour
{

    [SerializeField]
    string[] PossibleCreateCommands;
    [SerializeField]
    public GameObject[] AllPrefabs;

    private bool FoundCreateCommand;
    private GameObject FoundPrefab;
    public float maxDistanceCreate = 6;

    void Start()
    {
        PossibleCreateCommands = new string[] { "produce", "initiate", "generate", "form", "build", "construct", "give", "create", "spawn", "make", "instantiate" };
        AllPrefabs = Resources.LoadAll<GameObject>("Prefabs");

        // Register this class to get notified, when the user entered a message
        SpeechDecoder.speechDecoder.CommandTransmitter += CreateObject;
    }

    
    void Update()
    {
        
    }

    void CreateObject(string command) {

        FoundCreateCommand = SpeechDecoder.speechDecoder.FindCommand(command, PossibleCreateCommands);
        FoundPrefab = SpeechDecoder.speechDecoder.FindObject(command, AllPrefabs);

        if (FoundCreateCommand && FoundPrefab != null) {
            SpeechDecoder.speechDecoder.CommandWasFound = true;

            RaycastHit hit = Raycaster.rayCaster.hit;
            Vector3 fwd = Raycaster.rayCaster.fwd;

            // Creates a maxDistanceCreate long Ray to get the position at its end
            Ray ray = new Ray(transform.position, fwd);
            Vector3 spawnPoint = ray.GetPoint(maxDistanceCreate);
          

            // When a wall was hit, create the object slightly in front of it
            if (Raycaster.rayCaster.hitSomething && hit.transform.CompareTag("Wall") && hit.distance <= maxDistanceCreate) {
                // Set the position for the object before the wall
                spawnPoint = hit.point - fwd;
            }

            Vector3 pointToSpawn = new Vector3(spawnPoint.x, spawnPoint.y + 3, spawnPoint.z);
            Instantiate(FoundPrefab, pointToSpawn, FoundPrefab.transform.rotation);
            }

        }
}
