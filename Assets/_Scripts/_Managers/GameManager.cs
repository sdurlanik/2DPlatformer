using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance;
   private int _currentScene;
   [SerializeField] private TextMeshProUGUI _appleText;
   [SerializeField] private TextMeshProUGUI _bananaText;
   [SerializeField] private GameObject _shopPanel;


   private int _appleCount = 0;
   private int _bananaCount = 0;

   public int BananaCount
   {
      get { return _bananaCount; }
      set { _bananaCount = value; }
   }

   public TextMeshProUGUI BananaText
   {
      get {return _bananaText;}
      set { _bananaText = value; }
   }

   private bool _isMarketOpen; 
   public enum GameStates
   {
      RESTART,
      DEATH,
      NEXTLEVEL,
      PAUSE,
   }

   private void Awake()
   {
      Instance = this;
      _currentScene = SceneManager.GetActiveScene().buildIndex;
   }

   private void Update()
   {
      
      ShortCuts();

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
         case GameStates.PAUSE:
            Time.timeScale = _isMarketOpen ? 0 : 1;
            break;
         
            
      }
   }

   public void AppleUiCount()
   {
      _appleCount++;
      _appleText.text = "X" + _appleCount.ToString();
   }


   #region ButtonClickFuncs

   public void OnBananaBuyButtonClick()
   {
      if (_appleCount <5) Debug.Log("Elma sayısı yetersiz");
      else
      {
         _appleCount -= 5;
         _bananaCount++;
         
         _appleText.text = "X" + _appleCount.ToString();
         _bananaText.text = "X" + _bananaCount.ToString();
      }
      
   }

   public void OnMarketButtonClick()
   {
      _isMarketOpen = !_isMarketOpen;
      _shopPanel.SetActive(_isMarketOpen);
      ChangeState(GameStates.PAUSE);
   }

   private void ShortCuts()
   {
      if (Input.GetKeyDown(KeyCode.M))
      {
         _isMarketOpen = !_isMarketOpen;
         _shopPanel.SetActive(_isMarketOpen);
         ChangeState(GameStates.PAUSE);
      }
   }

   #endregion

}
