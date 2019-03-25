using UnityEngine;
using System.Collections;
using SimpleJSON;
using UnityEngine.Networking;
using System;

public class AlexaCommandStreamer : MonoBehaviour
{

    int counter = 1;

    IEnumerator DownloadWebService() {
        while (true) {

            //WWW w = new WWW("http://voicevr.herokuapp.com/?command");
            //yield return w;
            //ExtractCommand(w.text);

            UnityWebRequest www = UnityWebRequest.Get("https://voicevr.herokuapp.com/?command");
            yield return www.SendWebRequest();
            ExtractCommand(www.downloadHandler.text);

        }
    }

    void ExtractCommand(string json) {
        //JSONNode jsonstring = JSON.Parse(json);
        //string command = jsonstring["command"];
        string command = JsonUtility.FromJson<CommandClass>(json).command;
        if (command.Length == 0) { return; }


        print(command);
        SpeechDecoder.speechDecoder.CommandTransmitter(command);
       
    }

    

    void Start() {
        print("Started webservice import...\n");

        StartCoroutine(DownloadWebService());
    }


}

[Serializable]
public class CommandClass
{
    public string command;
}