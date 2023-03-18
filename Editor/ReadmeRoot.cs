using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace com.rk.readme
{
    internal class ReadmeRoot : EditorWindow
    {
        VisualElement visualElement;
        VisualTreeAsset visualElementItem;
        ScrollView scrollView;
        TextField tfSearch;
        Button btnSave;
        Button btnEdit;
        
        private void CreateGUI()
        {
            ReadMeManager.instance.Action_DataChange += Action_DataChange;
            visualElement = rootVisualElement;

            VisualTreeAsset visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Packages/com.rk.readme/Editor/UIToolKit/Readme.uxml");
            visualElement.Add(visualTreeAsset.Instantiate());

            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Packages/com.rk.readme/Editor/UIToolKit/Readme.uss");
            visualElement.styleSheets.Add(styleSheet);

            visualElementItem = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Packages/com.rk.readme/Editor/UIToolKit/VisualElementEdit.uxml");
            
            Query();
        }

        private void Action_DataChange()
        {
            GeneratAllData();
        }

        private void GeneratAllData()
        {
            int count = ReadMeManager.instance.readmeDatas.Count;
            scrollView.Clear();
            for (int i = 0; i < count; i++)
            {
                //Debug.Log("data added");
                AddItemToScrollView(i);
            }
        }

        private void Query()
        {
            scrollView = visualElement.Q<ScrollView>("scrollView");
            
            //clear old data
            scrollView.Clear();

            //fetch datas from SO and genetates into scrollview
            GeneratAllData();

            tfSearch = visualElement.Q<TextField>("txSearch");
            tfSearch.RegisterValueChangedCallback<string>(OnSearchValueChange);

            btnSave = visualElement.Q<Button>("btnSave");
            btnSave.clicked += OnBtnSaveClicked;
            btnEdit = visualElement.Q<Button>("btnEdit");
            btnEdit.clicked += OnBtnEditClicked;
        }

        private void AddItemToScrollView(int? i=null)
        {
            var item = visualElementItem.Instantiate();

            TextField title = item.Q<TextField>("tfLable");
            TextField body = item.Q<TextField>("tfBody");

            if (i!=null)
            {
                title.value = ReadMeManager.instance.readmeDatas[i.Value].title;
                body.value = ReadMeManager.instance.readmeDatas[i.Value].description;
            }
            else
            {
                title.value = "";
                body.value = "";
            }

            //add to scrollview
            scrollView.Add(item);
            if (i==null)
            {
                //add to list so we can use to other place
                ReadMeManager.instance.readmeDatas.Add(new ReadmeDataStruct(title.value, body.value));
            }
        }
        private void OnBtnEditClicked()
        {
            AddItemToScrollView();
        }

        private void OnBtnSaveClicked()
        {
            for (int i = 0; i < scrollView.childCount; i++)
            {
                //add to list so we can use to other place
                var data = scrollView[i];
                TextField title = data.Q<TextField>("tfLable");
                TextField body = data.Q<TextField>("tfBody");

                ReadMeManager.instance.readmeDatas[i].title = title.value;
                ReadMeManager.instance.readmeDatas[i].description = body.value;
            }
            
            ReadMeManager.instance.SaveData();
            
        }

        private void OnSearchValueChange(ChangeEvent<string> evt)
        {
            var searchValue = evt.newValue.ToLower();

            if (string.IsNullOrEmpty(searchValue))
            {
                //generate all data
                GeneratAllData();
            }
            else
            {
                //only generate
                scrollView.Clear();
                
                for (int i = 0; i < ReadMeManager.instance.readmeDatas.Count; i++)
                {
                    if (ReadMeManager.instance.readmeDatas[i].title.Contains(searchValue)
                        || ReadMeManager.instance.readmeDatas[i].description.Contains(searchValue))
                    {
                        AddItemToScrollView(i);
                    }
                }

            }
        }
    }
}
