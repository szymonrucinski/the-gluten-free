using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// ReSharper disable once CheckNamespace
public class ScoreChangePopup : MonoBehaviour
{
    private TextMeshPro textMesh;
    private float timestamp;
    private Color textColor;
    private int scoreValue;

    private ScoreChangePopupFactory factory;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
        factory = ScoreChangePopupFactory.Instance;
    }

    private void Start()
    {
        timestamp = Math.Max(TimerImpl.Instance.getRemainingTime() - factory.disappearTimer, 0);
    }

    private void Update()
    {
        transform.position += new Vector3(0, factory.moveYSpeed) * Time.deltaTime;

        // Check if timer is up, if not exit update
        if (TimerImpl.Instance.getRemainingTime() > timestamp) return;
        // Disappearing
        textColor.a -= factory.disappearSpeed * Time.deltaTime;
        textMesh.color = textColor;
        if (textColor.a < 0) Destroy(gameObject);
    }

    public void SetScoreValue(int value)
    {
        //setup
        scoreValue = value;
        var binOperator = (scoreValue > 0) ? "+" : "-";
        textColor = (scoreValue > 0) ? factory.positiveScoreChangeColor : factory.negaticeScoreChangeColor;
        textMesh.SetText(binOperator + Math.Abs(scoreValue));
        textMesh.faceColor = textColor;
    }
}