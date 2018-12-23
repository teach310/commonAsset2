using System.Collections;
using System.Collections.Generic;
using CA2.Data.MasterData;

namespace CA2.Data{
	public partial class ItemRepository{

		IItemStore dataStore;

		public ItemRepository(IItemStore dataStore){
			this.dataStore = dataStore;
		}

		public static ItemRepository Default = null;

		static List<Item> DataList(ItemRepository repository = null){
			if(repository == null)
				repository = Default;
			return repository != null ? repository.dataStore.ItemList : new List<Item>();
		}

		public static List<Item> FindAll(ItemRepository repository = null){
			return DataList(repository);
		}
	}
}

