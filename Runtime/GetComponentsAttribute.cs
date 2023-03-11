using System;
using System.Reflection;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Kogane
{
    /// <summary>
    /// GetComponents を実行する Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class GetComponentsAttribute
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
            MonoBehaviour monoBehaviour,
            FieldInfo fieldInfo,
            SerializedProperty serializedProperty
        )
        {
            var fieldType = fieldInfo.FieldType;
            var elementType = fieldType.GetElementType() ?? fieldType.GetGenericArguments().SingleOrDefault();
            var components = monoBehaviour.GetComponents(elementType);
            var componentCount = components.Length;

            serializedProperty.arraySize = componentCount;

            for (var i = 0; i < componentCount; i++)
            {
                var element = serializedProperty.GetArrayElementAtIndex(i);
                var component = components[i];

                element.objectReferenceValue = component;
            }
        }
#endif
    }
}