using System.Collections;
using System.Collections.Generic;
using Assets.MuscleDrop.Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.MuscleDrop.Scripts.Menus.UI {
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(RichOutline))]
    public class PlayerInfo : MonoBehaviour {
        [SerializeField]
        private int playerNum;

        private Button button;
        private RichOutline outline;

        public int Num { get { return playerNum; } }
        public Button Button { get { return button; } }
        public RichOutline Outline { get { return outline; } }

        private void Awake() {
            button = GetComponent<Button>();
            outline = GetComponent<RichOutline>();
        }
    }
}
