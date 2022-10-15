using System;
using System.Reflection;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Kogane
{
    /// <summary>
    /// GetComponentsInParent を実行する Attribute
    /// </summary>
    [AttributeUsage( AttributeTargets.Field )]
    public sealed class GetComponentsInParentAttribute
        : Attribute,
          IGetComponentAttribute
    {
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
        public GetComponentsInParentAttribute() : this( true )
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GetComponentsInParentAttribute( bool includeInactive )
        {
            m_includeInactive = includeInactive;
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
            var fieldType      = fieldInfo.FieldType;
            var elementType    = fieldType.GetElementType();
            var components     = monoBehaviour.GetComponentsInParent( elementType, m_includeInactive );
            var componentCount = components.Length;

            serializedProperty.arraySize = componentCount;

            for ( var i = 0; i < componentCount; i++ )
            {
                var element   = serializedProperty.GetArrayElementAtIndex( i );
                var component = components[ i ];

                element.objectReferenceValue = component;
            }
        }
#endif
    }
}