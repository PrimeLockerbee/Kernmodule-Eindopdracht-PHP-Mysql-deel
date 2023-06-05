using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class PlayerLogin : MonoBehaviour
{
    public string userLoginURL = "https://studenthome.hku.nl/~bradley.vanewijk/user_login.php";
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;

    public void Login()
    {
        string email = emailInputField.text; // Retrieve the email value from the InputField
        string password = passwordInputField.text; // Retrieve the password value from the InputField

        StartCoroutine(PlayerLoginRequest(email, password));
    }

    private IEnumerator PlayerLoginRequest(string email, string password)
    {
        // Create a form to hold the login data
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);

        // Send the POST request to the player login script
        using (UnityWebRequest www = UnityWebRequest.Post(userLoginURL, form))
        {
            yield return www.SendWebRequest();

            // Check for errors during the request
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError("Login request failed. Error: " + www.error);
            }
            else
            {
                // Get the response from the server
                string response = www.downloadHandler.text;

                // Debug the response
                //Debug.Log("Server Response: " + response);

                // Check if the response contains the username
                if (!string.IsNullOrEmpty(response) && !response.StartsWith("Invalid"))
                {
                    Debug.Log("Player Login succesfull");

                    // Debug the username
                    Debug.Log("Username + id: " + response);

                    // Perform any actions required upon successful player login
                }
                else
                {
                    Debug.Log("Player login failed");
                    // Perform any actions required upon failed player login
                }
            }
        }
    }
}