using UnityEngine;
using TMPro;
using System.Collections;

public class AndroidDeviceInfo : MonoBehaviour
{
    public TMP_Text displayText;

    private const string pacakgeName = "com.tedliou.mylibrary.DeviceInfoHelper";
    private AndroidJavaObject _instance;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(3);

        var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        _instance = new AndroidJavaObject(pacakgeName, currentActivity);
        var deviceInfo = _instance.Call<string>("getDeviceInfo");

        displayText.text = $"Get Model: {deviceInfo}";
        
        var androidToast = new AndroidJavaClass("android.widget.Toast");
        currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            var toastObject = androidToast.CallStatic<AndroidJavaObject>("makeText", currentActivity, deviceInfo, 0);
            toastObject.Call("show");
        }));
    }

    private void OnDestroy()
    {
        _instance.Dispose();
        _instance = null;        
    }
}
