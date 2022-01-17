using System.Collections.Generic;
using Interaction.Interactables;
using UnityEngine;

namespace RoomLoop
{
    public class RoomPuzzleController : MonoBehaviour
    {
        [SerializeField] private Transform normalRoom;
        [SerializeField] private Transform abstractRoom;
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
                var diff = obj.transform.position - abstractRoom.position;
                norm.transform.position = normalRoom.position + diff + new Vector3(0, 0.5f, 0);
                norm.transform.rotation = obj.transform.rotation;
            }
            else {
                var pair = objectPairs.Find(x => x.normalObject == obj);
                var abstr = pair.abstractObject;
                var diff = obj.transform.position - normalRoom.position;
                abstr.transform.position = abstractRoom.position + diff + new Vector3(0, 0.5f, 0);
                abstr.transform.rotation = obj.transform.rotation;
            }
        }

        public void SyncPair(RoomInteractable obj)
        {
            if (obj.isAbstract) {
                var pair = objectPairs.Find(x => x.abstractObject == obj);
                var norm = pair.normalObject;
                norm.Sync(obj);
            }
            else {
                var pair = objectPairs.Find(x => x.normalObject == obj);
                var abstr = pair.abstractObject;
                abstr.Sync(obj);
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