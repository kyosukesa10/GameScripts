using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHolder : MonoBehaviour {

    public int zombieLimit = 30; //ゾンビ生成最大数
    int zombieAmount = 1;            //ゾンビの数、初期値

    public void DiedZombie() //ゾンビの数を減らす
    {
        if(zombieAmount > 1)
        {
            zombieAmount--;
        }
    }

    public int GetZombieAmount()  //ゾンビの数を取得
    {
        return zombieAmount;
    }

    public int GetZombieLimit()  //ゾンビの最大数を取得
    {
        return zombieLimit;
    }

    public void MakeZombie()   // ゾンビを増やす
    {
        zombieAmount++;
    }
}
