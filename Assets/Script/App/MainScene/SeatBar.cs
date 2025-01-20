using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using App.MainScene;
using Config.LevelsDetail;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using SFramework.Game;
using UnityEngine;

public class SeatBar : RootEntity
{
    [SerializeField] private Transform _pos;
    [SerializeField] private Transform _posfill;

    private Levels_Detail _detail;

    private List<Item> _items;
    private Item _fillItem;
    private MainSceneView _view;
    private List<Sequence> _seqs;
    private const int max = 4;
    private int _index;
    private int _leavecount;

    public int Index
    {
        get { return _index; }
    }
    private bool _isLock; //当它是被移动的目标时会进行锁定，不能在执行移出操作

    public bool IsLock
    {
        get { return _isLock; }
        set { _isLock = value; }
    }

    private float _waittime = 2; 
    
    public async UniTask SetData(Levels_Detail detail)
    {
        this.gameObject.SetActive(true);
        _detail = detail;
        await createItem();
        // createItem().Forget();

        if (_detail.AddItem > 0)
        {
            int iid = ParentView.GetModel<MainSceneModel>().PopDataPool();
            if (iid > 0)
            {
                _fillItem = await _view.CreateItem(iid, _posfill.transform.position, _index % 2 != 0);
                _fillItem.Gray();
            }
        }
    }

    public void SetItems(List<Item> items)
    {
        _leavecount = 0;
        for (int i = 0; i < items.Count; i++)
        {
            var it = items[i];
            var child = _pos.GetChild(i);
            //不是直接刷新而是需要走过去。需要根据
            Vector3 p1 = child.position;
            Sequence seq = DOTween.Sequence();
            it.CaculateTime(p1);
            startSeq(it, p1);
            // seq.AppendCallback(() => startSeq(it, p1));
            seq.Append(it.transform.DOMove(p1, 1f).SetEase(Ease.Linear).SetDelay(0.1f));
            seq.OnComplete(() => completeSeq(it, items.Count));
            seq.Play();
        }
        _items = items;
    }

    private async UniTask createItem()
    {
        var props = _detail.Prop;
        //在对应的位置初始化item
        for (int i = 0; i < _detail.Items.Count; i++)
        {
            int id = _detail.Items[i];
            var child = _pos.GetChild(i);
            if (child != null)
            {
                var it = await _view.CreateItem(id, child.transform.position, _index % 2 != 0);
                it.CaculateTime(child.transform.position);
                _items.Add(it);
                
                //初始化物品的道具
                for (int j = 0; j < props.Count; j++)
                {
                    var pop = props[j];
                    if (pop.Values[1] == (i + 1))
                    {
                        it.Prop = pop.Values.ToArray<int>();
                    }
                }

                it.Show();
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

    public Item GetFist()
    {
        if(_items.Count > 0)
            return _items[_items.Count - 1];
        return null;
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
            for (int i = _items.Count - 1; i >= 0; i--)
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
            if (temp.HasLock()) //如果是锁定的则继续
                break;
            if (temp.HasEgg()) //如果是鸡蛋则继续
                break;
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
                else
                {
                    //如果第二个不相邻则直接退出查找
                    break;
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
        _leavecount = 0;
        // killSequence();
        for (int i = mergeItems.Count - 1; i >= 0; i--)
        {
            var it = mergeItems[i];
            Sequence seq = DOTween.Sequence();
            _seqs.Add(seq);
            // Vector3 p1 = new Vector3(0, other.transform.position.y, 0); // 中间的线
            // seq.AppendCallback(() => startSeq(it, p1)); //
            // Vector3 p2 = new Vector3(0, transform.position.y, 0); //
            // seq.Append(it.transform.DOMove(p1, duration).SetEase(Ease.Linear).SetDelay(0.1f * i));
            // if (p1 != p2)
            // {
                // seq.Append(it.transform.DOMove(p2, duration).SetEase(Ease.Linear).SetDelay(0.1f * i));
            // }

            int targetIndex = index + i;
            var pos = _pos.GetChild(targetIndex);
            float duration = it.CaculateTime(pos.position);

            seq.AppendCallback(() => startSeq(it, pos.position));
            seq.Append(it.transform.DOMove(pos.position, duration).SetEase(Ease.Linear).SetDelay(0.1f * i));
            seq.OnComplete(() => completeSeq(it, mergeItems.Count));
            seq.Play();
        }
    }

    private void leaveScene()
    {
        _isLock = true;
        int index = _items.Count;
        killSequence();
        
        for (int i = _items.Count - 1; i >= 0; i--)
        {
            var it = _items[i];
            Sequence seq = DOTween.Sequence();
            // _seqs.Add(seq);
            // Vector3 p1 = new Vector3(0, transform.position.y, 0);
            // seq.AppendCallback(() => startSeq(it, p1));
            // Vector3 p2 = new Vector3(0, 4.8f * 0.5f, 0);
            // seq.Append(it.transform.DOMove(p1, duration).SetEase(Ease.Linear).SetDelay(0.1f * i));
            // if (p1 != p2)
            // {
                // seq.Append(it.transform.DOMove(p2, duration).SetEase(Ease.Linear).SetDelay(0.1f * i));
            // }
            Vector3 p3 = new Vector3(-4.3f, 7f, 0); //门的位置
            float duration = it.CaculateTime(p3);
            startSeq(it, p3);
            // seq.AppendCallback(() => startSeq(it, p3));
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

    private void completeSeq(Item it, int count)
    {
        _leavecount += 1;
        it.Idle(_index % 2 != 0);
        _isLock = false;
        if (_leavecount >= count)
        {
            _leavecount = 0;
            bool rt = IsOperateComplete();
            if (rt)
            {
                leaveScene();
                _isLock = false;
                _items.Clear();
                ParentView.GetModel<MainSceneModel>().NextComplete(); // 检查是否要增加池子里的数据
                FillItem().Forget();
                if(_view.IsAllComplete())
                    _view.GameSuccess();
            }
        }

       
    }

    private void completeLeave(Item it)
    {
        it.Recycle();
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

    // 数据补位
    public async UniTask FillItem()
    {
        ParentView.GetModel<MainSceneModel>().RefreshDataPool(); //从新洗牌
        _waittime = 2;
        for (var i = 3; i >= 0; i--)
        {
            var child = _pos.GetChild(i);
            if (i == 3)
            {
                if (_fillItem != null)
                {
                    _fillItem.MoveTo(child.transform.position, _index % 2 != 0);
                    _fillItem.Normal();
                    _items.Add(_fillItem);
                }

                continue;
            }
            int iid = ParentView.GetModel<MainSceneModel>().PopDataPool();
            if(iid > 0)
            {
                await UniTask.Delay(100);
                var it = await _view.CreateItem(iid, _posfill.transform.position, _index % 2 != 0);
                it.MoveTo(child.transform.position, _index % 2 != 0);
                _items.Add(it);
            }
        }

        _items.Reverse();

        int iit = ParentView.GetModel<MainSceneModel>().PopDataPool();
        if(iit > 0)
        {
            await UniTask.Delay(1000);
            _fillItem = await _view.CreateItem(iit, _posfill.transform.position, _index % 2 != 0);
            _fillItem.Gray();
        }

    }

    //终局检查
    public bool IsSeatBarComplete()
    {
        if (_items == null || _items.Count == 0)
            return true;
        return false;
    }

    public void NextRound(SeatBar oldbar, SeatBar newbar)
    {
        for (var i = 0; i < _items.Count; i++)
        {
            var it = _items[i];
            it.NextRound(oldbar, newbar);
        }
    }

    private void Update()
    {
        _waittime -= Time.deltaTime;
        if(_waittime <= 0)
        {
            _waittime = 2;
            FixPos();
        }
    }

    public void FixPos()
    {
        if(_items != null)
        {
            for (var i = 0; i < _items.Count; i++)
            {
                var it = _items[i];
                it.FixPos(_index % 2 != 0);
            }
        }

    }
}
