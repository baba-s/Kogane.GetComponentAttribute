using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace UniGetComponentAttribute
{
	/// <summary>
	/// GetComponent 系の Attribute のインターフェイス
	/// </summary>
	public interface IGetComponentAttribute
	{
#if UNITY_EDITOR
		//================================================================================
		// 関数
		//================================================================================
		/// <summary>
		/// 指定されたパラメータに参照を割り当てます
		/// </summary>
		void Inject
		(
			MonoBehaviour      monoBehaviour,
			FieldInfo          fieldInfo,
			SerializedProperty serializedProperty
		);
#endif
	}
}