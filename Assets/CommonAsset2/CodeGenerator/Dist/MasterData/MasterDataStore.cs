using System.Collections;
using System.Collections.Generic;

namespace CA2.Data.MasterData{
	public class MasterDataStore : 
		IKeyValueStore {
		
		MasterDataSet dataSet;

		public List<KeyValue> KeyValueList{
			get{ return dataSet.keyValueList; }
		}

		public MasterDataStore(MasterDataSet dataSet){
			this.dataSet = dataSet;
		}		
	}
}

