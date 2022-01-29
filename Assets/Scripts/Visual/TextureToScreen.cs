using UnityEngine;
using UnityEngine.Rendering;

namespace Visual
{
    public class TextureToScreen : MonoBehaviour
    {
        public RenderTexture tex;
        public RenderTexture interactable;
        public RenderTexture cube;
        public RenderTexture item;
        private void Awake()
        {
            RenderPipelineManager.endFrameRendering += re;
        }

        private void re(ScriptableRenderContext ctx, Camera[] cam)
        {
            if (Application.isPlaying) {
                Graphics.CopyTexture(interactable, tex);
                Graphics.CopyTexture(cube, tex);
                Graphics.CopyTexture(item, tex);
                Graphics.Blit(tex, null as RenderTexture);
            }
        }
    }
}