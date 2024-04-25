using System.Collections;
using System.Collections.Generic;
using System.IO;
using Carrot;
using UnityEngine;

public class App : MonoBehaviour
{
    [Header("Main")]
    public Carrot.Carrot carrot;

    [Header("Config")]
    public bool is_sell = false;
    private Carrot_Box box = null;
    void Start()
    {
        this.carrot.Load_Carrot();
    }

    public void Btn_all_app()
    {
        if (carrot.os_app == Carrot.OS.Android)
        {
            List<string> list = this.GetInstalledApps();
            this.box = carrot.Create_Box();
            this.box.set_title("List apps");

            for(int i = 0; i < list.Count; i++)
            {
                Carrot_Box_Item item_app = this.box.create_item("item_app_" + i);
                item_app.set_title(list[i].ToString());
                item_app.set_tip(list[i].ToString());
            }
        }
        else
        {
            carrot.Show_msg("Clone Universe", "This function only works on Android devices",Carrot.Msg_Icon.Error);
        }
    }

    public void Btn_run()
    {
        if (Application.platform == RuntimePlatform.Android)
        {

            string filePath = Path.Combine(Application.streamingAssetsPath, "pi.apk");

            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_VIEW"));
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + filePath);
            intentObject.Call<AndroidJavaObject>("setDataAndType", uriObject, "application/vnd.android.package-archive");

            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

            currentActivity.Call("startActivity", intentObject);
            carrot.Show_msg("Run","App pi active",Msg_Icon.Success);
        }
    }

    private List<string> GetInstalledApps()
    {
        List<string> installedApps = new List<string>();

        AndroidJavaObject packageManager = new AndroidJavaClass("android.content.pm.PackageManager");
        AndroidJavaObject context = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity").Call<AndroidJavaObject>("getApplicationContext");
        AndroidJavaObject packages = packageManager.Call<AndroidJavaObject>("getInstalledPackages", 0);

        int packageCount = packages.Call<int>("size");
        for (int i = 0; i < packageCount; i++)
        {
            AndroidJavaObject packageInfo = packages.Call<AndroidJavaObject>("get", i);
            string appName = packageInfo.Get<string>("packageName");
            installedApps.Add(appName);
        }

        return installedApps;
    }

}
