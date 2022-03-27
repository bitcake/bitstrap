using System.Collections.Generic;
using BitStrap;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BitStageWindow : EditorWindow
{
    [System.Serializable]
    public class BitStage
    {
        public Object map;
        public List<Object> childMaps = new List<Object>();
    }

    public BitStage bitStage;
    private SerializedObject so;

    [MenuItem( "Test/BitStage" )]
    public static void OpenFolderStructure() => GetWindow<BitStageWindow>();


    private void OnEnable()
    {
        so = new SerializedObject( this );
    }

    private void OnGUI()
    {
        so.Update();
        EditorGUILayout.LabelField( "BitPipe", EditorStyles.largeLabel );
        EditorGUI.BeginChangeCheck();
        DrawBitFolder( bitStage );
        if( EditorGUI.EndChangeCheck() )
        {
            //
        }

        GUILayout.Space( 20 );
        if( GUILayout.Button( "Generate Folders", GUILayout.Height( 35 ) ) )
        {
            AssetDatabase.Refresh();
        }

        so.ApplyModifiedProperties();
    }


    private void DrawBitFolder( BitStage bitStage )
    {
        // string bitFolderFolderName = bitStage.map.name;

        using( Horizontal.Do() )
        {
            EditorGUILayout.PropertyField( so.FindProperty( "bitStage" ), true);
            
            if( GUILayout.Button( new GUIContent( "R", "Rename this Pipeline Folder" ),
                GUILayout.Width( 25 ) ) )
            {
            }


            if( GUILayout.Button( new GUIContent( "+", "Create new Pipeline Folder" ), GUILayout.Width( 25 ) ) )
            {
                //
            }

            if( GUILayout.Button( new GUIContent( "-", "Delete this Pipeline Folder" ), GUILayout.Width( 25 ) ) )
            {
            }
        }

        EditorGUI.indentLevel++;
    }


    // static void OpenScene()
    // {
    //     EditorSceneManager.OpenScene( "Assets/DoNotVersionControlThis/Scene.unity" );
    //     EditorSceneManager.OpenScene( "Assets/DoNotVersionControlThis/Props.unity", OpenSceneMode.Additive );
    //     EditorSceneManager.OpenScene( "Assets/DoNotVersionControlThis/Audio.unity", OpenSceneMode.Additive );
    //     EditorSceneManager.OpenScene( "Assets/DoNotVersionControlThis/Environment.unity", OpenSceneMode.Additive );
    //     EditorSceneManager.OpenScene( "Assets/DoNotVersionControlThis/Gameplay.unity", OpenSceneMode.Additive );
    //     EditorSceneManager.OpenScene( "Assets/DoNotVersionControlThis/Lighting.unity", OpenSceneMode.Additive );
    // }
}