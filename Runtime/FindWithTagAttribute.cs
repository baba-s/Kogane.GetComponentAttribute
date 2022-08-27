using System;
using System.Reflection;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Kogane
{
    /// <summary>
    /// GameObject.FindWithTag を実行する Attribute
    /// </summary>
    [AttributeUsage( AttributeTargets.Field )]
    public sealed class FindWithTagAttribute
        : Attribute,
          IGetComponentAttribute
    {
        //================================================================================
        // 変数(readonly)
        //================================================================================
        private readonly string m_tag;

        //================================================================================
        // 関数
        //================================================================================
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FindWithTagAttribute( string tag )
        {
            m_tag = tag;
        }

#if UNITY_EDITOR
        /// <summary>
        /// 指定されたパラメータに参照を割り当てます
        /// </summary>
        public void Inject
        (
            MonoBehaviour      monoBehaviour,
            FieldInfo          fieldInfo,
            SerializedProperty serializedProperty
        )
        {
            serializedProperty.objectReferenceValue = GameObject.FindWithTag( m_tag );
        }
#endif
    }
}