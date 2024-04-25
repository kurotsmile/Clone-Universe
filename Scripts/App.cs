using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class App : MonoBehaviour
{
    [Header("Main")]
    public Carrot.Carrot carrot;

    [Header("Config")]
    public bool is_sell = false;

    void Start()
    {
        this.carrot.Load_Carrot();
    }


    public void Btn_all_app()
    {
        
    }

    public void Btn_run()
    {

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
