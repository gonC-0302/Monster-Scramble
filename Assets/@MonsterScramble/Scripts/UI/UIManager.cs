using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;

public enum ResultType
{
    Win,
    Lose,
    Draw
}

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField]
    private TextMeshProUGUI _timerText;
    [SerializeField]
    private Transform _startLogoTran;
    [SerializeField]
    private TextMeshProUGUI _resultText;
    [SerializeField]
    private GameObject _resultPanel;
    [SerializeField]
    private Button _homeButton;
    [SerializeField]
    private TextMeshProUGUI _spText;
    [SerializeField]
    private Image _spGage;
    [SerializeField]
    private GameLauncher _launcher;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        _homeButton.onClick.AddListener(OnClickHomeButton);
    }

    public void PlayStartLogoAnimation()
    {
        _startLogoTran.transform.localScale = Vector3.zero;
        _startLogoTran.transform.DOScale(1.25f, 1f).OnComplete(() => _startLogoTran.gameObject.SetActive(false));
    }

    public void UpdateBattleTimer(float time)
    {
        var span = new TimeSpan(0, 0, (int)time);
        _timerText.text = span.ToString(@"mm\:ss");
    }

    public void OpenResultPanel(ResultType type)
    {
        switch(type)
        {
            case ResultType.Win:
                _resultText.text = "YOU WIN";

                break;
            case ResultType.Lose:
                _resultText.text = "YOU LOSE";

                break;
            case ResultType.Draw:
                _resultText.text = "Draw";

                break;
        }
        _resultPanel.SetActive(true);
    }

    private void OnClickHomeButton()
    {
        _launcher.ShutDown();
        SceneManager.LoadScene("Battle");
    }

    public void UpdateSPText(int chargedsp)
    {
        _spText.text = chargedsp.ToString();
    }
    public void UpdateSPGage(float value)
    {
        _spGage.fillAmount = value;
    }
}
