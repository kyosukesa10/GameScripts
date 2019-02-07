
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItemScript : MonoBehaviour {//GetItemScriptオブジェクトにアタッチ
    public FlagChecker flagChecker;
    public CharacterControlScript characterControlScript;
    AudioSource reloadSound; //アイテムをゲットした時の音
    AudioSource getGoldSound;
    AudioSource getKeySound;
    AudioSource getMilkSound;

    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        reloadSound = audioSources[0];
        getGoldSound = audioSources[1];
        getKeySound = audioSources[2];
        getMilkSound = audioSources[3];
    }

    public void GetItem(GameObject obj) //プレイヤーがアイテムと接触したら名前で識別

    {
        if(obj.name == "Gold1(Clone)")
        {
            getGoldSound.PlayOneShot(getGoldSound.clip);
            characterControlScript.Gold(1);
            Destroy(obj);
        }
        else if (obj.name == "Gold10(Clone)")
        {
            getGoldSound.PlayOneShot(getGoldSound.clip);
            getGoldSound.PlayOneShot(getGoldSound.clip);
            characterControlScript.Gold(10);
            Destroy(obj);
            
        }
        else if (obj.name == "Key(Clone)")
        {
            getKeySound.PlayOneShot(getKeySound.clip);  //撃った音
            FlagChecker.SetFlagParameter(0, 1);
            Destroy(obj);
        }
        else if (obj.name == "Magazine(Clone)")
        {
            reloadSound.PlayOneShot(reloadSound.clip);  //撃った音
            characterControlScript.Ammo(20);
            Destroy(obj);
        }
        else if (obj.name == "Milk(Clone)")
        {
            if(characterControlScript.Life() < characterControlScript.MaxLife())// maxLifeよりlifeが小さい場合lifeを１回復する
            {
                getMilkSound.PlayOneShot(getKeySound.clip);
                characterControlScript.Life(1);
            }
            Destroy(obj);
        }
        Destroy(obj);
    }
}
