using System.Collections;
using System.Collections.Generic;
using CA2.Data.MasterData;
using UnityEngine;
using UniRx;
using System.Threading.Tasks;
using System;

namespace CA2.Data {
	public class MasterDataManager{

		static MasterDataManager instance = null;
		public static MasterDataManager Instance{
			get{ return instance ?? (instance = new MasterDataManager());}
		}

		public MasterDataStore DataStore{get; private set; }

        public async Task LoadAsync(IProgress<float> progress = null){
			var masterDataSet = await new MasterDataLoader().LoadAsync(CASettings.Instance.masterDataUrl, progress);
			DataStore = new MasterDataStore(masterDataSet);
			var repositoryBuilder = new MasterDataRepositoryBuilder();
			repositoryBuilder.Build(DataStore);
		}
    }
}