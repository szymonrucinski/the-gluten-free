using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;

// ReSharper disable once CheckNamespace
public class ScoreController : MonoBehaviour, ScoreAction
{
    public static ScoreAction Instance;


    public List<TextMeshPro> scoreTextFields;
    public List<TextMeshPro> highScoreTextFields;
    public List<TextMeshPro> timerTextFields;
    public int maxDigitsTimer = 2;

    private int score;
    private int highScore;
    private bool syncHighScore = false;

    private const int goodBasePoints = 1;
    private const int badBasePoints = 1;
    public float shoppingListMultiplier = 2.5f;

    private void Awake()
    {
        Instance = this;
        score = 0;
    }
    
    private void Start() {
        setScore();
        highScore = queryHighestScore();
        setHighScore();
    }

    private int queryHighestScore()
    {
        return 1;
    }

    public void scoreAction(bool isGood, bool isOnShoppingList, Vector3 collisionPoint)
    {
        if (isGood)
        {
            var multiplier = isOnShoppingList ? shoppingListMultiplier : 1f;
            score += (int)(goodBasePoints * multiplier);
        }
        else
        {
            score -= badBasePoints;
        }
        setScore();
    }

    private void setScore()
    {
        foreach (var textField in scoreTextFields)
        {
            textField.text = score.ToString();
        }

        if (syncHighScore)
        {
            highScore = score;
            setHighScore();
        }
        else if (score > highScore)
        {
            syncHighScore = true;
            highScore = score;
            setHighScore();
        }
    }
    
    private void setHighScore()
    {
        foreach (var highScoreTextField in highScoreTextFields)
        {
            highScoreTextField.text = highScore.ToString();
        }
    }

    public void nextTime(float timeLeft)
    {
        var timeString = timeLeft.ToString(CultureInfo.CurrentCulture);
        // TODO Shorten string
        foreach (var timerTextField in timerTextFields)
        {
            timerTextField.text = timeString;
        }
    }
}

public interface ScoreAction
{
    /// <summary>
    /// Invokes the scoring logic for an item.
    /// </summary>
    /// <param name="isGood">Determine the nature of the action, true if the action is considered to be good.</param>
    /// <param name="isOnShoppingList">true, if the item is part of the shopping list</param>
    /// <param name="collisionPoint">Vector3 of collision point, needed to display overlay of point gain/loss</param>
    void scoreAction(bool isGood, bool isOnShoppingList, Vector3 collisionPoint);

    void nextTime(float timeLeft);
}