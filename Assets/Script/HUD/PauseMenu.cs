using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void SaveGame(){
        //call save and load manager function calls
        Debug.Log("Saved");
    }

    public void Settings(){
        Debug.Log("Open Settings");
    }
    public void BackToMainMenu(){
        SceneManager.LoadScene("Main Menu");
        //ExitPauseMenu();
        Debug.Log("Main Menu");
    }

    public void ExitGame(){
        Debug.Log("Exited");
        Application.Quit();
    }
}
