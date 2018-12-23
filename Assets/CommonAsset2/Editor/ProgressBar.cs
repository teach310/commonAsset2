using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
namespace CA2 {
    public class ProgressBar : IProgress<float>, IDisposable
    {
		bool disposedValue = false;
		float progress = 0f;
        string titleCache;
        public string info;
        public void Report(float value)
        {
            this.progress = value;
            Display();
        }
		
        public ProgressBar(string title, string info = null){
            titleCache = title;
            this.info = info;
        }

		public void Display(string info){
            this.info = info;
			Display();
		}

        void Display(){
            if(!disposedValue)
            	EditorUtility.DisplayProgressBar(titleCache, info, progress);
        }

		public void Clear(){
			progress = 0f;
			EditorUtility.ClearProgressBar();
		}

        public void Dispose()
        {
            EditorUtility.ClearProgressBar();
			disposedValue = true;
        }
    }
}