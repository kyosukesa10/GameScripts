using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterControlScript : MonoBehaviour
{
    // playerのステータス
    public static int maxLife = 3;      //最大HP
    public static int life = maxLife;   //HP
    public static int gold;             //所持金
    public static int ammo = 60;        // 残弾数

    //移動処理に必要なコンポーネントを設定
    public Animator animator;                 //モーションをコントロールするためAnimatorを取得
    public CharacterController controller;    //キャラクター移動を管理するためCharacterControllerを取得

    //必要なコンポネントを取得
    public GetItemScript getItem;
    public FlagChecker flagChecker;
    private GameObject muzzle; // 銃口 タマが生成されるポイント

    //移動速度等のパラメータ用変数(inspectorビューで設定)
    public float speed;         //キャラクターの移動速度
    public float jumpSpeed;     //キャラクターのジャンプ力
    public float rotateSpeed;   //キャラクターの方向転換速度
    public float gravity;       //キャラにかかる重力の大きさ
    public float shotDistance; //銃の射撃可能距離

    //Touch関数用
//    public float sensitivity = 1.0f;    //マウス感度
//    public GameObject mainCamera;    //メインカメラオブジェクト
//    public float upMaxAngle = 30.0f;    //メインカメラのX軸の振れ幅
//    public float downMaxAngle = -30.0f;
	private bool isShot = false;
	public Animator mobileShotButtonAnimator;

    //タッチ操作
    Dictionary<int, Touch> touch = new Dictionary<int, Touch>(); //指のIDを保存する
    int posControlFingerId = -1;                              //方向を操作する指のID
	Vector2 startPos = new Vector2(0, 0);        // スマホ左画面をタッチした最初の位置
	Vector2 nextPos = new Vector2(0, 0);         // スマホ左画面を移動した位置

    Vector3 targetDirection;        //移動する方向のベクトル
    Vector3 moveDirection = Vector3.zero;
    private float timeleft;         // 処理を一定時間毎にするときに残り時間を表すのに使われる

    AudioSource shotSound;
    AudioSource footSound;
    AudioSource deathAgonySound;

    public GameObject hitEffectPrefab;//エフェクトのプレファブ
    public GameObject shotEffectPrefab;//エフェクトのプレファブ
    public Vector3 effectRotation; //エフェクト回転

    public int MaxLife() { return maxLife; }           //プレイヤのlifeのgetter
    public void MaxLife(int value) { maxLife += value; }
    public int Life() { return life; }                 //プレイヤのlifeのgetter
    public void Life(int value)
    {
        life += value;
        if (life < 0) { life = 0; }
    }
    public bool IsDying() { return life <= 0; }        //プレイヤのisDyingのgetter
    public static int Gold() { return gold; }                //プレイヤのGoldのgetter setter
    public void Gold(int value) { gold += value; }
    public int Ammo() { return ammo; }                 //プレイヤの残弾数のgetter setter
    public void Ammo(int value)
    {
        ammo += value;
        if (life < 0) { life = 0; }
    }

    void Start()
	{
        muzzle = transform.Find("Pistol Run/Player/muzzle").gameObject;//プレイヤの子としてからのオブジェクトを銃口の位置とする
        AudioSource[] audioSources = GetComponents<AudioSource>();
        shotSound = audioSources[0];
        footSound = audioSources[1];
        deathAgonySound = audioSources[2];
    }

    void Update()
    {
        TouchCheck();    //touch用関数
        Shot();           //射撃用関数
		if(Mathf.Abs(nextPos.magnitude) > 0){
        controller.Move(moveDirection * Time.deltaTime);       //最終的な移動処理
		}
//		Debug.Log("move : " + controller.Move(moveDirection * Time.deltaTime));
    }

//    void MoveControl()
//    {//★進行方向計算 キーボード入力を取得
//        float v = Input.GetAxisRaw("Vertical");         //InputManagerの↑(1)↓(-1)の入力       
//        float h = Input.GetAxisRaw("Horizontal");       //InputManagerの←(1),→(-1)の入力 
//
//        //カメラの正面方向ベクトルからY成分を除き、正規化してキャラが走る方向を取得
//        Vector3 forward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
//        Vector3 right = Camera.main.transform.right; //カメラの右方向を取得
//
//        //カメラの方向を考慮したキャラの進行方向を計算
//        targetDirection = h * right + v * forward;
//
//        //★地上にいる場合の処理
//        if (controller.isGrounded)
//        {
//            //移動のベクトルを計算
//            moveDirection = targetDirection * speed;
////            //Jumpボタンでジャンプ処理
////            if (Input.GetButton("Jump"))
////            {
////                moveDirection.y = jumpSpeed;
////            }
////        }
////        else        //空中操作の処理（重力加速度等）
////        {
////            float tempy = moveDirection.y;
////            //(↓の２文の処理があると空中でも入力方向に動けるようになる)
////            //moveDirection = Vector3.Scale(targetDirection, new Vector3(1, 0, 1)).normalized;
////            //moveDirection *= speed;
////            moveDirection.y = tempy - gravity * Time.deltaTime;
//        }
//
//        //★走行アニメーション管理
//        if (v > .1 || v < -.1 || h > .1 || h < -.1) //(移動入力があると)
//        {
//            animator.SetBool("isRun", true); //キャラ走行のアニメーションON
//            FootSound(); // 音が連射されないように一定時間に一回歩く音が鳴るようにする
//        }
//        else    //(移動入力が無いと)
//        {
//            animator.SetBool("isRun", false); //キャラ走行のアニメーションOFF
//        }
//    }
//
//    void RotationControl()  //キャラクターが移動方向を変えるときの処理
//    {
//        Vector3 rotateDirection = moveDirection;
//        rotateDirection.y = 0;
//
//        //それなりに移動方向が変化する場合のみ移動方向を変える
//        if (rotateDirection.sqrMagnitude > 0.01)
//        {
//            //緩やかに移動方向を変える
//            float step = rotateSpeed * Time.deltaTime;
//            Vector3 newDir = Vector3.Slerp(transform.forward, rotateDirection, step);
//            transform.rotation = Quaternion.LookRotation(newDir);
//        }
//    }

    void Shot() // Shot関数　エディタと実機で処理を分ける
    {
		if(isShot){
				ShotMain();
			isShot = false;
		}
    }

	public void OnMobileShotButtonClicked(){ // 実機の時Shotボタンが押されたら
		mobileShotButtonAnimator.SetTrigger("Pressed");
		isShot = true;
    }

    void ShotMain(){
		if (ammo > 0)
		{
			Ammo(-1);                           //残弾数を１減らす
			animator.SetTrigger("ShotTrigger"); //キャラ走行のアニメーションON
			GetShotEffect(muzzle.transform.position, shotEffectPrefab); // 銃を撃った時のエフェクトを生成
			RaycastHit hit;  //レイキャストによる情報を得るための構造体

			var radius = 0.5f; //レイ球の半径
			var isHit = Physics.SphereCast(transform.position, radius, transform.forward * 10, out hit);
			if (isHit) //プレイヤから正面に向けて撃つ
			{
				if (hit.collider.tag == "Enemy")       //当たったオブジェクトのタグがEnemy
				{
					if (hit.distance < shotDistance)    // 距離が射撃可能距離なら
					{
						GetShotEffect(hit.point + new Vector3(0, 1.0f, 0), hitEffectPrefab);
					}
					Debug.DrawLine(transform.position, hit.transform.position, Color.red, 5.0f);//カメラからhitしたオブジェクトへ赤の光線を１秒はなつ
				}
			}
		}
		else if (ammo == 0)
		{
			// 弾切れ状態　音を鳴らしたい
		}
	}
    private void OnCollisionEnter(Collision col)    // Enemyとの衝突判定
    {
        if (col.gameObject.tag == "Enemy")
        {
            animator.SetTrigger("damage"); //キャラ
            Life(-1);
            if (life <= 0)
            {
                animator.SetTrigger("dying");
                deathAgonySound.PlayOneShot(deathAgonySound.clip);
                Invoke("ReturnToTitle", 3.0f);
            }
        }
    }
    private void OnTriggerEnter(Collider other) // アイテムプレファブのcolliderはisTrigger
    {
        if (other.gameObject.tag == "Item")
        {
            getItem.GetItem(other.gameObject);
        }
        else if (other.gameObject.name == "GoalGate" && FlagChecker.GetFlagParameter(0) == 1) //ワープゲートに触れていて、フラグが立っていれば
        {
            SceneManager.LoadScene("Planet");
            FlagChecker.SetFlagParameter(1, 1);
            RotatePlanet.placesWent = 1;
        }
        else if (other.gameObject.name == "GameOverArea")
        {
            animator.SetTrigger("dying");
            deathAgonySound.PlayOneShot(deathAgonySound.clip);
            Invoke("ReturnToTitle", 3.0f);
        }
    }

    private void ReturnToTitle()   //タイトルシーンに戻る
    {
        life = maxLife;
        ammo = 60;
        gold = 0;
        FlagChecker.SetFlagParameter(0, 0);
        SceneManager.LoadScene("Title");
    }

    void GetShotEffect(Vector3 pos, GameObject obj)
    {
        shotSound.PlayOneShot(shotSound.clip);  //撃った音
        if (obj != null)   // 当たったエフェクトを生成
        {
            Instantiate(       //hitした位置にエフェクトを生成
                obj,
                pos,
                Quaternion.Euler(effectRotation)
                );
        }
    }
    public void FootSound()         // 歩いている時の音　だいたい0.35秒ごとに処理を行う
    {
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
//					Debug.Log ("指が触れた");
					//位置を操作する指のIDの値が無くて、画面左半分をタッチした時
					if (posControlFingerId == -1 && t.position.x < Screen.width / 2)
					{
						posControlFingerId = t.fingerId;		     //その指を位置を操作する指としてそのIDを保存する
						touch.Add(t.fingerId, t);                    //ディクショナリーに指のIDを追加
						startPos = touch[posControlFingerId].position;   // 指を触れた瞬間の位置を取得
					}
				}
				if (t.phase == TouchPhase.Moved)  //指を動かした時
				{
//					Debug.Log ("指が動いている");
					if (posControlFingerId == t.fingerId)//その指が位置を操作する指だった時
					{	
						//前フレームの指の位置との移動量を計算
						nextPos = t.position - startPos;
						TouchPlayerMove (nextPos);
					}
					touch[t.fingerId] = t;			//ディクショナリーの指のID情報を更新
				}
				if (t.phase == TouchPhase.Stationary) // 指が触れてはいるが動いてない時
				{
//					Debug.Log ("指が動いていない");
					if (posControlFingerId == t.fingerId)//その指が位置を操作する指だった時
					{
						TouchPlayerMove (nextPos);
                    }
                    //ディクショナリーの指のID情報を更新
                    touch[t.fingerId] = t;
                }
                //指を離した時
                if (t.phase == TouchPhase.Ended)
                {
//					Debug.Log ("指が離れた");
                    //その指が位置を操作する指だった時、そのIDをリセット
                    if (posControlFingerId == t.fingerId) posControlFingerId = -1;
					nextPos = new Vector2(0,0);
					TouchPlayerMove(nextPos);
					//ディクショナリーの指のIDを削除
                    touch.Remove(t.fingerId);
                }
            }
        }
    }
	public void TouchPlayerMove(Vector2 nextPos){
		Vector3 forward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
		Vector3 right = Camera.main.transform.right;
		targetDirection = nextPos.normalized.x * right + nextPos.normalized.y * forward;
//		Debug.Log ("targetDirecton; " + targetDirection);
//		Debug.Log ("targetDirecton.magnitude; " + targetDirection.magnitude);
		if (controller.isGrounded) {
			moveDirection = targetDirection * speed;//移動のベクトルを計算
//			Debug.Log ("moveDirecton; " + moveDirection);
	        }
	        else        //空中操作の処理（重力加速度等）
	        {
	            float tempy = moveDirection.y;
	            moveDirection.y = tempy - gravity * Time.deltaTime;
	        }

		Vector3 rotateDirection = moveDirection;
        rotateDirection.y = 0;

        //それなりに移動方向が変化する場合のみ移動方向を変える
        if (rotateDirection.sqrMagnitude > 0.01)
        {
            //緩やかに移動方向を変える
            float step = rotateSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.Slerp(transform.forward, rotateDirection, step);
            transform.rotation = Quaternion.LookRotation(newDir);
        }

		//走行アニメーション管理
		if (Mathf.Abs(nextPos.normalized.x) > 0 || Mathf.Abs(nextPos.normalized.y) > 0) //(移動入力があると)
		{
			animator.SetBool("isRun", true); //キャラ走行のアニメーションON
			FootSound(); // 音が連射されないように一定時間に一回歩く音が鳴るようにする
		}
		else    //(移動入力が無いと)
		{
			animator.SetBool("isRun", false); //キャラ走行のアニメーションOFF
		}
	}
}