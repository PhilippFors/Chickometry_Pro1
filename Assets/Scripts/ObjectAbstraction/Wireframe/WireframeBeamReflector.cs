using UnityEngine;

namespace ObjectAbstraction.Wireframe
{
    /// <summary>
    /// Enables/disables beams attached to this object.
    /// Beams attached to this object can only be enabled/disabled by another beam.
    /// </summary>
    public class WireframeBeamReflector : MonoBehaviour
    {
        [SerializeField] private bool enableOnStart;
        [SerializeField] private WireframeBeam[] beamList;
        private bool isEnabled;
    
        void Start()
        {
            if (enableOnStart) {
                foreach (var beam in beamList) {
                    beam.EnableBeam();
                }
            }
            else {
                foreach (var beam in beamList) {
                    beam.DisableBeam();
                }
            }

            isEnabled = enableOnStart;
        }

        public void ToggleBeam()
        {
            if (isEnabled) {
                DisableBeams();
            }
            else {
                EnableBeams();
            }

            isEnabled = !isEnabled;
        }
    
        public void EnableBeams()
        {
            if (enableOnStart) {
                foreach (var beam in beamList) {
                    beam.DisableBeam();
                }
            }
            else {
                foreach (var beam in beamList) {
                    beam.EnableBeam();
                }
            }
        }

        public void DisableBeams()
        {
            if (enableOnStart) {
                foreach (var beam in beamList) {
                    beam.EnableBeam();
                }
            }
            else {
                foreach (var beam in beamList) {
                    beam.DisableBeam();
                }
            }
        }
    }
}