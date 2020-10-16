using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ToggleController : MonoBehaviour
{
    //generic objects
    public ToggleGroup shaderToggleGroup;

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
        //initialize shader
        currentShader = ActiveShaderName;
        if (currentShader.Equals("MandelbrotToggle")) {
            fractal.shader = mandelbrot;
        } else {
            fractal.shader = julia;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateShader();
    }

    void UpdateShader() {
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
    }
}
