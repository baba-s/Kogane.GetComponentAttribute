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
    /// 自分自身は対象にしない GetComponentInParent を実行する Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class GetComponentInParentOnlyAttribute
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
        public GetComponentInParentOnlyAttribute() : this(true)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GetComponentInParentOnlyAttribute(bool includeInactive)
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
            if (serializedProperty.isArray)
            {
                return;
            }

            serializedProperty.objectReferenceValue = monoBehaviour
                    .GetComponentsInParent(fieldInfo.FieldType, m_includeInactive)
                    .FirstOrDefault(x => x.gameObject != monoBehaviour.gameObject)
                ;
        }
#endif
    }
}