using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuggingFace.API;

public class Chatbot_SU : MonoBehaviour
{
    [Header("Context and conditioning")]
    [TextArea(15, 15)]
    public string Context, FailedToGenerateText;
    public Chatbot mainChatbot;

    public void Summarize(string originalText)
    {
        string ContextedText = Context + mainChatbot.InputHeader +originalText + mainChatbot.InputCloser + mainChatbot.Seperator;

        HuggingFaceAPI.TextGeneration(ContextedText, OnSendMessageSuccess, OnSendMessageFailure);
    }
    public void OnSendMessageSuccess(string response)
    {
        mainChatbot.SendBotMessage("Summary:\n"+response);
    }

    public void OnSendMessageFailure(string response)
    {
        mainChatbot.SendBotMessage("An error occured while trying to summarize article: "+response);
    }
}
