using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CinematicVideoController : MonoBehaviour
{
    public VideoPlayer endingVideo;
    // Start is called before the first frame update
    void Start()
    {
        endingVideo.Play();   
        endingVideo.loopPointReached += NextScene;
    }

    void NextScene(UnityEngine.Video.VideoPlayer vp){
        SceneManager.LoadScene("Main Menu");
    }
}
