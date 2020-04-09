using System;
using UnityEngine;


// ReSharper disable once CheckNamespace
public class ScoreChangePopupFactory : MonoBehaviour
{
    public static ScoreChangePopupFactory Instance = null;


    private void Awake()
    {
        Instance = this;
    }

    public float moveYSpeed = 20f;
    public float disappearTimer = 2f;
    public float disappearSpeed = 3f;
    public Color positiveScoreChangeColor = Color.green;
    public Color negaticeScoreChangeColor = Color.red;

    public ScoreChangePopup popupPrefab;

    public static ScoreChangePopup create(int pointChange, Vector3 collisionPoint)
    {
        var transformPopup = Instantiate(Instance.popupPrefab, collisionPoint, Quaternion.identity);
        transformPopup.SetScoreValue(pointChange);
        return transformPopup;
    }
}