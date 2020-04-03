using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// ReSharper disable once CheckNamespace
public class ScoreChangePopup : MonoBehaviour
{
    public TextMeshPro textMesh;
    private float timestamp;
    private Color textColor;

    private ScoreChangePopupConfig config;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    private void Start()
    {
        config = ScoreChangePopupConfig.Instance;
        timestamp = Math.Max(TimerImpl.Instance.getRemainingTime() - config.disappearTimer, 0);
    }

    private void Update()
    {
        transform.position += new Vector3(0, config.moveYSpeed) * Time.deltaTime;

        // Check if timer is up, if not exit update
        if (TimerImpl.Instance.getRemainingTime() > timestamp) return;
        // Disappearing
        textColor.a -= config.disappearSpeed * Time.deltaTime;
        textMesh.color = textColor;
        if (textColor.a < 0) Destroy(gameObject);
    }

    private void Setup(int value)
    {
        var binOperator = (value > 0) ? "+" : "-";
        textMesh.SetText(binOperator + Math.Abs(value));
        textColor = textMesh.color;
    }

    public static ScoreChangePopup create(int pointChange, Vector3 collisionPoint)
    {
        var transformPopup = Instantiate(ScoreChangePopupConfig.Instance.popupPrefab, collisionPoint, Quaternion.identity);
        var scriptComponent = transformPopup.GetComponent<ScoreChangePopup>();
        scriptComponent.Setup(pointChange);
        return scriptComponent;
    }
}