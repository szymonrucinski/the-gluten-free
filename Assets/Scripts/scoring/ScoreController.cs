using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

// ReSharper disable once CheckNamespace
public class ScoreController : MonoBehaviour, ScoreAction
{
    public static ScoreAction Instance;

    public List<TextMeshPro> scoreTextFields;
    public List<TextMeshPro> highScoreTextFields;
    public List<TextMeshPro> timerTextFields;
    public List<TextMeshPro> streakFields;

    public int maxDigitsTimer = 2;

    private int score;
    private int highScore;
    private bool syncHighScore = false;
    private int highscoreInital;
    private bool scorePlaced = false;
    private String saveFilePath = "save.txt";
    private String saveFilePattern = "^.+:[0-9]+$";

    // Streaks
    private int streakCount = 0;
    private int streakMultiplier = 0;
    private float timeDelta = 0; // Time change due to hits and misses

    // Streak Settings - T
    private int maxStreak = 6; // Maximum streak modifier
    private int streakWeight = 4; // Controls the increase of the streak modifier [more weight = harder to increase]

    private const int goodBasePoints = 1;
    private const int badBasePoints = 2;
    public float shoppingListMultiplier = 2.5f;
    public List<TextMeshProUGUI> highscoreGameOverViewTextfields;

    private void Awake()
    {
        Instance = this;
        score = 0;
    }

    private void Start()
    {
        setScore();
        highScore = queryHighestScore();
        highscoreInital = highScore;
        setHighScore();
    }

    private int queryHighestScore()
    {
        if (File.Exists(saveFilePath))
        {
            using (var streamReader = new StreamReader(saveFilePath))
            {
                String saveData = streamReader.ReadLine();
                if (saveData == null)
                {
                    return 0;
                }

                Regex rgx = new Regex(saveFilePattern);

                if (rgx.IsMatch(saveData))
                {
                    String[] saveDataSplit = saveData.Split(':');
                    return Int32.Parse(saveDataSplit[1]);
                }
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
        }
        else
        {
            streakCount = 0;
            timeDelta -= 0.5f;
            streakMultiplier = 1;
            pointChange -= badBasePoints;
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
        float change = timeDelta;
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

        if (syncHighScore)
        {
            if (score < highscoreInital)
            {
                highScore = highscoreInital;
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

    private void setStreak()
    {
        foreach (var streakField in streakFields)
        {
            streakField.text = streakCount.ToString() + "\nx" + streakMultiplier.ToString();
        }
    }

    public void nextTime(float timeLeft)
    {
        var timeString = timeLeft.ToString(CultureInfo.CurrentCulture);
        foreach (var timerTextField in timerTextFields)
        {
            timerTextField.text = $"{timeLeft:0.###}";
        }
    }

    public void gameOverLeaderBoard()
    {
        List<String> fileData = new List<String>();

        if (File.Exists(saveFilePath))
        {
            using (var streamReader = new StreamReader(saveFilePath))
            {
                String line;
                Regex rgx = new Regex(saveFilePattern);
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (rgx.IsMatch(line))
                    {
                        fileData.Add(line);
                    }
                }
            }
        }

        int i = 0;
        while (i < fileData.Count && scorePlaced == false)
        {
            String[] fileDataSplit = fileData[i].Split(':');
            if (score > Int32.Parse(fileDataSplit[1]))
            {
                String oldScore = fileData[i];
                fileData[i] = Environment.UserName + ':' + score;
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
            fileData.Add(Environment.UserName + ':' + score);
            scorePlaced = true;
        }

        using (var streamWriter = new StreamWriter(saveFilePath))
        {
            int j = 0;
            while (j < fileData.Count && j < 5)
            {
                streamWriter.WriteLine(fileData[j]);
                j++;
            }
        }

        int fileDataPos = 0;
        foreach (var highscoreGameOverViewTextfield in highscoreGameOverViewTextfields)
        {
            if (fileDataPos < fileData.Count)
            {
                String[] fileDataSplit = fileData[fileDataPos].Split(':');
                highscoreGameOverViewTextfield.text = (fileDataSplit[0] + ": " + fileDataSplit[1]).ToString();
                fileDataPos++;
            }
            else
            {
                highscoreGameOverViewTextfield.text = " ";
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