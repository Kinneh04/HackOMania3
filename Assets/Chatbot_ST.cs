using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuggingFace.API;
using UnityEngine.Events;
public class Chatbot_ST : MonoBehaviour
{

    [Header("Variables")]
    public Chatbot chatBotMain;
    //I cant turn this into a class because i need a global string[] Var that cant be class encapped;

    public string[] Actions;
    public float[] ResponseSimilarityFactor;
    public string[] ActionResponses;
    public UnityEvent[] ActionResponseEvents;

    //God forgive me 
    //ResponseSimilarityFactor is from 0 to 1. find the highest in the array, and correlate it to the string in Actions[]

    public string HighestString;
    public float HighestStringFactor;

    [Range(0, 0.95f)]
    public float SentenceMatchThreshold;

  public void OnApply(string s)
    {
        HuggingFaceAPI.SentenceSimilarity(s, OnSuccess, OnFailure, Actions);
    }

    public void OnSuccess(float[] f)
    {
        ResponseSimilarityFactor = f;
        chatBotMain.OnSTSuccess();
       // FindHighestFactor();
    }

    public void OnFailure(string s)
    {
        Debug.LogError("Cannot for some reason" + s);
    }

    public bool FindHighestFactor()
    {
        HighestString = "";
        if (Actions.Length == 0 || ResponseSimilarityFactor.Length == 0 || Actions.Length != ResponseSimilarityFactor.Length)
        {
            Debug.LogError("Arrays are empty or not of the same length!");
            return false;
        }

        float highestValue = ResponseSimilarityFactor[0];
        int highestIndex = 0;

        // Loop through the ResponseSimilarityFactor to find the highest value
        for (int i = 1; i < ResponseSimilarityFactor.Length; i++)
        {
            if (ResponseSimilarityFactor[i] > 0 && ResponseSimilarityFactor[i] > SentenceMatchThreshold)
            {
                highestValue = ResponseSimilarityFactor[i];
                highestIndex = i;
            }
        }

        if (highestValue >= SentenceMatchThreshold)
        {
            // Output the action corresponding to the highest response similarity factor
            Debug.Log($"Action with the highest response similarity factor: {Actions[highestIndex]} ({highestValue})");

            HighestStringFactor = highestValue;
            HighestString = Actions[highestIndex];
            return true;
        }
        return false;

    }
}
