using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SpeechBubblePrefab : MonoBehaviour
{
    public TMP_Text Text;
    public string FullSpeech;
    public bool typewrite = false;

    [Header("TypewriterSettings")]
    public float typewriterSpeed = 0.02f;

    private Coroutine typewriterCoroutine;


    public void OnDisplaySpeech(string s)
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
    }
}
