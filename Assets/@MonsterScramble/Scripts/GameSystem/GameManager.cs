using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public enum GameState
{
    Matching,
    Battle,
    Finish
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState CurrentGameState => _currentGameState;
    [SerializeField]
    private GameLauncher _launcher;
    [SerializeField]
    private AudioManager _audioManager;
    private GameState _currentGameState;

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
    }

    public void StartBattle()
    {
        _audioManager.PlayBattleBGM();
        SwitchGameState(GameState.Battle);
        UIManager.instance.PlayStartLogoAnimation();
    }
    public void Win()
    {
        SwitchGameState(GameState.Finish);
        UIManager.instance.OpenResultPanel(ResultType.Win);
    }

    public void Lose()
    {
        SwitchGameState(GameState.Finish);
        UIManager.instance.OpenResultPanel(ResultType.Lose);
    }
    public void Draw()
    {
        SwitchGameState(GameState.Finish);
        UIManager.instance.OpenResultPanel(ResultType.Draw);
    }
    private void SwitchGameState(GameState nextState)
    {
        _currentGameState = nextState;
    }
}
