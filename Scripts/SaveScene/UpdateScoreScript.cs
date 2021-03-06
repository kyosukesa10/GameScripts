﻿using MiniJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateScoreScript : MonoBehaviour
{
    public Text rankingBoardName;    //結果の名前を格納するテキスト
    public Text rankingBoardScore;
    public GameObject updateScoreButton;

    AudioSource clickSound;

    private string nameStr = "NAME\n\n";
    private string scoreStr = "SCORE\n\n";

    public string ServerAddress = "kyosukesa10.starfree.jp/game/UpdateScore.php";

    private void Start()
    {
        clickSound = GetComponent<AudioSource>();
    }

    //SendSignalボタンを押した時に実行されるメソッド
    public void SendSignal_Button_Push()
    {
        clickSound.Play();
        StartCoroutine("Access");   //Accessコルーチンの開始
    }

    private IEnumerator Access()
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();                // goldをscoreとして取得
        dic.Add("id", ShowRankingScript.PlayerId().ToString());  //インプットフィールドからidの取得
        dic.Add("name", TitleController.PlayerName());  //タイトルシーンで入力した名前
        dic.Add("score", CharacterControlScript.Gold().ToString());                     // goldをscoreとして取得);

        StartCoroutine(Post(ServerAddress, dic));  // POST
        yield return 0;
    }

    private IEnumerator Post(string url, Dictionary<string, string> post)
    {
        WWWForm form = new WWWForm();                                // データ送信準備
        foreach (KeyValuePair<string, string> post_arg in post)      // 
        {
            form.AddField(post_arg.Key, post_arg.Value);
        }
        WWW www = new WWW(url, form);                                //実際にサーバに接続。データを取得している

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
                nameStr += name + "\n";
                scoreStr += score + "\n";
            }
            updateScoreButton.SetActive(false);
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
                //
                //
                break;
            }
        }
        yield return null;
    }
}