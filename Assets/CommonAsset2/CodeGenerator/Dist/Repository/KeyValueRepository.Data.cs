using System.Collections;
using System.Collections.Generic;
using CA2.Data.MasterData;

namespace CA2.Data{
	public partial class KeyValueRepository{

		IKeyValueStore dataStore;

		public KeyValueRepository(IKeyValueStore dataStore){
			this.dataStore = dataStore;
		}

		public static KeyValueRepository Default = null;

		static List<KeyValue> DataList(KeyValueRepository repository = null){
			if(repository == null)
				repository = Default;
			return repository != null ? repository.dataStore.KeyValueList : new List<KeyValue>();
		}

		public static List<KeyValue> FindAll(KeyValueRepository repository = null){
			return DataList(repository);
		}
	}
}

