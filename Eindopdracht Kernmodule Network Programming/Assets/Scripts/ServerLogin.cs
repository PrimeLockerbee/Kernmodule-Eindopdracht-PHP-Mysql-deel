using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class ServerLogin : MonoBehaviour
{
    public string serverLoginURL = "https://studentdav.hku.nl/webdav/public_html/server_login.php";
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;

    public void Login()
    {
        string email = emailInputField.text; // Retrieve the email value from the InputField
        string password = passwordInputField.text; // Retrieve the password value from the InputField

        StartCoroutine(ServerLoginRequest(email, password));
    }

    private IEnumerator ServerLoginRequest(string email, string password)
    {
        // Create a form to hold the login data
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);

        // Send the POST request to the server login script
        using (UnityWebRequest www = UnityWebRequest.Post(serverLoginURL, form))
        {
            // Add the authorization header
            string usernameserver = "bradley.vanewijk@student.hku.nl";
            string passwordserver = "QuantumWardutch1997!#@";
            string credentials = System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(usernameserver + ":" + passwordserver));
            www.SetRequestHeader("Authorization", "Basic " + credentials);

            yield return www.SendWebRequest();

            // Check for errors during the request
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError("Login request failed. Error: " + www.error);
            }
            else
            {
                // Print the response from the server
                Debug.Log("Server response: " + www.downloadHandler.text);

                // Check the response from the server
                if (www.downloadHandler.text.Contains("Server login successful"))
                {
                    Debug.Log("Server login successful");
                    // Perform any actions required upon successful server login
                }
                else
                {
                    Debug.Log("Server login failed");
                    // Perform any actions required upon failed server login
                }
            }
        }
    }
}
