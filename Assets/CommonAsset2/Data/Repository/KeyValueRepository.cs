using System.Collections;
using System.Collections.Generic;
using CA2.Data;
using CA2.Data.MasterData;
using UnityEngine;

namespace CA2.Data
{
	public partial class KeyValueRepository{
		public static KeyValue FindByKey(string key, KeyValueRepository repository = null){
			return DataList(repository).Find(x=>x.key == key);
		}
	}
}