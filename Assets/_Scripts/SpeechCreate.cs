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

    void Start()
    {
        PossibleCreateCommands = new string[] { "produce", "initiate", "generate", "form", "build", "construct", "give", "create", "spawn", "make", "instantiate" };
        AllPrefabs = Resources.LoadAll<GameObject>("Prefabs");

        // Register this class to get notified, when the user entered a message
        SpeechDecoder.CommandTransmitter += CreateObject;
    }

    
    void Update()
    {
        
    }

    void CreateObject(string command) {

        FoundCreateCommand = SpeechDecoder.FindCommand(command, PossibleCreateCommands);
        FoundPrefab = SpeechDecoder.FindPrefab(command, AllPrefabs);

        if (FoundCreateCommand && FoundPrefab != null) {
            SpeechDecoder.CommandWasFound = true;
            Vector3 pointToSpawn = new Vector3(RayCaster.spawnPoint.x, RayCaster.spawnPoint.y + 3, RayCaster.spawnPoint.z);
            Instantiate(FoundPrefab, pointToSpawn, Quaternion.identity);
            }

        }
}
