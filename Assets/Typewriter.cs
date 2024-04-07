using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Typewriter : MonoBehaviour
{
    public TMP_Text textComponent;
    public List<TutorialPart> Tutorials;
    public float typingSpeed = 0.05f;
    private int currentTextIndex = 0;
    private bool isTyping = false;

    public GameObject TutorialMenu, ChatWithTheGuyMenu;
    public HomeUI homeUI;

    public bool doneTutorial;
    public bool Dev = true;

    private void OnDestroy()
    {
        if (doneTutorial) PlayerPrefs.SetInt("DoneTutorial", 1);
        else PlayerPrefs.SetInt("DoneTutorial", 0);
    }

    private void Start()
    {
        int i = PlayerPrefs.GetInt("DoneTutorial");
        if (Tutorials.Count > 0 && i == 0 || Dev)
        {
            StartCoroutine(TypeText(Tutorials[currentTextIndex].TutorialString));
        }
        else
        {
            TutorialMenu.SetActive(false);
            ChatWithTheGuyMenu.SetActive(true);
            homeUI.SetSpeechBbl();
            doneTutorial = true;
        }
    }

    IEnumerator TypeText(string textToType)
    {
        if(currentTextIndex > 0)
            Tutorials[currentTextIndex-1].TutorialPanel.SetActive(false);
        Tutorials[currentTextIndex].TutorialPanel.SetActive(true);
        isTyping = true;
        textComponent.text = ""; // Clear the text component

        foreach (char letter in textToType.ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        currentTextIndex++; // Move to the next text after finishing typing
    }

    public void SkipToNextText()
    {
        if (isTyping)
        {
            // If currently typing, stop the current typing coroutine
            StopAllCoroutines();
            // Display the full text immediately
            textComponent.text = Tutorials[currentTextIndex].TutorialString;
            isTyping = false;
            currentTextIndex++;
            return;
        }

        if (currentTextIndex < Tutorials.Count)
        {
            // If there are more texts to type, start typing the next one
            StartCoroutine(TypeText(Tutorials[currentTextIndex].TutorialString));
        }
        else
        {
            TutorialMenu.SetActive(false);
            ChatWithTheGuyMenu.SetActive(true);
            homeUI.SetSpeechBbl();
            doneTutorial = true;
        }
    }

}

[System.Serializable]
public class TutorialPart
{
    public string TutorialString;
    public GameObject TutorialPanel;
}