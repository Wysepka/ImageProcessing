using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using ImageProcessing.ImgColSlicer.Quad;

[CustomEditor(typeof(QuadColorFacade))]
public class QuadColorSlicerFacadeEditor : Editor
{
    public const string DataSavePathKey = "Editor.QuadColorDataSavePath";
    public const string SettingsSavePathKey = "Editor.QuadColorSettingsSavePath";
    public const string ResultSavePathKey = "Editor.QuadColorResultSavePath";

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement myInspector = new VisualElement();

        VisualTreeAsset visualTree =
            AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/2D/XML/QuadColorSlicerXML.uxml");
        visualTree.CloneTree(myInspector);

        ObjectField dataField = myInspector.Q<ObjectField>("DataField");
        ObjectField settingsField = myInspector.Q<ObjectField>("SettingsField");
        ObjectField resultField = myInspector.Q<ObjectField>("ResultField");

        Button generateButton = myInspector.Q<Button>("GenerateButton");

        dataField.objectType = typeof(QuadColorSlicerData);
        settingsField.objectType = typeof(QuadColorSlicerSettings);
        resultField.objectType = typeof(QuadColorSlicerResult);

        dataField.RegisterValueChangedCallback(OnDataChanged);
        settingsField.RegisterValueChangedCallback(OnSettingsChanged);
        resultField.RegisterValueChangedCallback(OnResultChanged);


        QuadColorFacade facade = (QuadColorFacade)target;

        if (LoadObject(DataSavePathKey, out QuadColorSlicerData data))
        {
            dataField.value = data;
            if (facade != null)
            {
                facade.SetData(data);
            }
        }

        if (LoadObject(SettingsSavePathKey, out QuadColorSlicerSettings settings))
        {
            settingsField.value = settings;
            if (facade)
            {
                facade.SetSettings(settings);
            }
        }

        if (LoadObject(ResultSavePathKey, out QuadColorSlicerResult result))
        {
            resultField.value = result;
            if (facade)
            {
                facade.SetResult(result);
            }
        }

        generateButton.clicked += OnGenerate;

        return myInspector;
    }


    private void OnDataChanged(ChangeEvent<Object> data)
    {
        SaveObject(data.newValue, DataSavePathKey);
        QuadColorFacade facade = (QuadColorFacade)target;
        if (facade)
        {
            facade.SetData((QuadColorSlicerData)data.newValue);
        }
    }

    private void OnSettingsChanged(ChangeEvent<Object> settings)
    {
        SaveObject(settings.newValue, SettingsSavePathKey);
        QuadColorFacade facade = (QuadColorFacade)target;
        if (facade)
        {
            facade.SetSettings((QuadColorSlicerSettings)settings.newValue);
        }
    }

    private void OnResultChanged(ChangeEvent<Object> result)
    {
        SaveObject(result.newValue, ResultSavePathKey);
        QuadColorFacade facade = (QuadColorFacade)target;
        if (facade)
        {
            facade.SetResult((QuadColorSlicerResult)result.newValue);
        }
    }

    private bool LoadObject<T>(string key, [CanBeNull] out T obj) where T : Object
    {
        if (EditorPrefs.HasKey(key))
        {
            obj = AssetDatabase.LoadAssetAtPath<T>(EditorPrefs.GetString(key));
            return true;
        }

        obj = null;
        return false;
    }

    private void SaveObject(Object obj, string key)
    {
        string path = AssetDatabase.GetAssetPath(obj);
        EditorPrefs.SetString(key, path);
    }

    private void OnGenerate()
    {
        QuadColorFacade facade = (QuadColorFacade)target;
        facade.Generate();
    }
}