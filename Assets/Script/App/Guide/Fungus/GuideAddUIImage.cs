using System;
using Fungus;
using SFramework;
using SFramework.Event;
using UnityEngine;
using UnityEngine.UI;

namespace App.Guide.Fungus
{
    [CommandInfo("Guide", 
        "UI Add Image", 
        "新手引导的点击操作")]
    [AddComponentMenu("")]
    public class GuideAddUIImage: Command
    {
        [SerializeField] private GuideLayer _layer;
        [SerializeField] private Sprite _guideImage;
        [SerializeField] private string _imageName;
        [SerializeField] private Vector3 _pos;
        [SerializeField] private Vector3 _scale = new Vector3(1, 1, 1);
        [SerializeField] private Color _color = Color.white;
        public override void OnEnter ()
        {
            if (string.IsNullOrEmpty(_imageName))
                _imageName = _guideImage.name;
            else
                _guideImage.name = _imageName;
            var ctl = SBundleManager.Instance.GetControl<GuideControl>();
            // 创建一个新的GameObject
            GameObject imageObject = new GameObject(_guideImage.name);
            // 添加Image组件
            Image image = imageObject.AddComponent<Image>();
            // 设置Image的Sprite
            image.sprite = _guideImage;
            // 设置Image的颜色（可选）
            image.color = _color;
            image.raycastTarget = false;
            switch(_layer)
            {
                case GuideLayer.GuideFrontLayer:
                    (ctl.View as GuideView)?.AddFrontImage(image, _pos, _scale);
                    break;
                case GuideLayer.GuideMiddelLayer:
                    (ctl.View as GuideView)?.AddMiddelImage(image, _pos, _scale);
                    break;
                case GuideLayer.GuideBackLayer:
                    (ctl.View as GuideView)?.AddBackImage(image, _pos, _scale);
                    break; 
            }
            
            Continue();
        }
        
    }
}