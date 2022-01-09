using System.Collections.Generic;
using UnityEngine;

namespace RoomLoop
{
    public class RoomPuzzleController : MonoBehaviour
    {
        [SerializeField] private List<AbstractionObjectPair> objectPairs = new List<AbstractionObjectPair>();

        private void Awake()
        {
            foreach (var obj in objectPairs) {
                obj.abstractObject.Init(this);
                obj.normalObject.Init(this);
            }
        }

        public void RemoveObject(RoomInteractable obj)
        {
            if (obj.isAbstract) {
                var pair = objectPairs.Find(x => x.abstractObject == obj);

                var norm = pair.normalObject;
                if (norm == null) {
                    return;
                }
                norm.gameObject.SetActive(false);
                norm.MakeInvisible();
            }
            else {
                var pair = objectPairs.Find(x => x.normalObject == obj);
                var abstr = pair.abstractObject;
                if (abstr == null) {
                    return;
                }
                abstr.gameObject.SetActive(false);
                abstr.MakeInvisible();
            }
        }

        public void ReturnObject(RoomInteractable obj)
        {
            if (obj.isAbstract) {
                var pair = objectPairs.Find(x => x.abstractObject == obj);
                var norm = pair.normalObject;
                if (norm == null) {
                    return;
                }
                norm.gameObject.SetActive(true);
                norm.MakeVisible();
            }
            else {
                var pair = objectPairs.Find(x => x.normalObject == obj);
                var abstr = pair.abstractObject;
                if (abstr == null) {
                    return;
                }
                abstr.gameObject.SetActive(true);
                abstr.MakeVisible();
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