using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    void Start() {
        transform.position = new Vector3(0, (Settings.rows-1) / 2f, -10);
        GetComponent<Camera>().orthographicSize = Settings.rows / 2f;
    }

    void Update() {
        transform.position += new Vector3(Settings.camSpeed * Time.deltaTime, 0, 0);
    }
}
