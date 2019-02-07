using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class CameraScript : MonoBehaviour
{
    private const float YAngle_MIN = -89.0f; //カメラのY方向の最小角度
    private const float YAngle_MAX = 1.0f; //カメラのY方向の最大角度

    public Transform target; //追跡するオブジェクトのtransform
    public Vector3 offset;   //追跡対象の中心位置調整用オフセット
    private Vector3 lookAt;   //targetとoffsetによる注視する座標

    private float distance = 10.0f;     //キャラクターとカメラ間の角度
    private float distance_min = 1.0f;  //キャラクターとの最小距離
    private float distance_max = 20.0f; //キャラクターとの最大距離
    private float currentX = 0.0f;      //カメラをX方向に回転させる角度
    private float currentY = 0.0f;      //カメラをY方向に回転させる角度

    //カメラ回転用係数（値が大きいほど回転速度が上がる）
    private float moveX = 2.0f;      //マウスドラッグによるカメラX方向回転係数
    private float moveY = 1.0f;      //マウスドラッグによるカメラY方向回転係数

	//タッチ操作
	Dictionary<int, Touch> touch = new Dictionary<int, Touch>(); //指のIDを保存する
	int angleControlFingerId = -1;                              //方向を操作する指のID
	Vector2 startAngle = new Vector2(0, 0);        // スマホ左画面をタッチした最初の位置
	Vector2 nextAngle = new Vector2(0, 0);         // スマホ左画面を移動した位置

    void Update()
    {
		TouchCheck ();

        distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel"), distance_min, distance_max);
    }

	void TouchCheck()
	{
		if (Input.touchCount > 0)		           //1本以上の指が触れていたら
		{
			foreach (Touch t in Input.touches)		//触れている指の情報を全て取得
			{
				if (t.phase == TouchPhase.Began)	//指が触れた時
				{
				   //カメラ回転を操作する指のIDの値が無くて、画面右半分をタッチした時
					if (angleControlFingerId == -1 && t.position.x > Screen.width / 2)
					{
						//その指をカメラ回を操作する指としてそのIDを保存する
						angleControlFingerId = t.fingerId;
						touch.Add(t.fingerId, t);			//ディクショナリーに指のIDを追加
						startAngle = touch[angleControlFingerId].position;   // 指を触れた瞬間の位置を取得
					}
				}

				if (t.phase == TouchPhase.Moved)				//指を動かした時
				{
					if (angleControlFingerId == t.fingerId)//その指がカメラ回転を操作する指だった時
					{
						nextAngle = t.position - startAngle;//前フレームの指の位置との移動量を計算
						TouchCameraAngle (nextAngle);
						touch[t.fingerId] = t;		  		//ディクショナリーの指のID情報を更新
					}
				}

				if (t.phase == TouchPhase.Ended)				//指を離した時
				{
					//その指が方向を操作する指だった時、そのIDをリセット
					if (angleControlFingerId == t.fingerId) angleControlFingerId = -1;
					nextAngle = new Vector2 (0, 0);
					TouchCameraAngle (nextAngle);
					touch.Remove(t.fingerId);				//ディクショナリーの指のIDを削除
				}
			}
		}
	}
	void TouchCameraAngle(Vector2 nextAngle){ //LateUpdateで使うカメラの旋回角度を求める
		currentX -= nextAngle.normalized.x * moveX; // Input.GetAxis("Mouse X")はクリックしてなくても値を得る
		currentY += nextAngle.normalized.y * moveY;
		currentY = Mathf.Clamp(currentY, YAngle_MIN, YAngle_MAX);  // Mathf.Clamp(1,2,3) 第2，3引数を最大最小値として、第１引数の値を特定する
	}

    void LateUpdate()
    {
        if (target != null)  //targetが指定されるまでのエラー回避
        {
            lookAt = target.position + offset;  //注視座標はtarget位置+ offsetの座標
            //カメラ旋回処理
            Vector3 dir = new Vector3(0, 0, -distance);
            Quaternion rotation = Quaternion.Euler(-currentY, currentX, 0);
            transform.position = lookAt + rotation * dir;  //カメラの位置を変更
            transform.LookAt(lookAt);     //カメラをLookAtの方向に向けさせる
        }
    }
}
