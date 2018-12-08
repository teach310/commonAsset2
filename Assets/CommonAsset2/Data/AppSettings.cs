﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CA2 {
	public class AppSettings{

		static AppSettings instance = null;

		public static AppSettings Instance {
			get {
				if (instance == null) {
					instance = new AppSettings () {
						masterDataUrl = "https://script.google.com/macros/s/AKfycbwEv_sXgDzAS-D_1GEj8-91tMwka4h2uGTMETsbOhwtiM-lFDd1/exec"
					};
				}
				return instance;
			}
		}

		public string masterDataUrl;
	}
}