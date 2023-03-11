using System;
using System.Reflection;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Kogane
{
    /// <summary>
    /// Transform.Find を実行する Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class FindChildAttribute
        : Attribute,
          IGetComponentAttribute
    {
        //================================================================================
        // 変数(readonly)
        //================================================================================
        private readonly string m_name;

        //================================================================================
        // 関数
        //================================================================================
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FindChildAttribute(string name)
        {
            m_name = name;
        }

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

            serializedProperty.objectReferenceValue = monoBehaviour.transform.Find(m_name);
        }
#endif
    }
}