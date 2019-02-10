using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SpeechDecoder : MonoBehaviour
{

    public static SpeechDecoder speechDecoder;
    // Colored indicators if the user was understood. Assigned in the inspector
    public GameObject[] AllIndicators;

    public AlexaCommandStreamer acs;
    public SpeechSandboxStreaming sss;
    public GyroController gc;

    public delegate void AllCommandsFinder(string wholeCommand);
    public AllCommandsFinder CommandTransmitter;


    public bool CommandWasFound;

    public Behaviour[] behaviours;



    void Start() {

        speechDecoder = this;
        CheckStartMenuSettings();

        StartCoroutine(RegisterRepresenterAfterSeconds(2));

        /* Experiment to deactivate all scripts of an object; Later on for the menu
        behaviours = gameObject.GetComponentsInChildren<Behaviour>();
        foreach (var item in behaviours) {
            item.enabled = false;
        }
        */
    }

    // Ensures that the command representer is at the last position of the delegate
    IEnumerator RegisterRepresenterAfterSeconds(int seconds) {
        yield return new WaitForSeconds(seconds);
        CommandTransmitter += RepresentIfCommandFound;
    }

    private void CheckStartMenuSettings() {
        // Decide which speech recognition system should be used
        if (PlayerPrefs.HasKey("RecognitionType")) {
            if (PlayerPrefs.GetString("RecognitionType") == "Watson") {
                acs.enabled = false;
                sss.enabled = true;
            } else if (PlayerPrefs.GetString("RecognitionType") == "Alexa") {
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
    }


    // Can be called by each action class to test if one of their command words were used
    public bool FindCommand(string userCommand, string[] commandCollection) {
        if (commandCollection.Any(userCommand.Contains)) {
            return true;
        }

        return false;
    }


    public GameObject FindObject(string userCommand, GameObject[] possibleObjects) {
        bool somethingFound = false;
        foreach (GameObject prefab in possibleObjects) {
            if (userCommand.Contains(prefab.name.ToLower()) && !somethingFound) {
                return prefab;
            }
        }
        return null;
    }



    public void RepresentIfCommandFound(string wholeCommand) {
        StartCoroutine(IndicateIfCommandUnderstood(CommandWasFound));
        CommandWasFound = false;
    }

    IEnumerator IndicateIfCommandUnderstood(bool understood) {
        foreach (GameObject indicator in AllIndicators) {
            indicator.SetActive(true);
        }
        Color finalColor = understood ? Color.green : Color.red;
        foreach (GameObject indicator in AllIndicators) {
            indicator.GetComponent<Image>().color = finalColor;
        }

        float fadeDuration = 0.5f;


            for (float i = 0; i <= 1; i += Time.deltaTime / fadeDuration) {
                FadeInOut(finalColor, i);
                yield return null;   
            }
       
        // Wait until fading out
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
        Color finalColor = color;
        color.a = i;
  
        foreach (GameObject indicator in AllIndicators) {
            indicator.GetComponent<Image>().color = finalColor;
        }

    }




}
