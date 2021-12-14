using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

namespace RoomLoop
{
    public class RoomPuzzle : MonoBehaviour
    {
        [SerializeField] private List<AbstractionObjectPair> objectPairs = new List<AbstractionObjectPair>();

        private void Awake()
        {
            foreach (var obj in objectPairs) {
                obj.abstractObject.Init(this);
                obj.normalObject.Init(this);
                // obj.abstractObject.transform.localPosition = obj.normalObject.transform.localPosition;
            }
        }

        public void RemoveObject(RoomInteractable obj)
        {
            if (obj.isAbstract) {
                var pair = objectPairs.Find(x => x.abstractObject == obj);
                var norm = pair.normalObject;
                norm.gameObject.SetActive(false);
            }
            else {
                var pair = objectPairs.Find(x => x.normalObject == obj);
                var abstr = pair.abstractObject;
                abstr.gameObject.SetActive(false);
            }
        }

        public void ReturnObject(RoomInteractable obj)
        {
            if (obj.isAbstract) {
                var pair = objectPairs.Find(x => x.abstractObject == obj);
                var norm = pair.normalObject;
                norm.gameObject.SetActive(true);
            }
            else {
                var pair = objectPairs.Find(x => x.normalObject == obj);
                var abstr = pair.abstractObject;
                abstr.gameObject.SetActive(true);
            }
        }

        public void UpdatePosition(RoomInteractable obj)
        {
            if (obj.isAbstract) {
                var pair = objectPairs.Find(x => x.abstractObject == obj);
                var norm = pair.normalObject;
                norm.transform.localPosition = obj.transform.localPosition;
            }
            else {
                var pair = objectPairs.Find(x => x.normalObject == obj);
                var abstr = pair.abstractObject;
                abstr.transform.localPosition = obj.transform.localPosition;
            }
        }
    }

    [System.Serializable]
    public class AbstractionObjectPair
    {
        public RoomInteractable normalObject;
        public RoomInteractable abstractObject;
    }
}