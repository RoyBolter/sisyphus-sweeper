using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField] private int speed;

    private void Update() {
        if (Input.GetKey("w")) {
            transform.position += Vector3.up * (Time.deltaTime * speed);
        }

        if (Input.GetKey("s")) {
            transform.position += Vector3.down * (Time.deltaTime * speed);
        }

        if (Input.GetKey("d")) {
            transform.position += Vector3.right * (Time.deltaTime * speed);
        }

        if (Input.GetKey("a")) {
            transform.position += Vector3.left * (Time.deltaTime * speed);
        }
    }
}