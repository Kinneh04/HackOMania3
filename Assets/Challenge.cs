using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Challenge
{
    [Header("MainVariables")]
    public string NameOfChallenge;
    public Sprite ChallengeSprite;
    public string ChallengeDescription;

    [Header("Tokenizer")]
    public List<string> KeywordTokens;
}
