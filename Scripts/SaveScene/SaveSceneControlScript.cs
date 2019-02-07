using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSceneControlScript : MonoBehaviour { // SaveSceneControlScriptオブジェクトにアタッチ

    public Text playerNameText;
    public Text goldAmount;    //現在のGold量を表示するtext
    private int score;

    void Start () {
        score = CharacterControlScript.Gold();
    }
	
	void Update () {
        playerNameText.GetComponent<Text>().text = "Name: " + TitleController.PlayerName();
        goldAmount.GetComponent<Text>().text = "Gold: " + score.ToString();
    }
}
