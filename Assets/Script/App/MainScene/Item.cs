using System.Collections.Generic;
using App.MainScene;
using Config.ItemsBase;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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
    private Vector3 _pos;
    private float _speed = 4; //速度
    private int[] _prop; // 道具数据
    private bool _isMove = false;
    private int _fixtime = 0;

    private bool _isGray = false;
    public bool IsGray
    {
        get {
            return _isGray;
        }

        set 
        {
            _isGray = value;
        }
    }
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
        _prop = null;
    }

    public override void Show()
    {
        _propTransform.gameObject.SetActive(false);
        StartRound();
    }

    // 初始化时候设置
    private void Start()  
    {
        Show();
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
        _isMove = true;
        if (isleft)
            _spine.transform.localScale = new Vector3(-1, 1, 1);
        else
            _spine.transform.localScale = new Vector3(1, 1, 1);
        _spine.AnimationState.SetAnimation(0, "run", true);
    }

    public void Idle(bool isleft)
    {
        _isMove = false;
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

    public void StartRound()
    {
        _propTransform.gameObject.SetActive(false);
        _bombTransform.gameObject.SetActive(false);
        _eggTransform.gameObject.SetActive(false);
        _lockTransform.gameObject.SetActive(false);
        _keyTransform.gameObject.SetActive(false);
        if(_prop != null && _prop.Length > 0)
        {
            switch (_prop[0])
            {
                case 1:
                    _propTransform.gameObject.SetActive(true);
                    _bombTransform.gameObject.SetActive(true);
                    var text = _bombTransform.Find("Num").GetComponent<TextMeshPro>();
                    text.text = _prop[2].ToString();
                break;
                case 2:
                    _propTransform.gameObject.SetActive(true);
                    _lockTransform.gameObject.SetActive(true);
                break;
                case 3:
                    _propTransform.gameObject.SetActive(true);
                    _keyTransform.gameObject.SetActive(true);
                break;
                case 4:
                    _propTransform.gameObject.SetActive(true);
                    _eggTransform.gameObject.SetActive(true);
                break;
            }
        }
    }

    public void NextRound(SeatBar oldbar, SeatBar newbar)
    {
        if(_prop != null && _prop.Length > 0)
        {
            switch (_prop[0])
            {
                case 1:
                    _prop[2] -= 1;
                    if (_prop[2] == 0)
                    {
                        //游戏结束
                        (ParentView as MainSceneView).GameOver();
                    }
                    _propTransform.gameObject.SetActive(true);
                    _bombTransform.gameObject.SetActive(true);
                    var text = _bombTransform.Find("Num").GetComponent<TextMeshPro>();
                    text.text = _prop[2].ToString();
                break;
                case 2:
                break;
                case 3:
                    if (newbar.IsOperateComplete())
                    {
                        (ParentView as MainSceneView).UnLockItem();    
                    }
                break;
                case 4:
                    // 解锁下一个鸡蛋
                    var it = oldbar.GetFist();
                    it?.UnEgg();
                break;
            }
        }
    }

    public void Gray()
    {
        var render = _spine.GetComponent<SkeletonRenderer>();
        if (render!= null)
        {
            render.Skeleton.SetColor(Color.gray);
        }
    }

    public void Normal()
    {
        var render = _spine.GetComponent<SkeletonRenderer>();
        if (render!= null)
        {
            render.Skeleton.SetColor(Color.white);
        }
    }

    public void UnLock()
    {
        _lockTransform.gameObject.SetActive(false);
    }

    public bool HasBomb()
    {
        return _bombTransform.gameObject.activeSelf == true && _propTransform.gameObject.activeSelf == true;
    }

    public bool HasLock()
    {
        return _lockTransform.gameObject.activeSelf == true && _propTransform.gameObject.activeSelf == true;
    }

    public bool HasKey()
    {
         return _keyTransform.gameObject.activeSelf == true && _propTransform.gameObject.activeSelf == true;
    }

    public void UnEgg()
    {
        _eggTransform.gameObject.SetActive(false);
    }

    public bool HasEgg()
    {
        return _eggTransform.gameObject.activeSelf == true && _propTransform.gameObject.activeSelf == true;
    }

    public float CaculateTime(Vector3 pos)
    {
        _pos = pos;
        float distance = Vector2.Distance(pos, transform.position);
        return distance / _speed;
    }

    public void MoveTo(Vector3 pos, bool isleft)
    {
        _isMove = true;
        _pos = pos;
        float distance = Vector2.Distance(pos, transform.position);
        float duration = distance / _speed;
        transform.DOMove(_pos, duration).SetEase(Ease.Linear).SetDelay(0.1f).OnComplete(()=>{
            Idle(isleft);
        });
        Move(isleft);
    }

    public void FixPos(bool isleft)
    {
        if(_pos != transform.position && _isMove == false)
        {
            _fixtime += 1;
            if(_fixtime > 4)
            {
                transform.position = _pos;
                _fixtime = 0;
            }
            else
            {
                // transform.position = _pos;
                bool rt = false;
                if (_pos.x > transform.position.x)
                    rt = true;
                float distance = Vector2.Distance(_pos, transform.position);
                float deltime = distance / _speed;
                transform.DOMove(_pos, deltime).SetEase(Ease.Linear).SetDelay(0.1f).OnComplete(()=>{
                Idle(isleft);
                });
                Move(rt);
            }

        }
    }
}
