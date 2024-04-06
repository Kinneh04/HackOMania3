using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Challenge : MonoBehaviour
{
    [Header("MainVariables")]
    public string NameOfChallenge;
    public Sprite ChallengeSprite;
    public string ChallengeDescription;

    [Header("Tokenizer")]
    public List<string> KeywordTokens;
}
