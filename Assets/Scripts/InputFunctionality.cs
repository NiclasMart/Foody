using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class InputFunctionality : MonoBehaviour
{
  TMP_InputField fieldIn;

  private void Awake()
  {
    fieldIn = GetComponent<TMP_InputField>();
  }

  public string GetValue()
  {
    if (fieldIn.text == null) return "";
    else return fieldIn.text;
  }

  public void SetValue(string value)
  {
    if (fieldIn == null) fieldIn = GetComponent<TMP_InputField>();
    fieldIn.text = value;
  }

  public void ClearField()
  {
    fieldIn.text = "";
  }

  public void ToggleInteractionState(bool on)
  {
    fieldIn.interactable = on;
  }

  public void Select()
  {
    fieldIn.ActivateInputField();
  }
}
