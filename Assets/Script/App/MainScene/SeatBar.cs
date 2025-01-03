using System;
using System.Collections;
using System.Collections.Generic;
using App.MainScene;
using Config.ItemsBase;
using Config.LevelsDetail;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using SFramework.Game;
using SFramework.Statics;
using UnityEngine;

public class SeatBar : RootEntity
{
    [SerializeField] private Transform _pos;
    [SerializeField] private Transform _posfill;

    private Levels_Detail _detail;

    private List<Item> _items;
    private MainSceneView _view;
    private List<Sequence> _seqs;
    private const int max = 4;
    private int _index;
    private bool _isLock; //当它是被移动的目标时会进行锁定，不能在执行移出操作

    public bool IsLock
    {
        get { return _isLock; }
        set { _isLock = value; }
    }
    
    public void SetData(Levels_Detail detail)
    {
        this.gameObject.SetActive(true);
        _detail = detail;
        createItem().Forget();
    }

    public void SetItems(List<Item> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            var it = items[i];
            var child = _pos.GetChild(i);
            //不是直接刷新而是需要走过去。需要根据
            Vector3 p1 = child.position;
            Sequence seq = DOTween.Sequence();
            seq.AppendCallback(() => startSeq(it, p1));
            seq.Append(it.transform.DOMove(p1, 1f).SetEase(Ease.Linear).SetDelay(0.1f));
            seq.OnComplete(() => completeSeq(it));
            seq.Play();
        }
        _items = items;
    }

    private async UniTask createItem()
    {
        //在对应的位置初始化item
        for (int i = 0; i < _detail.Items.Count; i++)
        {
            int id = _detail.Items[i];
            var child = _pos.GetChild(i);
            if (child != null)
            {
                var it = await _view.CreateItem(id, child.transform.position, _index % 2 != 0);
                _items.Add(it);
            }
        }
    }

    public void SetIndex(int index)
    {
        _index = index;
    }

    public void Delete(List<Item> items)
    {
        for (int j = 0; j < items.Count; j++)
        {
            var temp = items[j];
            _items.Remove(temp);
        }
    }

    public List<Item> GetItems()
    {
        return _items;
    }

    public bool Merge(SeatBar other)
    {
        // 如果是同一个则直接返回
        if (this == other)
            return false;
        int leftNum = HasLeftNum();
        // 如果没有剩余位置则直接返回
        if (leftNum <= 0)
            return false;

        List<Item> myselfs = GetLastSameItem();
        List<Item> others = other.GetLastSameItem();

        if (others.Count == 0)
            return false;

        if (myselfs.Count == 0 || myselfs[0].IsSame(others[0]))
        {
            //判断是否是两个相同得物体，则可以考虑移动
            List<Item> subs = others;
            if(others.Count > leftNum)
                subs = others.GetRange(0, leftNum);
            movePath(subs, other);
            other.Delete(subs);
            _items.AddRange(subs);
            return true;
        }
        
        return false;
    }

    public int HasLeftNum()
    {
        return max - _items.Count;
    }

    public override void Show()
    {
        if(_items == null)
            _items = new List<Item>();
        else
        {
            _items.Clear();
        }
        if(_seqs == null)
            _seqs = new List<Sequence>();
        else
        {
            _seqs.Clear();
        }
        _view = (MainSceneView)ParentView;
    }
    
    public override void Recycle()
    {
        if (_items != null)
        {
            for (int i = _items.Count - 1; i > 0; i--)
            {
                var item = _items[i];
                ParentView.ReleaseGameObjectDestroy(item.gameObject, false);
            }
        
            _items.Clear();
        }
    }
    
    public List<Item> GetLastSameItem()
    {
        List<Item> resuls = new List<Item>();
        Item it = null;
        for (int i = _items.Count - 1; i >= 0; i--)
        {
            var temp = _items[i];
            if (it == null)
            {
                it = temp;
                resuls.Add(it);
            }
            else
            {
                if (it.IsSame(temp))
                {
                    resuls.Add(temp);
                }
            }
        }

        return resuls;
    }

    public void Select()
    {
        List<Item> result = GetLastSameItem();
        for (int i = 0; i < result.Count; i++)
        {
            var it = result[i];
            it.Select();
        }
    }

    public void Idle()
    {
        List<Item> result = GetLastSameItem();
        for (int i = 0; i < result.Count; i++)
        {
            var it = result[i];
            it.Idle(_index % 2 != 0);
        }
    }
    
    private void movePath(List<Item> mergeItems, SeatBar other)
    {
        _isLock = true;
        int index = _items.Count;
        float duration = 1f;
        // killSequence();
        for (int i = mergeItems.Count - 1; i >= 0; i--)
        {
            var it = mergeItems[i];
            Sequence seq = DOTween.Sequence();
            _seqs.Add(seq);
            Vector3 p1 = new Vector3(0, other.transform.position.y, 0);
            seq.AppendCallback(() => startSeq(it, p1));
            Vector3 p2 = new Vector3(0, transform.position.y, 0);
            seq.Append(it.transform.DOMove(p1, duration).SetEase(Ease.Linear).SetDelay(0.1f * i));
            if (p1 != p2)
            {
                seq.Append(it.transform.DOMove(p2, duration).SetEase(Ease.Linear).SetDelay(0.1f * i));
            }

            int targetIndex = index + i;
            var pos = _pos.GetChild(targetIndex);
            seq.AppendCallback(() => startSeq(it, pos.position));
            seq.Append(it.transform.DOMove(pos.position, duration).SetEase(Ease.Linear).SetDelay(0.1f * i));
            seq.OnComplete(() => completeSeq(it));
            seq.Play();
        }
    }

    private void leaveScene()
    {
        _isLock = true;
        int index = _items.Count;
        float duration = 1f;
        killSequence();
        for (int i = _items.Count - 1; i >= 0; i--)
        {
            var it = _items[i];
            Sequence seq = DOTween.Sequence();
            // _seqs.Add(seq);
            Vector3 p1 = new Vector3(0, transform.position.y, 0);
            seq.AppendCallback(() => startSeq(it, p1));
            Vector3 p2 = new Vector3(0, 4.8f * 0.5f, 0);
            seq.Append(it.transform.DOMove(p1, duration).SetEase(Ease.Linear).SetDelay(0.1f * i));
            if (p1 != p2)
            {
                seq.Append(it.transform.DOMove(p2, duration).SetEase(Ease.Linear).SetDelay(0.1f * i));
            }
            
            Vector3 p3 = new Vector3(-4.3f, 7f, 0); //门的位置
            seq.AppendCallback(() => startSeq(it, p3));
            seq.Append(it.transform.DOMove(p3, duration).SetEase(Ease.Linear).SetDelay(0.1f * i));
            seq.OnComplete(() => completeLeave(it));
            seq.Play();
        }
    }

    private void startSeq(Item it, Vector3 pos)
    {
        bool rt = false;
        if (pos.x > it.transform.position.x)
            rt = true;
        it.Move(rt);
    }

    private void completeSeq(Item it)
    {
        it.Idle(_index % 2 != 0);
        _isLock = false;
        bool rt = IsOperateComplete();
        if (rt)
        {
            leaveScene();
        }
    }

    private void completeLeave(Item it)
    {
        it.Recycle();
        _isLock = false;
        _items.Clear();
        if(_view.IsAllComplete())
            ParentControl.OpenControl(SFStaticsControl.App_GameSuccess_GameSuccessControl);
    }

    private void killSequence()
    {
        for (int i = 0; i < _seqs.Count; i++)
        {
            var seq = _seqs[i];
            seq.Kill();
        }
        
        _seqs.Clear();
    }

    //操作完成检查
    public bool IsOperateComplete()
    {
        if (_items == null || _items.Count == 0)
            return false;
        List<Item> result = GetLastSameItem();
        if (result.Count == max || result.Count == 0)
        {
            return true;
        }

        return false;
    }

    //终局检查
    public bool IsSeatBarComplete()
    {
        if (_items == null || _items.Count == 0)
            return true;
        return false;
    }
}
