using System.Collections;
using System.Collections.Generic;
using CA2.Data.MasterData;
using UnityEngine;
using UniRx;
using System.Threading.Tasks;

namespace CA2.Data {
	public class MasterDataManager{

		static MasterDataManager instance = null;
		public static MasterDataManager Instance{
			get{ return instance ?? (instance = new MasterDataManager());}
		}

		public MasterDataSet MasterDataSet{get; private set; }

        public async Task LoadAsync() => MasterDataSet = await new MasterDataLoader().Load(AppSettings.Instance.masterDataUrl);
    }
}