using UnityEngine.UIElements;
using UnityEngine;
using UnityEditor;

namespace com.rk.readme
{
    internal class ReadmeUser : EditorWindow
    {
        [MenuItem("Readme/Setup ReadMe")]
        public static void SetUpReadme()
        {
            var myWindow = GetWindow<ReadmeUser>();
            myWindow.minSize = new Vector2(500, 500);
        }


        VisualElement visualElement;
        Button testWindow;
        VisualTreeAsset visualElementItem;
        ScrollView scrollView;
        TextField tfSearch;

        private void CreateGUI()
        {
            ReadMeManager.instance.Action_DataChange += Action_DataChange;
            visualElement = rootVisualElement;
            visualElement = rootVisualElement;

            VisualTreeAsset visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Packages/com.rk.readme/Editor/UIToolKit/ReadMeUser.uxml");
            visualElement.Add(visualTreeAsset.Instantiate());

            visualElementItem = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Packages/com.rk.readme/Editor/UIToolKit/VisualElementUser.uxml");

            Querys();
        }

        private void Action_DataChange()
        {
            FetchDataFromSO();
        }

        private void Querys()
        {
            testWindow = visualElement.Q<Button>("btnEdit");
            testWindow.clicked += onTestWindowButtonClicked;

            tfSearch = visualElement.Q<TextField>("tfSearch");
            tfSearch.RegisterValueChangedCallback<string>(OnSearchKeywordValueChange);

            scrollView = visualElement.Q<ScrollView>("scrollView");
            
            //fetch datas from SO and genetates into scrollview
            FetchDataFromSO();
        }

        private void OnSearchKeywordValueChange(ChangeEvent<string> evt)
        {
            var searchValue = evt.newValue.ToLower();

            if (string.IsNullOrEmpty(searchValue))
            {
                //generate all data
                FetchDataFromSO();
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
                        var item = visualElementItem.Instantiate();
                        Label title = item.Q<Label>("txtLable");
                        Label body = item.Q<Label>("txtBody");
                        title.text = ReadMeManager.instance.readmeDatas[i].title;
                        body.text = ReadMeManager.instance.readmeDatas[i].description;
                        scrollView.Add(item);
                    }
                }

            }
        }

        private void FetchDataFromSO()
        {
            scrollView.Clear();
            int count = ReadMeManager.instance.readmeDatas.Count;
            for (int i = 0; i < count; i++)
            {
                var item = visualElementItem.Instantiate();
                Label title = item.Q<Label>("txtLable");
                Label body = item.Q<Label>("txtBody");
                title.text = ReadMeManager.instance.readmeDatas[i].title;
                body.text = ReadMeManager.instance.readmeDatas[i].description;
                scrollView.Add(item);
            }
        }

        private void onTestWindowButtonClicked()
        {
            var userEditWindow = GetWindow<ReadmeRoot>();
            userEditWindow.minSize = new Vector2(500, 500);
        }
    }
}