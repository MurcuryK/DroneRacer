using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwap : MonoBehaviour {

    public GameObject FirstPerson;
    public GameObject ThirdPerson;

    AudioListener FirstPersonAudio;
    AudioListener ThirdPersonAudio;



    // Use this for initialization
    void Start()
    {

        //Get Camera Listeners
        FirstPersonAudio = FirstPerson.GetComponent<AudioListener>();
        ThirdPersonAudio = ThirdPerson.GetComponent<AudioListener>();

        //Camera Position Set
        cameraPositionChange(PlayerPrefs.GetInt("CameraPosition"));
    }

    // Update is called once per frame
    void Update()
    {
        //Change Camera Keyboard
        switchCamera();
    }

    //UI JoyStick Method
    public void cameraPositonM()
    {
        cameraChangeCounter();
    }

    //Change Camera Keyboard
    void switchCamera()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            cameraChangeCounter();
        }
         
    }

    //Camera Counter
    void cameraChangeCounter()
    {
        int cameraPositionCounter = PlayerPrefs.GetInt("CameraPosition");
        cameraPositionCounter++;
        cameraPositionChange(cameraPositionCounter);
    }

    //Camera change Logic
    void cameraPositionChange(int camPosition)
    {
        if (camPosition > 1)
        {
            camPosition = 0;
        }

        //Set camera position database
        PlayerPrefs.SetInt("CameraPosition", camPosition);

        //Set camera position 1
        if (camPosition == 0)
        {
            FirstPerson.SetActive(true);
            FirstPersonAudio.enabled = true;

            ThirdPersonAudio.enabled = false;
            ThirdPerson.SetActive(false);
        }

        //Set camera position 2
        if (camPosition == 1)
        {
            ThirdPerson.SetActive(true);
            ThirdPersonAudio.enabled = true;

            FirstPersonAudio.enabled = false;
            FirstPerson.SetActive(false);
        }

    }
}
