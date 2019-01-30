using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechCreate : MonoBehaviour
{
    
    void Start()
    {
        // Registrate this class to get notified, when the user entered a message
        SpeechDecoder.InteractionEvent.AddListener(CreateObject);
    }

    
    void Update()
    {
        
    }

    void CreateObject() {
        
            if (SpeechDecoder.FoundCreateCommand && SpeechDecoder.FoundPrefab != null) {
            Vector3 pointToSpawn = new Vector3(RayCaster.spawnPoint.x, RayCaster.spawnPoint.y + 3, RayCaster.spawnPoint.z);
            Instantiate(SpeechDecoder.FoundPrefab, pointToSpawn, Quaternion.identity);
            }

        }
}
