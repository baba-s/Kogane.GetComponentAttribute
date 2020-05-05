using System;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniGetComponentAttribute
{
	/// <summary>
	/// Object.FindObjectOfType を実行する Attribute
	/// </summary>
	[AttributeUsage( AttributeTargets.Field )]
	public sealed class FindObjectOfTypeAttribute :
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
			serializedProperty.objectReferenceValue =
				Object.FindObjectOfType( fieldInfo.FieldType );
		}
#endif
	}
}