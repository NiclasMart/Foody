using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

  public void Select(string sceneName)
  {
    SceneManager.LoadScene(sceneName);
  }
}
