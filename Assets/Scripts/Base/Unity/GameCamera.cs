using System.Collections;
using UnityEngine;

public class GameCamera : MonoBehaviour {

    public float devHeight = 6.4f;
    public float devWidth = 11.36f;
    void Awake() {
        //变更帧数
        Application.targetFrameRate = 20;
    }

    // Use this for initialization
    void Start () {

        float screenHeight = Screen.height;
        Debug.Log ("screenHeight = " + screenHeight);

        //this.GetComponent<Camera>().orthographicSize = screenHeight / 200.0f;

        float orthographicSize = this.GetComponent<Camera> ().orthographicSize;
        Debug.Log ("orthographicSize = " + orthographicSize);

        float aspectRatio = Screen.width * 1.0f / Screen.height;
        Debug.Log ("aspectRatio = " + aspectRatio);

        float cameraWidth = orthographicSize * 2 * aspectRatio;
        Debug.Log ("cameraWidth = " + cameraWidth);

        if (cameraWidth < devWidth) {
            orthographicSize = devWidth / (2 * aspectRatio);
            Debug.Log ("new orthographicSize = " + orthographicSize);
            this.GetComponent<Camera> ().orthographicSize = orthographicSize;
        }
    }

    // Update is called once per frame
}