# Kogane Get Component Attribute

シリアライズ対象の変数に自動で参照を割り当てるエディタ拡張

## 使用例

```cs
using Kogane;
using UnityEngine;

public class Test : MonoBehaviour
{
    [GetComponent]                public BoxCollider   m_boxCollider;
    [GetComponentInParent]        public BoxCollider   m_boxColliderInParent;
    [GetComponentInParentOnly]    public BoxCollider   m_boxColliderInParentOnly;
    [GetComponentInChildren]      public BoxCollider   m_boxColliderInChildren;
    [GetComponentInChildrenOnly]  public BoxCollider   m_boxColliderInChildrenOnly;
    [GetComponents]               public BoxCollider[] m_boxColliders;
    [GetComponentsInParent]       public BoxCollider[] m_boxCollidersInParent;
    [GetComponentsInParentOnly]   public BoxCollider[] m_boxCollidersInParentOnly;
    [GetComponentsInChildren]     public BoxCollider[] m_boxCollidersInChildren;
    [GetComponentsInChildrenOnly] public BoxCollider[] m_boxCollidersInChildrenOnly;
    [Find( "Main Camera" )]       public GameObject    m_cameraGameObject;
    [FindWithTag( "MainCamera" )] public GameObject    m_cameraGameObjectWithTag;
    [FindObjectOfType]            public Camera        m_camera;
    [FindObjectsOfType]           public Camera[]      m_cameras;
    [FindChild( "Main Camera" )]  public Transform     m_transformChild;
}
```

* GetComponent などの属性をシリアライズ対象の変数に適用すると  
  下記のタイミングで自動で参照が割り当てられるようになります
    * スクリプトがコンパイルされた時
    * シーンファイルを開いた時
    * コンポーネントがアタッチされた時
    * Unity を再生した時
* MonoBehaviour のコンテキストメニューから「UniGetComponentAttribute > Inject」を選択すると  
  手動で参照を割り当てることも可能です

## 属性の説明

|属性|説明|
|:--|:--|
|GetComponent|GetComponent で取得した参照を設定|
|GetComponentInParent|GetComponentInParent で取得した参照を設定|
|GetComponentInParentOnly|GetComponentInParent で取得した参照を設定（自分以外）|
|GetComponentInChildren|GetComponentInChildren で取得した参照を設定|
|GetComponentInChildrenOnly|GetComponentInChildren で取得した参照を設定（自分以外）|
|GetComponents|GetComponents で取得した参照を設定|
|GetComponentsInParent|GetComponentsInParent で取得した参照を設定|
|GetComponentsInParentOnly|GetComponentsInParent で取得した参照を設定（自分以外）|
|GetComponentsInChildren|GetComponentsInChildren で取得した参照を設定|
|GetComponentsInChildrenOnly|GetComponentInChildren で取得した参照を設定（自分以外）|
|Find|GameObject.Find で取得した参照を設定|
|FindWithTag|GameObject.FindWithTag で取得した参照を設定|
|FindObjectOfType|Object.FindObjectOfType で取得した参照を設定|
|FindObjectsOfType|Object.FindObjectsOfType で取得した参照を設定|
|FindChild|Transform.Find で取得した参照を設定|

## 拡張方法

```cs
using System;
using System.Reflection;
using UniGetComponentAttribute;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[AttributeUsage( AttributeTargets.Field )]
public sealed class MyGetComponentAttribute :
    Attribute,
    IGetComponentAttribute
{
#if UNITY_EDITOR
    public void Inject
    (
        MonoBehaviour      monoBehaviour,
        FieldInfo          fieldInfo,
        SerializedProperty serializedProperty
    )
    {
        serializedProperty.objectReferenceValue =
            monoBehaviour.GetComponent( fieldInfo.FieldType );
    }
#endif
}
```

* Attribute クラスを継承して IGetComponentAttribute インターフェイスを定義すると  
  参照割り当て機能を自作することができます

## 補足

* GetComponents 系の関数は List&lt;T&gt; には対応していません
    * 配列にのみ対応しています  