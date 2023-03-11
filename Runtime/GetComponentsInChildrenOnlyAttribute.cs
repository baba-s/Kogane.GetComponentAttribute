using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Kogane
{
    /// <summary>
    /// 自分自身は対象にしない GetComponentsInChildren を実行する Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class GetComponentsInChildrenOnlyAttribute
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
        public GetComponentsInChildrenOnlyAttribute() : this(true)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GetComponentsInChildrenOnlyAttribute(bool includeInactive)
        {
            m_includeInactive = includeInactive;
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
            if (!serializedProperty.isArray)
            {
                return;
            }

            var fieldType = fieldInfo.FieldType;
            var elementType = fieldType.GetElementType() ?? fieldType.GetGenericArguments().SingleOrDefault();

            var components = monoBehaviour
                    .GetComponentsInChildren(elementType, m_includeInactive)
                    .Where(x => x.gameObject != monoBehaviour.gameObject)
                    .ToArray()
                ;

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