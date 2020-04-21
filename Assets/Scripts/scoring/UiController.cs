using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

// ReSharper disable once CheckNamespace
public class UiController : MonoBehaviour, UiAction
{
    public static UiController Instance;

    public GameObject shoppingListCanvas;
    public GameObject inGameCanvas;
    public GameObject gameOverCanvas;

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
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowShoppingList()
    {
        shoppingListCanvas.SetActive(true);
        inGameCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);
        pauseText.enabled = false;
        pauseAudioSliderLabel.enabled = false;
        pauseAudioSlider.SetActive(false);
    }

    public void ShowInGame()
    {
        shoppingListCanvas.SetActive(false);
        inGameCanvas.SetActive(true);
        gameOverCanvas.SetActive(false);
        pauseText.enabled = false;
        pauseAudioSliderLabel.enabled = false;
        pauseAudioSlider.SetActive(false);
    }

    public void ShowGameOver()
    {
        shoppingListCanvas.SetActive(false);
        inGameCanvas.SetActive(false);
        gameOverCanvas.SetActive(true);
        pauseText.enabled = false;
        pauseAudioSliderLabel.enabled = false;
        pauseAudioSlider.SetActive(false);
    }

    public void ShowPause()
    {
        shoppingListCanvas.SetActive(false);
        inGameCanvas.SetActive(true);
        gameOverCanvas.SetActive(false);
        pauseText.enabled = true;
        pauseAudioSliderLabel.enabled = true;
        pauseAudioSlider.SetActive(true);
    }

}

public interface UiAction
{
    void ShowMenu();

    void ShowShoppingList();

    void ShowInGame();

    void ShowGameOver();

    void ShowPause();
}
