using RoomLoop;
using UnityEngine;

namespace Interaction.Interactables
{
    public class PlantRoomInteractable : RoomInteractable
    {
        [SerializeField] private GameObject smallPlant;
        [SerializeField] private GameObject bigPlant;

        private bool plantEnabled;
        public void EnablePlant()
        {
            if (!plantEnabled) {
                plantEnabled = true;
                bigPlant.SetActive(true);
                smallPlant.SetActive(false);
                roomPuzzle.SyncPair(this);
            }
        }

        public override void Sync(RoomInteractable obj)
        {
            EnablePlant();
        }
    }
}