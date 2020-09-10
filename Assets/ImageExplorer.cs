using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageExplorer : MonoBehaviour {
    public Material material;
    public Vector2 position;
    public float scale;

    private void FixedUpdate() {
        Zoom();
        Resize();
    }

    void Resize() {
        float aspectRatio = (float)Screen.width / (float)Screen.height;
        float scaleX = scale;
        float scaleY = scale;

        if (aspectRatio > 1f) {
            scaleY /= aspectRatio;
        } else {
            scaleX *= aspectRatio;
        }
        material.SetVector("_Area", new Vector4(position.x, position.y, scaleX, scaleY));
    }

    void Zoom() {
        //Controls zoom
        if (Input.GetKey(KeyCode.Equals)) {
            scale *= 0.99f;
        }
        if (Input.GetKey(KeyCode.Minus)) {
            scale *= 1.01f;
        }

        //Controls 
        if (Input.GetKey(KeyCode.A)) {
            position.x -= 0.01f * scale;
        }
        if (Input.GetKey(KeyCode.D)) {
            position.x += 0.01f * scale;
        }
        if (Input.GetKey(KeyCode.W)) {
            position.y += 0.01f * scale;
        }
        if (Input.GetKey(KeyCode.S)) {
            position.y -= 0.01f * scale;
        }
    }
}
