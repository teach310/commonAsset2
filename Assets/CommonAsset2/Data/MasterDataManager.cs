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

        public async Task LoadAsync(bool forceLoadFromWeb = false ,IProgress<float> progress = null){
			MasterDataSet masterDataSet = null;
			if(MasterDataSettings.Instance.useLocal && MasterDataSettings.Instance.masterData != null && !forceLoadFromWeb)
				masterDataSet = MasterDataSettings.Instance.masterData.dataSet;
			else
				masterDataSet = await new MasterDataLoader().LoadAsync(MasterDataSettings.Instance.masterDataUrl, progress);
			DataStore = new MasterDataStore(masterDataSet);
			var repositoryBuilder = new MasterDataRepositoryBuilder();
			repositoryBuilder.Build(DataStore);
		}
    }
}