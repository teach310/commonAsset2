using System.Collections;
using System.Collections.Generic;

namespace CA2.Data.MasterData{
	public class MasterDataStore : IKeyValueStore, IItemStore {
		
		MasterDataSet dataSet;

		public List<KeyValue> KeyValueList { get { return dataSet.keyValueList; } }
		public List<Item> ItemList { get { return dataSet.itemList; } }

		public MasterDataStore(MasterDataSet dataSet){
			this.dataSet = dataSet;
		}		
	}
}