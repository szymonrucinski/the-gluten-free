using System;
using System.Collections.Generic;
using UnityEngine;


// ReSharper disable once CheckNamespace
public interface Timer
{
    /// <summary>
    /// Pauses the game and shows the menu.
    /// </summary>
    void pauseTimer();

    /// <summary>
    /// Resumes the game.
    /// </summary>
    void resumeTimer();

    /// <summary>
    /// Returns Paused state.  
    /// </summary>
    /// <returns>true, if the timer is paused</returns>
    bool isPaused();

    /// <summary>
    /// Returns the remaining time of this game.
    /// </summary>
    /// <returns>remaining time in float</returns>
    float getRemainingTime();

    /// <summary>
    /// Adds Consumer for all time updates.
    /// </summary>
    /// <param name="timeConsumer"></param>
    void addTimeConsumer(Action<float> timeConsumer);

    /// <summary>
    /// Returns the game-over state of the game. 
    /// </summary>
    /// <returns>true, if the game is over</returns>
    bool isTimeOver();

    /// <summary>
    /// Adds time to the counter.
    /// </summary>
    void addTime(float timeIncrement);
}

public class TimerImpl : MonoBehaviour, Timer
{
    public static Timer Instance;

    public float gameTimeInitial = 20f;

    private readonly List<Action<float>> timeConsumers = new List<Action<float>>();

    private bool paused;
    private float time;

    private void Awake()
    {
        Instance = this;
        time = gameTimeInitial;
        pauseTimer();
    }

    private void Update()
    {
        if (paused) return;

        time -= Time.deltaTime;
        foreach (var timeConsumer in timeConsumers)
        {
            timeConsumer.Invoke(getRemainingTime());
        }
    }

    public bool isTimeOver() => time <= 0f;

    public void addTime(float timeIncrement)
    {
        time += timeIncrement;
    }

    public void pauseTimer()
    {
        paused = true;
        Time.timeScale = 0f;
    }

    public void resumeTimer()
    {
        paused = false;
        Time.timeScale = 1f;
    }

    public bool isPaused() => paused;

    public float getRemainingTime() => isTimeOver() ? 0f : time;

    public void addTimeConsumer(Action<float> timeConsumer) => timeConsumers.Add(timeConsumer);
}