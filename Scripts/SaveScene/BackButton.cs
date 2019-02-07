using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour {

    AudioSource clickSound;

    private void Start()
    {
        clickSound = GetComponent<AudioSource>();
    }

    public void OnBackButtonClicked()
    {
        clickSound.Play();
        SceneManager.LoadScene("Planet");
        RotatePlanet.placesWent = 2; // Inn の前に戻る
    }
}
