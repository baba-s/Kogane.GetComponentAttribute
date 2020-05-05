using System;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace UniGetComponentAttribute
{
	/// <summary>
	/// Object.FindObjectsOfType を実行する Attribute
	/// </summary>
	[AttributeUsage( AttributeTargets.Field )]
	public sealed class FindObjectsOfTypeAttribute :
		Attribute,
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
			var fieldType      = fieldInfo.FieldType;
			var elementType    = fieldType.GetElementType();
			var components     = UnityEngine.Object.FindObjectsOfType( elementType );
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