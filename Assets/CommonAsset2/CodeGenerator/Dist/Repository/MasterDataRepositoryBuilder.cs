using System.Collections;
using System.Collections.Generic;

/// <summary>
/// MasterDataStoreとRepositoryを紐づける
/// </summary>
namespace CA2.Data.MasterData {
	public class MasterDataRepositoryBuilder {
		public void Build (MasterDataStore dataStore) {
			KeyValueRepository.Default = new KeyValueRepository (dataStore);
			ItemRepository.Default = new ItemRepository (dataStore);
		}
	}
}