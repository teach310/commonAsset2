using System.Collections;
using System.Collections.Generic;
using System.Text;
using CA2;
using CA2.Data;
using UniRx;
using UnityEditor;
using UnityEngine;
using UniRx.Async;

namespace CA2 {
	public class CATools : EditorWindow {

		public static readonly string MasterDataDistDir = "Assets/CommonAsset2/CodeGenerator/Dist/";

		[MenuItem ("Tools/CommonAsset2")]
		static void Open () {
			GetWindow<CATools> ();
		}

		void OnGUI () {
			if (GUILayout.Button ("LoadMasterDataAsyncSpec")) {
				OnLoadMasterDataAsyncSpec ().Forget();
			}

			if (GUILayout.Button ("Generate MasterData Script")) {
				OnGenerateMasterDataScript ().Forget();
			}
			GUILayout.Label (string.Format ("SaveDir : {0}", MasterDataDistDir));

			GUILayout.Space(5);
			if (GUILayout.Button ("Save MasterDataSet")) {
				OnSaveMasterDataSet ().Forget();
			}

			if (GUILayout.Button ("MasterDataSettings Test")) {
				Debug.Log(MasterDataSettings.Instance.masterDataUrl);
				EditorUtility.ClearProgressBar();
			}
		}

		async UniTaskVoid OnLoadMasterDataAsyncSpec () {
			using (var progressBar = new ProgressBar ("GenerateMasterDataScript", "LoadAsync")) {
				await MasterDataManager.Instance.LoadAsync (progress: progressBar);
			}
			var keyValuePair = KeyValueRepository.FindAll () [0];
			Debug.LogFormat ("Key {0}, Value{1}", keyValuePair.key, keyValuePair.value);
		}

		async UniTaskVoid OnGenerateMasterDataScript () {
			using (var progressBar = new ProgressBar ("GenerateMasterDataScript")) {
				progressBar.info = "GetClassInfoSetAsync";
				var classInfoSet = await new MasterDataLoader ().GetClassInfoSetAsync (MasterDataSettings.Instance.masterDataUrl, progressBar);

				var codeGenerator = new CA2.CD.MasterDataCodeGenerator ();
				progressBar.info = "Generate";
				await codeGenerator.Generate (MasterDataDistDir, classInfoSet, progressBar);
			}
		}

		async UniTaskVoid OnSaveMasterDataSet(){
			string savePath = EditorUtility.SaveFilePanelInProject("Save", "MasterDataSet", "asset", "");
			using (var progressBar = new ProgressBar ("Save MasterDataSet", "MasterData Loading..")) {
				await MasterDataManager.Instance.LoadAsync (true, progressBar);
			}

			var dataSet = MasterDataManager.Instance.DataStore.DataSet;
			var so = ScriptableObject.CreateInstance<MasterDataObject>();
			so.dataSet = dataSet;
			so.createdAt = TimeUtil.GetCurrentUnixTime();
			AssetDatabase.CreateAsset (so, savePath);
        	AssetDatabase.Refresh ();
		}
	}
}