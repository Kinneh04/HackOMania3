using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HuggingFace.API;

public class TextSummaryManager : MonoBehaviour
{
    [Header("Context and conditioning")]
    [TextArea(5, 15)]
    public string Context, FailedToGenerateText;
    [TextArea(2, 3)]
    public string InputHeader, InputCloser, Seperator;

    public Challenge CurrentChallenge;

    public TMP_Text ResponseSummaryText;

    public GameObject SummaryMenu;

    public void OnClickSummarizeArticle()
    {
        string ContextedText = Context + InputHeader + CurrentChallenge.ChallengeDescription + InputCloser + Seperator;

        HuggingFaceAPI.TextGeneration(ContextedText, OnSendMessageSuccess, OnSendMessageFailure);
    }
    public string ExtractStringAfterSeparator(string s)
    {
        if (!s.Contains(Seperator)) return s;
        int separatorIndex = s.IndexOf(Seperator);

        // Check if the separator was found
        if (separatorIndex != -1)
        {
            // Extract and return the substring after the separator
            // Adding the length of the separator to skip over it
            return s.Substring(separatorIndex + Seperator.Length);
        }
        else
        {
            // Separator not found, return an empty string or the original string,
            // depending on your needs. Here, returning an empty string as an example.
            return string.Empty;
        }
    }

    public void OnSendMessageSuccess(string response)
    {
        ResponseSummaryText.text = ExtractStringAfterSeparator(response);
        SummaryMenu.SetActive(true);

    }

    public void OnSendMessageFailure(string response)
    {
        ResponseSummaryText.text = response;
    }
}
