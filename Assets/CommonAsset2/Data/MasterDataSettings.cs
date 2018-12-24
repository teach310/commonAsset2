using System.Collections;
using System.Collections.Generic;
using CA2.Data;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CA2 {
	[CreateAssetMenu (menuName = "CA2/Create MasterDataSettings")]
	public class MasterDataSettings : ScriptableObject {

		static MasterDataSettings instance = null;

		public static MasterDataSettings Instance {
			get {
				if (instance == null) {
					instance = Resources.Load<MasterDataSettings> ("MasterDataSettings");
				}
				return instance;
			}
		}

		public string masterDataUrl;
		public bool useLocal = false;
		public MasterDataObject masterData;
	}

#if UNITY_EDITOR
	[CustomEditor (typeof (MasterDataSettings))]
	public class MasterDataSettingsEditor : Editor {

		SerializedProperty masterDataUrlProperty;
		SerializedProperty useLocalProperty;
		SerializedProperty masterDataProperty;

		void OnEnable () {
			masterDataUrlProperty = serializedObject.FindProperty ("masterDataUrl");
			useLocalProperty = serializedObject.FindProperty ("useLocal");
			masterDataProperty = serializedObject.FindProperty ("masterData");
		}

		public override void OnInspectorGUI () {
			serializedObject.Update ();

			EditorGUILayout.PropertyField (masterDataUrlProperty);
			EditorGUILayout.PropertyField (useLocalProperty);
			if (useLocalProperty.boolValue) {
				EditorGUILayout.PropertyField (masterDataProperty);
			}

			serializedObject.ApplyModifiedProperties ();
		}
	}
#endif
}