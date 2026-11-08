using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsGameOver { get; private set; }
    public bool IsGameWon { get; private set; }

    public bool IsGameEnded => IsGameOver || IsGameWon;

    public event Action OnGameOver;
    public event Action OnGameWin; // Kazanma eventi

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EndGame()
    {
        if (IsGameOver || IsGameWon) return;

        IsGameOver = true;
        Debug.Log("<color=red>GAME OVER! Oyun Bitti.</color>");

        OnGameOver?.Invoke();
        Time.timeScale = 0f;
    }

    public void WinGame()
    {
        if (IsGameOver || IsGameWon) return;

        IsGameWon = true;
        Debug.Log("<color=green>VICTORY! Tüm Dalgalar Temizlendi!</color>");

        OnGameWin?.Invoke();
        Time.timeScale = 0f;
    }
}