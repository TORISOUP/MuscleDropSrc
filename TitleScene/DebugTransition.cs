using System;
using Assets.MuscleDrop.Scripts.Utilities;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.MuscleDrop.Scripts.TitleScene
{
    public class DebugTransition : MonoBehaviour
    {
        void Start()
        {
            Observable.Timer(TimeSpan.FromSeconds(2))
                .Subscribe(_ => SceneLoader.LoadScene(GameScenes.Menu));
            
        }

    }
}
