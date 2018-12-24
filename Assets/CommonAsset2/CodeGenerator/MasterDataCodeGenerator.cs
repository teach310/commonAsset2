using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using System;

namespace CA2.CD {
	public class MasterDataCodeGenerator : CodeGenerator {
		// 全部まるっと生成
		public async Task Generate (string distDir, ClassInfoSet classInfoSet, IProgress<float> progress = null) {
			string masterDataSaveDir = Path.Combine (distDir, "MasterData");
			string repositorySaveDir = Path.Combine (distDir, "Repository");

			if(progress != null)
				progress.Report(0);

			foreach (var classInfo in classInfoSet.classInfoList) {
				await CreateMasterData (masterDataSaveDir, classInfo);
			}
			if(progress != null)
				progress.Report(0.25f);

			var masterDataClassInfoList = classInfoSet.classInfoList.Where (x => x.name != "MasterDataSet").ToList ();
			foreach (var classInfo in masterDataClassInfoList) {
				await CreateIDataStore (masterDataSaveDir, classInfo);
				await CreateRepository (repositorySaveDir, classInfo);
			}

			if(progress != null)
				progress.Report(0.75f);

			await CreateMasterDataStore (masterDataSaveDir, masterDataClassInfoList);
			await CreateMasterDataRepositoryBuilder (repositorySaveDir, masterDataClassInfoList);
			if(progress != null)
				progress.Report(1.0f);
		}

		#region Data
		public async Task CreateMasterData (string dirPath, ClassInfo classInfo) {
			var templateLoader = new TemplateLoader ();
			var writer = new Writer ();
			var template = templateLoader.LoadTemplate (TemplatesDir + "DataTemplate.txt");
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
		#endregion

		#region IDataStore
		public async Task CreateIDataStore (string dirPath, ClassInfo classInfo) {
			var templateLoader = new TemplateLoader ();
			var writer = new Writer ();
			var template = templateLoader.LoadTemplate (TemplatesDir + "IDataStoreTemplate.txt");
			string dataName = classInfo.name;
			var content = template.text.Replace ("$DataName$", dataName);
			string path = Path.Combine (dirPath, $"I{dataName}Store");
			await writer.CreateNewScript (path, content);
			Debug.Log ("Create " + path);
		}
		#endregion

		#region MasterDataRepositoryBuilder
		public async Task CreateMasterDataRepositoryBuilder (string dirPath, List<ClassInfo> classInfoList) {
			var templateLoader = new TemplateLoader ();
			var writer = new Writer ();
			var template = templateLoader.LoadTemplate (TemplatesDir + "MasterDataRepositoryBuilderTemplate.txt");
			var build = GetMasterDataRepositoryBuilderBuild (classInfoList.Select (x => x.name).ToList ());
			var content = template.text.Replace ("$Build$", build);
			string path = Path.Combine (dirPath, "MasterDataRepositoryBuilder");
			await writer.CreateNewScript (path, content);
			Debug.Log ("Create " + path);
		}

		string GetMasterDataRepositoryBuilderBuild (List<string> dataNameList) {
			var stringBuilder = new StringBuilder ();
			for (int i = 0; i < dataNameList.Count; i++) {
				int indentLevel = i == 0 ? 0 : 3;
				var dataName = dataNameList[i];
				var line = Tabs (indentLevel) + $"{dataName}Repository.Default = new {dataName}Repository (dataStore);";
				if (i == dataNameList.Count - 1)
					stringBuilder.Append (line);
				else
					stringBuilder.AppendLine (line);
			}
			return stringBuilder.ToString ();
		}
		#endregion

		#region MasterDataStore
		public async Task CreateMasterDataStore (string dirPath, List<ClassInfo> classInfoList) {
			var templateLoader = new TemplateLoader ();
			var writer = new Writer ();
			var template = templateLoader.LoadTemplate (TemplatesDir + "MasterDataStoreTemplate.txt");
			var dataNameList = classInfoList.Select (x => x.name).ToList ();
			string interfaces = GetMasterDataStoreInterfaces (dataNameList);
			string properties = GetMasterDataStoreProperties (dataNameList);
			var content = template.text.Replace ("$Interfaces$", interfaces);
			content = content.Replace ("$Properties$", properties);
			string path = Path.Combine (dirPath, "MasterDataStore");
			await writer.CreateNewScript (path, content);
			Debug.Log ("Create " + path);
		}

		string GetMasterDataStoreInterfaces (List<string> dataNameList) {
			var stringBuilder = new StringBuilder ();
			for (int i = 0; i < dataNameList.Count; i++) {
				var dataName = dataNameList[i];
				var value = $"I{dataName}Store";
				if (i != dataNameList.Count - 1)
					value += ", ";
				stringBuilder.Append (value);
			}
			return stringBuilder.ToString ();
		}

		string GetMasterDataStoreProperties (List<string> dataNameList) {
			var stringBuilder = new StringBuilder ();
			for (int i = 0; i < dataNameList.Count; i++) {
				int indentLevel = i == 0 ? 0 : 2;
				var arg1 = dataNameList[i];
				var arg2 = UpperCamelToCamel (dataNameList[i]);
				var line = Tabs (indentLevel) + $"public List<{arg1}> {arg1}List {{ get {{ return DataSet.{arg2}List; }} }}";
				if (i == dataNameList.Count - 1)
					stringBuilder.Append (line);
				else
					stringBuilder.AppendLine (line);
			}
			return stringBuilder.ToString ();
		}
		#endregion

		#region Repository
		public async Task CreateRepository (string dirPath, ClassInfo classInfo) {
			var templateLoader = new TemplateLoader ();
			var writer = new Writer ();
			var template = templateLoader.LoadTemplate (TemplatesDir + "RepositoryTemplate.txt");
			string dataName = classInfo.name;
			var content = template.text.Replace ("$DataName$", dataName);
			string path = Path.Combine (dirPath, $"{dataName}Repository.Data");
			await writer.CreateNewScript (path, content);
			Debug.Log ("Create " + path);
		}
		#endregion

	}
}