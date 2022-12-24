using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Kogane
{
    /// <summary>
    /// AssetDatabase.FindAssets を実行する Attribute
    /// </summary>
    [AttributeUsage( AttributeTargets.Field )]
    public sealed class FindAssetsAttribute
        : Attribute,
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
            var guid = AssetDatabase
                    .FindAssets( $"t:{fieldInfo.FieldType.Name}" )
                    .FirstOrDefault()
                ;

            if ( string.IsNullOrWhiteSpace( guid ) ) return;

            var assetPath = AssetDatabase.GUIDToAssetPath( guid );
            var asset     = AssetDatabase.LoadAssetAtPath<Object>( assetPath );

            serializedProperty.objectReferenceValue = asset;
        }
#endif
    }
}