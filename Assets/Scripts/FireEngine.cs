using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEngine : MonoBehaviour {
	public List<Vector2> route;

    void Start() {
        this.route = new List<Vector2>();

        // GameObject marker = (GameObject)Instantiate(Resources.Load("prefabs/Marker"));
        // (marker.GetComponent<Marker>()).f = this;

    	transform.GetChild(1).transform.localScale = new Vector3(2f*getHoseDistance(), 2f*getHoseDistance(), 1);
    }

    void Update() {
    	if(this.route.Count > 0) {
	    	Vector3 targetDir = ((Vector3)this.route[0] - transform.position).normalized;

			Vector3 dir = Vector3.Lerp(transform.right, targetDir, Time.deltaTime * Settings.turningSpeed);
			float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
			float headingMult = 1f - Vector2.Angle(transform.right, targetDir) / 180f; // (multiplier is between 0.5 and 1)
	        transform.position += transform.right * Settings.playerSpeed * Time.deltaTime * headingMult;

			if(Vector3.Distance(transform.position, this.route[0]) < Settings.stopDistance)
				this.route.RemoveAt(0);
    	}
    }

    public float getHoseDistance() {
    	return Settings.hoseDistance;
    }
}
