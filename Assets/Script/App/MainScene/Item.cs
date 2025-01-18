using System;
using System.Collections;
using System.Collections.Generic;
using App.MainScene;
using Config.ItemsBase;
using Cysharp.Threading.Tasks;
using SFramework.Extension;
using SFramework.Game;
using SFramework.Sprites;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Item : RootEntity
{
    [SerializeField] private SkeletonAnimation _spine;
    [SerializeField] private Transform _propTransform;
    [SerializeField] private Transform _bombTransform;
    [SerializeField] private Transform _lockTransform;
    [SerializeField] private Transform _keyTransform;
    [SerializeField] private Transform _eggTransform;
    private List<string> _waitNames = new List<string>() { "wait1", "wait2", "wait3" };
    private Items_Base _base;
    private int[] _prop; // 道具数据
    public int[] Prop
    {
        set {
            _prop = value;
        }
    }
    public string ID
    {
        get { return _base.ID; }
    }
    public override void Recycle()
    {
        gameObject.SetActive(false);
    }

    public override void Show()
    {
        _propTransform.gameObject.SetActive(false);
        NextRound(true);
    }

    // 初始化时候设置
    private void Start()  
    {

    }

    public bool IsSame(Item it)
    {
        if (_base.ID == it.ID)
        {
            return true;
        }

        return false;
    }

    public void Select()
    {
        _spine.AnimationState.SetAnimation(0, "jing", true);
    }

    public void Move(bool isleft)
    {
        if (isleft)
            _spine.transform.localScale = new Vector3(-1, 1, 1);
        else
            _spine.transform.localScale = new Vector3(1, 1, 1);
        _spine.AnimationState.SetAnimation(0, "run", true);
    }

    public void Idle(bool isleft)
    {
        if (isleft)
            _spine.transform.localScale = new Vector3(-1, 1, 1);
        else
            _spine.transform.localScale = new Vector3(1, 1, 1);
        _waitNames.ShuffleList();
        _spine.AnimationState.SetAnimation(0, _waitNames[0], true);
    }

    public async UniTask LoadSpine(Items_Base config, bool isleft)
    {
        _base = config;
        //加载动画文件
        SkeletonDataAsset sc = await ParentView.LoadFromBundleAsync<SkeletonDataAsset>("app_pigs_" + config.Anim + ".sfp", config.Anim + "_SkeletonData");
        _spine.skeletonDataAsset = sc;
        _spine.Initialize(true);
        Idle(isleft);
    }

    public void NextRound(bool start = false)
    {
        if(_prop != null && _prop.Length > 0)
        {
            switch (_prop[0])
            {
                case 1:
                    if(!start)
                        _prop[2] -= 1;
                    if (_prop[2] == 0)
                    {
                        //游戏结束
                        (ParentView as MainSceneView).GameOver();
                    }
                    _propTransform.gameObject.SetActive(true);
                    _bombTransform.gameObject.SetActive(true);
                    _eggTransform.gameObject.SetActive(false);
                    _lockTransform.gameObject.SetActive(false);
                    _keyTransform.gameObject.SetActive(false);
                    var text = _bombTransform.Find("Num").GetComponent<TextMeshPro>();
                    text.text = _prop[2].ToString();
                break;
                case 2:
                    _propTransform.gameObject.SetActive(true);
                    _bombTransform.gameObject.SetActive(false);
                    _eggTransform.gameObject.SetActive(false);
                    _lockTransform.gameObject.SetActive(true);
                    _keyTransform.gameObject.SetActive(false);
                break;
                case 3:
                    _propTransform.gameObject.SetActive(true);
                    _bombTransform.gameObject.SetActive(false);
                    _eggTransform.gameObject.SetActive(false);
                    _lockTransform.gameObject.SetActive(false);
                    _keyTransform.gameObject.SetActive(true);
                break;
                case 4:
                    _propTransform.gameObject.SetActive(true);
                    _bombTransform.gameObject.SetActive(false);
                    _eggTransform.gameObject.SetActive(true);
                    _lockTransform.gameObject.SetActive(false);
                    _keyTransform.gameObject.SetActive(false);
                break;
            }
        }
    }

    public bool HasBomb()
    {
        return _bombTransform.gameObject.activeSelf == true && _propTransform.gameObject.activeSelf == true;
    }
}
