using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using VerticesEngine.ContentManagement;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.Events;

namespace VerticesEngine
{
    /// <summary>
    /// The falloff rate for area of effects. Often time this is used in Terrain Editing
    /// and painting.
    /// </summary>
    public enum vxEnumFalloffRate
    {
        /// <summary>
        /// A Linear Rate is used for porpoatial fall off.
        /// </summary>
        Linear,

        /// <summary>
        /// The same value is used across the entire area of effect, useful for creating clifs
        /// or sharp drops.
        /// </summary>
        Flat,

        /// <summary>
        /// A Smooth transition between the center and outsides of the mesh.
        /// </summary>
        Smooth
    }

    /// <summary>
    /// The mode for scultping, whether it's averaging, or creates a delta (addative/subtractive).
    /// </summary>
    public enum vxEnumAreaOfEffectMode
    {
        /// <summary>
        /// Creates a Delta 
        /// </summary>
        Delta,

        /// <summary>
        /// Averages the values within the area of effect
        /// </summary>
        Averaged,
    }

    public partial class vxGameplayScene3D
    {

        public int TexturePaintType = 1;

        /// <summary>
        /// The Terrain Sculpt Toggel Button
        /// </summary>
        vxRibbonButtonControl SculptButton;

        /// <summary>
        /// The Terrain Texture Paint Toggel Button.
        /// </summary>
        //vxRibbonButtonControl TexturePaintButton;


        vxRibbonButtonControl DeltaSculptButton;
        vxRibbonButtonControl AverageSculptButton;

        vxRibbonContextualTabPage terrainTabPage;
        vxRibbonContextualTabPage screenCaptureTabPage;
        List<vxToolbarButton> FallOffButtons = new List<vxToolbarButton>();

        /// <summary>
        /// A List of the Terrain Texture Paint Buttons.
        /// </summary>
        List<vxTxtrPaintToolbarButton> TxtrPaintButtons = new List<vxTxtrPaintToolbarButton>();

        public vxEnumFalloffRate FalloffRate;

        public vxEnumAreaOfEffectMode AreaOfEffectMode;

        void OnInitialiseUITerrainToolbar()
        {
            terrainTabPage = new vxRibbonContextualTabPage(EditorRibbonControl, "Terrain", "Edit Terrain", Color.Green);


            var terraincameraGroup = new vxRibbonControlGroup(terrainTabPage, "Edit Terrain");
            SculptButton = new vxRibbonButtonControl(terraincameraGroup, vxLocKeys.Sandbox_Terrain_Scuplt, vxInternalAssets.UI.Terrain_Mode_Sculpt, vxEnumButtonSize.Big);
            SculptButton.IsTogglable = true;
            SculptButton.ToggleState = true;
            SculptButton.Clicked += SculptButton_Clicked;
            SculptButton.SetToolTip("Toggle Terrain Sculpt Mode");

            //TexturePaintButton = new vxRibbonButtonControl(terraincameraGroup, "Texture Paint",
            //                                                     vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/rbn/terrain/mode_txtrpaint"), vxEnumButtonSize.Big);
            //TexturePaintButton.IsTogglable = true;
            //TexturePaintButton.Clicked += TexturePaintButton_Clicked;
            //TexturePaintButton.SetToolTip("Toggle Terrain Texture Painting Mode");


            var terrainEditFalloffGroup = new vxRibbonControlGroup(terrainTabPage, "Falloff Type");

            DeltaSculptButton = new vxRibbonButtonControl(terrainEditFalloffGroup, vxLocKeys.Sandbox_Terrain_Delta, vxInternalAssets.UI.Terrain_Sculpt_Delta, vxEnumButtonSize.Big);

            DeltaSculptButton.IsTogglable = true;
            DeltaSculptButton.ToggleState = true;
            DeltaSculptButton.Clicked += DeltaSculptButton_Clicked; ;
            DeltaSculptButton.SetToolTip("Area effect creates a Delta");


            AverageSculptButton = new vxRibbonButtonControl(terrainEditFalloffGroup, vxLocKeys.Sandbox_Terrain_Average, vxInternalAssets.UI.Terrain_Sculpt_Avg, vxEnumButtonSize.Big);
            AverageSculptButton.IsTogglable = true;
            AverageSculptButton.Clicked += AverageSculptButton_Clicked; ;
            AverageSculptButton.SetToolTip("Area effect creates an Average");




            var terrainEditTypeGroup = new vxRibbonControlGroup(terrainTabPage, "Terrain Edit Type");

            var terrainEditSmooth = new vxRibbonButtonControl(terrainEditTypeGroup, vxLocKeys.Sandbox_Terrain_Smooth, vxInternalAssets.UI.Terrain_Edit_Smooth);

            var terrainEditLinear = new vxRibbonButtonControl(terrainEditTypeGroup, vxLocKeys.Sandbox_Terrain_Linear, vxInternalAssets.UI.Terrain_Edit_Linear);
            var terrainEditFlat = new vxRibbonButtonControl(terrainEditTypeGroup, vxLocKeys.Sandbox_Terrain_Flat,vxInternalAssets.UI.Terrain_Edit_Flat);

           


            var terrainEditEnd = new vxRibbonButtonControl(new vxRibbonControlGroup(terrainTabPage, "Finish"), "Exit", vxInternalAssets.UI.Terrain_Edit_Exit, vxEnumButtonSize.Big);
            
            terrainEditEnd.Clicked += delegate
            {
                CloseTerrainEditor();
            };



            //// Handle Scuplting Porporational Types

            //foreach (vxEnumFalloffRate type in Enum.GetValues(typeof(vxEnumFalloffRate)))
            //{
            //    string texturePath = "Textures/sandbox/tlbr/terrain/sculpt_" + type.ToString().ToLower();
            //    vxToolbarButton fallOffButton = new vxToolbarButton(Engine, Engine.InternalAssets, texturePath);
            //    fallOffButton.IsTogglable = true;
            //    fallOffButton.Clicked += FallOffButton_Clicked;
            //    fallOffButton.Text = ((int)type).ToString();
            //    fallOffButton.SetToolTip("Set Active Sculpting Falloff too: " + type);

            //    FallOffButtons.Add(fallOffButton);
            //}
            //FallOffButtons[0].ToggleState = true;


            //// Terrain Painint
            //for (int i = 0; i < TerrainManager.Textures.Count; i++)
            //{
            //    vxTxtrPaintToolbarButton terrainPaintBtn = new vxTxtrPaintToolbarButton(Engine, TerrainManager, i);
            //    //terrainPaintBtn.IsTogglable = true;
            //    terrainPaintBtn.Enabled = false;
            //    terrainPaintBtn.Clicked += TerrainPaintBtn_Clicked;
            //    TxtrPaintButtons.Add(terrainPaintBtn);
            //    terrainPaintBtn.SetToolTip("Set Active Texture for Painting To Texture " + (i+1).ToString());
            //}
            //TxtrPaintButtons[0].ToggleState = true;


            //vxToolbarButton ExitTerrainEdtiorButton = new vxToolbarButton(Engine, Engine.InternalAssets, "Textures/sandbox/tlbr/terrain/exit");
            //ExitTerrainEdtiorButton.Clicked += ExitTerrainEdtiorButton_Clicked;
            //ExitTerrainEdtiorButton.SetToolTip("Exit from Terrain Editing");


            //TerrainEditorToolbar.AddItem(SculptButton);
            //TerrainEditorToolbar.AddItem(TexturePaintButton);
            //TerrainEditorToolbar.AddItem(new vxToolbarSpliter(Engine, 5));
            
            //TerrainEditorToolbar.AddItem(DeltaSculptButton);
            //TerrainEditorToolbar.AddItem(AverageSculptButton);
            //TerrainEditorToolbar.AddItem(new vxToolbarSpliter(Engine, 5));
            
            //foreach (vxToolbarButton fllOffbtn in FallOffButtons)
            //    TerrainEditorToolbar.AddItem(fllOffbtn);

            //TerrainEditorToolbar.AddItem(new vxToolbarSpliter(Engine, 5));

            //foreach (vxTxtrPaintToolbarButton button in TxtrPaintButtons)
            //    TerrainEditorToolbar.AddItem(button);

            //TerrainEditorToolbar.AddItem(new vxToolbarSpliter(Engine, 5));
            //TerrainEditorToolbar.AddItem(ExitTerrainEdtiorButton);


        }


        void CloseTerrainEditor()
        {

            if (SandboxEditMode == vxEnumSanboxEditMode.TerrainEdit)
            {
                terrainTabPage.IsAdded = false;
                //EntitiesTabPage.SelectTab();
                EditorRibbonControl.Pages[2].SelectTab();
                EditorRibbonControl.RemoveContextTab(terrainTabPage);

                SandboxEditMode = vxEnumSanboxEditMode.SelectItem;
            }
        }

        void OnInitialiseUIScreenCaptureToolbar()
        {
            screenCaptureTabPage = new vxRibbonContextualTabPage(EditorRibbonControl, "Screen Capture", vxLocKeys.Sandbox_ScreenCapture, Color.DeepSkyBlue);


            var screenCaptureGroup = new vxRibbonControlGroup(screenCaptureTabPage, vxLocKeys.Sandbox_ScreenCapture);
            var takeScreenShot = new vxRibbonButtonControl(screenCaptureGroup, vxLocKeys.Sandbox_TakeScreenshot, vxInternalAssets.UI.Terrain_Mode_Sculpt, vxEnumButtonSize.Big);
            screenCaptureGroup.Clicked += 
                delegate {

                    EditorRibbonControl.Pages[0].SelectTab();
                    EditorRibbonControl.RemoveContextTab(screenCaptureTabPage);

                    SandboxEditMode = vxEnumSanboxEditMode.SelectItem;

                    screenShotCallback?.Invoke();
                
                };
            //SculptButton.SetToolTip("Toggle Terrain Sculpt Mode");


            var terrainEditEnd = new vxRibbonButtonControl(new vxRibbonControlGroup(screenCaptureTabPage, "Finish"), "Exit", vxInternalAssets.UI.Terrain_Edit_Exit, vxEnumButtonSize.Big);

            terrainEditEnd.Clicked += delegate
            {
                EditorRibbonControl.Pages[0].SelectTab();
                EditorRibbonControl.RemoveContextTab(screenCaptureTabPage);

                SandboxEditMode = vxEnumSanboxEditMode.SelectItem;

                // exit out and re-show the settings dialog
                this.OnShowSettingsDialog();
            };
        }

        Action screenShotCallback;

        public void EnterScreenShotMode(Action callback)
        {
            screenShotCallback = callback;
            EditorRibbonControl.AddContextTab(screenCaptureTabPage);
            screenCaptureTabPage.SelectTab(); 
        }



        // Scult or Texture Paint Mode
        // ***************************************************************************************************
        private void SculptButton_Clicked(object sender, vxUIControlClickEventArgs e)
        {
            SculptButton.ToggleState = true;
            //TexturePaintButton.ToggleState = false;

            TerrainEditState = vxEnumTerrainEditMode.Sculpt;

            // Disable all Texture Painting Buttons
            foreach (vxTxtrPaintToolbarButton button in TxtrPaintButtons)
                button.IsEnabled = false;
        }

        private void TexturePaintButton_Clicked(object sender, vxUIControlClickEventArgs e)
        {
            SculptButton.ToggleState = false;
            //TexturePaintButton.ToggleState = true;

            TerrainEditState = vxEnumTerrainEditMode.TexturePaint;

            // Enable all Texture Painting Buttons
            foreach (vxTxtrPaintToolbarButton button in TxtrPaintButtons)
                button.IsEnabled = true;
        }



        private void DeltaSculptButton_Clicked(object sender, vxUIControlClickEventArgs e)
        {
            AverageSculptButton.ToggleState = false;
            DeltaSculptButton.ToggleState = true;

            AreaOfEffectMode = vxEnumAreaOfEffectMode.Delta;
        }

        private void AverageSculptButton_Clicked(object sender, vxUIControlClickEventArgs e)
        {

            AverageSculptButton.ToggleState = true;
            DeltaSculptButton.ToggleState = false;

            AreaOfEffectMode = vxEnumAreaOfEffectMode.Averaged;
        }


        // Scult Mode
        // ***************************************************************************************************

        private void FallOffButton_Clicked(object sender, vxUIControlClickEventArgs e)
        {
            FalloffRate = (vxEnumFalloffRate)int.Parse(e.GUIitem.Text);

            foreach (vxToolbarButton btn in FallOffButtons)
                btn.ToggleState = false;

            e.GUIitem.ToggleState = true;
        }



        // Texture Paint Mode
        // ***************************************************************************************************

        private void TerrainPaintBtn_Clicked(object sender, vxUIControlClickEventArgs e)
        {
            foreach (vxTxtrPaintToolbarButton button in TxtrPaintButtons)
                button.ToggleState = false;

            vxTxtrPaintToolbarButton item = (vxTxtrPaintToolbarButton)e.GUIitem;

            TexturePaintType = item.TexturePaintIndex;
            item.ToggleState = true;
        }
    }
}