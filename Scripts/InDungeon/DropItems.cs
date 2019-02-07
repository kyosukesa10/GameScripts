using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItems : MonoBehaviour {

    public GameObject[] itemHolder;
    public int[] rarity;
    int sum= 0, num1 = 0, i = 0;
    
	void Start () {   
    }
    
    public GameObject ItemInstantiate(Vector3 pos)
    {
        sum = 0; num1 = 0;
        foreach (int n in rarity){ sum += n;} // すべてのアイテムのレア度の合計値

        int r = Random.Range(0, sum);         //レア度の合計値からランダムな値をとる
        for(i = 0; i < itemHolder.Length; i++)
        {
            num1 = 0;
            for (int j = 0; j <= i ; j++)// 
            {
                num1 += rarity[j];
            }
            if(num1 - r > 0) {
                break;
            }
        }

        GameObject dropItem = (GameObject)Instantiate(
            itemHolder[i],
            pos,
            Quaternion.identity
            );
        return dropItem;
    }
}
