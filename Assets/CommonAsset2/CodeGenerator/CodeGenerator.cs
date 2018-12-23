using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace CA2.CD {

	[System.Serializable]
	public class ClassInfoSet{
		public List<ClassInfo> classInfoList;
	}

	[System.Serializable]
	public class ClassInfo {
		public string name;
		public List<FieldInfo> fieldInfoList;
	}

	[System.Serializable]
	public class FieldInfo {
		public string name;
		public string fieldType;

		public string GetText (bool isPublic = true) {
			string format = isPublic ? "public {0} {1};" : "{0} {1};";
			return string.Format (format, fieldType, name);
		}
	}

	public class CodeGenerator {
		
		public static readonly string TemplatesDir = "Assets/CommonAsset2/CodeGenerator/Templates/";

		public class TemplateLoader {
			public TextAsset LoadTemplate (string path) {
				return AssetDatabase.LoadAssetAtPath<TextAsset> (path);
			}
		}

		public class Writer {

			public async Task CreateNewScript (string path, string content) {
				CreateFolderRecursively(Path.GetDirectoryName (path));

				using (StreamWriter streamWriter = File.CreateText (path + ".cs")) {
					await streamWriter.WriteAsync (content);
				}
				AssetDatabase.Refresh ();
			}

			/// <summary>
			/// 複数階層のフォルダを作成する
			/// </summary>
			void CreateFolderRecursively (string path) {

				// もう存在すれば処理は不要
				if (AssetDatabase.IsValidFolder (path)) return;

				// スラッシュで終わっていたら除去
				if (path[path.Length - 1] == '/') {
					path = path.Substring (0, path.Length - 1);
				}

				var names = path.Split ('/');
				for (int i = 1; i < names.Length; i++) {
					var parent = string.Join ("/", names.Take (i).ToArray ());
					var target = string.Join ("/", names.Take (i + 1).ToArray ());
					var child = names[i];
					if (!AssetDatabase.IsValidFolder (target)) {
						AssetDatabase.CreateFolder (parent, child);
					}
				}
			}
		}

		

		public string Tabs (int level) {
			string rtn = "";
			for (int i = 0; i < level; i++) {
				rtn += "\t";
			}
			return rtn;
		}

		public string UpperCamelToCamel(string src){
			if ( string.IsNullOrEmpty( src ) ) return src;
			return char.ToLowerInvariant( src[ 0 ] ) + src.Substring( 1, src.Length - 1 );
		}
	}
}