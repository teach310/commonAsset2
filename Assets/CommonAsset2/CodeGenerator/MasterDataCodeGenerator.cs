using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace CA2.CD {
	public class MasterDataCodeGenerator : CodeGenerator {
		// 全部まるっと生成
		public async Task Generate(string distDir, ClassInfoSet classInfoSet){
			string masterDataSaveDir = Path.Combine(distDir, "MasterData");
			string repositorySaveDir = Path.Combine(distDir, "Repository");

			foreach (var classInfo in classInfoSet.classInfoList) {
				await CreateMasterData (masterDataSaveDir, classInfo);
			}
			
			var masterDataClassInfoList = classInfoSet.classInfoList.Where(x=>x.name != "MasterDataSet").ToList();
			foreach (var classInfo in masterDataClassInfoList)
			{
				await CreateIDataStore(masterDataSaveDir, classInfo);
				await CreateRepository(repositorySaveDir, classInfo);
			}

			await CreateMasterDataStore(masterDataSaveDir, masterDataClassInfoList);
			await CreateMasterDataRepositoryBuilder(repositorySaveDir, masterDataClassInfoList);
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
		public async Task CreateIDataStore(string dirPath, ClassInfo classInfo){
			var templateLoader = new TemplateLoader ();
			var writer = new Writer ();
			var template = templateLoader.LoadTemplate (TemplatesDir + "IDataStoreTemplate.txt");
			string dataName = classInfo.name;
			var content = template.text.Replace ("$DataName$", dataName);
			string path = Path.Combine (dirPath, string.Format("I{0}Store", dataName));
			await writer.CreateNewScript (path, content);
			Debug.Log ("Create " + path);
		}
		#endregion

		#region MasterDataRepositoryBuilder
		public async Task CreateMasterDataRepositoryBuilder(string dirPath, List<ClassInfo> classInfoList){
			var templateLoader = new TemplateLoader ();
			var writer = new Writer ();
			var template = templateLoader.LoadTemplate (TemplatesDir + "MasterDataRepositoryBuilder.txt");
			var build = GetMasterDataRepositoryBuilderBuild(classInfoList.Select(x=>x.name).ToList());
			var content = template.text.Replace ("$Build$", build);
			string path = Path.Combine (dirPath, "MasterDataRepositoryBuilder");
			await writer.CreateNewScript (path, content);
			Debug.Log ("Create " + path);
		}

		string GetMasterDataRepositoryBuilderBuild(List<string> dataNameList){
			var stringBuilder = new StringBuilder ();
			for (int i = 0; i < dataNameList.Count; i++)
			{
				int indentLevel = i == 0 ? 0 : 3;
				var line = Tabs(indentLevel) + string.Format("{0}Repository.Default = new {0}Repository (dataStore);", dataNameList[i]);
				if(i == dataNameList.Count - 1)
					stringBuilder.Append(line);
				else
					stringBuilder.AppendLine(line);
			}
			return stringBuilder.ToString();
		}
		#endregion

		#region MasterDataStore
		public async Task CreateMasterDataStore(string dirPath, List<ClassInfo> classInfoList){
			var templateLoader = new TemplateLoader ();
			var writer = new Writer ();
			var template = templateLoader.LoadTemplate (TemplatesDir + "MasterDataStore.txt");
			var dataNameList = classInfoList.Select(x=>x.name).ToList();
			string interfaces = GetMasterDataStoreInterfaces(dataNameList);
			string properties = GetMasterDataStoreProperties(dataNameList);
			var content = template.text.Replace ("$Interfaces$", interfaces);
			content = content.Replace ("$Properties$", properties);
			string path = Path.Combine (dirPath, "MasterDataStore");
			await writer.CreateNewScript (path, content);
			Debug.Log ("Create " + path);
		}

		string GetMasterDataStoreInterfaces(List<string> dataNameList){
			var stringBuilder = new StringBuilder ();
			for (int i = 0; i < dataNameList.Count; i++)
			{
				int indentLevel = i == 0 ? 0 : 3;
				var line = Tabs(indentLevel) + string.Format("I{0}Store", dataNameList[i]);
				if(i != dataNameList.Count - 1)
					line += ", ";
				stringBuilder.Append(line);
			}
			return stringBuilder.ToString();
		}

		string GetMasterDataStoreProperties(List<string> dataNameList){
			var stringBuilder = new StringBuilder ();
			for (int i = 0; i < dataNameList.Count; i++)
			{
				int indentLevel = i == 0 ? 0 : 3;
				var line = Tabs(indentLevel) + string.Format("public List<{0}> {0}List { get { return dataSet.{1}List; } }", dataNameList[i], UpperCamelToCamel(dataNameList[i]));
				if(i == dataNameList.Count - 1)
					stringBuilder.Append(line);
				else
					stringBuilder.AppendLine(line);
			}
			return stringBuilder.ToString();
		}
		#endregion

		#region Repository
		public async Task CreateRepository(string dirPath, ClassInfo classInfo){
			var templateLoader = new TemplateLoader ();
			var writer = new Writer ();
			var template = templateLoader.LoadTemplate (TemplatesDir + "RepositoryTemplate.txt");
			string dataName = classInfo.name;
			var content = template.text.Replace ("$DataName$", dataName);
			string path = Path.Combine (dirPath, string.Format("{0}Repository.Data", dataName));
			await writer.CreateNewScript (path, content);
			Debug.Log ("Create " + path);
		}
		#endregion
		
	}
}