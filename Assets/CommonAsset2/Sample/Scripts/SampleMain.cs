using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Async;
namespace CA2 {
    public class SampleMain : SceneBase {

        public void GoResult(){
            SimpleSceneNavigator.Instance.GoForwardAsync<SampleResult>(new SampleResult.Options(10)).Forget();
        }
    }
}