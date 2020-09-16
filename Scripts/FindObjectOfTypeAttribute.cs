using System;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Object = UnityEngine.Object;

namespace Kogane
{
	/// <summary>
	/// Object.FindObjectOfType を実行する Attribute
	/// </summary>
	[AttributeUsage( AttributeTargets.Field )]
	public sealed class FindObjectOfTypeAttribute :
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
		public FindObjectOfTypeAttribute() : this( false )
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public FindObjectOfTypeAttribute( bool includeInactive )
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
#if UNITY_2020_1_OR_NEWER
			serializedProperty.objectReferenceValue =
				Object.FindObjectOfType( fieldInfo.FieldType, m_includeInactive );
#else
			serializedProperty.objectReferenceValue =
				Object.FindObjectOfType( fieldInfo.FieldType );
#endif
		}
#endif
	}
}