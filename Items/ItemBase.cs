using System;
using Assets.MuscleDrop.Scripts.Players;
using UnityEngine;

namespace Assets.MuscleDrop.Scripts.Items
{

    public abstract class ItemBase : MonoBehaviour
    {
        [SerializeField]
        protected ItemEffect _itemEffect;

        public ItemEffect ItemEffect
        {
            get { return _itemEffect; }
        }

        public virtual void PickedUp()
        {
            Destroy(gameObject);
        }
    }
}