using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        // Play the video when the scene starts
        videoPlayer.Play();
    }

    void Update()
    {
        // Check for any key press to proceed to the main menu
        if (Input.anyKey)
        {
            // Load the main menu scene
            SceneManager.LoadScene("MainMenu");
        }
    }
}