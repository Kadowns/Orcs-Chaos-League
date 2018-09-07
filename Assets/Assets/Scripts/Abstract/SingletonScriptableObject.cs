using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject {
    private static readonly string Path = typeof(T).Name;
    private static T instance;

    public static T Instance {
        get {
            return instance != null ? instance : (instance = Load());
        }
    } 

    private static T Load() {
#if UNITY_EDITOR
        //C:\Users\Public\Projetos\Galatic Ropers\Horde Fighters League\Cotton Swab Lava\Assets\Resources
        var resourcePath = string.Format("Assets/Resources/{0}.asset", Path);
        if (!AssetDatabase.LoadAssetAtPath<T>(resourcePath)) {
            Debug.LogFormat("Creating new singleton @ {0}", resourcePath);
            var asset = CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, resourcePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return asset;
        }

#endif
        return Resources.Load<T>(Path);
    }
}