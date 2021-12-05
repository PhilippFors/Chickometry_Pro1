using UnityEngine;

namespace RoomLoop
{
    public class AnchorId : MonoBehaviour
    {
        [SerializeField] private int id;
        
        public bool CheckAchors()
        {
            var hits = Physics.OverlapSphere(transform.position, 0.5f, LayerMask.GetMask("Anchor"), QueryTriggerInteraction.Collide);
            if (hits.Length > 0) {
                for (int i = 0; i < hits.Length; i++) {
                    var anchor = hits[i].GetComponent<AnchorId>();
                    if (anchor && anchor != this && anchor.id != id) {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}