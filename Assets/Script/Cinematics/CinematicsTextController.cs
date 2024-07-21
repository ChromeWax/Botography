using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CinematicsTextController : MonoBehaviour
{
    public TextMeshProUGUI cinematicTextBox;
    [SerializeField] List<CinematicText> cinematicTexts;

    private int numberOfCinematicText, cinematicControlVariable;
    [SerializeField] float textDelay;
    private float timeElapsedSinceText = 0.0f;
    private bool enabled = true;

    void Start()
    {
        numberOfCinematicText = cinematicTexts.Count;
        cinematicTextBox.SetText(cinematicTexts[0].text);
        cinematicControlVariable = 1;
    }

    
    void Update()
    {
        if (enabled == false) return;
        timeElapsedSinceText+= Time.deltaTime;

        if (timeElapsedSinceText > textDelay - 1)
            cinematicTextBox.color = Color.clear;

        if(timeElapsedSinceText > textDelay){
            cinematicTextBox.color = Color.white;
            timeElapsedSinceText = 0.0f;
            if(cinematicControlVariable<numberOfCinematicText){
                cinematicTextBox.SetText(cinematicTexts[cinematicControlVariable++].text);
                if(cinematicControlVariable==numberOfCinematicText){
                    enabled = false;
                    StartCoroutine(LoadScene());
                }
            }
        }
    }

    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Overworld");
    }
}
