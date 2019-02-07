using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionToEventObject : MonoBehaviour { // playerにアタッチ（イベントオブジェクトと衝突したとき））

    public RotatePlanet rotatePlanet;
    public FlagChecker flagChecker;
    
    void OnTriggerEnter(Collider other) //  playerに衝突したオブジェクトのタグが"EventObject"ならPlanetの回転を逆にしplayerとぶつからないようにする
    {
        if (other.gameObject.tag == "EventObject")
        {
            rotatePlanet.IsCollisionToPlayer(true);
        }

        if (other.gameObject.name == "DungeonEntrance") // name(DungeonEntrance)に衝突したら
        {
            FlagChecker.SetFlagParameter(0, 0);
            SceneManager.LoadScene("ZombieWorld");
        }
        else if (other.transform.gameObject.name == "WeaponShopDoor" && FlagChecker.GetFlagParameter(1) >= 1 )
        {

        }
        else if (other.transform.gameObject.name == "InnDoor" && FlagChecker.GetFlagParameter(1) >= 1)
        {
            SceneManager.LoadScene("SaveScene");
        }
    }
}
