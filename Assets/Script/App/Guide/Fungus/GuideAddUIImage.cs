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
        [SerializeField] private Sprite guideImage;
        [SerializeField] private String imageName;
        [SerializeField] private Vector3 pos;
        [SerializeField] private Vector3 scale = new Vector3(1, 1, 1);
        [SerializeField] private Color color = Color.white;
        public override void OnEnter ()
        {
            if (string.IsNullOrEmpty(imageName))
                imageName = guideImage.name;
            else
                guideImage.name = imageName;
            var ctl = SBundleManager.Instance.GetControl<GuideControl>();
            // 创建一个新的GameObject
            GameObject imageObject = new GameObject(guideImage.name);
            // 添加Image组件
            Image image = imageObject.AddComponent<Image>();
            // 设置Image的Sprite
            image.sprite = guideImage;
            // 设置Image的颜色（可选）
            image.color = color;
            image.raycastTarget = false;
            (ctl.View as GuideView)?.AddUIImage(image, pos, scale);
            Continue();
        }
        
    }
}