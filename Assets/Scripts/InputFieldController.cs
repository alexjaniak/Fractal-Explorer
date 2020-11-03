using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

public class InputFieldController : MonoBehaviour {

    //generic fields
    public GameObject image;
    ImageExplorer imageExpScript;
    public Material fractal;

    //shader properties
    int currentMaxIter;
    float currentSpeed;
    float currentRepeat;
    float currentR;
    float currentRotateSpeed;
    Vector4 currentArea;
    Vector4 currentJulia;

    //input fields
    public TMPro.TMP_InputField maxIterInputField;
    public TMPro.TMP_InputField speedInputField;
    public TMPro.TMP_InputField repeatInputField;
    public TMPro.TMP_InputField rInputField;
    public TMPro.TMP_InputField xPosInputField;
    public TMPro.TMP_InputField yPosInputField;
    public TMPro.TMP_InputField scaleInputField;
    public TMPro.TMP_InputField realInputField;
    public TMPro.TMP_InputField imagInputField;
    public TMPro.TMP_InputField rotateSpeedInputField;

    //called before the first frame
    void Start() {
        //initialize input fieldvariables
        //& convert from nullable type (initial properties always have values)
        currentMaxIter = (int)FetchInput(maxIterInputField);
        currentSpeed = (float)FetchInput(speedInputField);
        currentRepeat = (float)FetchInput(repeatInputField);
        currentR = (float)FetchInput(rInputField);
        currentRotateSpeed = (float)FetchInput(rotateSpeedInputField);
        currentArea = (Vector4)FetchInputArea();
        currentJulia = (Vector4)FetchInputJulia();

        //initialize shader properties 
        fractal.SetInt("_MaxIter", currentMaxIter);
        fractal.SetFloat("_Speed", currentSpeed);
        fractal.SetFloat("_Repeat", currentRepeat);
        fractal.SetFloat("_R", currentR);
        fractal.SetFloat("_RotateSpeed", currentRotateSpeed);

        //initialize instance of the ImageExplorer script
        imageExpScript = image.GetComponent<ImageExplorer>();
    }

    //called at fixed intervals
    void FixedUpdate() {
        if (imageExpScript.inputDetected) {
            UpdateInputFields();
        }
        ManageShaderProperties();
    }



    void ManageShaderProperties() {
        //updates shader properties
        //float properties
        SetFloatInShader("_Speed", speedInputField, ref currentSpeed);
        SetFloatInShader("_Repeat", repeatInputField, ref currentRepeat);
        SetFloatInShader("_R", rInputField, ref currentR, true);
        SetFloatInShader("_RotateSpeed", rotateSpeedInputField, ref currentRotateSpeed);

        //MaxIter needs to be passed as an int
        int? inputMaxIter = (int?)FetchInput(maxIterInputField);
        if (inputMaxIter != null && currentMaxIter != inputMaxIter && inputMaxIter > 0) {
            currentMaxIter = (int)inputMaxIter;
            fractal.SetInt("_MaxIter", currentMaxIter);
        }

        //Area needs to be passed as a vector4
        Vector4? nInputArea = FetchInputArea();
        if (nInputArea != null) {
            Vector4 inputArea = (Vector4)nInputArea;
            if (currentArea.magnitude != inputArea.magnitude && !imageExpScript.inputDetected) {
                if (xPosInputField.isFocused || yPosInputField.isFocused || scaleInputField.isFocused) {
                    currentArea = inputArea;
                    imageExpScript.RawArea = currentArea;
                }
            }
        }

        //real & imaginary components need to be passed as a vector4
        Vector4? nInputJulia = FetchInputJulia();
        if (nInputJulia != null && fractal.shader.name.Equals("Fractals/Julia"))
        {
            Vector4 inputJulia = (Vector4) nInputJulia;
            if (currentJulia.magnitude != inputJulia.magnitude)
            {
                if (realInputField.isFocused || imagInputField.isFocused)
                {
                    currentJulia = inputJulia;
                    fractal.SetVector("_c", currentJulia);
                }
            }
        }
    }


    
    //updates float shader property from input field
    void SetFloatInShader(string property, TMPro.TMP_InputField inputField, ref float currentValue, bool positive = false) {
        bool posCondition = true;
        float? input = FetchInput(inputField);

        if (positive) posCondition = input > 0;

        if (input != null && currentValue != input && posCondition) {
            currentValue = (float)input;
            fractal.SetFloat(property, currentValue);
        }
    }

    //returns float (or null) from input field
    public float? FetchInput(TMPro.TMP_InputField input) {
        string text = input.text;
        float value;
        if (!string.IsNullOrWhiteSpace(text) && float.TryParse(text, out value)) return value;
        else return null; //empty input field
    }

    //returns area (or null) from position & scale input fields
    Vector4? FetchInputArea() {
        float? posX = FetchInput(xPosInputField);
        float? posY = FetchInput(yPosInputField);
        float? scale = FetchInput(scaleInputField);
        if (posX == null || posY == null || scale == null) return null;
        else return new Vector4((float)posX, (float)posY, (float)scale, (float)scale);
    }

    //returns real & imaginaryh components from input fields
    Vector4? FetchInputJulia()
    {
        float? real = FetchInput(realInputField);
        float? imaginary = FetchInput(imagInputField);
        if (real == null || imaginary == null) return null;
        else return new Vector4((float)real, (float)imaginary);
    }

    //updates input field text
    void UpdateInputFields() {
        //rewrites text to display new area
        Vector4 imageArea = fractal.GetVector("_Area");
        xPosInputField.text = imageArea.x.ToString();
        yPosInputField.text = imageArea.y.ToString();
        scaleInputField.text = imageArea.z.ToString();
        currentArea = imageArea;

        if (fractal.shader.name.Equals("Fractals/Julia"))
        {
            Vector4 juliaComponents = fractal.GetVector("_c");
            realInputField.text = juliaComponents.x.ToString();
            imagInputField.text = juliaComponents.y.ToString();
            currentJulia = juliaComponents;
        }

        //*bug fix - resets text box origin
        ResetTextBox(xPosInputField);
        ResetTextBox(yPosInputField);
        ResetTextBox(scaleInputField);
        ResetTextBox(realInputField);
        ResetTextBox(imagInputField);
        ResetTextBox(rotateSpeedInputField);
    }

    void ResetTextBox(TMPro.TMP_InputField inputField)
    {
        if (inputField.textComponent.rectTransform.localPosition != Vector3.zero)
        {
            inputField.textComponent.rectTransform.localPosition = Vector3.zero;
        }
    }
}
