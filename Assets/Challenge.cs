using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Challenge
{
    [Header("MainVariables")]
    [TextArea(4,4)]
    public string NameOfChallenge;
    public Sprite ChallengeSprite;
    [TextArea(15,20)]
    public string ChallengeDescription;
    public int rewardForCompletion = 0;

    [Header("Tokenizer")]
    public List<string> KeywordTokens;
}
