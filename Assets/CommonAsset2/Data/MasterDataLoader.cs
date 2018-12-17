using System;
using System.Collections;
using System.Collections.Generic;
using CA2.Data.MasterData;
using UniRx;
using UnityEngine;
using CA2.CD;

namespace CA2.Data {
	public class MasterDataLoader {
		public IObservable<MasterDataSet> Load (string url) {
			
			return ObservableWWW.Get (url + "?command=master_data_set", null, new ProgressLogger())
				.Do(t => Debug.Log(t))
				.Select (x => {
					try {
						return JsonUtility.FromJson<MasterDataSet> (x);
					} catch (System.Exception) {
						throw;
					}
				});
		}

		public IObservable<ClassInfoSet> GetClassInfoSetAsync (string url) {
			
			return ObservableWWW.Get (url + "?command=class_info_set", null, new ProgressLogger())
				.Do(t => Debug.Log(t))
				.Select (x => {
					try {
						return JsonUtility.FromJson<ClassInfoSet> (x);
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