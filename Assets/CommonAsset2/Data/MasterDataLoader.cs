using System;
using System.Collections;
using System.Collections.Generic;
using CA2.CD;
using CA2.Data.MasterData;
using UniRx;
using UniRx.Async;
using UnityEngine;
using UnityEngine.Networking;

namespace CA2.Data {
	public class MasterDataLoader {
		public async UniTask<MasterDataSet> LoadAsync (string url, IProgress<float> progress = null) {

			// var req = UnityWebRequest.Get (url + "?command=master_data_set");
			// await req.SendWebRequest ().ConfigureAwait (progress);
			// return JsonUtility.FromJson<MasterDataSet> (req.downloadHandler.text);

			// ConfigureAwaitがEditorだとうまく動かないためObservableWWWを使う
			var text = await ObservableWWW.Get (url + "?command=master_data_set", null, progress);
			Debug.Log (text);
			return JsonUtility.FromJson<MasterDataSet> (text);
		}

		public async UniTask<ClassInfoSet> GetClassInfoSetAsync (string url, IProgress<float> progress = null) {
			// var req = UnityWebRequest.Get (url + "?command=class_info_set");
			// await req.SendWebRequest ().ConfigureAwait (progress);
			// return JsonUtility.FromJson<ClassInfoSet> (req.downloadHandler.text);

			// ConfigureAwaitがEditorだとうまく動かないためObservableWWWを使う
			var text = await ObservableWWW.Get (url + "?command=class_info_set", null, progress);
			Debug.Log(text);
			return JsonUtility.FromJson<ClassInfoSet> (text);
		}
	}
}