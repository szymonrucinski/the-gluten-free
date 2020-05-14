using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public static readonly string KEY_PLAYER_NAME = "playername";
    public TMP_InputField playerInput;
    private int maxLength = 12;
    
    public void openLevel(string sceneName)
    {
        var playerInputText = playerInput.text;
        playerInputText = playerInputText.Length < 1 ? Environment.UserName : playerInputText; 
        PlayerPrefs.SetString(KEY_PLAYER_NAME, playerInputText.Length > maxLength ? playerInputText.Substring(0,maxLength): playerInputText);
        SceneManager.LoadScene(sceneName);
    }

    private void Start()
    {
        var playerName = PlayerPrefs.GetString(KEY_PLAYER_NAME,  Environment.UserName);
        playerInput.text = playerName;
    }
}
