using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Audio;
using System;

public class MainMenuManager : MonoBehaviour
{
    public GameObject MainMenuFirstSelected;

    private void Start()
    {
        Cursor.visible = true;
    }
    public void StartNewGame()
    {
        LevelManager.Instance.LoadScene("Scenes/Zona 1");
        Cursor.visible = false;
    }
}
