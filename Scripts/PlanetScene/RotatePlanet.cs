using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RotatePlanet : MonoBehaviour
{ //回転させる惑星にアタッチする

	private float rotationSpeed;
	private float forwardSpeed;
	private bool isHumanRun = false;          //　playerのアニメーションを切り替えるための変数
	private bool isCollisionToPlayer = false;
	public static int placesWent = 0;      //行ってた場所をintで表すplaces配列のインデックス
	private float timeleft;

	//タッチ操作
	Dictionary<int, Touch> touch = new Dictionary<int, Touch>(); //指のIDを保存する
	int posControlFingerId = -1; //位置を操作する指のID
	Vector2 startPos = new Vector2(0, 0);
	Vector2 nextPos = new Vector2(0, 0);

	AudioSource footSound;

	Vector3[] places = new Vector3[3];
	// パラメター達
	public bool IsHumanRun() { return isHumanRun; }
	public bool IsCollisionToPlayer() { return isCollisionToPlayer; }
	public void IsCollisionToPlayer(bool value) { isCollisionToPlayer = value; }

	void Start()
	{
		// それぞれの場所から戻ってきたときの位置
		places[0] = new Vector3(355.0f, 136.2f, 66.4f); // 1.ダンジョン
		places[1] = new Vector3(335.3f, 134.7f, 160.6f); // 2.INN
		places[2] = new Vector3(3.8f, 338.3f, 291.9f); // 3.WeaponShop

		footSound = GetComponent<AudioSource>(); //歩く音
	}

	void Update()
	{
		if (placesWent != 0)                         // もしplacesWentが０じゃない（どっかに行って戻ってきたのなら）
		{                                            //その場所から始める
			transform.Rotate(places[placesWent - 1]);
			placesWent = 0;
		}

		TouchCheck ();
		if (isCollisionToPlayer) // playerが"EventObject"と衝突したならPlanetを逆回転させる(playerが後ろに下がったように見える）
		{
			transform.Rotate(new Vector3(5.0f, 0, 0), Space.World);
			isCollisionToPlayer = false;
		}
	}

	public void FootSound()
	{
		//だいたい0.35秒ごとに処理を行う
		timeleft -= Time.deltaTime;
		if (timeleft <= 0.0)
		{
			timeleft = 0.35f;
			footSound.Play();
		}
	}

	void TouchCheck()
	{
		if (Input.touchCount > 0)        //1本以上の指が触れていたら
		{
			foreach (Touch t in Input.touches)            //触れている指の情報を全て取得
			{
				if (t.phase == TouchPhase.Began)                //指が触れた時
				{
					//位置を操作する指のIDの値が無くて、画面左半分をタッチした時
					if (posControlFingerId == -1 && t.position.x < Screen.width / 2)
					{
						//その指を位置を操作する指としてそのIDを保存する
						posControlFingerId = t.fingerId;
					}
					touch.Add(t.fingerId, t);                    //ディクショナリーに指のIDを追加
					startPos = touch[t.fingerId].position;       // 指を触れた瞬間の位置を取得
				}
				if (t.phase == TouchPhase.Moved)  //指を動かした時
				{
					if (posControlFingerId == t.fingerId)//その指が位置を操作する指だった時
					{	
						//前フレームの指の位置との移動量を計算
						nextPos = startPos - t.position;
						TouchMove (nextPos);
					}
					//ディクショナリーの指のID情報を更新
					touch[t.fingerId] = t;
				}
				if (t.phase == TouchPhase.Stationary) // 指が触れてはいるが動いてない時
				{
					if (posControlFingerId == t.fingerId)//その指が位置を操作する指だった時
					{	
						TouchMove (nextPos);
					}
					//					//ディクショナリーの指のID情報を更新
					//					touch[t.fingerId] = t;
				}
				if (t.phase == TouchPhase.Ended)          //指を離した時
				{
					//その指が位置を操作する指だった時、そのIDをリセット
					if (posControlFingerId == t.fingerId) posControlFingerId = -1;
					touch.Remove(t.fingerId);                    //ディクショナリーの指のIDを削除
					isHumanRun = false;                          // playerの移動動作を止める
				}
			}
		}
	}

	public void TouchMove(Vector2 nextPos){ // 移動する処理部分
		FootSound();                                                       // 音が連射されないように一定時間に一回歩く音が鳴るようにする
		isHumanRun = true;                                                 //playerが移動しているなら
		rotationSpeed = nextPos.normalized.x * 50.0f * Time.deltaTime;     //　回転速度   (2Dでx軸、3Dでx軸)
		forwardSpeed = nextPos.normalized.y * 20.0f * Time.deltaTime;      //　前進速度 (2Dでz軸、3Dでy軸)

		transform.Rotate(new Vector3(forwardSpeed, 0, 0), Space.World);   // 前後方向の回転（x軸回転）
		transform.Rotate(new Vector3(0, rotationSpeed, 0), Space.World);  // 左右方向の回転（y軸回転）
	}
}