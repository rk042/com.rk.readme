using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace com.rk.readme
{
    internal class ReadMeManager
    {
        private const string assetPath = "Assets/ReadmePackageAsset/ReameSO.asset";
        private ReadmeSO readmeSO;
        
        public List<ReadmeDataStruct> readmeDatas = new();
        public System.Action Action_DataChange;
        
        private static ReadMeManager _instance;
        public static ReadMeManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ReadMeManager();
                    if (!AssetDatabase.IsValidFolder("Assets/ReadmePackageAsset/"))
                    {
                        AssetDatabase.CreateFolder("Assets", "ReadmePackageAsset");
                        var mySo = ScriptableObject.CreateInstance<ReadmeSO>();
                        AssetDatabase.CreateAsset(mySo, "Assets/ReadmePackageAsset/ReameSO.asset");
                        AssetDatabase.SaveAssets();
                        Debug.Log("Asset Generated");
                    }
                    _instance.readmeSO = AssetDatabase.LoadAssetAtPath<ReadmeSO>("Assets/ReadmePackageAsset/ReameSO.asset");
                    _instance.readmeDatas = _instance.readmeSO.readmeDataStructs;
                }
                return _instance;
            }
        }
        public void SaveData()
        {
            readmeDatas.RemoveAll(f => f.title == "" && f.description == "");
            Debug.Log("save data.....");
            if (readmeSO!=null)
            {
                EditorUtility.SetDirty(readmeSO);
            }
            Action_DataChange?.Invoke();
            AssetDatabase.SaveAssets();
        }
    }
}