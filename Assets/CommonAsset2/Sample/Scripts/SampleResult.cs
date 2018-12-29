using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx.Async;

namespace CA2 {
    public class SampleResult : SceneBase {

        public Text label;

        public class Options {
            public Options (int score) {
                this.Score = score;

            }
            public int Score { get; private set; }
        }

        public override void OnLoad (object options) {
            var op = options as Options;
            label.text = op.Score.ToString();
        }

        public void GoTitle () => SimpleSceneNavigator.Instance.GoForwardAsync<SampleTitle> ().Forget();
    }
}