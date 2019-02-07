using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalGateScript : MonoBehaviour { // GoalGateにアタッチ
    private Renderer renderer1;
    public FlagChecker flagChecker;
    public GameObject obj;
    private Vector3 pos;

    void Start () {
        renderer1 = GetComponent<Renderer>();
        pos = new Vector3(2f, 1f, 1f);
    }
	
	void Update () {
		var originalmaterial = new Material(renderer1.material);  //GoalGateのRendererの、元の状態を入れておく

        if (FlagChecker.GetFlagParameter(0) == 1)              // ゴールフラグが立っていれば
        {
            Instantiate(
                obj,
                pos,
                Quaternion.identity
                );
            renderer1.material.EnableKeyword("_EMISSION");      //　ゲートを光らせる
        }
        else
        {
            renderer1.material = originalmaterial;              //　そうでないときは元に戻す（光らせない）
        }
    }
}
