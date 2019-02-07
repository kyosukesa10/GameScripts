using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMaker : MonoBehaviour {

    [SerializeField]
    private Transform[] zombieMaker;   //ゾンビ生成球、それぞれの位置が入っている配列
    [SerializeField]
    private GameObject zombiePrefab;  //ゾンビプレファブ
    [SerializeField]
    private GameObject summonsEffectPrefab;  //ゾンビプレファブ
    [SerializeField]
    private Transform target;         //ターゲット（プレイヤ）

    private int makerNumber;     //ゾンビ生成球の数
    private float timeleft = 0;  // 次のゾンビ生成タイムまでの時間

     public ZombieHolder zombieHolder;   //ZombieHolderスクリプトを取得

    void Update () {
        makerNumber = zombieMaker.Length;     //ゾンビ生成機の数
        timeleft -= Time.deltaTime;

        if(timeleft <= 0.0f)
        {
            timeleft = Random.Range(1, 5.0f);                                 // ランダムな時間を空けてゾンビを生成(1～4秒)
            if (zombieHolder.GetZombieAmount() < zombieHolder.GetZombieLimit())//現在のゾンビ数がlimitじゃなければゾンビを作る
            {
                Vector3 relativePos = target.position - transform.position; //プレイヤと玉の位置を比較してベクトルをとる
                int pos = Random.Range(0, makerNumber);                     // ランダムな数(0～3)
                MakeSummonsEffect(pos);                 // エフェクト生成　
                MakeZombie(relativePos, pos);           // ゾンビ生成
            }
        }
	}
    public void MakeSummonsEffect(int pos)
    {
        GameObject summonsEffect = (GameObject)Instantiate(    //ゾンビが生成された玉の位置にエフェクトを作る
            summonsEffectPrefab,                               //　エフェクトプレファブ
            zombieMaker[pos].position,                         //  ランダムな玉の位置(0～3)
            Quaternion.Euler(90, 0, 0)                         // 　下方向
            );
        Destroy(summonsEffect, 2.0f);
    }

    public void MakeZombie(Vector3 relativePos, int pos)
    {
        GameObject newZombie = (GameObject)Instantiate(             //ゾンビをランダムな玉の位置に作る
                    zombiePrefab,                                   //　ゾンビプレファブ
                    zombieMaker[pos].position,                      //  ランダムな玉の位置(0～3)
                    Quaternion.LookRotation(relativePos)            // プレイヤの方向を向かせる
                    );
        //生成したゾンビの親をZombieHolderとする
        newZombie.transform.parent = zombieHolder.transform;
        zombieHolder.MakeZombie();            //ゾンビの数を1増やした
    }
}
