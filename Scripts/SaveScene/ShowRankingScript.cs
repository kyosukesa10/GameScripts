using MiniJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowRankingScript : MonoBehaviour //SaveScoreControlオブジェクトにアタッチ、SaveSceneに移動してきたらランキングを表示
{
    public Text rankingBoardName;    //結果の名前を格納するテキスト
    public Text rankingBoardScore;    //結果のスコアを格納するテキスト
    public GameObject addScoreButton;      //Score登録時にaddScoreButtonをアクティブにするために取得
    public GameObject updateScoreButton;   //Score更新時にupdateScoreButtonをアクティブにするために取得

    private string nameStr = "NAME\n\n";
    private string scoreStr = "SCORE\n\n";
    private int lastScore;  // ランキングのscoreの最低値
    private int savedScore;   // ランキングに登録されているplayerのscore

    public static int playerId = 0;//現在のplayerのDBに登録してあるid
    public static int PlayerId(){ return playerId;}

    public string url = "kyosukesa10.starfree.jp/game/getRanking.php";  //selecttest.phpを指定

    private void Start() {
        StartCoroutine("Access");   //Accessコルーチンの開始
    }

    private IEnumerator Access()
    {
        WWWForm form = new WWWForm();                      // データ送信準備
        WWW www = new WWW(url, form);                      //実際にサーバに接続。データを取得している

        yield return StartCoroutine(CheckTimeOut(www, 3f)); //TimeOutSecond = 3s;

        if (www.error != null)
        {
            Debug.Log("HttpPost NG: " + www.error);
            //そもそも接続ができていないとき

        }
        else if (www.isDone)
        {
            string json = www.text;
            // JSONデータは最初は配列から始まるので、Deserialize（デコード）した直後にリストへキャスト      
            IList dataList = (IList)Json.Deserialize(json);
            // リストの内容はオブジェクトなので、辞書型の変数に一つ一つ代入しながら、処理
            foreach (IDictionary scores in dataList)
            {
                // name,dataは文字列なので、文字列型へキャストしてから変数へ格納
                // id,scoreは整数型だが、intとかlongに変換できないからstringにしてる
                string id = (string)scores["id"];
                string name = (string)scores["name"];
                string score = (string)scores["score"];
                nameStr += name + "\n";                //表示するために名前をランキング順に改行して足していってる
                scoreStr += score + "\n";               //表示するためにスコアををランキング順に改行して足していってる
                if (name == TitleController.PlayerName()) //ランキングの中に現在の名前と同じものがいたらそのidを取得
                {
                    playerId = int.Parse(id);
                    savedScore = int.Parse(score);
                    Debug.Log(playerId);
                }
                lastScore = int.Parse(score);                     // 一番低いスコアの値を取得
            }
            Debug.Log(lastScore);
            if(lastScore < CharacterControlScript.Gold())         // 現在のスコアがランキング最下位より大きかったとき
            {
                if(playerId == 0)
                {
                    addScoreButton.SetActive(true);
                }
                else if(playerId != 0 && CharacterControlScript.Gold() > savedScore)
                {
                    updateScoreButton.SetActive(true);
                }

            }
            //送られてきたデータをテキストに反映
            rankingBoardName.GetComponent<Text>().text = nameStr;
            rankingBoardScore.GetComponent<Text>().text = scoreStr;
        }
    }

    private IEnumerator CheckTimeOut(WWW www, float timeout)
    {
        float requestTime = Time.time;

        while (!www.isDone)
        {
            if (Time.time - requestTime < timeout)
                yield return null;
            else
            {
                Debug.Log("TimeOut");  //タイムアウト
                //タイムアウト処理
                break;
            }
        }
        yield return null;
    }
}
