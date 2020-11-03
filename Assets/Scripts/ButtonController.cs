using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {

    //generic fields
    TMPro.TMP_Text text;
    public GameObject image;
    public Material fractal;
    ImageExplorer imageExpScript;

    //tracks if the object is being clicked
    bool pointerDown;

    //called before the first frame
    void Start() {
        //initialize TMP text component
        text = gameObject.GetComponent<TMPro.TMP_Text>();

        //initialize instance of the ImageExplorer script
        imageExpScript = image.GetComponent<ImageExplorer>();
    }



    //Event System methods 
    //triggered when pointer enters the object
    public void OnPointerEnter(PointerEventData pointerEventData) {
        text.fontSize = 15;
    }

    //triggered when pointer exits the object
    public void OnPointerExit(PointerEventData pointerEventData) {
        text.fontSize = 10;
    }

    //triggered when object is clicked
    public void OnPointerDown(PointerEventData pointerEventData) {
        pointerDown = true;

        //runs coroutine that applies with the object
        if (gameObject.name.Equals("XPosIncreaseButton")) {
            StartCoroutine(IncreaseXPosition());
        } 
        else if (gameObject.name.Equals("XPosDecreaseButton")) {
            StartCoroutine(DecreaseXPosition());
        }
        else if (gameObject.name.Equals("YPosIncreaseButton"))
        {
            StartCoroutine(IncreaseYPosition());
        }
        else if (gameObject.name.Equals("YPosDecreaseButton"))
        {
            StartCoroutine(DecreaseYPosition());
        }
        else if (gameObject.name.Equals("ScaleIncreaseButton"))
        {
            StartCoroutine(IncreaseScale());
        }
        else if (gameObject.name.Equals("ScaleDecreaseButton"))
        {
            StartCoroutine(DecreaseScale());
        }
        else if (gameObject.name.Equals("RealIncreaseButton"))
        {
            StartCoroutine(IncreaseReal()); ;
        }
        else if (gameObject.name.Equals("RealDecreaseButton"))
        {
            StartCoroutine(DecreaseReal());
        }
        else if (gameObject.name.Equals("ImagIncreaseButton"))
        {
            StartCoroutine(IncreaseImag());
        }
        else if (gameObject.name.Equals("ImagDecreaseButton"))
        {
            StartCoroutine(DecreaseImag());
        }
    }

    //triggered when object is unclicked
    public void OnPointerUp(PointerEventData pointerEventData) {
        pointerDown = false;
    }



    //coroutines that change property while the object is clicked
    IEnumerator IncreaseXPosition() {
        while (pointerDown) {
            imageExpScript.inputDetected = true;
            Vector4 area = imageExpScript.RawArea;
            area.x += 0.01f/5f * area.z;
            imageExpScript.RawArea = area;
            yield return null;
        }
    }

    IEnumerator DecreaseXPosition() {
        while (pointerDown) {
            imageExpScript.inputDetected = true;
            Vector4 area = imageExpScript.RawArea;
            area.x -= 0.01f / 5f * area.z;
            imageExpScript.RawArea = area;
            yield return null;
        }
    }

    IEnumerator IncreaseYPosition() {
        while (pointerDown) {
            imageExpScript.inputDetected = true;
            Vector4 area = imageExpScript.RawArea;
            area.y += 0.01f / 5f * area.z;
            imageExpScript.RawArea = area;
            yield return null;
        }
    }

    IEnumerator DecreaseYPosition() {
        while (pointerDown) {
            imageExpScript.inputDetected = true;
            Vector4 area = imageExpScript.RawArea;
            area.y -= 0.01f / 5f * area.z;
            imageExpScript.RawArea = area;
            yield return null;
        }
    }

    IEnumerator IncreaseScale() {
        while (pointerDown) {
            imageExpScript.inputDetected = true;
            Vector4 area = imageExpScript.RawArea;
            area.z *= 1f + 0.01f / 5f;
            imageExpScript.RawArea = area;
            yield return null;
        }
    }

    IEnumerator DecreaseScale() {
        while (pointerDown) {
            imageExpScript.inputDetected = true;
            Vector4 area = imageExpScript.RawArea;
            area.z *= 1f - 0.01f / 5f;
            imageExpScript.RawArea = area;
            yield return null;
        }
    }

    IEnumerator IncreaseReal()
    {
        while (pointerDown)
        {
            imageExpScript.inputDetected = true;
            Vector2 juliaComponents = fractal.GetVector("_c");
            juliaComponents.x += 0.01f/5f;
            fractal.SetVector("_c", juliaComponents);
            yield return null;
        }
    }

    IEnumerator DecreaseReal()
    {
        while (pointerDown)
        {
            imageExpScript.inputDetected = true;
            Vector2 juliaComponents = fractal.GetVector("_c");
            juliaComponents.x -= 0.01f/5f;
            fractal.SetVector("_c", juliaComponents);
            yield return null;
        }
    }

    IEnumerator IncreaseImag()
    {
        while (pointerDown)
        {
            imageExpScript.inputDetected = true;
            Vector2 juliaComponents = fractal.GetVector("_c");
            juliaComponents.y += 0.01f/5f;
            fractal.SetVector("_c", juliaComponents);
            yield return null;
        }
    }

    IEnumerator DecreaseImag()
    {
        while (pointerDown)
        {
            imageExpScript.inputDetected = true;
            Vector2 juliaComponents = fractal.GetVector("_c");
            juliaComponents.y -= 0.01f/5f;
            fractal.SetVector("_c", juliaComponents);
            yield return null;
        }
    }


}
