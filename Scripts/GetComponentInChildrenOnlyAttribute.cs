using System;
using System.Linq;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace UniGetComponentAttribute
{
	/// <summary>
	/// 自分自身は対象にしない GetComponentInChildren を実行する Attribute
	/// </summary>
	[AttributeUsage( AttributeTargets.Field )]
	public sealed class GetComponentInChildrenOnlyAttribute :
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
		public GetComponentInChildrenOnlyAttribute() : this( false )
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public GetComponentInChildrenOnlyAttribute( bool includeInactive )
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
			serializedProperty.objectReferenceValue = monoBehaviour
					.GetComponentsInChildren( fieldInfo.FieldType, m_includeInactive )
					.FirstOrDefault( x => x.gameObject != monoBehaviour.gameObject )
				;
		}
#endif
	}
}