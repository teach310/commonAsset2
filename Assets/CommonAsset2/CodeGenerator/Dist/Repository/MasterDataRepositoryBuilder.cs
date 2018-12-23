using System.Collections;
using System.Collections.Generic;

/// <summary>
/// MasterDataSetとRepositoryを紐づける
/// </summary>
namespace CA2.Data.MasterData {
	public class MasterDataRepositoryBuilder {
		public void Build (MasterDataStore dataStore) {
			var keyValueRepository = new KeyValueRepository (dataStore);
			KeyValueRepository.Default = keyValueRepository;
		}
	}
}