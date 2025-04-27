using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class AlwaysDrawRenderFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class AlwaysDrawSettings
    {
        public LayerMask layerMask = -1; // Mặc định vẽ tất cả Layer
        public string shaderTagId = "UniversalForward"; // Shader pass name
        public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingOpaques; // Thời điểm inject
    }

    class AlwaysDrawPass : ScriptableRenderPass
    {
        private FilteringSettings filteringSettings;
        private ShaderTagId shaderTagId;

        public AlwaysDrawPass(AlwaysDrawSettings settings)
        {
            filteringSettings = new FilteringSettings(RenderQueueRange.all, settings.layerMask);
            shaderTagId = new ShaderTagId(settings.shaderTagId);
            renderPassEvent = settings.renderPassEvent;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var drawSettings = CreateDrawingSettings(shaderTagId, ref renderingData, SortingCriteria.None);
            context.DrawRenderers(renderingData.cullResults, ref drawSettings, ref filteringSettings);
        }
    }

    public AlwaysDrawSettings settings = new AlwaysDrawSettings();
    private AlwaysDrawPass pass;

    public override void Create()
    {
        pass = new AlwaysDrawPass(settings);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(pass);
    }
}
