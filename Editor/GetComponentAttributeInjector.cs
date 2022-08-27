using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kogane.Internal
{
    /// <summary>
    /// GetComponentAttribute 付きの変数に参照を割り当てるクラス
    /// </summary>
    internal static class GetComponentAttributeInjector
    {
        //================================================================================
        // 関数(static)
        //================================================================================
        /// <summary>
        /// MonoBehaviour のコンテキストメニューから参照を割り当てます
        /// </summary>
        [MenuItem( "CONTEXT/MonoBehaviour/Get Component Attribute/Inject" )]
        private static void InjectFromContextMenu( MenuCommand menuCommand )
        {
            Inject( menuCommand.context as MonoBehaviour );
        }

        /// <summary>
        /// スクリプトがコンパイルされた時に呼び出されます
        /// </summary>
        [DidReloadScripts]
        private static void OnReloadScripts()
        {
            EditorSceneManager.sceneOpened         += OnSceneOpened;
            ObjectFactory.componentWasAdded        += OnComponentWasAdded;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            InjectAll();
        }

        /// <summary>
        /// シーンファイルを開いた時に参照を割り当てます
        /// </summary>
        private static void OnSceneOpened( Scene scene, OpenSceneMode mode )
        {
            if ( EditorApplication.isPlaying ) return;
            InjectAll();
        }

        /// <summary>
        /// コンポーネントがアタッチされた時に参照を割り当てます
        /// </summary>
        private static void OnComponentWasAdded( Component component )
        {
            if ( EditorApplication.isPlaying ) return;
            InjectAll();
        }

        /// <summary>
        /// Unity を再生する時に参照を割り当てます
        /// </summary>
        private static void OnPlayModeStateChanged( PlayModeStateChange change )
        {
            if ( change != PlayModeStateChange.ExitingEditMode ) return;
            InjectAll();
        }

        /// <summary>
        /// シーンに存在するすべての MonoBehaviour の
        /// GetComponentAttribute 付きの変数に参照を割り当てます
        /// </summary>
        private static void InjectAll()
        {
            if ( EditorApplication.isPlaying ) return;

            var monoBehaviours = GetAllMonoBehaviour();

            foreach ( var monoBehaviour in monoBehaviours )
            {
                Inject( monoBehaviour );
            }
        }

        /// <summary>
        /// 指定された MonoBehaviour が持つ GetComponentAttribute 付きの変数に参照を割り当てます
        /// </summary>
        private static void Inject( MonoBehaviour monoBehaviour )
        {
            var serializedObject = new SerializedObject( monoBehaviour );
            var type             = monoBehaviour.GetType();
            var fieldInfos       = type.GetFields( BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic );

            foreach ( var fieldInfo in fieldInfos )
            {
                var attribute = fieldInfo
                        .GetCustomAttributes()
                        .OfType<IGetComponentAttribute>()
                        .FirstOrDefault()
                    ;
                if ( attribute == null ) continue;

                var fieldName          = fieldInfo.Name;
                var serializedProperty = serializedObject.FindProperty( fieldName );

                attribute.Inject( monoBehaviour, fieldInfo, serializedProperty );
            }

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// シーンに存在するすべての MonoBehaviour を返します
        /// </summary>
        private static IList<MonoBehaviour> GetAllMonoBehaviour()
        {
            return Resources
                    .FindObjectsOfTypeAll<MonoBehaviour>()
                    .Where( x => x.gameObject.scene.isLoaded )
                    .Where( x => x.gameObject.hideFlags == HideFlags.None )
                    .Where( x => PrefabUtility.GetPrefabAssetType( x.gameObject ) == PrefabAssetType.NotAPrefab )
                    .ToArray()
                ;
        }
    }
}