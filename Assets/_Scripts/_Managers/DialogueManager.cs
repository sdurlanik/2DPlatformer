using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI _textDisplay;
   [SerializeField] private GameObject _continueButton1;
   [SerializeField] private GameObject _continueButton2;
   [SerializeField] private Animator _dialogAnimator;
   [SerializeField] private string[] _sentences;
   [SerializeField] private float _typingSpeed;
   
   private int _index = 0;

   private void Update()
   {
      if (_textDisplay.text == _sentences[_index]) _continueButton1.SetActive(true);
   }

   IEnumerator Type()
   {
      foreach (var letter in _sentences[_index].ToCharArray())
      {
         _textDisplay.text += letter;
         yield return new WaitForSeconds(_typingSpeed);
      }
   }
   
   // onContinueButton Clicked
   public void NextSentence()
   {
      _continueButton1.SetActive(false);
      if (_index < _sentences.Length - 1)
      {
         _index++;
         _textDisplay.text = "";
         StartCoroutine(Type());
      }
      else
      {
         _textDisplay.text = "";
         _continueButton1.SetActive(false);
         _dialogAnimator.SetBool("isPanelActive", false);
         gameObject.SetActive(false);
      }
      
   }

   private void OnTriggerEnter2D(Collider2D col)
   {
      if (col.gameObject.CompareTag("Player"))
      {
         _dialogAnimator.SetBool("isPanelActive", true);
         StartCoroutine(Type());
      }
   }
}
