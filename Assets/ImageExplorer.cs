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
        float aspect = (float)Screen.width / (float)Screen.height;
        float scaleX = scale;
        float scaleY = scale;

        if (aspect > 1f) {
            scaleY /= aspect;
        } else {
            scaleX *= aspect;
        }
        material.SetVector("_Area", new Vector4(position.x, position.y, scaleX, scaleY));
    }

    void Zoom() {
        if (Input.GetKeyDown(KeyCode.Equals)) {
            scale *= 0.99f;
        }
        if (Input.GetKeyDown(KeyCode.Minus)) {
            scale *= 1.01f;
        }
    }
}
