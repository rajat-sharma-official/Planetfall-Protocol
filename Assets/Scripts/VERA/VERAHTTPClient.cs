using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class VERAHTTPClient : MonoBehaviour
{
    // YES, hardcoding the IP is OK for this project
    private string serverUrl = "http://129.146.43.236:8000/vera";

    // Anyone (UI, Menu, etc.) can listen for VERA’s response
    public event Action<string> OnResponse;

    public void SendToVera(string jsonPayload)
    {
        StartCoroutine(PostRequest(jsonPayload));
    }

    private IEnumerator PostRequest(string jsonPayload)
    {
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);

        UnityWebRequest request = new UnityWebRequest(serverUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("VERA request failed: " + request.error);
        }
        else
        {
            string response = request.downloadHandler.text;
            OnResponse?.Invoke(response);
        }
    }
}
