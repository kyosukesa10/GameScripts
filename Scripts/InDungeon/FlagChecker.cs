using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagChecker : MonoBehaviour { // FlagCheckerオブジェクトにタッチ
    
    //フラグ状況を整数で保持しておく
    public static int[] flagChecker = { 0, 0, 0 };
    
    public static int GetFlagParameter(int index)
    {
        return flagChecker[index];
    }

    public static void SetFlagParameter(int index, int value)
    {
        flagChecker[index] = value;
    }

    //[index]
    // [0]     ダンジョンの各階のキーを見つけたときその階の階数をセットする
    // [1]     住民との会話によりフラグが解放される
    // → 1 、一度ダンジョンに入ってカギをとって出てこれたらINNが解放され、セーブできるようになる
    // [2]     未定
}
