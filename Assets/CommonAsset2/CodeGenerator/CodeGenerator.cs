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

		public async Task CreateMasterData (string dirPath, ClassInfo classInfo) {
			var templateLoader = new TemplateLoader ();
			var writer = new Writer ();
			var template = templateLoader.LoadTemplate ("Assets/CommonAsset2/CodeGenerator/Templates/DataTemplate.txt");
			var content = template.text.Replace ("$ClassName$", classInfo.name);
			content = content.Replace ("$Fields$", GetFields (classInfo, 2));
			string path = Path.Combine (dirPath, classInfo.name);
			await writer.CreateNewScript (path, content);
			Debug.Log ("Create " + path);
		}

		string GetFields (ClassInfo classInfo, int indentLevel) {
			if (classInfo.fieldInfoList.Count == 0)
				return "";
			var stringBuilder = new StringBuilder ();
			stringBuilder.AppendLine (classInfo.fieldInfoList[0].GetText ());
			for (int i = 1; i < classInfo.fieldInfoList.Count; i++) {
				var text = Tabs (indentLevel) + classInfo.fieldInfoList[i].GetText ();
				if (i == classInfo.fieldInfoList.Count - 1)
					stringBuilder.Append (text);
				else
					stringBuilder.AppendLine (text);
			}

			return stringBuilder.ToString ();
		}

		string Tabs (int level) {
			string rtn = "";
			for (int i = 0; i < level; i++) {
				rtn += "\t";
			}
			return rtn;
		}
	}
}