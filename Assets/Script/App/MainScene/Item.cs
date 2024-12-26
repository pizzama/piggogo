using System;
using System.Collections;
using System.Collections.Generic;
using Config.ItemsBase;
using Cysharp.Threading.Tasks;
using SFramework.Extension;
using SFramework.Game;
using SFramework.Sprites;
using Spine.Unity;
using UnityEngine;

public class Item : RootEntity
{
    [SerializeField] private SkeletonAnimation _spine;
    [SerializeField] private SpriteSorting _sorting;
    [SerializeField] private int _curIndex; //当前所在位置
    private List<string> waitNames = new List<string>() { "wait1", "wait2", "wait3" };
    private Items_Base _base;

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
        waitNames.ShuffleList();
        _spine.AnimationState.SetAnimation(0, waitNames[0], true);
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
}
