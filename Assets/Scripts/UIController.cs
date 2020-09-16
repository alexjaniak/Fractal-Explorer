using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIController : MonoBehaviour
{
    public Material fractal;
    public Shader julia;
    public Shader mandelbrot;

    public TMPro.TMP_InputField maxIterInputField;
    public ToggleGroup shaderToggleGroup;

    //Properties
    int currentMaxIter;
    int inputMaxIter {
        get {
            string text = maxIterInputField.text;
            if (!text.Equals(string.Empty)) return int.Parse(maxIterInputField.text);
            else return -1;
        }
    }

    //Shader 
    string currentShader;
    string ActiveShaderName {
        get { return shaderToggleGroup.ActiveToggles().FirstOrDefault().name; }
    }

    void Start() {
        currentMaxIter = inputMaxIter;
        currentShader = ActiveShaderName;
    }

    void Update() {
        ManageProperties();
        ToggleSelectedShader();
    }

    void ToggleSelectedShader() {
        if(!currentShader.Equals(ActiveShaderName)) {
            if (ActiveShaderName.Equals("MandelbrotToggle")) {
                fractal.shader = mandelbrot;
                currentShader = ActiveShaderName;
            } else {
                fractal.shader = julia;
                currentShader = ActiveShaderName;
            }
        }
    }

    void ManageProperties() {
        if (currentMaxIter != inputMaxIter && inputMaxIter > 0) {
            fractal.SetInt("_MaxIter", inputMaxIter);
            currentMaxIter = inputMaxIter;
        }
    }
}
