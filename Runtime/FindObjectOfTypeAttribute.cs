using System;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

#endif

namespace Kogane
{
    /// <summary>
    /// Object.FindObjectOfType を実行する Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class FindObjectOfTypeAttribute
        : Attribute,
          IGetComponentAttribute
    {
#if UNITY_2020_1_OR_NEWER
        //================================================================================
        // 変数(readonly)
        //================================================================================
        private readonly bool m_includeInactive;

        //================================================================================
        // 関数
        //================================================================================
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FindObjectOfTypeAttribute() : this(true)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FindObjectOfTypeAttribute(bool includeInactive)
        {
            m_includeInactive = includeInactive;
        }
#endif

#if UNITY_EDITOR
        /// <summary>
        /// 指定されたパラメータに参照を割り当てます
        /// </summary>
        public void Inject
        (
            MonoBehaviour monoBehaviour,
            FieldInfo fieldInfo,
            SerializedProperty serializedProperty
        )
        {
            if (serializedProperty.isArray)
            {
                return;
            }

            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            var fieldType = fieldInfo.FieldType;

            serializedProperty.objectReferenceValue = prefabStage != null && prefabStage.IsPartOfPrefabContents(monoBehaviour.gameObject)
                    ? prefabStage.scene.GetRootGameObjects()[0].GetComponentInChildren(fieldType, m_includeInactive)
                    : Object.FindObjectOfType(fieldType, m_includeInactive)
                ;
        }
#endif
    }
}