using System;
using System.Collections;
using System.Collections.Generic;
using DLAM;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DLBASE;

public class PopUpManager : MonoBehaviour
{
     public Button _homepage;
     public Button _reservie;
     public Transform _plane;
     public GameObject _main;
     public CanvasGroup _mask;
     public static PopUpManager Ins;

     public void Awake()
     {
          Ins = this;
          _homepage.onClick.AddListener(() =>
          {
               GameManager.Instance.EndGame();
               DLDialogManager.Instance.CloseDialog<GameDialog>();
          });
          _reservie.onClick.AddListener(() =>
          {
               _mask.DOFade(0, 0.2f);
               _plane.DOMoveY(-100, 0.2f).OnComplete(() =>
               {
                    _main.SetActive(false);
               });
          });
     }

     public void Show()
     {
          _main.SetActive(true);
          _mask.alpha = 0;
          _mask.DOFade(1, 0.2f);
          _plane.position = new Vector3(Screen.width/2.0f, 0, 0);
          _plane.DOMoveY(Screen.height/2.0f, 0.2f);
     }
}
