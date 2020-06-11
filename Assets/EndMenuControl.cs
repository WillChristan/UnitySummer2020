using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EndMenuControl : MonoBehaviour
{
    public PlayerUtility playerUtility;

    public void RestartGame()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        playerUtility.ResetPlayer();

        this.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
