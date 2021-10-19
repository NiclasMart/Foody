using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
  Animator animator;

  private void Start()
  {
    animator = GetComponent<Animator>();
    SavingSystem.Load("FoodList");
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
      Application.Quit();
  }

  public void ToggleAddCard(bool on)
  {
    animator.SetBool("addCardActive", on);
  }
}
