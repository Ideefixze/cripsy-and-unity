using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

public class MyPipeline : RenderPipeline
{
    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        foreach (var camera in cameras)
        {
            Render(context, camera);
        }
    }

    /*
    void Render(ScriptableRenderContext context, Camera camera)
    {
        

        ScriptableCullingParameters cullingParameters;
        if(!camera.TryGetCullingParameters(out cullingParameters))
        {
            return;
        }

        #if UNITY_EDITOR
        if (camera.cameraType == CameraType.SceneView)
        {
            ScriptableRenderContext.EmitWorldGeometryForSceneView(camera);
        }
        #endif

        CullingResults cullingResults = context.Cull(ref cullingParameters);

        context.SetupCameraProperties(camera);

        CommandBuffer buffer = new CommandBuffer() { name = camera.name };

        CameraClearFlags clearFlags = camera.clearFlags;
        buffer.ClearRenderTarget(
            (clearFlags & CameraClearFlags.Depth) != 0,
            (clearFlags & CameraClearFlags.Color) != 0,
            camera.backgroundColor
        );

        context.ExecuteCommandBuffer(buffer);

        buffer.Clear();

        //Opaque
        SortingSettings sortingSettings = new SortingSettings(camera);
        sortingSettings.criteria = SortingCriteria.CommonOpaque;
        DrawingSettings drawingSettings = new DrawingSettings(new ShaderTagId("SRPDefaultUnlit"),sortingSettings);

        FilteringSettings filteringSettings = new FilteringSettings()
        {
            renderQueueRange = RenderQueueRange.opaque
        };

        context.DrawRenderers(
            cullingResults, ref drawingSettings, ref filteringSettings
        );

        context.DrawSkybox(camera);

        //Transparent
        sortingSettings = new SortingSettings(camera);
        sortingSettings.criteria = SortingCriteria.CommonTransparent;
        drawingSettings.sortingSettings = sortingSettings;
        filteringSettings.renderQueueRange = RenderQueueRange.transparent;
        context.DrawRenderers(
             cullingResults, ref drawingSettings, ref filteringSettings
        );

        context.ExecuteCommandBuffer(buffer);
        buffer.Clear();

        context.Submit();
    }*/

    void Render(ScriptableRenderContext context, Camera camera)
    {

        context.SetupCameraProperties(camera);
        ScriptableCullingParameters cullingParams;
        if (!camera.TryGetCullingParameters(out cullingParams)) return;

#if UNITY_EDITOR
        if (camera.cameraType == CameraType.SceneView)
        {
            ScriptableRenderContext.EmitWorldGeometryForSceneView(camera);
        }
#endif


        CullingResults cull = context.Cull(ref cullingParams);
        CommandBuffer buffer = CommandBufferPool.Get(camera.name);

        var clearFlags = camera.clearFlags;
        buffer.ClearRenderTarget(
            (clearFlags & CameraClearFlags.Depth) != 0,
            (clearFlags & CameraClearFlags.Color) != 0,
            camera.backgroundColor
        );

        context.ExecuteCommandBuffer(buffer);
        buffer.Clear();

        var drawSettings = new DrawingSettings(new ShaderTagId("SRPDefaultUnlit"), new SortingSettings(camera));
        drawSettings.enableDynamicBatching = true;
        drawSettings.enableInstancing = true;
        var filterSettings = new FilteringSettings(null, camera.cullingMask);

        filterSettings.renderQueueRange = RenderQueueRange.opaque;
        context.DrawRenderers(cull, ref drawSettings, ref filterSettings);

        filterSettings.renderQueueRange = RenderQueueRange.transparent;
        context.DrawRenderers(cull, ref drawSettings, ref filterSettings);

        CommandBufferPool.Release(buffer);
        context.Submit();
    }

}

