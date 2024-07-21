using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pauseMenu;
    public bool isOpen;
    void Start()
    {
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isOpen)
            {
                ClosePauseMenu();
                isOpen = false;
                PlayerStateMachine.Instance.BindControls();
            }
            else
            {
                pauseMenu.SetActive(true);
                isOpen = true;
                PlayerStateMachine.Instance.UnbindControls();
            }
        }
    }
    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
    }
}
