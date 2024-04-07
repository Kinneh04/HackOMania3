using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardButtonsUI : MonoBehaviour
{
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _selectedTextColor;
    [SerializeField] private Color _normalColor;
    [SerializeField] private Color _normalTextColor;

    [SerializeField] private Button _singaporeButton;
    [SerializeField] private Button _friendButton;
    [SerializeField] private TextMeshProUGUI _friendText;
    [SerializeField] private TextMeshProUGUI _singaporeText;

    public bool SingaporeScope;


    // Start is called before the first frame update
    void Start()
    {
        SingaporeScope = true;

        SelectSingaporeButton(true);
    }

    public void SelectSingaporeButton(bool isTrue)
    {
        if (isTrue)
        {
            _singaporeButton.image.color = _selectedColor;
            _singaporeText.color = _selectedTextColor;

            _friendButton.image.color = _normalColor;
            _friendText.color = _normalTextColor;
        }
        else
        {
            _friendButton.image.color = _selectedColor;
            _friendText.color = _selectedTextColor;

            _singaporeButton.image.color = _normalColor;
            _singaporeText.color = _normalTextColor;
        }
    }
}
