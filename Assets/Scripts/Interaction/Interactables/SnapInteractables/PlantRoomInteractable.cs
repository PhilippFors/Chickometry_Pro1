using RoomLoop;
using UnityEngine;

namespace Interaction.Interactables
{
    public class PlantRoomInteractable : RoomInteractable
    {
        [SerializeField] private GameObject plant;

        private bool plantEnabled;
        public void EnablePlant()
        {
            if (!plantEnabled) {
                plantEnabled = true;
                plant.SetActive(true);
                roomPuzzle.SyncPair(this);
            }
        }

        public override void Sync(RoomInteractable obj)
        {
            EnablePlant();
        }
    }
}