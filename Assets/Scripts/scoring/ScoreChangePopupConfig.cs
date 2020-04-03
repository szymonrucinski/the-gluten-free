using System;
using UnityEngine;


// ReSharper disable once CheckNamespace
public class ScoreChangePopupConfig : MonoBehaviour
{
    private static ScoreChangePopupConfig _instance = null;

    public static ScoreChangePopupConfig Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Instantiate(Resources.Load<ScoreChangePopupConfig>("../../Prefabs/scoring/ScoreChangePopupConfig"));   
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    public float moveYSpeed = 20f;
    public float disappearTimer = 2f;
    public float disappearSpeed = 3f;

    public Transform popupPrefab;
}