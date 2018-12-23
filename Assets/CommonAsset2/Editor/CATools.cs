﻿using System.Collections;
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
			await MasterDataManager.Instance.LoadAsync ();
			var keyValuePair = KeyValueRepository.FindAll () [0];
			Debug.LogFormat ("Key {0}, Value{1}", keyValuePair.key, keyValuePair.value);
		}

		async void OnGenerateMasterDataScript () {
			var classInfoSet = await new MasterDataLoader ().GetClassInfoSetAsync (CASettings.Instance.masterDataUrl);

			var codeGenerator = new CA2.CD.MasterDataCodeGenerator ();
			await codeGenerator.Generate(MasterDataDistDir, classInfoSet);
		}
	}
}