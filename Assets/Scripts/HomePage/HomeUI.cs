using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using PlayFab;

public class HomeUI : MonoBehaviour
{
    [SerializeField, Range(1, 10)] private float _speechBubbleDuration;
    [SerializeField] private GameObject _speechBubble;
    [SerializeField] private GameObject _spencerButton;
    [SerializeField] private GameObject _spencerButton_fake;

    // Start is called before the first frame update
    public void SetSpeechBbl()
    {
        StartCoroutine(SetSpeechBubble());
    }

    private IEnumerator SetSpeechBubble()
    {
        Sequence seq = DOTween.Sequence();
        _speechBubble.transform.localScale = Vector3.zero;
        seq.Append(_speechBubble.transform.DOScale(1.2f, 0.4f));
        seq.Append(_speechBubble.transform.DOScale(1f, 0.1f));
        _speechBubble.SetActive(true);

        yield return new WaitForSeconds(_speechBubbleDuration);

        seq = DOTween.Sequence();
        seq.Append(_speechBubble.transform.DOScale(1.2f, 0.4f));
        seq.Append(_speechBubble.transform.DOScale(0f, 0.1f));
        seq.AppendCallback(() => _speechBubble.SetActive(false));
    }

    public void ToGreenUp()
    {
        SceneManager.LoadScene("PlantScene");
    }

    public void ToSpencerChat()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => _spencerButton.SetActive(false));
        seq.AppendCallback(() => _spencerButton_fake.SetActive(true));
        seq.Append(_spencerButton_fake.transform.DOMoveY(50, 0.3f));
        seq.Append(_spencerButton_fake.transform.DOMoveY(-50, 0.3f));
        seq.AppendCallback(() => SceneManager.LoadScene("SPencerChatScene"));
    }

    public void Logout()
    {
        PlayerPrefs.DeleteKey("SPGroupCustomID");
        PlayFabClientAPI.ForgetAllCredentials();
        SceneManager.LoadScene("LoginScene");
    }
}

