using System.Collections;
using System.Collections.Generic;

namespace CA2.Data.MasterData{
	public class MasterDataStore : IKeyValueStore, IItemStore {
		
		public MasterDataSet DataSet{ get; private set; }

		public List<KeyValue> KeyValueList { get { return DataSet.keyValueList; } }
		public List<Item> ItemList { get { return DataSet.itemList; } }

		public MasterDataStore(MasterDataSet dataSet){
			DataSet = dataSet;
		}		
	}
}