using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DLAM;
using DLBASE;
using DTT.MiniGame.Jigsaw;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class RewardManager : MonoBehaviour
{
    public Transform _plane;
    public GameObject _main;
    public CanvasGroup _mask;
    public Text _amount;
    public static RewardManager Ins;
    private PhotoData _data;
    private int _time;

    public void Awake()
    {
        Ins = this;
        _mask.DOFade(0, 0.2f);
        _mask.GetComponent<Button>().onClick.AddListener(() =>
        {
            GameManager.Instance.EndGame();
            DLDialogManager.Instance.CloseDialog<GameDialog>();
            RewardPresenter.Instance.AddGold(_data.reward[_data.selecetId]);
            MyPuzzlePresenter.Instance.AchivePhoto(_data,_time);
        });
    }

    public void Show(PhotoData data,int time)
    {
        _data = data;
        _time = time;
        _main.SetActive(true);
        _mask.alpha = 0;
        _mask.DOFade(1, 0.2f);
        _amount.text = data.reward[data.selecetId].ToString();
    }
}
