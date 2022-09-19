using System;
using System.Reflection;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Kogane
{
    /// <summary>
    /// GetOrAddComponent を実行する Attribute
    /// </summary>
    [AttributeUsage( AttributeTargets.Field )]
    public sealed class GetOrAddComponentAttribute
        : Attribute,
          IGetComponentAttribute
    {
        //================================================================================
        // 関数
        //================================================================================
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
            var fieldType = fieldInfo.FieldType;

            if ( !monoBehaviour.TryGetComponent( fieldType, out var component ) )
            {
                component = monoBehaviour.gameObject.AddComponent( fieldType );
            }

            serializedProperty.objectReferenceValue = component;
        }
#endif
    }
}