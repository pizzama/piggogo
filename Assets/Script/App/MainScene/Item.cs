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
    [SerializeField] private SkeletonAnimation spine;
    [SerializeField] private Transform propTransform;
    [SerializeField] private Transform bombTransform;
    [SerializeField] private SpriteSorting sorting;
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
        propTransform.gameObject.SetActive(false);
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

    public void NextRound(bool start = false)
    {
        if(_prop != null && _prop.Length > 0)
        {
            if (_prop[0] == 1)
            {
                if(!start)
                    _prop[2] -= 1;
                if (_prop[2] == 0)
                {
                    //游戏结束
                    (ParentView as MainSceneView).GameOver();
                }
                propTransform.gameObject.SetActive(true);
                var text = bombTransform.Find("Num").GetComponent<TextMeshPro>();
                text.text = _prop[2].ToString();
            }
        }
    }

    public bool HasBomb()
    {
        return bombTransform.gameObject.activeSelf == true && propTransform.gameObject.activeSelf == true;
    }

    public void ChangeAboveUI()
    {
        var group = this.GetComponent<SortingGroup>();
        group.sortingLayerName = "AboveUI";
    }

    public void ChangeNormal()
    {
        var group = this.GetComponent<SortingGroup>();
        group.sortingLayerName = "Default";
    }
}
