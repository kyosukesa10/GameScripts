using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneControl : MonoBehaviour {

    public GameObject rotatingLogo;
    public Camera camera_object; //カメラを取得
    private RaycastHit hit; //レイキャストが当たったものを取得する入れ物

    private float speed = 10.0f;
    void Update()
    {
        float y = 0; 
        if (Input.anyKey)
        {
            y = Input.GetAxis("Mouse X");
        }
        rotatingLogo.transform.Rotate(new Vector3(0, -3 * Time.deltaTime - y * speed));

        if (Input.GetMouseButtonDown(0)) //マウスがクリックされたら
        {
            Ray ray = camera_object.ScreenPointToRay(Input.mousePosition); //マウスのポジションを取得してRayに代入

            if (Physics.Raycast(ray, out hit))  //マウスのポジションからRayを投げて何かに当たったらhitに入れる
            {
                string objectName = hit.collider.gameObject.name; //オブジェクト名を取得して変数に入れる
                Debug.Log("raycastにあたったオブジェクト" + objectName);
                if(objectName == "StartButton")
                {
                    SceneManager.LoadScene("ZombieWorld");
                }
                Debug.Log(objectName); //オブジェクト名をコンソールに表示
            }
        }
    }
}
