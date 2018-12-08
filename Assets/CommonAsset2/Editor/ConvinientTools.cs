using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CA2.Data;

public class ConvinientTools : EditorWindow {

	[MenuItem("Tools/Convinient")]
	static void Open(){
		GetWindow<ConvinientTools>();
	}

	async void OnGUI(){
		if(GUILayout.Button("AnyTest")){
			await MasterDataManager.Instance.LoadAsync();
			var keyValuePair = MasterDataManager.Instance.MasterDataSet.itemList[0];
			Debug.LogFormat("ItemId {0}, Name{1}", keyValuePair.id, keyValuePair.name);	
		}
	}
}
