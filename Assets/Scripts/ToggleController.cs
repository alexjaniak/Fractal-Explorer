using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEditor;

public class ToggleController : MonoBehaviour
{
    //generic objects
    public GameObject juliaProperties;
    public ToggleGroup shaderToggleGroup;
    public Toggle rotate;
    public Toggle interp;
    public GameObject realComponentInput;
    public GameObject imagComponentInput;
    public GameObject rotateSpeedInput;
    public GameObject fractalImage;
    ImageExplorer imageExpScript;

    //generic fields
    float currentInterp = 1;
    float newInterp;
    float currentRotate = 0;
    float newRotate;

    //shader fields
    public Material fractal;
    public Shader julia;
    public Shader mandelbrot;
    string currentShader;
    string ActiveShaderName {
        get { return shaderToggleGroup.ActiveToggles().FirstOrDefault().name; }
    }

    //called before the first frame
    void Start()
    {
        //initialize imageExpScript var
        imageExpScript = fractalImage.GetComponent<ImageExplorer>();

        //initialize shader
        currentShader = ActiveShaderName;
        if (currentShader.Equals("MandelbrotToggle")) {
            fractal.shader = mandelbrot;
            juliaProperties.SetActive(false);
        } else {
            fractal.shader = julia;
            juliaProperties.SetActive(true);
        }

        //initialize shader properties
        fractal.SetFloat("_Rotate", currentRotate);
        fractal.SetFloat("_Interp", currentInterp);
    }

    //called at fixed intervals
    void FixedUpdate()
    {
        UpdateShader();
        UpdateRotateToggle();
        UpdateInterpToggle();
    }

    //updates toggled shader
    void UpdateShader() {
        if (!currentShader.Equals(ActiveShaderName)) {
            if (ActiveShaderName.Equals("MandelbrotToggle")) {
                fractal.shader = mandelbrot;
                currentShader = ActiveShaderName;
                juliaProperties.SetActive(false);
                imageExpScript.RawArea = new Vector4(0, 0, 5, 5);
            } else {
                fractal.shader = julia;
                currentShader = ActiveShaderName;
                juliaProperties.SetActive(true);
                imageExpScript.RawArea = new Vector4(0, 0, 5, 5);
            }
        }
    }

    //update input fields with rotate toggle value
    void UpdateRotateToggle()
    {
        if (rotate.isOn) newRotate = 1;
        else newRotate = 0;

        if(currentRotate != newRotate)
        {
            ActivateCompInputFields((!rotate.isOn));
            rotateSpeedInput.SetActive(rotate.isOn);
            fractal.SetFloat("_Rotate", newRotate);
            currentRotate = newRotate;
        }
    }

    void UpdateInterpToggle()
    {
        if (interp.isOn) newInterp = 1;
        else newInterp = 0;

        if (currentInterp != newInterp)
        {
            fractal.SetFloat("_Interp", newInterp);
            currentInterp = newInterp;
        }
    }

    //activates/deactivates real & imaginary 
    //equation input fields for julia set
    void ActivateCompInputFields(bool active = true)
    {
        realComponentInput.SetActive(active);
        imagComponentInput.SetActive(active);
    }

    
}
