using Assets.MuscleDrop.Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.MuscleDrop.Scripts.Menus.UI {
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(RichOutline))]
    public class StageInfo : MonoBehaviour {
        [SerializeField]
        private GameScenes gameScene;

        private Button button;
        private RichOutline outline;

        public GameScenes GameScene { get { return gameScene; } }
        public Button Button { get { return button; } }
        public RichOutline Outline { get { return outline; } }

        private void Awake() {
            button = GetComponent<Button>();
            outline = GetComponent<RichOutline>();
        }
    }
}