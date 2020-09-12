using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageExplorer : MonoBehaviour {
    public Material material;
    public Vector2 position;
    public float scale;
    float smoothScale;
    Vector2 smoothPosition;

    private void FixedUpdate() {
        UpdateShader();
        InputController();
    }

    void UpdateShader() {
        smoothScale = Mathf.Lerp(smoothScale, scale, 0.5f);
        smoothPosition = Vector2.Lerp(smoothPosition, position, 0.1f);

        float aspectRatio = (float)Screen.width / (float)Screen.height;
        float scaleX = smoothScale;
        float scaleY = smoothScale;

        if (aspectRatio > 1f) {
            scaleY /= aspectRatio;
        } else {
            scaleX *= aspectRatio;
        }
        material.SetVector("_Area", new Vector4(smoothPosition.x, smoothPosition.y, scaleX, scaleY));
    }

    void InputController() {

        //Controls zoom
        if (Input.GetKey(KeyCode.Equals)) {
            scale *= 0.99f;
        }
        if (Input.GetKey(KeyCode.Minus)) {
            scale *= 1.01f;
        }

        //Controls origin position
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

//add higher percision double/decimal/BigInteger
//add colors
//add julia set alternatives (future itererations)
//add tetration (explore it)