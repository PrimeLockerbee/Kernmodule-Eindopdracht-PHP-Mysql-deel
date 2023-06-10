using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class ScoreSubmission : MonoBehaviour
{
    public string scoreSubmitURL = "https://studenthome.hku.nl/~bradley.vanewijk/score_submit.php";

    public ServerLogin serverLogin;

    public void SubmitScore(string playername, int playerscore)
    {
        if (serverLogin.serverLoggedIn)
        {
            string playerName = playername;
            int score = playerscore;

            StartCoroutine(SendScore(playerName, score));
        }
        else
        {
            Debug.Log("Server not logged in");
        }
    }

    private IEnumerator SendScore(string playerName, int score)
    {
        // Create a form to hold the score data
        WWWForm form = new WWWForm();
        form.AddField("playerName", playerName);
        form.AddField("score", score);

        // Send the POST request to the score submission script
        using (UnityWebRequest www = UnityWebRequest.Post(scoreSubmitURL, form))
        {
            yield return www.SendWebRequest();

            // Check for errors during the request
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError("Score submission request failed. Error: " + www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);

                // Check the response from the server
                if (www.downloadHandler.text.Contains("Score updated successfully"))
                {
                    Debug.Log("Score submitted successfully");
                    // Perform any actions required upon successful score submission
                }
                else if (www.downloadHandler.text.Contains("Score inserted successfully"))
                {
                    Debug.Log("Score inserted successfully");
                    // Perform any actions required upon successful score insertion
                }
                else if (www.downloadHandler.text.Contains("Player does not exist"))
                {
                    Debug.Log("Player does not exist");
                    // Perform any actions for a non-existing player
                }
                else
                {
                    Debug.Log("Score submission failed");
                    // Perform any actions required upon failed score submission
                }
            }
        }
    }
}