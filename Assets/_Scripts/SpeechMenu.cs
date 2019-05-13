using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechMenu : MonoBehaviour
{

    [SerializeField]
    GameObject MenuObject;

    [SerializeField]
    string[] PossibleMenuCommands;

    private bool FoundMenuCommand;

    void Start()
    {
        MenuObject.SetActive(false);
        PossibleMenuCommands = new string[] { "menu", "help", "pause", "stop" };
        // Registrate this class to get notified, when the user entered a message
        SpeechDecoder.speechDecoder.CommandTransmitter += ToggleMenu;
    }

    
    void Update()
    {
        
    }

    void ToggleMenu(string command) {
        FoundMenuCommand = SpeechDecoder.speechDecoder.FindCommand(command, PossibleMenuCommands);
        /* TODO:
         * Open or close menu according to its current state
         * Stop the rest of the game expect the head rotation
         * Make the background behind the canvas black and white
            */
        if (FoundMenuCommand) {
            SpeechDecoder.speechDecoder.CommandWasFound = true;
            MenuObject.SetActive(true);
        }
    }


}
