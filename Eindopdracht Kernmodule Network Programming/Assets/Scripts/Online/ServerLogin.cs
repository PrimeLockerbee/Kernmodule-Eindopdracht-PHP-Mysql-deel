using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class ServerLogin : MonoBehaviour
{
    public string serverLoginURL = "https://studenthome.hku.nl/bradley.vanewijk/server_login.php";

    public TextMeshProUGUI serverStatus;

    public PlayerLogin playerlogin;

    public bool serverLoggedIn = false;

    private void Start()
    {
        Login();
    }

    public void Login()
    {
        string serverID = "1"; // Retrieve the server ID value from the InputField
        string password = "qwerty123"; // Retrieve the password value from the InputField

        StartCoroutine(ServerLoginRequest(serverID, password));
    }

    private IEnumerator ServerLoginRequest(string serverID, string password)
    {
        // Create a form to hold the login data
        WWWForm form = new WWWForm();
        form.AddField("server_id", serverID);
        form.AddField("password", password);

        // Send the POST request to the server login script
        using (UnityWebRequest www = UnityWebRequest.Post(serverLoginURL, form))
        {
            yield return www.SendWebRequest();

            // Check for errors during the request
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError("Login request failed. Error: " + www.error);
            }
            else
            {
                // Get the response headers
                Dictionary<string, string> headers = www.GetResponseHeaders();

                // Check if the 'Session-ID' header exists
                if (headers.ContainsKey("Session-ID"))
                {
                    // Retrieve the session ID from the response headers
                    string sessionId = headers["Session-ID"];

                    Debug.Log("Session ID: " + sessionId);

                    // Perform any actions required upon successful server login
                    serverLoggedIn = true;

                    serverStatus.text = "Online";
                }
                else if (www.downloadHandler.text == "0")
                {
                    Debug.Log("Server login failed: Invalid server ID or password");
                }
                else
                {
                    Debug.Log("Server login failed: No matching server ID");
                }
            }
        }
    }
}