using System;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Kogane
{
	/// <summary>
	/// GameObject.Find を実行する Attribute
	/// </summary>
	[AttributeUsage( AttributeTargets.Field )]
	public sealed class FindAttribute :
		Attribute,
		IGetComponentAttribute
	{
		//================================================================================
		// 変数(readonly)
		//================================================================================
		private readonly string m_name;

		//================================================================================
		// 関数
		//================================================================================
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public FindAttribute( string name )
		{
			m_name = name;
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
			serializedProperty.objectReferenceValue = GameObject.Find( m_name );
		}
#endif
	}
}