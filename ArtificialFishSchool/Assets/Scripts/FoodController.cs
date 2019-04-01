using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        if(Random.value < 0.01)
        {
            float tanksize = TankManager.Instance.GetTankSize();
            transform.position = new Vector3(Random.Range(-1 * tanksize, tanksize), Random.Range(-1 * tanksize, tanksize), Random.Range(-1 * tanksize, tanksize));
        }
    }
}
