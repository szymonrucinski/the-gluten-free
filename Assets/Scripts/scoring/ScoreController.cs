using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.Serialization;

// ReSharper disable once CheckNamespace
public class ScoreController : MonoBehaviour, ScoreAction
{
    public static ScoreAction Instance;

    // UI Text Fields
    public List<TextMeshPro> scoreTextFields;
    public List<TextMeshProUGUI> scoreTextFieldsUGUI;
    public List<TextMeshPro> highScoreTextFields;
    public List<TextMeshPro> timerTextFields;
    [FormerlySerializedAs("streakFields")] 
    public List<TextMeshPro> streakTextFields;
    [FormerlySerializedAs("highscoreGameOverViewTextfields")] 
    public List<TextMeshProUGUI> highScoreGameOverViewTextFields;

    //ScoreSounds
    public GameObject musicController;
    private SoundManager soundMangerScript;
    
    // Score
    private int score;
    private int highScore;
    private bool syncHighScore;
    private int highScoreInitial;
    
    // Score Settings
    public int goodBasePoints = 1;
    public int badBasePoints = 2;
    public float shoppingListMultiplier = 2.5f;
    
    // Score Saves
    private bool scorePlaced;
    private const string saveFilePath = "save.txt";
    private const string saveFilePattern = "^.+:[0-9]+$";

    // Streaks
    private int streakCount;
    private int streakMultiplier = 1;
    private float timeDelta; // Time change due to hits and misses

    // Streak Settings - T
    public int maxStreak = 6; // Maximum streak modifier
    public int streakWeight = 4; // Controls the increase of the streak modifier [more weight = harder to increase]

    public static readonly string KEY_PLAYER_NAME = "playername";
    private void Awake()
    {
        Instance = this;
        score = 0;
    }

    private void Start()
    {
        soundMangerScript = SoundManager.Instance;
        setScore();
        highScore = queryHighestScore();
        highScoreInitial = highScore;
        setHighScore();
        setStreak();
       
    }

    private int queryHighestScore()
    {
        if (File.Exists(saveFilePath))
        {
            using (var streamReader = new StreamReader(saveFilePath))
            {
                var saveData = streamReader.ReadLine();
                if (saveData == null)
                {
                    return 0;
                }

                var rgx = new Regex(saveFilePattern);

                if (rgx.IsMatch(saveData))
                {
                    var saveDataSplit = saveData.Split(':');
                    return int.Parse(saveDataSplit[1]);
                }
                // ReSharper disable once RedundantIfElseBlock
                else
                {
                    //file data corrupt
                    return 0;
                }
            }
        }

        //no save file found
        return 0;
    }

    public void scoreAction(bool isGood, bool isOnShoppingList, Vector3 collisionPoint)
    {
        var pointChange = 0;
        if (isGood)
        {
            streakCount += 1;
            timeDelta += 0.5f;
            streakMultiplier = calculateStreakMultiplier(streakCount);
            var multiplier = isOnShoppingList ? shoppingListMultiplier : 1f;
            pointChange += (int) (goodBasePoints * multiplier * streakMultiplier);
            soundMangerScript.playSuccessSound();
        }
        else
        {
            streakCount = 0;
            timeDelta -= 0.5f;
            streakMultiplier = 1;
            pointChange -= badBasePoints;
            soundMangerScript.playFailSound();
        }

        score += pointChange;
        setScore();
        setStreak();
        if (pointChange != 0) setPopup(pointChange, collisionPoint);
    }

    private static void setPopup(int pointChange, Vector3 collisionPoint)
    {
        ScoreChangePopupFactory.create(pointChange, collisionPoint);
    }


    public float getTimeDelta()
    {
        var change = timeDelta;
        timeDelta = 0;
        return change;
    }

    private int calculateStreakMultiplier(int count)
    {
        return Math.Min(Math.Max(
            count / streakWeight -
            Convert.ToInt32(((count < 0) ^ (streakWeight < 0)) && (count % streakWeight != 0)), 1), maxStreak);
    }

    private void setScore()
    {
        foreach (var textField in scoreTextFields)
        {
            textField.text = score.ToString();
        }
        foreach (var textField in scoreTextFieldsUGUI)
        {
            //textField.text = score.ToString();
        }


        if (syncHighScore)
        {
            highScore = (score < highScoreInitial) ? highScoreInitial : score;
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

    private void setStreak()
    {
        foreach (var streakField in streakTextFields)
        {
            streakField.text = "x" + streakMultiplier.ToString();
        }
    }

    public void nextTime(float timeLeft)
    {
        foreach (var timerTextField in timerTextFields)
        {
            timerTextField.text = $"{timeLeft:0.###}";
        }
    }

    public void gameOverLeaderBoard()
    {
        var fileData = new List<string>();

        if (File.Exists(saveFilePath))
        {
            using (var streamReader = new StreamReader(saveFilePath))
            {
                string line;
                var rgx = new Regex(saveFilePattern);
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (rgx.IsMatch(line))
                    {
                        fileData.Add(line);
                    }
                }
            }
        }

        var i = 0;
        var userName = PlayerPrefs.GetString(KEY_PLAYER_NAME);
        while (i < fileData.Count && scorePlaced == false)
        {
            var fileDataSplit = fileData[i].Split(':');
            if (score > int.Parse(fileDataSplit[1]))
            {
                var oldScore = fileData[i];
                fileData[i] = userName + ':' + score;
                scorePlaced = true;
                if (i < 5)
                {
                    fileData.Insert(i + 1, oldScore);
                }
            }

            i++;
        }

        if (!scorePlaced && fileData.Count < 5)
        {
            fileData.Add(userName + ':' + score);
            scorePlaced = true;
        }

        using (var streamWriter = new StreamWriter(saveFilePath))
        {
            var j = 0;
            while (j < fileData.Count && j < 5)
            {
                streamWriter.WriteLine(fileData[j]);
                j++;
            }
        }

        var fileDataPos = 0;
        foreach (var highScoreGameOverViewTextField in highScoreGameOverViewTextFields)
        {
            if (fileDataPos < fileData.Count)
            {
                var fileDataSplit = fileData[fileDataPos].Split(':');
                //highScoreGameOverViewTextField.text = fileDataSplit[0] + ": " + fileDataSplit[1];
                fileDataPos++;
            }
            else
            {
                //highScoreGameOverViewTextField.text = " ";
            }
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
    
    void gameOverLeaderBoard();
    float getTimeDelta();
}