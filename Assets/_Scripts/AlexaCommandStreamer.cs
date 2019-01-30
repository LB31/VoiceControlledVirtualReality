using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using SimpleJSON;
using System.Linq;

public class AlexaCommandStreamer : MonoBehaviour
{
    public SpeechDecoder speechDecoder;

    int counter = 1;

    IEnumerator DownloadWebService() {
        while (true) {
            WWW w = new WWW("http://voicevr.herokuapp.com/?command");
            yield return w;
            
            ExtractCommand(w.text);

        }
    }

    void ExtractCommand(string json) {
        JSONNode jsonstring = JSON.Parse(json);
        string command = jsonstring["command"];
        if (command.Length == 0) { return; }
        //string[] commands_array = command.Split(" "[0]);
        //CreateObject(commands_array);
        print(command);
        speechDecoder.CommandsFinder(command);
      
    }

    

    void CreateObject(string[] commands) {
        string shape = "";
        string name = "NewObject_" + counter;
        counter += 1;
        GameObject NewObject = new GameObject(name);
        string[] possibleShapes = { "cube", "sphere", "cylinder", "capsule" };
        IEnumerable listCommon = commands.Intersect(possibleShapes);
        foreach (string s in listCommon) shape = s;

        switch (shape) {
            case "cube":
                NewObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                break;
            case "sphere":
                NewObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                break;
            case "cylinder":
                NewObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                break;
            case "capsule":
                NewObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                break;
            default:
                break;
        }
        NewObject.transform.position = new Vector3(0, 5, 0);
        NewObject.AddComponent<Rigidbody>();
        //switch (color) {
        //    case "red":
        //        NewObject.GetComponent<Renderer>().material.color = Color.red;
        //        break;
        //    case "yellow":
        //        NewObject.GetComponent<Renderer>().material.color = Color.yellow;
        //        break;
        //    case "green":
        //        NewObject.GetComponent<Renderer>().material.color = Color.green;
        //        break;
        //    case "blue":
        //        NewObject.GetComponent<Renderer>().material.color = Color.blue;
        //        break;
        //    case "black":
        //        NewObject.GetComponent<Renderer>().material.color = Color.black;
        //        break;
        //    case "white":
        //        NewObject.GetComponent<Renderer>().material.color = Color.white;
        //        break;
        //}
    }

    // Use this for initialization
    void Start() {
        print("Started webservice import...\n");

        StartCoroutine(DownloadWebService());
    }


}