using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InputFieldController : MonoBehaviour {

    //generic fields
    public GameObject image;
    public ToggleGroup shaderToggleGroup;
    ImageExplorer imageExpScript;

    //shader properties
    int currentMaxIter;
    float currentSpeed;
    float currentRepeat;
    float currentR;
    Vector4 currentArea;

    //input fields
    public TMPro.TMP_InputField maxIterInputField;
    public TMPro.TMP_InputField speedInputField;
    public TMPro.TMP_InputField repeatInputField;
    public TMPro.TMP_InputField rInputField;
    public TMPro.TMP_InputField xPosInputField;
    public TMPro.TMP_InputField yPosInputField;
    public TMPro.TMP_InputField scaleInputField;

    //shader fields
    public Material fractal;
    public Shader julia;
    public Shader mandelbrot;
    string currentShader;
    string ActiveShaderName {
        get { return shaderToggleGroup.ActiveToggles().FirstOrDefault().name; }
    }



    //called before the first frame
    void Start() {
        //initialize variables
        //& convert from nullable type (initial properties always have values)
        currentShader = ActiveShaderName;
        currentMaxIter = (int)FetchInput(maxIterInputField);
        currentSpeed = (float)FetchInput(speedInputField);
        currentRepeat = (float)FetchInput(repeatInputField);
        currentR = (float)FetchInput(rInputField);
        currentArea = (Vector4)FetchInputArea();

        //initizalize shader properties 
        if (currentShader.Equals("MandelbrotToggle")) {
            fractal.shader = mandelbrot;
        } else {
            fractal.shader = julia;
        }
        fractal.SetInt("_MaxIter", currentMaxIter);
        fractal.SetFloat("_Speed", currentSpeed);
        fractal.SetFloat("_Repeat", currentRepeat);
        fractal.SetFloat("_R", currentR);

        //instance of the ImageExplorer script
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
        //updates toggled shader
        if (!currentShader.Equals(ActiveShaderName)) {
            if (ActiveShaderName.Equals("MandelbrotToggle")) {
                fractal.shader = mandelbrot;
                currentShader = ActiveShaderName;
            } else {
                fractal.shader = julia;
                currentShader = ActiveShaderName;
            }
        }

        //updates shader properties
        //float properties
        SetFloatInShader("_Speed", speedInputField, ref currentSpeed);
        SetFloatInShader("_Repeat", repeatInputField, ref currentRepeat);
        SetFloatInShader("_R", rInputField, ref currentR, true);

        //MaxIter needs to be passed as an int
        int? inputMaxIter = (int?)FetchInput(maxIterInputField);
        if (inputMaxIter != null && currentMaxIter != inputMaxIter && inputMaxIter > 0) {
            currentMaxIter = (int)inputMaxIter;
            fractal.SetInt("_MaxIter", currentMaxIter);
        }

        //Area needs to be passed as a vector
        Vector4? nInputArea = FetchInputArea();
        if (nInputArea != null) {
            Vector4 inputArea = (Vector4)nInputArea;
            if (currentArea.magnitude != inputArea.magnitude && !imageExpScript.inputDetected) {
                if (xPosInputField.isFocused || yPosInputField.isFocused || scaleInputField.isFocused) {
                    currentArea = inputArea;
                    imageExpScript.SetShaderArea(currentArea);
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
    float? FetchInput(TMPro.TMP_InputField input) {
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

    //updates input field text
    void UpdateInputFields() {
        Vector4 shaderArea = fractal.GetVector("_Area");
        xPosInputField.text = shaderArea.x.ToString();
        yPosInputField.text = shaderArea.y.ToString();
        scaleInputField.text = shaderArea.z.ToString();
        currentArea = shaderArea;
    }
}

//add button increments
//add updateshader