using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Challenge
{
    [Header("MainVariables")]
    public string NameOfChallenge;
    public Sprite ChallengeSprite;
    [TextArea(10,10)]
    public string ChallengeDescription;

    [Header("Tokenizer")]
    public List<string> KeywordTokens;
}
