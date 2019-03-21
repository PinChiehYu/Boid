using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.localScale = new Vector3(1f, 1f, 1f) * TankManager.Instance.GetTankSize() / 5;		
	}

}
