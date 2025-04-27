using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RendererUtils;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

public class AlwaysDrawRenderFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class AlwaysDrawSettings
    {
        public LayerMask layerMask = -1;
        public string shaderTagId = "UniversalForward";
        public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingOpaques;
    }

    private class AlwaysDrawPass : ScriptableRenderPass
    {
        private FilteringSettings filteringSettings;
        private ShaderTagId shaderTagId;

        public AlwaysDrawPass(AlwaysDrawSettings settings)
        {
            filteringSettings = new FilteringSettings(RenderQueueRange.all, settings.layerMask);
            shaderTagId = new ShaderTagId(settings.shaderTagId);
            renderPassEvent = settings.renderPassEvent;
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            var cameraData = frameData.Get<UniversalCameraData>();
            var cullResults = frameData.Get<UniversalRenderingData>().cullResults;

            if (cameraData.camera == null || !cameraData.renderType.Equals(CameraRenderType.Base))
                return;

            using (var builder = renderGraph.AddRenderPass<PassData>("Always Draw Pass", out var passData))
            {
                builder.AllowPassCulling(false);

                passData.cameraData = cameraData;
                passData.cullResults = cullResults;
                var simpleCameraData = new CameraData
                {
                    camera = cameraData.camera
                };
                RenderingData renderingData = new RenderingData
                {
                    cameraData = simpleCameraData,
                    cullResults = cullResults
                };
                builder.SetRenderFunc((PassData data, RenderGraphContext ctx) =>
                {

                    var drawingSettings = CreateDrawingSettings(shaderTagId, ref renderingData, SortingCriteria.CommonOpaque);
                    // Replace obsolete DrawRenderers with RendererList API  
                    var rendererListDesc = new RendererListDesc(shaderTagId, data.cullResults, data.cameraData.camera)
                    {
                        sortingCriteria = SortingCriteria.CommonOpaque,
                        rendererConfiguration = PerObjectData.None,
                        renderQueueRange = RenderQueueRange.all,
                        layerMask = filteringSettings.layerMask
                    };

                    var rendererList = ctx.renderContext.CreateRendererList(rendererListDesc);
                    ctx.cmd.DrawRendererList(rendererList);
                });
            }
        }

        private class PassData
        {
            public UniversalCameraData cameraData;
            public CullingResults cullResults;
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
