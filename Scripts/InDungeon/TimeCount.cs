using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCount : MonoBehaviour {
    private float count = 0;

    public float Count() { return count; }

	void Update () {
        count  += Time.deltaTime;
	}
}
