using System;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Kogane
{
	/// <summary>
	/// GetComponentInChildren を実行する Attribute
	/// </summary>
	[AttributeUsage( AttributeTargets.Field )]
	public sealed class GetComponentInChildrenAttribute :
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
		public GetComponentInChildrenAttribute() : this( false )
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public GetComponentInChildrenAttribute( bool includeInactive )
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
			serializedProperty.objectReferenceValue =
				monoBehaviour.GetComponentInChildren( fieldInfo.FieldType, m_includeInactive );
		}
#endif
	}
}