using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerRegister : MonoBehaviour
{
    public string userRegisterURL = "https://studenthome.hku.nl/~bradley.vanewijk/register.php";
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public TMP_InputField nickNameInputField;

    public GameObject succesText;
    public GameObject failText;

    public ServerLogin serverlogin;
    public PlayerLogin playerlogin;

    public void Register()
    {
        string email = emailInputField.text; // Retrieve the email value from the InputField
        string password = passwordInputField.text; // Retrieve the password value from the InputField
        string nickname = nickNameInputField.text; // Retrieve the password value from the InputField

        StartCoroutine(PlayerRegisterRequest(email, password, nickname));
    }

    private IEnumerator PlayerRegisterRequest(string email, string password, string nickname)
    {
        // Create a form to hold the login data
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);
        form.AddField("nickname", nickname);

        // Send the POST request to the player login script
        using (UnityWebRequest www = UnityWebRequest.Post(userRegisterURL, form))
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
                    Debug.Log("Player Register successful");

                    // Perform any actions required upon successful player register
                    succesText.SetActive(true);
                    failText.SetActive(false);
                }
                else
                {
                    Debug.Log("Player Register failed");
                    // Perform any actions required upon failed player register
                    succesText.SetActive(true);
                    failText.SetActive(true);
                }
            }
        }
    }
}
