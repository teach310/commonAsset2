using System.Collections;
using System.Collections.Generic;
using System.Text;
using CA2;
using CA2.Data;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace CA2 {
	public class ConvinientTools : EditorWindow {

		[MenuItem ("Tools/Convinient")]
		static void Open () {
			GetWindow<ConvinientTools> ();
		}

		async void OnGUI () {
			if (GUILayout.Button ("AnyTest")) {
				await MasterDataManager.Instance.LoadAsync ();
				var keyValuePair = MasterDataManager.Instance.MasterDataSet.itemList[0];
				Debug.LogFormat ("ItemId {0}, Name{1}", keyValuePair.id, keyValuePair.name);
			}

			if (GUILayout.Button ("CDTest")) {
				var classInfoSet = await new MasterDataLoader ().GetClassInfoSetAsync (AppSettings.Instance.masterDataUrl);

				var codeGenerator = new CA2.CD.CodeGenerator ();
				foreach (var classInfo in classInfoSet.classInfoList) {
					await codeGenerator.CreateMasterData ("Assets/CommonAsset2/CodeGenerator/Dist/MasterData", classInfo);
				}
			}
		}
	}
}