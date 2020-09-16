using System;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Kogane
{
	/// <summary>
	/// Object.FindObjectsOfType を実行する Attribute
	/// </summary>
	[AttributeUsage( AttributeTargets.Field )]
	public sealed class FindObjectsOfTypeAttribute :
		Attribute,
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
		public FindObjectsOfTypeAttribute() : this( false )
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public FindObjectsOfTypeAttribute( bool includeInactive )
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
			MonoBehaviour      monoBehaviour,
			FieldInfo          fieldInfo,
			SerializedProperty serializedProperty
		)
		{
			var fieldType      = fieldInfo.FieldType;
			var elementType    = fieldType.GetElementType();
#if UNITY_2020_1_OR_NEWER
			var components     = UnityEngine.Object.FindObjectsOfType( elementType, m_includeInactive );
#else
			var components     = UnityEngine.Object.FindObjectsOfType( elementType );
#endif
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