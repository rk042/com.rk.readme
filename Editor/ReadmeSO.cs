using System.Collections.Generic;
using UnityEngine;

namespace com.rk.readme
{
    [CreateAssetMenu(fileName = "ReadMeAssetItem", menuName = "ReadmeAsset")]
    internal class ReadmeSO : ScriptableObject
    {
        public List<ReadmeDataStruct> readmeDataStructs = new List<ReadmeDataStruct>();
    }

    [System.Serializable]
    internal class ReadmeDataStruct
    {
        public string title;
        public string description;

        public ReadmeDataStruct(string title, string description)
        {
            this.title = title;
            this.description = description;
        }
    }
}