using System.Collections;
using System.Collections.Generic;
using System.Text;
using CA2;
using CA2.Data;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace CA2 {
	public class CATools : EditorWindow {

		public static readonly string MasterDataDistDir = "Assets/CommonAsset2/CodeGenerator/Dist/";

		[MenuItem ("Tools/CommonAsset2")]
		static void Open () {
			GetWindow<CATools> ();
		}

		void OnGUI () {
			if (GUILayout.Button ("LoadMasterDataAsyncSpec")) {
				OnLoadMasterDataAsyncSpec ();
			}

			if (GUILayout.Button ("Generate MasterData Script")) {
				OnGenerateMasterDataScript ();
			}
			GUILayout.Label (string.Format ("SaveDir : {0}", MasterDataDistDir));
		}

		async void OnLoadMasterDataAsyncSpec () {
			using (var progressBar = new ProgressBar ("GenerateMasterDataScript", "LoadAsync")) {
				await MasterDataManager.Instance.LoadAsync (progressBar);
			}
			var keyValuePair = KeyValueRepository.FindAll () [0];
			Debug.LogFormat ("Key {0}, Value{1}", keyValuePair.key, keyValuePair.value);
		}

		async void OnGenerateMasterDataScript () {
			using (var progressBar = new ProgressBar ("GenerateMasterDataScript")) {
				progressBar.info = "GetClassInfoSetAsync";
				var classInfoSet = await new MasterDataLoader ().GetClassInfoSetAsync (CASettings.Instance.masterDataUrl, progressBar);

				var codeGenerator = new CA2.CD.MasterDataCodeGenerator ();
				progressBar.info = "Generate";
				await codeGenerator.Generate (MasterDataDistDir, classInfoSet, progressBar);
			}
		}
	}
}