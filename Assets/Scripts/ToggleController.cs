using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEditor;

public class ToggleController : MonoBehaviour
{
    //menu objects
    public GameObject typeMenu;
    public GameObject propMenu;
    public GameObject gradientMenu;
    public GameObject juliaMenu;
    public GameObject colorMenu;

    //generic fields
    public ToggleGroup shaderToggleGroup;
    public GameObject realComponentInput;
    public GameObject imagComponentInput;
    public GameObject rotateSpeedInput;
    public GameObject fractalImage;
    ImageExplorer imageExpScript;

    //shader fields
    public Material fractal;
    public Shader julia;
    public Shader mandelbrot;
    string activeShader;

    //called before the first frame
    void Start()
    {
        //initialize imageExpScript var
        imageExpScript = fractalImage.GetComponent<ImageExplorer>();

        //initialize shader
        fractal.shader = mandelbrot;
        activeShader = "Mandelbrot";
        juliaMenu.SetActive(false);

        //initialize shader properties
        fractal.SetFloat("_Rotate", 0);
        fractal.SetFloat("_Interp", 1);
    }

    public void UpdateShader(string name)
    {
        if (name.Equals("Mandelbrot"))
        {
            activeShader = "Mandelbrot";
            fractal.shader = mandelbrot;
            juliaMenu.SetActive(false);
            imageExpScript.RawArea = new Vector4(0, 0, 5, 5);
        }
        else
        {
            activeShader = "Julia";
            fractal.shader = julia;
            juliaMenu.SetActive(true);
            imageExpScript.RawArea = new Vector4(0, 0, 5, 5);
        }
    }

    //update input fields with rotate toggle value
    public void UpdateRotateToggle(bool value)
    {
        ActivateCompInputFields(!value);
        rotateSpeedInput.SetActive(value);
        fractal.SetFloat("_Rotate", ConvertBool(value));
    }

    public void UpdateInterpToggle(bool value)
    {
        fractal.SetFloat("_Interp", ConvertBool(value));
    }

    public void UpdateManualToggle(bool value)
    {
        colorMenu.SetActive(value);
        propMenu.SetActive(!value);
        typeMenu.SetActive(!value);
        if(activeShader.Equals("Julia")) juliaMenu.SetActive(!value);
        if (value) gradientMenu.transform.localPosition = new Vector3(0, 318, 0);
        else gradientMenu.transform.localPosition = new Vector3(0 ,138,0);
    }

    //activates/deactivates real & imaginary 
    //equation input fields for julia set
    void ActivateCompInputFields(bool active = true)
    {
        realComponentInput.SetActive(active);
        imagComponentInput.SetActive(active);
    }

    //return 1 if true and 0 when false
    int ConvertBool(bool value)
    {
        if (value) return 1;
        return 0;
    }

}
