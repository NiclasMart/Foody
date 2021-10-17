using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    Animator animator;

    private void Awake() 
    {
      animator = GetComponent<Animator>();
    }

    public void ToggleAddCard(bool on)
    {
      animator.SetBool("addCardActive", on);
    }
}
