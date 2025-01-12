using System;
using System.Collections;
using System.Collections.Generic;
using Config.ItemsBase;
using Cysharp.Threading.Tasks;
using SFramework.Extension;
using SFramework.Game;
using SFramework.Sprites;
using Spine.Unity;
using TMPro;
using UnityEngine;

public class Item : RootEntity
{
    [SerializeField] private SkeletonAnimation spine;
    [SerializeField] private Transform propTransform;
    [SerializeField] private Transform bombTransform;
    [SerializeField] private SpriteSorting sorting;
    [SerializeField] private int curIndex; //当前所在位置
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
    }

    // 初始化时候设置
    private void Start() 
    {
        propTransform.gameObject.SetActive(false);
        if(_prop != null && _prop.Length > 0)
        {
            if (_prop[0] == 1)
            {
                propTransform.gameObject.SetActive(true);
                var text = bombTransform.Find("Num").GetComponent<TextMeshPro>();
                text.text = _prop[2].ToString();
            }
        }
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
        spine.AnimationState.SetAnimation(0, "jing", true);
    }

    public void Move(bool isleft)
    {
        if (isleft)
            spine.transform.localScale = new Vector3(-1, 1, 1);
        else
            spine.transform.localScale = new Vector3(1, 1, 1);
        spine.AnimationState.SetAnimation(0, "run", true);
    }

    public void Idle(bool isleft)
    {
        Debug.Log(ParentView);
        if (isleft)
            spine.transform.localScale = new Vector3(-1, 1, 1);
        else
            spine.transform.localScale = new Vector3(1, 1, 1);
        _waitNames.ShuffleList();
        spine.AnimationState.SetAnimation(0, _waitNames[0], true);
    }

    public async UniTask LoadSpine(Items_Base config, bool isleft)
    {
        _base = config;
        //加载动画文件
        SkeletonDataAsset sc = await ParentView.LoadFromBundleAsync<SkeletonDataAsset>("app_pigs_" + config.Anim + ".sfp", config.Anim + "_SkeletonData");
        spine.skeletonDataAsset = sc;
        spine.Initialize(true);
        Idle(isleft);
    }

    public void NextRound()
    {
        
    }
}
