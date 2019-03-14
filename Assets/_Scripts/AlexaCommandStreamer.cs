using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using SimpleJSON;
using System.Linq;
using UnityEngine.Networking;

public class AlexaCommandStreamer : MonoBehaviour
{

    int counter = 1;

    IEnumerator DownloadWebService() {
        while (true) {
            //WWW w = new WWW("http://voicevr.herokuapp.com/?command");
            UnityWebRequest www = UnityWebRequest.Get("http://voicevr.herokuapp.com/?command");
            //yield return w;
            yield return www.SendWebRequest();

            //ExtractCommand(w.text)
            ExtractCommand(www.downloadHandler.text);

        }
    }

    void ExtractCommand(string json) {
        JSONNode jsonstring = JSON.Parse(json);
        string command = jsonstring["command"];
        if (command.Length == 0) { return; }
        //string[] commands_array = command.Split(" "[0]);
        //CreateObject(commands_array);
        print(command);
        SpeechDecoder.speechDecoder.CommandTransmitter(command);
      
    }

    

    void Start() {
        print("Started webservice import...\n");

        StartCoroutine(DownloadWebService());
    }


}