using System;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace UniGetComponentAttribute
{
	/// <summary>
	/// GetComponentsInChildren を実行する Attribute
	/// </summary>
	[AttributeUsage( AttributeTargets.Field )]
	public sealed class GetComponentsInChildrenAttribute :
		Attribute,
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
		public GetComponentsInChildrenAttribute() : this( false )
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public GetComponentsInChildrenAttribute( bool includeInactive )
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
			var components     = monoBehaviour.GetComponentsInChildren( elementType, m_includeInactive );
			var componentCount = components.Length;

			serializedProperty.arraySize = componentCount;

			for ( int i = 0; i < componentCount; i++ )
			{
				var element   = serializedProperty.GetArrayElementAtIndex( i );
				var component = components[ i ];

				element.objectReferenceValue = component;
			}
		}
#endif
	}
}