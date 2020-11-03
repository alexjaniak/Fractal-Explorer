using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageExplorer : MonoBehaviour {

    //generic fields
    public Material imageMaterial;
    public Vector2 pos; //origin 
    Vector2 smoothPos; 
    public float scale; //zoom
    float smoothScale;
    public bool inputDetected;



    //called at fixed intervals
    private void FixedUpdate() {
        //main updates
        InputController();
        RescaleShader();
    }


    //updates image area
    void RescaleShader() {
        //smooths zoom and position change
        smoothScale = Mathf.Lerp(smoothScale, scale, 0.5f);
        smoothPos = Vector2.Lerp(smoothPos, pos, 0.1f);

        Rect imageRect = GetComponent<RectTransform>().rect;
        float aspectRatio = imageRect.width / imageRect.height;
        float scaleX = smoothScale;
        float scaleY = smoothScale;

        //updates scale based on aspect ratio
        if (aspectRatio > 1f) {
            scaleY /= aspectRatio;
        } else {
            scaleX *= aspectRatio;
        }

        //applies changes to image
        Vector4 scaledArea = new Vector4(smoothPos.x, smoothPos.y, scaleX, scaleY);
        imageMaterial.SetVector("_Area", scaledArea);
    }



    //controls user input 
    void InputController() {
        inputDetected = false; //resets every frame
 
        //Controls zoom
        if (Input.GetKey(KeyCode.X)) {
            scale *= 0.99f;
            inputDetected = true;
        }
        if (Input.GetKey(KeyCode.Z)) {
            scale *= 1.01f;
            inputDetected = true;
        }

        //Controls origin position
        if (Input.GetKey(KeyCode.A)) {
            pos.x -= 0.01f * scale;
            inputDetected = true;
        }
        if (Input.GetKey(KeyCode.D)) {
            pos.x += 0.01f * scale;
            inputDetected = true;
        }
        if (Input.GetKey(KeyCode.W)) {
            pos.y += 0.01f * scale;
            inputDetected = true;
        }
        if (Input.GetKey(KeyCode.S)) {
            pos.y -= 0.01f * scale;
            inputDetected = true;
        }
    }

    public Vector4 RawArea
    {
        get
        {
            return new Vector4(pos.x, pos.y, scale, scale);
        }
        set
        {
            pos = new Vector2(value.x, value.y);
            scale = value.z;
        }
    }
}
