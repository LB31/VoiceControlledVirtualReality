using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SpeechDecoder : MonoBehaviour
{

    public GameObject[] AllIndicators;
    // TODO: Source this out into a game manager
    public AlexaCommandStreamer acs;
    public SpeechSandboxStreaming sss;
    public GyroController gc;

    public delegate void AllCommandsFinder(string wholeCommand);
    public AllCommandsFinder CommandsFinder;

    public static UnityEvent InteractionEvent = new UnityEvent();

    [SerializeField]
    public GameObject[] AllPrefabs;
    //[SerializeField]
    //string[] PossiblePrefabs = { "CubeMonster", "cube", "sphere", "cylinder", "capsule" }; // more to come
    [SerializeField]
    string[] PossibleCreateCommands;
    [SerializeField]
    string[] PossibleMoveCommands;
    [SerializeField]
    string[] PossibleMenuCommands;

    // Markers for what was found
    public static GameObject FoundPrefab;
    public static bool FoundCreateCommand;
    public static bool FoundMoveCommand;
    public static bool FoundMenuCommand;


    void Start() {
            // TODO: Source this out into a game manager
            // Decide which speech recognition system should be used
            if (PlayerPrefs.HasKey("RecognitionType")) {
            if (PlayerPrefs.GetString("RecognitionType") == "Watson") {
                acs.enabled = false;
                sss.enabled = true;
            }
            else if (PlayerPrefs.GetString("RecognitionType") == "Alexa") {
                acs.enabled = true;
                sss.enabled = false;
            } 
        } else {
            // Sets Watson by default when nothing was selected
            acs.enabled = false;
            sss.enabled = true;
        }
        // Check if the control by touch should be activated
        if (PlayerPrefs.GetInt("WithTouch") == 1)
            gc.enabled = true;


        PossibleCreateCommands = new string[] { "produce", "initiate", "generate", "form", "build", "construct", "give", "create", "spawn", "make", "instantiate"};
        PossibleMoveCommands = new string[] { "move", "go", "teleport", "drive" };
        PossibleMenuCommands = new string[] { "menu", "pause", "stop", "help" };

        AllPrefabs = Resources.LoadAll<GameObject>("Prefabs");

        // Registers all finding methods to the delegate
        CommandsFinder += FindCreateCommand;
        CommandsFinder += FindPrefab;
        CommandsFinder += FindMoveCommand;
        CommandsFinder += FindMenuCommand;
        // After all finding methods were called
        CommandsFinder += SelectAction;

    }

    void FindCreateCommand(string userCommand) {
        if (PossibleCreateCommands.Any(userCommand.Contains)) {
            FoundCreateCommand = true;
        } else {
            FoundCreateCommand = false;
        }
    }

    void FindMoveCommand(string userCommand) {
        if (PossibleMoveCommands.Any(userCommand.Contains)) {
            FoundMoveCommand = true;
        } else {
            FoundMoveCommand = false;
        }
    }

    void FindMenuCommand(string userCommand) {
        if (PossibleMenuCommands.Any(userCommand.Contains)) {
            FoundMenuCommand = true;
        } else {
            FoundMenuCommand = false;
        }
    }

    void FindPrefab(string userCommand) {
        bool somethingFound = false;
        foreach (GameObject prefab in AllPrefabs) {
            if (userCommand.Contains(prefab.name.ToLower()) && !somethingFound) {
                FoundPrefab = prefab;
                somethingFound = true;
            }
        }
        // If nothing was found, remove the value from the last command
        if (!somethingFound) {
            FoundPrefab = null;
        }
    }



    public void SelectAction(string wholeCommand) {
        print(wholeCommand);
        //if (FoundCreateCommand && FoundPrefab != null) {
        //    Instantiate(FoundPrefab);
        //    // TODO extract the interaction in a new class
        //}

        if(InteractionEvent != null)
        InteractionEvent.Invoke();

        bool commandFound = false;
        if(FoundCreateCommand && FoundPrefab != null || FoundMoveCommand && RayCaster.hitBottom || FoundMenuCommand) {
            commandFound = true;
        }
        StartCoroutine(IndicateIfCommandUnderstood(commandFound));
    }

    IEnumerator IndicateIfCommandUnderstood(bool understood) {
        foreach (GameObject indicator in AllIndicators) {
            indicator.SetActive(true);
        }
        Color finalColor = understood ? Color.green : Color.red;
        float fadeDuration = 0.5f;
        if (understood) {

            for (float i = 0; i <= 1; i += Time.deltaTime / fadeDuration) {
                FadeInOut(finalColor, i);
                yield return null;   
            }
        } else {
            for (float i = 0; i <= 1; i += Time.deltaTime / fadeDuration) {
                FadeInOut(finalColor, i);
                yield return null;
            }
        }
        // How long to wait until fading out
        yield return new WaitForSeconds(fadeDuration);
        for (float i = 1; i >= 0; i -= Time.deltaTime / fadeDuration) {
            FadeInOut(finalColor, i);
            yield return null;
        }

        foreach (GameObject indicator in AllIndicators) {
            indicator.SetActive(false);
        }

    }

    void FadeInOut(Color color, float i) {
        //int fadeDirection = fadeIn ? 1 : -1;
        foreach (GameObject indicator in AllIndicators) {
            indicator.GetComponent<Image>().color = color;
        }

            Color finalColor;
            if (color == Color.red) finalColor = new Color(1, 0, 0, i);
            else finalColor = new Color(0, 1, 0, i);
            AllIndicators[0].GetComponent<Image>().color = finalColor;
            AllIndicators[1].GetComponent<Image>().color = finalColor;
        
    }




}
