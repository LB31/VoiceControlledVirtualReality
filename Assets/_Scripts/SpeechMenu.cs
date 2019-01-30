using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechMenu : MonoBehaviour
{
    
    void Start()
    {
        // Registrate this class to get notified, when the user entered a message
        SpeechDecoder.InteractionEvent.AddListener(ToggleMenu);
    }

    
    void Update()
    {
        
    }

    void ToggleMenu() {
        /* TODO:
         * Open or close menu according to its current state
         * Stop the rest of the game expect the head rotation
         * Make the background behind the canvas black and white
            */
    }


}
