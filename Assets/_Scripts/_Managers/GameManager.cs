using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance;
   private int _currentScene;
   public enum GameStates
   {
      RESTART,
      DEATH,
      NEXTLEVEL,
   }

   private void Awake()
   {
      Instance = this;
      _currentScene = SceneManager.GetActiveScene().buildIndex;
   }


   public void ChangeState(GameStates state)
   {
      switch (state)
      {
         case GameStates.RESTART:
            SceneManager.LoadScene(_currentScene);
            break;
         case GameStates.NEXTLEVEL:
            SceneManager.LoadScene(_currentScene + 1);
            break;
         case GameStates.DEATH:
            print("GAME OVER");
            break;
         
            
      }
   }
   
}
