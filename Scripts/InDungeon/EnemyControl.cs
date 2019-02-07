using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControl : MonoBehaviour
{
    public GameObject target; //navmeshのagentが追跡するターゲット
    public ZombieHolder zombieHolder;
    public DropItems dropItems;
    public TimeCount timeCount;
    public GameController gameController;
    Animator animator;          //このオブジェクトのアニメーターを取得
    private NavMeshAgent agent; // nav meshを取得

    const int MaxLife = 2;         //最大HP
    private int life = MaxLife;    //HP
    private bool isDying;
    private bool isIdle = false;
    private bool isCounting = false; //休憩時間もしくは歩く時間をカウントし始めている
    private float nowCount;         // 現在のカウント
    private float finishCount;     // カウント終了時間

    public int Life() { return life; }           //enemyのlifeのgetter
    public void SetLife(int value) { life += value; }
    public bool IsDying() { return life <= 0; }
    public bool IsIdle() { return isIdle; }
    public void IsIdle(bool value) { isIdle = value; }
    public bool IsCounting() { return isCounting; }
    public void IsCounting(bool value) { isCounting = value; }

	void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        nowCount = timeCount.Count();                  // 現在の時間を取得
        if (agent != null)
        {
            Vector3 playerPos = target.transform.position;     // プレイヤの位置
            Vector3 enemyPos = agent.transform.position;       //   敵の位置

            if (Vector3.Distance(enemyPos, playerPos) < 1.5f && !IsDying())   // enemyがplayerへ近づいたとき死んでなければ攻撃する
            {
                agent.SetDestination(agent.transform.position);// enemyの目的地を自分の位置に固定してnavMeshで移動しないようにする
                animator.SetTrigger("attack");
            }

            if (!IsIdle() && !IsDying())                       // Idle状態でなく死んでないなら移動する。
            {
                if (!isCounting)                               // 移動時間をカウント始める前に移動終了時間をセット、とかいろいろ
                {
                    finishCount = nowCount + Random.Range(5, 7);//終了時間
                    animator.SetBool("isIdle", false);          //アニメータをWalkingに変える
                    IsCounting(true);                            //カウント始める
                }
                else{                                          // 現在カウントが終了カウントと等しくなるまで歩く
                    if (nowCount < finishCount)
                    {
                        if (agent.pathStatus != NavMeshPathStatus.PathInvalid)
                        {
                            agent.SetDestination(target.transform.position); // navMeshのagentの目的地をtargetの位置にする         //休憩まで追いかける
                        }
                    }
                    else                                       // 終了したら(現在カウントが終了カウントと等しくなった）
                    {
                        IsIdle(true);                         // Idle状態にする
                        IsCounting(false);                    // カウントやめ
                    }
                }
            }
            else if (IsIdle() && !IsDying())
            {
                agent.SetDestination(agent.transform.position); // navmeshの目的地を自分に変えて動かなくする

                if (!isCounting)                   //休憩時間をカウント始める前に休憩終了時間をセット、とかいろいろ
                {
                    finishCount = nowCount + Random.Range(1, 4);   // 休憩時間を設定(0～3秒)
                    animator.SetBool("isIdle", true);              //アニメータをIdleに変える
                    IsCounting(true);                               //カウント始める
                }
                else
                {
                    if (nowCount >= finishCount)  // ==だと上手く条件式が作動されなかった。Update関数は時間がとびとびになってしまうからと思われる。 
                    {
                        IsIdle(false);              // Idle状態解除
                        IsCounting(false);          // カウントやめ
                    }
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "HitEffect") // 弾が当たっときHitEffectオブジェクトとの衝突判定で呼ばれる
        {
            animator.SetBool("isDamage",true);
            SetLife(-1);
            if (life <= 0 )
            {
                animator.SetTrigger("dying");
                agent.SetDestination(agent.transform.position);  // playerを追いかけないようにする
                GetComponent<CapsuleCollider>().enabled = false;  // コライダーを消してplayerに攻撃できないようにする
                Destroy(gameObject, 4.0f);                        //４秒後にゾンビオブジェクトを消す
                zombieHolder.DiedZombie();                        //ゾンビの数を１減らす
                gameController.SetDefeatNumber();

                int r = Random.Range(0, 2);     //1/2の確率でアイテムをドロップするとする
                if (r == 0) { dropItems.ItemInstantiate(transform.position); }
            }
            else
            {
                animator.SetBool("isDamage", false);
                agent.updatePosition = true;
            }
        }
    }
}
