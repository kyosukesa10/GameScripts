using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleController : MonoBehaviour {

    public Text inputName;     //nameを入力するインプットフィールド
    public GameObject target;
    public GameObject inputNamePanel;

    AudioSource clickSound;
    AudioSource boopSound;

    public static string playerName;
    public static string PlayerName() { return playerName; }
    private string inputString;

    private void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        clickSound = audioSources[0];
        boopSound = audioSources[1];
        target.SetActive(false);
        if (playerName != null) {
            target.SetActive(true);
            inputNamePanel.SetActive(false);
        }
    }

    public void OnStartButtonClicked() // Startボタンを押したとき、クリック音を鳴らし、Planetシーンへ遷移
    {
        clickSound.PlayOneShot(clickSound.clip);

        SceneManager.LoadScene("Planet");
//        Debug.Log(playerName);
    }
    public void OnOKButtonClicked() // Okボタンを押したとき
    {
        inputString = inputName.GetComponent<Text>().text;
        if(inputString.Length > 6) // 入力した名前が６文字以上なら
        {
            boopSound.PlayOneShot(boopSound.clip);
        }
        else                      // 文字数が６文字以内なら、クリック音を鳴らし、Startボタンを表示,inputNamePanelを非表示
        {
            playerName = inputString;
            clickSound.Play();
            target.SetActive(true);
            inputNamePanel.SetActive(false);
        }
        
    }
}
