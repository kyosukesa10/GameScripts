using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileShotButtonScript : MonoBehaviour {

	Image image;
	private bool isShot = false;

	void Start () {
		image = GetComponent<Image>();
	}

	void Update ()
	{
		if (isShot) {
			image.fillAmount -= 1.0f;
//			if (image.fillAmount >= 1) {
//				image.fillAmount = 0;
//			}
			isShot = false;
		}

	}
	public void OnMobileShotButtonClicked(){
		isShot = true;
	}
}
