using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Async;

namespace CA2 {
    public class SampleTitle : SceneBase {
        
        public void GoMain() => SimpleSceneNavigator.Instance.GoForwardAsync<SampleMain>().Forget();
    }
}