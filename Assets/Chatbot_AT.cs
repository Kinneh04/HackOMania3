using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuggingFace.API;
public class Chatbot_AT : MonoBehaviour
{
    //Chatbot Auto Tokenizer module

    [Header("Tokenizer Context and conditioning")]
    [TextArea(5, 5)]
    public string Context, FailedToGenerateText;
    [TextArea(2, 3)]
    public string InputHeader, InputCloser, Seperator;

    public string Response;

    public List<string> Tokens = new();


    public void AutoTokenize(string s)
    {
        string contextMessage = Context + InputHeader + s + InputCloser + Seperator;
        HuggingFaceAPI.TextGeneration(contextMessage, OnSendMessageSuccess, OnSendMessageFailure);
    }

    public void OnSendMessageSuccess(string response)
    {
        Response = response;
       Tokens = QueryResponse(ExtractStringAfterSeparator(response));
    }
    public string ExtractStringAfterSeparator(string s)
    {
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
    public List<string> QueryResponse(string response)
    {
        // Remove the starting and ending braces
        string trimmedResponse = response.Trim('{', '}');

        // Split the string by commas
        string[] items = trimmedResponse.Split(',');

        List<string> sortedItems = new List<string>();

        // Trim each item and add it to the list
        foreach (string item in items)
        {
            sortedItems.Add(item.Trim());
        }

        // Optionally, sort the list alphabetically
        sortedItems.Sort();

        return sortedItems;
    }

    public void OnSendMessageFailure(string response)
    {
        Response = response;
    }
}
