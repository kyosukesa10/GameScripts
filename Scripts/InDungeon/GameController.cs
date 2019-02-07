using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour  //Scene:ZombieWorld GameControllerオブジェクトにアタッチ
{
    public CharacterControlScript characterControlScript;
    public TimeCount timeCount;

    public Text goldLabel; // GoldのUI表示用
    public Text ammoLabel;
    public Text stateLabel;
    public Text lifeLabel;
    public Text defeatNumberLabel;

    private int defeatNumber;
    public void SetDefeatNumber() { defeatNumber++; }
    public int GetDefeatNumber() { return defeatNumber; }

    void Update()
    {
        lifeLabel.text = "Life: " + characterControlScript.Life() + "/" + characterControlScript.MaxLife();
        goldLabel.text = "G: " + CharacterControlScript.Gold();
        ammoLabel.text = "残弾数: " + characterControlScript.Ammo();
        stateLabel.text = "ダンジョン１階";
        defeatNumberLabel.text = "倒した数: " + defeatNumber;
    }
}
