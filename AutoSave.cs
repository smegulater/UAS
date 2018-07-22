using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;

public class AutoSave : EditorWindow {

    public double AutoSaveInterval = 10;

    public bool InPlayMode;
    public bool SaveInPlayMode;
    public bool ShowOptions;

    public DateTime LastSave;
    public DateTime CurrentTime;
    public DateTime NextSave;

    public Vector2 ScrollBar;


    // Creates a menu Item which when clicked runs ShowWindow();
    [MenuItem("Window/Auto Save")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(AutoSave));
    }

    public void Awake()
    {
        NextSave = GetCurrentTime().AddMinutes(AutoSaveInterval);
    }

    public void Update()
    {
        // update times
        InPlayMode = EditorApplication.isPlaying;
        CurrentTime = GetCurrentTime();
        

        if(CurrentTime >= NextSave)
        {
            if (InPlayMode && !SaveInPlayMode)
                return;

            // save scene
            EditorSceneManager.SaveOpenScenes();
            // save assets
            AssetDatabase.SaveAssets();

            LastSave = GetCurrentTime();
            NextSave = CurrentTime.AddMinutes(AutoSaveInterval);
        }

        this.Repaint();
    }

    private void OnGUI()
    {
        ScrollBar = GUILayout.BeginScrollView(ScrollBar, false, true);
        // window code
        GUILayout.Label("Save Info", EditorStyles.miniBoldLabel);

        EditorGUI.indentLevel++;

        EditorGUILayout.LabelField("Current Time:" , CurrentTime.ToLongTimeString());
        EditorGUILayout.LabelField("Next Save:" , NextSave.ToLongTimeString());
        EditorGUILayout.LabelField("Last Save:" , LastSave.ToLongTimeString());

        EditorGUI.indentLevel--;

        ShowOptions = EditorGUILayout.Foldout(ShowOptions, "Options", true, EditorStyles.miniBoldLabel);
        if (ShowOptions)
        {
            EditorGUI.indentLevel++;
            AutoSaveInterval = EditorGUILayout.DoubleField("Save Interval (mins) ", AutoSaveInterval);
            SaveInPlayMode = EditorGUILayout.Toggle("Save in Play Mode?", SaveInPlayMode);
            EditorGUI.indentLevel--;
        }
        GUILayout.EndScrollView();

        
    }

    private DateTime GetCurrentTime()
    {
        DateTime Now = System.DateTime.Now.ToLocalTime();

        return Now;

    }




}
