using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class ScoreSubmission : MonoBehaviour
{
    public string scoreSubmitURL = "https://studenthome.hku.nl/~bradley.vanewijk/score_submit.php";

    public void SubmitScore(int score)
    {
        string playerId = PlayerPrefs.GetString("ID"); // Retrieve the player's ID
        if (!string.IsNullOrEmpty(playerId))
        {
            StartCoroutine(SendScore(playerId, score));
        }
        else
        {
            Debug.Log("Player ID not found");
        }
    }

    private IEnumerator SendScore(string playerId, int score)
    {
        // Create a form to hold the score data
        WWWForm form = new WWWForm();
        form.AddField("user_id", playerId);
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
                // Handle the response from the server as before
                Debug.Log(www.downloadHandler.text);
                // ... (rest of your code)
            }
        }
    }
}