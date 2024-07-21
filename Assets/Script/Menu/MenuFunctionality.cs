using Botography.SaveManagerNS;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuFunctionality : MonoBehaviour
{
    private bool _savedGameExists = false;
    private int _buttonSelected = 0;

    [SerializeField] private GameObject _menuButtons;
    [SerializeField] private GameObject _title;
    [SerializeField] private GameObject _lastCheckButtons;
    [SerializeField] private GameObject _warningText;
	private SaveManager _saveManagerRef;
    public Vector2 MovementInput { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        //InputHandler.Instance.OnMovementPressed += OnMovementPressed;
        BackToMainMenu();
        GameObject _resumeGameButton = _menuButtons.transform.GetChild(0).gameObject;
        if (!File.Exists("BotoSave.json"))
        {
            _resumeGameButton.GetComponent<Image>().color = Color.gray;
            _resumeGameButton.GetComponent<Button>().interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*if (InputHandler.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            _buttonSelected %= 4;
            _menuButtons.transform.GetChild(_buttonSelected).GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(50, 50, 50);
            _buttonSelected -= 1;
            _buttonSelected %= 4;
            _menuButtons.transform.GetChild(_buttonSelected).GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255);
            Debug.Log("Pressing up gives " +  _buttonSelected);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            _buttonSelected %= 4;
            _menuButtons.transform.GetChild(_buttonSelected).GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(50, 50, 50);
            _buttonSelected += 1;
            _buttonSelected %= 4;
            _menuButtons.transform.GetChild(_buttonSelected).GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255);
            Debug.Log("Pressing down gives " + _buttonSelected);
        }*/
    }

    public void NewGame()
    {
        _title.SetActive(false);
        _menuButtons.SetActive(false);
        _warningText.SetActive(true);
        _lastCheckButtons.SetActive(true);
    }

    public void Settings()
    {
        Debug.Log("Settings Opened");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void StartNewGame()
    {
		SaveManager.loadGame = false;
        SceneManager.LoadScene("Intro");
    }

    public void LoadGame()
    {
		SaveManager.loadGame = true;
        SceneManager.LoadScene("Overworld");
    }

    public void BackToMainMenu()
    {
        _title.SetActive(true);
        _menuButtons.SetActive(true);
        _warningText.SetActive(false);
        _lastCheckButtons.SetActive(false);
    }

    // Why do we need this? this can be done without this function
    /*
    private void OnMovementPressed(Vector2 movement)
    {
        MovementInput = movement;
        //_menuButtons.transform.GetChild(_buttonSelected/2).GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(50, 50, 50);
        Debug.Log(_buttonSelected + " before");
        _buttonSelected -= (int)MovementInput.y;
        if (_buttonSelected < 0)
        {
            _buttonSelected += 8;
        }
        _buttonSelected %= 8;
        for(int x=0; x<4; x++)
        {
            _menuButtons.transform.GetChild(x).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.black;
        }
        _menuButtons.transform.GetChild(_buttonSelected/2).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.gray;
        Debug.Log(_buttonSelected + " after");
    }
    */
}
