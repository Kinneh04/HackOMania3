using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Events;

public class SpeechBubblePrefab : MonoBehaviour
{
    public TMP_Text Text;
    public string FullSpeech;
    public bool typewrite = false;

    public Transform BubbleCustomButtonprefabParent;

    public Image image;
    public UnityEvent FinishSpeech;

    [Header("TypewriterSettings")]
    public float typewriterSpeed = 0.02f;

    private Coroutine typewriterCoroutine;

    public Challenge SavedChallenge;

    public Chatbot mainChatbot;
    public void RedirectToChallenge()
    {
        // Redirect to challenge page, closing the bot.;
    }


    public void OnDisplaySpeech(string s, Sprite image = null)
    {
        FullSpeech = s;
        if (typewrite)
        {
            if (typewriterCoroutine != null)
                StopCoroutine(typewriterCoroutine);

            typewriterCoroutine = StartCoroutine(TypewriterEffect());
        }
        else
        {
            Text.text = FullSpeech;
        }
     
    }

    IEnumerator TypewriterEffect()
    {
        Text.text = ""; // Clear the text first

        for (int i = 0; i < FullSpeech.Length; i++)
        {
            Text.text += FullSpeech[i];
            yield return new WaitForSeconds(typewriterSpeed);
        }
        FinishSpeech.Invoke();
        mainChatbot.SendButton.interactable = true; 
    }
}
