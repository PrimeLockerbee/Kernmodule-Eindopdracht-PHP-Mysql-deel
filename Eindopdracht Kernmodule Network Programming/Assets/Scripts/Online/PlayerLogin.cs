using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerLogin : MonoBehaviour
{
    public string userLoginURL = "https://studenthome.hku.nl/~bradley.vanewijk/user_login.php";
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;

    public ServerLogin serverlogin;

    public int numPlayers;

    public void Login()
    {
        if (serverlogin.serverLoggedIn)
        {
            string email = emailInputField.text; // Retrieve the email value from the InputField
            string password = passwordInputField.text; // Retrieve the password value from the InputField

            StartCoroutine(PlayerLoginRequest(email, password));
        }
        else
        {
            Debug.Log("Server not logged in");
        }
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
                Debug.LogError("Player login request failed. Error: " + www.error);
            }
            else
            {
                // Get the response from the server
                string response = www.downloadHandler.text;

                // Check if the response contains the username
                if (!string.IsNullOrEmpty(response) && !response.StartsWith("Invalid"))
                {
                    Debug.Log("Player Login successful");

                    // Assuming the response string contains the id, nickname, and session id separated by newlines
                    string[] lines = response.Split('\n');

                    if (lines.Length >= 3)
                    {
                        string id = lines[0];
                        string nickname = lines[1];
                        string sessionId = lines[2];

                         // Get the number of players already logged in
                         numPlayers  = PlayerPrefs.GetInt("NumPlayers", 0);

                         // Increment the number of players
                         numPlayers++;

                         // Store the variables in PlayerPrefs
                         PlayerPrefs.SetString("ID", id);
                         PlayerPrefs.SetString("Nickname", nickname);
                         PlayerPrefs.SetString("SessionID", sessionId);
                         PlayerPrefs.SetInt("NumPlayers", numPlayers);

                         // Now you can use the variables as needed
                         Debug.Log("ID: " + id);
                         Debug.Log("Nickname: " + nickname);
                         Debug.Log("Session ID: " + sessionId);

                        // Set the current player based on the number of players
                        PlayerPrefs.SetInt("CurrentPlayer", numPlayers);
                    }
                    else
                    {
                        Debug.LogError("Invalid response format");
                    }

                    // Perform any actions required upon successful player login
                    SceneManager.LoadScene(1);

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