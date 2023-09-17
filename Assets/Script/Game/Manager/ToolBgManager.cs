using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DTT.MiniGame.Jigsaw;
using UnityEngine;
using UnityEngine.UI;

public class ToolBgManager : MonoBehaviour
{
    public Transform _plane;
    public GameObject _main;
    public CanvasGroup _mask;
    public Image _toolBg;
    public static ToolBgManager Ins;
    private Vector3 _oldpos;

    public void Awake()
    {
        Ins = this;
        _mask.DOFade(0, 0.2f);
        _mask.GetComponent<Button>().onClick.AddListener(() =>
        {
            _mask.DOFade(0, 0.2f);
            _plane.DOMoveY(-1000, 0.2f).OnComplete(() =>
            {
                _main.SetActive(false);
            });
        });
        _oldpos = _toolBg.transform.position;
    }

    public void Show()
    {
        _toolBg.sprite = JigsawManager.Instance._currentConfig.Image; 
        _main.SetActive(true);
        _mask.alpha = 0;
        _mask.DOFade(1, 0.2f);
        _plane.position = new Vector3(Screen.width/2.0f, 0, 0);
        _plane.DOMoveY(_oldpos.y, 0.2f);
    }
}
