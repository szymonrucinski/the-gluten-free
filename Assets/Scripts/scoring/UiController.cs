using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

// ReSharper disable once CheckNamespace
public class UiController : MonoBehaviour, UiAction
{
    public static UiController Instance;

    public Canvas menuCanvas;
    public Canvas inGameCanvas;
    public Canvas gameOverCanvas;

    public TextMeshPro pauseText;
    public TextMeshProUGUI pauseAudioSliderLabel;
    public GameObject pauseAudioSlider;

    private void Awake()
    {
        Instance = this;
        pauseText.text = "Pause";
    }

    public void ShowMenu()
    {
        menuCanvas.enabled = true;
        inGameCanvas.enabled = false;
        gameOverCanvas.enabled = false;
        pauseText.enabled = false;
        pauseAudioSliderLabel.enabled = false;
        pauseAudioSlider.SetActive(false);
    }

    public void ShowInGame()
    {
        menuCanvas.enabled = false;
        inGameCanvas.enabled = true;
        gameOverCanvas.enabled = false;
        pauseText.enabled = false;
        pauseAudioSliderLabel.enabled = false;
        pauseAudioSlider.SetActive(false);
    }

    public void ShowGameOver()
    {
        menuCanvas.enabled = false;
        inGameCanvas.enabled = false;
        gameOverCanvas.enabled = true;
        pauseText.enabled = false;
        pauseAudioSliderLabel.enabled = false;
        pauseAudioSlider.SetActive(false);
    }

    public void ShowPause()
    {
        menuCanvas.enabled = false;
        inGameCanvas.enabled = true;
        gameOverCanvas.enabled = false;
        pauseText.enabled = true;
        pauseAudioSliderLabel.enabled = true;
        pauseAudioSlider.SetActive(true);
    }

}

public interface UiAction
{
    void ShowMenu();

    void ShowInGame();

    void ShowGameOver();

    void ShowPause();
}
