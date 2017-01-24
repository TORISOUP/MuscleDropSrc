using Assets.MuscleDrop.Scripts.GamaManagers;
using UniRx;
using UnityEngine;

namespace Assets.MuscleDrop.Scripts.Utilities.Editor
{
    [UnityEditor.CustomPropertyDrawer(typeof(GameStateReactiveProperty))] // 他、沢山ここにtypeofを追加していく
    public class ExtendInspectorDisplayDrawer : InspectorDisplayDrawer
    {
    }
}
