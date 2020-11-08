using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Transactions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorController : MonoBehaviour
{
    //generic fields
    public Material fractal;
    public Material gradient;
    public GameObject graph;
    Material graphMaterial;

    //input fields
    public TMPro.TMP_InputField[] colorInputFields;
    public Slider[] colorSliders;

    //color presets
    Vector4[] custom;
    Vector4[] rainbow1;
    Vector4[] grayScale2;

    //updated fields for each object 
    public string propertyName;
    Vector2 propertyIndex;

    //called before the first frame
    void Start()
    {
        //initialize graph material
        graphMaterial = graph.GetComponent<RawImage>().material;

        //initialize color presets
        InitializePresets();
        custom = new Vector4[4];
        fractal.SetFloat("_Gray", 0);
        SetPreset(rainbow1);
    }


    //triggered when slider is changed
    public void OnSliderValueChange(float newValue)
    {
        ChangeGradientProperty(newValue);
        UpdateInputFields(propertyIndex, newValue);
    }

    //triggered when input field is changed
    public void OnInputFieldValueChange(string newInput)
    {
        float newValue;
        if (!string.IsNullOrWhiteSpace(newInput) && float.TryParse(newInput, out newValue))
        {
            ChangeGradientProperty(newValue);
            UpdateSlider(propertyIndex, newValue);
        }
    }

    //triggered when dropdown value is changed
    public void OnDropDownValueChange(int index)
    {
        if(index == 0)
        {
            SetPreset(rainbow1);
            fractal.SetFloat("_Gray", 0);
        }
        else if (index == 1)
        {
            fractal.SetFloat("_Gray", 1);
        }
        else if (index == 2)
        {
            SetPreset(grayScale2);
            fractal.SetFloat("_Gray", 0);
        }
    }



    //called by object - changes vector values & sets shader
    void ChangeGradientProperty(float newValue)
    {
        char colName = propertyName[0];
        if(colName == 'R')
        {
            UpdateShaderMatrixByCol(0, newValue);
        }
        else if (colName == 'G')
        {
            UpdateShaderMatrixByCol(1, newValue);
        }
        else if (colName == 'B')
        {
            UpdateShaderMatrixByCol(2, newValue);
        }
    }

    //changes vector values & sets shader with given column
    void UpdateShaderMatrixByCol(int col, float newValue)
    {
        if (propertyName.Substring(1).Equals("Offset"))
        {
            int row = 0;
            custom[row][col] = newValue;
            UpdateShaders("_A", custom[row]);
            propertyIndex = new Vector2(row, col);
        }
        else if (propertyName.Substring(1).Equals("Amp"))
        {
            int row = 1;
            custom[row][col] = newValue;
            UpdateShaders("_B", custom[row]);
            propertyIndex = new Vector2(row, col);
        }
        else if (propertyName.Substring(1).Equals("Freq"))
        {
            int row = 2;
            custom[row][col] = newValue;
            UpdateShaders("_C", custom[row]);
            propertyIndex = new Vector2(row, col);
        }
        else if (propertyName.Substring(1).Equals("Phase"))
        {
            int row = 3;
            custom[row][col] = newValue;
            UpdateShaders("_D", custom[row]);
            propertyIndex = new Vector2(row, col);
        }
    }

    //updates shaders with new vector
    void UpdateShaders(string vectorName, Vector4 vector)
    {
        fractal.SetVector(vectorName, vector);
        gradient.SetVector(vectorName, vector);
        graphMaterial.SetVector(vectorName, vector);

    }

    //resets & updates input field
    void UpdateInputFields(Vector2 index, float newValue)
    {
        //updates input field with new value
        colorInputFields[ObjectIndex(index)].SetTextWithoutNotify(newValue.ToString());

        //*bug fix - resets text box origin
        ResetTextBox(colorInputFields[ObjectIndex(index)]);
    }

    //updates slider
    void UpdateSlider(Vector2 index, float newValue)
    {
        colorSliders[ObjectIndex(index)].SetValueWithoutNotify(newValue);
    }

    //updates all vectors of the shaders with a preset
    void SetPreset(Vector4[] preset)
    {
        UpdateShaders("_A", preset[0]);
        UpdateShaders("_B", preset[1]);
        UpdateShaders("_C", preset[2]);
        UpdateShaders("_D", preset[3]);

        int n = 0;
        for(int m = 0; m < 4;)
        {
            UpdateInputFields(new Vector2(m,n), preset[m][n]);
            UpdateSlider(new Vector2(m, n), preset[m][n]);
            custom[m][n] = preset[m][n];
            if (n == 2) { n = 0; m++;}
            else n++;
        }
    }



    //specifies property changed when value is changed
    public void FocusedProperty(string property)
    {
        propertyName = property;
    }

    //returns the 1D index of color property matrix
    //used for colorInputFields & colorSliders
    int ObjectIndex(Vector2 index)
    {
        //note: UI matrix is the transposed shader matrix
        Vector2 tIndex = new Vector2(index.y, index.x); //transpose
        if (tIndex.x == 0) return (int) tIndex.y;
        else return (int) ((4*(tIndex.x)+(tIndex.y+1))-1);
    }


    void InitializePresets()
    {
        //the gradient contains a 4x4 matrix
        //with columns R,G,B,A & rows offset, amplitude, frequency, phase
        //the 4th column, Alpha, is always 1

        rainbow1 = new Vector4[] {
            new Vector4(0.5f, 0.5f, 0.5f, 1f), 
            new Vector4(0.5f, 0.5f, 0.5f, 1f), 
            Vector4.one,
            new Vector4(0f, 0.33f, 0.67f, 1f)};

        grayScale2 = new Vector4[] {
            new Vector4(0.5f, 0.5f, 0.5f, 1f),
            new Vector4(0.5f, 0.5f, 0.5f, 1f),
            Vector4.one,
            new Vector4(0f, 0f, 0f, 1f)};
    }


    //fixes bug - resets text box origin
    void ResetTextBox(TMPro.TMP_InputField inputField)
    {
        if (inputField.textComponent.rectTransform.localPosition != Vector3.zero)
        {
            inputField.textComponent.rectTransform.localPosition = Vector3.zero;
        }
    }

}   
