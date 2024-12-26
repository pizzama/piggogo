using SFramework;
using SFramework.Statics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameApp : GameLauncher
{
[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)] //not load unity logo
        // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            SRDebug.Init();
            Scene scene = SceneManager.GetActiveScene();
            if (!scene.name.Equals("Persisitence"))
            {
                SceneManager.LoadScene("Persisitence");
            }

            Debug.unityLogger.logEnabled = true; // whether close log
        }

        protected override void installBundle()
        {
            try
            {
                initAllControl();
                //读取语言，读取用户数据
                SBundleManager.Instance.OpenControl(SFStaticsControl.App_Language_LanguageControl, null, false, "", 0, (control) =>
                {
                    SBundleManager.Instance.OpenControl(SFStaticsControl.App_Inventory_InventoryControl, null, false, "", 0, (control)=>{
                        SBundleManager.Instance.OpenControl(SFStaticsControl.App_MainScene_MainSceneControl);
                    });
                });

            }
            catch (System.Exception err)
            {
                Debug.LogError(err.ToString());
            }

        }
}
