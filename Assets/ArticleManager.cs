using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ArticleManager : MonoBehaviour
{
    public TMP_Text TitleText, DescText;

    public Image ThumbnailImage;

    public void LoadChallenge(Challenge C)
    {
        TitleText.text = C.NameOfChallenge;
        DescText.text = C.ChallengeDescription;

        ThumbnailImage.sprite = C.ChallengeSprite;
    }
}
