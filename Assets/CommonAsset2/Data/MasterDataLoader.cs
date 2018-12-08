using System;
using System.Collections;
using System.Collections.Generic;
using CA2.Data.MasterData;
using UniRx;
using UnityEngine;

namespace CA2.Data {
	public class MasterDataLoader {
		public IObservable<MasterDataSet> Load (string url) {
			return ObservableWWW.Get (url, null, new ProgressLogger())
				.Do(t => Debug.Log(t))
				.Select (x => {
					try {
						return JsonUtility.FromJson<MasterDataSet> (x);
					} catch (System.Exception) {
						throw;
					}
				});
		}

		public class ProgressLogger : IProgress<float> {
			public void Report (float value) {
				Debug.Log (value);
			}
		}
	}
}