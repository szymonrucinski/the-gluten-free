﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.IO;

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
    private int highsoreInital;

    private const int goodBasePoints = 1;
    private const int badBasePoints = 1;
    public float shoppingListMultiplier = 2.5f;
    public List<TextMeshPro> highscoreGameOverViewTextfields;

    private void Awake()
    {
        Instance = this;
        score = 0;
    }
    
    private void Start() {
        setScore();
        highScore = queryHighestScore();
        highsoreInital = highScore;
        setHighScore();
    }

    private int queryHighestScore()
    {
        StreamReader sr = new StreamReader("save.txt");
        if (sr == null)
        {
            //no save file found
            return 0;
        }
        String saveData = sr.ReadLine();
        String[] saveDataSplit = saveData.Split(':');
        sr.Close();
        return Int32.Parse(saveDataSplit[1]);
    }

    public void scoreAction(bool isGood, bool isOnShoppingList, Vector3 collisionPoint)
    {
        var pointChange = 0;
        if (isGood)
        {
            var multiplier = isOnShoppingList ? shoppingListMultiplier : 1f;
            pointChange += (int)(goodBasePoints * multiplier);
        }
        else
        {
            pointChange -= badBasePoints;
        }
        score += pointChange;
        setScore();
        if (pointChange != 0) setPopup(pointChange, collisionPoint);
    }

    private static void setPopup(int pointChange, Vector3 collisionPoint)
    {
        ScoreChangePopup.create(pointChange, collisionPoint);
    }

    private void setScore()
    {
        foreach (var textField in scoreTextFields)
        {
            textField.text = score.ToString();
        }

        if (syncHighScore)
        {
            if(score < highsoreInital)
            {
                highScore = highsoreInital;
            }
            else
            {
                highScore = score;
            }
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

    public void gameOverLeaderBoard()
    {
        if(syncHighScore)
        {
            //when save highscore

        }
        //update Textfields

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
    void gameOverLeaderBoard();
}