using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class MenuController : MonoBehaviour
{
    public bool playInVR = true;

    public string sceneToLoad = "Main";

    // Call via `StartCoroutine(SwitchToVR())` from your code. Or, use
    // `yield SwitchToVR()` if calling from inside another coroutine.
    IEnumerator SwitchToVR() {
        // Device names are lowercase, as returned by `XRSettings.supportedDevices`.
        string desiredDevice = "cardboard";

        // Some VR Devices do not support reloading when already active, see
        // https://docs.unity3d.com/ScriptReference/XR.XRSettings.LoadDeviceByName.html
        if (String.Compare(XRSettings.loadedDeviceName, desiredDevice, true) != 0) {
            XRSettings.LoadDeviceByName(desiredDevice);

            // Must wait one frame after calling `XRSettings.LoadDeviceByName()`.
            yield return null;
        }

        // Now it's ok to enable VR mode.
        XRSettings.enabled = playInVR;

        // To enable or disable the touch controls
        int withTouch; // 0 == without; 1 == with
        withTouch = playInVR ? 0 : 1;
        PlayerPrefs.SetInt("WithTouch", withTouch);

        SceneManager.LoadScene(sceneToLoad);
    }

    public void PlayWithWatson() {
        PlayerPrefs.SetString("RecognitionType", "Watson");
        StartCoroutine(SwitchToVR());
    }

    public void PlayWithAlexa() {
        PlayerPrefs.SetString("RecognitionType", "Alexa");
        StartCoroutine(SwitchToVR());
    }

    public void PlayInVR(bool inVR) {
        playInVR = inVR;
    }

}
