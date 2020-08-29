using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour {
	public FireEngine f;

    void Update() {
        if(f != null && f.route.Count > 0) {
        	transform.position = (Vector3)f.route[0];
        }
    }
}
