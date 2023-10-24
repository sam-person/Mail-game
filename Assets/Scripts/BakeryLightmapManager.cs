/// <summary>
/// Author: tudor, https://github.com/tdbe
/// You found this gist at: https://gist.github.com/tdbe/e3c41e6a2dedd88dad907218126a2c39
/// 
/// This is free and unencumbered software released into the public domain.
//  
//  Anyone is free to copy, modify, publish, use, compile, sell, or
//  distribute this software, either in source code form or as a compiled
//  binary, for any purpose, commercial or non-commercial, and by any
//  means.
//  
//  In jurisdictions that recognize copyright laws, the author or authors
//  of this software dedicate any and all copyright interest in the
//  software to the public domain. We make this dedication for the benefit
//  of the public at large and to the detriment of our heirs and
//  successors. We intend this dedication to be an overt act of
//  relinquishment in perpetuity of all present and future rights to this
//  software under copyright law.
//  
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//  EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//  MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//  IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//  OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//  ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//  OTHER DEALINGS IN THE SOFTWARE.
//  
//  For more information, please refer to <http://unlicense.org/>
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// [EDIT] For now I disabled the dictionary / mesh renderer recording functionality (actually there's a checkbox in the inspector). It's more complicated than I thought to synchronize the snapshots in all situations. And serializing to the inspector is very slow if you have thousands of baked mesh renderers.
/// So the workflow now is to bake the whole scene once, then next bakes when you bake only parts of the scene, duplicate the array entry for the full scene bake, and drag into it the lightmaps generated in the subsequent bakes to make a new snapshot for e.g. night mode.
/// /// [/EDIT]
/// 
/// This is a lightmap manager for the Bakery lightmapper from the unity asset store. You can now swap between multiple versions of your lightmaps, and combine different bakes from different lightmap groups.
/// ( https://assetstore.unity.com/packages/tools/level-design/bakery-gpu-lightmapper-122218 )
/// ( https://geom.io/bakery/wiki/index.php?title=Bakery_-_GPU_Lightmapper )
/// ( https://forum.unity.com/threads/bakery-gpu-lightmapper-v1-55-released.536008/ )
/// 
/// Usage:
/// Place this script in your scene and make it a prefab. Every time you bake, click the save checkbox to save and append a new snapshot. (If you want to delete an array element from that list, right click it and remove it manually)
/// This script does not make copies of your lightmap textures. It only saves the references your scene has recorded in its LightmapData to the lightmap textures you've just generated, and saves all your scene's Mesh Renderers' lightmap indexes and scales, and restores all these references later (or in a different scene).
/// 
/// This script allows you to have 2+ copies of the same scene, where you can use Bakery to bake with different lightmap settings or light settings (e.g. night mode),
/// then you can apply the prefab, and come back to your main original scene, and then load in that bake or toggle between the different bakes (even at runtime!).
/// 
/// In order to bake 2+ different lightmaps, what I do is use a bakery Lightmap Group, and create 2+ LMG assets e.g.: [LMG][Bedroom]Daytime.asset, and [LMG][Bedroom]Nighttime.asset. Baking with either, produces lightmaps named accordingly.
/// 
/// (call LoadLightmapData(index) to swap lightmaps at runtime)
/// (this script uses a dictionary of mesh renderer instance IDs, so it should sync lightmaps for all objects that have the same index id but were baked in a different scene or at a different time.)
/// </summary>
[ExecuteInEditMode]
public class BakeryLightmapManager : MonoBehaviour
{
    LightmapData[] m_savedSceneLightmapData;

    [System.Serializable]
    public struct CustomLightmaps
    {
        public Texture2D lightmapColor;
        public Texture2D lightmapDir;
        //public Texture2D lightmapLight;
        public Texture2D shadowMask;
    }
    [System.Serializable]
    public struct MeshRendererLightmapData
    {
        public string friendlyName;
        public MeshRenderer actualMeshRenderer;
        public int objectHash;
        public int RendererLightmapIndex;
        public Vector4 RendererLightmapScaleOffset;

        public MeshRendererLightmapData(MeshRenderer actualMeshRenderer, string friendlyName, int objectHash, int RendererLightmapIndex, Vector4 RendererLightmapScaleOffset)
        {
            this.actualMeshRenderer = actualMeshRenderer;
            this.friendlyName = friendlyName;
            this.objectHash = objectHash;
            this.RendererLightmapIndex = RendererLightmapIndex;
            this.RendererLightmapScaleOffset = RendererLightmapScaleOffset;
        }
    }
    [Space(15)]
    [SerializeField]
    List<CustomLightmapsArray> m_savedSceneLightmaps;
    [Space(30)]
    [TextArea(3, 3)]
    public string info0 = "Press this bool after you finished a full bake \nto record all generated lightmap references.";
    [Space(5)]
    [SerializeField]
    bool m_SaveNewLightmapData;

    [Space(15)]
    [TextArea(3, 3)]
    public string info1 = "Press this bool after you Render Selected Groups \nand you see the other non-re-rendered groups \nsuddenly got broken lightmaps/UVs. Or if \nyou just want to swap between different lightmap variants (e.g. day/night).";
    [Space(15)]
    [SerializeField]
    int m_LightmapSelectionToRestoreFrom = 0;
    [Space(5)]
    [SerializeField]
    bool m_LoadLightmapFromSelection;

    [Space(20)]
    [Header("________________________________________________________________________________")]
    [Space(45)]
    [SerializeField]
    bool m_AlsoSaveLightmapDataFromAllMeshRenderers = false;

    [TextArea(3, 3)]
    public string info2 = "Be patient when you click Save or Load. It may take a while (e.g. up to 10 seconds) if you have many thousands of mesh renderers in your scene.";

    [System.Serializable]
    public class CustomLightmapsArray
    {//we need a struct/class to hold arrays because unity inspector does not support arrays of arrays, but supports arrays of structs/classes that hold arrays........!
        public string friendlyName;
        public CustomLightmaps[] customLightmaps;
        public Dictionary<int, MeshRendererLightmapData> meshRendererLightmapDataFromScene;
        [TextArea(4, 4)]
        public string info;// "The following array is just for show in the inspector. Internally I use a dictionary of instance ids, so it can be ported between similar scenes without needing mesh renderer references.";
        public List<MeshRendererLightmapData> meshRendererLightmapDataFromScene_inspector;

        public CustomLightmapsArray(int length)
        {
            friendlyName = "";
            customLightmaps = new CustomLightmaps[length];
            meshRendererLightmapDataFromScene = new Dictionary<int, MeshRendererLightmapData>();

            info = "The following array is for show in the inspector. BUT also to save the non-serializable internal dictionary of instance ids";
            meshRendererLightmapDataFromScene_inspector = new List<MeshRendererLightmapData>();
        }
        public CustomLightmapsArray(int length, string friendlyName)
        {
            this.friendlyName = friendlyName;
            customLightmaps = new CustomLightmaps[length];
            meshRendererLightmapDataFromScene = new Dictionary<int, MeshRendererLightmapData>();

            info = "The following array is for show in the inspector. BUT also to save the non-serializable internal dictionary of instance ids";
            meshRendererLightmapDataFromScene_inspector = new List<MeshRendererLightmapData>();
        }

        public void CloneToInspectorArray()
        {
            meshRendererLightmapDataFromScene_inspector.Clear();
            meshRendererLightmapDataFromScene_inspector.AddRange(meshRendererLightmapDataFromScene.Values);
        }

        public void LoadDictionaryFromInspectorArray()
        {
            meshRendererLightmapDataFromScene = new Dictionary<int, MeshRendererLightmapData>();
            foreach (MeshRendererLightmapData lmd in meshRendererLightmapDataFromScene_inspector)
            {
                meshRendererLightmapDataFromScene.Add(lmd.objectHash, lmd);
            }
        }
    }


    public void SaveLightmapData()
    {
        m_savedSceneLightmapData = new LightmapData[LightmapSettings.lightmaps.Length];

        for (int i = 0; i < LightmapSettings.lightmaps.Length; i++)
        {
            m_savedSceneLightmapData[i] = LightmapSettings.lightmaps[i];
        }

        int index;
        if (m_AlsoSaveLightmapDataFromAllMeshRenderers)
        {
            MeshRenderer[] allMeshRenderers = (MeshRenderer[])FindObjectsOfType(typeof(MeshRenderer));
            m_savedSceneLightmaps.Add(new CustomLightmapsArray(m_savedSceneLightmapData.Length, m_savedSceneLightmaps.Count + " "));
            index = m_savedSceneLightmaps.Count - 1;
            m_LightmapSelectionToRestoreFrom = index;

            for (int i = 0; i < allMeshRenderers.Length; i++)
            {
                if (allMeshRenderers[i].lightmapIndex == -1)
                {
                    continue;
                }

                MeshRendererLightmapData newLMD = new MeshRendererLightmapData(allMeshRenderers[i], allMeshRenderers[i].name, allMeshRenderers[i].GetHashCode(), allMeshRenderers[i].lightmapIndex, allMeshRenderers[i].lightmapScaleOffset);
                m_savedSceneLightmaps[index].meshRendererLightmapDataFromScene.Add(newLMD.objectHash, newLMD);
            }
            m_savedSceneLightmaps[m_LightmapSelectionToRestoreFrom].CloneToInspectorArray();
        }
        else
        {
            index = m_savedSceneLightmaps.Count - 1;
            m_LightmapSelectionToRestoreFrom = index;
        }

        for (int i = 0; i < LightmapSettings.lightmaps.Length; i++)
        {
            m_savedSceneLightmaps[index].customLightmaps[i].lightmapColor = m_savedSceneLightmapData[i].lightmapColor;
            m_savedSceneLightmaps[index].customLightmaps[i].lightmapDir = m_savedSceneLightmapData[i].lightmapDir;
            //m_savedSceneLightmaps[i].lightmapLight = m_savedSceneLightmapData[i].lightmapLight;
            m_savedSceneLightmaps[index].customLightmaps[i].shadowMask = m_savedSceneLightmapData[i].shadowMask;
        }

        Debug.Log("[BakeryLightmapManager][SAVED] Current LightmapSettings.lightmaps (lightmap texture references) saved to m_savedSceneLightmaps!");
    }

    public void LoadLightmapData(int lightmapSelectionToRestoreFrom)
    {
        m_LightmapSelectionToRestoreFrom = lightmapSelectionToRestoreFrom;

        //LightmapSettings.lightmaps = new LightmapData[m_savedSceneLightmapData.Length];
        LightmapData[] customLightmapDataArr = new LightmapData[m_savedSceneLightmaps[m_LightmapSelectionToRestoreFrom].customLightmaps.Length];
        /*
        if(m_savedSceneLightmapData!=null)
            for(int i = 0; i< m_savedSceneLightmaps.Length; i++){
                if(i<m_savedSceneLightmapData.Length)
                    customLightmapDataArr[i] = m_savedSceneLightmapData[i];
                else{
                    customLightmapDataArr[i] = new LightmapData();
                    Debug.Log("[BakeryLightmapManager] The internal m_savedSceneLightmapData number of textures does not match the m_savedSceneLightmaps number of textures that you have in the inspector. Did you forget to Save New Lightmap Data after the previous bake completed?");
                }
            }
        else*/
        for (int i = 0; i < m_savedSceneLightmaps[m_LightmapSelectionToRestoreFrom].customLightmaps.Length; i++)
        {
            customLightmapDataArr[i] = new LightmapData();
        }

        if (m_AlsoSaveLightmapDataFromAllMeshRenderers)
        {
            //Unity can't serialize dictionaries, and this editor script will have its internal dictionary flushed/reset for instance every time you recompile any scripts in your project.
            if (m_savedSceneLightmaps[m_LightmapSelectionToRestoreFrom].meshRendererLightmapDataFromScene == null)
            {
                m_savedSceneLightmaps[m_LightmapSelectionToRestoreFrom].LoadDictionaryFromInspectorArray();
            }

            MeshRenderer[] allMeshRenderers = (MeshRenderer[])FindObjectsOfType(typeof(MeshRenderer));
            //foreach(KeyValuePair<int,MeshRendererLightmapData> mrld in m_savedSceneLightmaps[m_LightmapSelectionToRestoreFrom].meshRendererLightmapDataForLoadedScenes){
            //foreach(MeshRendererLightmapData mrld in m_savedSceneLightmaps[m_LightmapSelectionToRestoreFrom].meshRendererLightmapDataForLoadedScenes){
            //mrld.actualMeshRenderer.lightmapIndex = mrld.RendererLightmapIndex;
            //mrld.actualMeshRenderer.lightmapScaleOffset = mrld.RendererLightmapScaleOffset;
            //}
            for (int i = 0; i < allMeshRenderers.Length; i++)
            {
                MeshRendererLightmapData mrld;

                bool success = m_savedSceneLightmaps[m_LightmapSelectionToRestoreFrom].meshRendererLightmapDataFromScene.TryGetValue(allMeshRenderers[i].GetInstanceID(), out mrld);
                if (success)
                {
                    allMeshRenderers[i].lightmapIndex = mrld.RendererLightmapIndex;
                    allMeshRenderers[i].lightmapScaleOffset = mrld.RendererLightmapScaleOffset;

                    //this is so you can see it in the inspector. I wish Dictionaries worked witht he inspector without having to install 3rd party assets...
                    mrld.actualMeshRenderer = allMeshRenderers[i];
                }
            }
            m_savedSceneLightmaps[m_LightmapSelectionToRestoreFrom].CloneToInspectorArray();
        }

        for (int i = 0; i < m_savedSceneLightmaps[m_LightmapSelectionToRestoreFrom].customLightmaps.Length; i++)
        {
            customLightmapDataArr[i].lightmapColor = m_savedSceneLightmaps[m_LightmapSelectionToRestoreFrom].customLightmaps[i].lightmapColor;
            customLightmapDataArr[i].lightmapDir = m_savedSceneLightmaps[m_LightmapSelectionToRestoreFrom].customLightmaps[i].lightmapDir;
            customLightmapDataArr[i].shadowMask = m_savedSceneLightmaps[m_LightmapSelectionToRestoreFrom].customLightmaps[i].shadowMask;
        }
        LightmapSettings.lightmaps = customLightmapDataArr;

        Debug.Log("[BakeryLightmapManager][RESTORED] Current LightmapSettings.lightmaps (lightmap texture references) restored from m_savedSceneLightmaps!");
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (m_SaveNewLightmapData)
        {
            m_SaveNewLightmapData = false;
            SaveLightmapData();
        }

        if (m_LoadLightmapFromSelection)
        {
            m_LoadLightmapFromSelection = false;

            LoadLightmapData(m_LightmapSelectionToRestoreFrom);
        }
#endif
    }
}