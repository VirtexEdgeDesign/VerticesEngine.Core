
using BEPUphysics;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.CollisionRuleManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using VerticesEngine.Diagnostics;

using VerticesEngine.Util;
using VerticesEngine.Input;
using VerticesEngine.UI.Events;
using VerticesEngine.Commands;
using VerticesEngine.Editor.Entities;
using VerticesEngine.Editor.UI;
using VerticesEngine.Graphics;

namespace VerticesEngine
{
    public partial class vxGameplayScene3D
    {

        protected override void HandleInputBase()
        {
            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            //KeyboardState keyboardState = vxInput.KeyboardState;// input.CurrentKeyboardStates[playerIndex];
            //GamePadState gamePadState = vxInput.GamePadState;// input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !vxInput.GamePadState.IsConnected;


            if(SandboxCurrentState == vxEnumSandboxStatus.EditMode && UIManager.HasFocus == false)
            {
                // Left Click
                if (vxInput.IsNewMouseButtonPress(MouseButtons.LeftButton))
                {
                    if(vxInput.IsKeyDown(Keys.LeftControl))
                    {
                        // do box selection
                        vxSelectionBox.Instance.SetStartPoint(vxInput.Cursor);
                    }
                    else
                    {
                        // if no modifier is down, then let's do a basic left click
                        OnLeftClick();
                    }
                }
                                
                if (vxInput.ScrollWheelDelta != 0)
                {
                    OnMouseWheelScroll();
                }

                // Right Click
                if (vxInput.IsNewMouseButtonPress(MouseButtons.RightButton))
                {
                    OnRightClick();
                }
                
                /**********************************************************/
                /*                  Process Keys                          */
                /**********************************************************/

                //Delete Selected Items
                if (vxInput.IsNewKeyPress(Keys.Delete))
                {
                    DeleteSelectedEntities();
                }

                // Handle Keyboard Shortcuts
                if (vxInput.KeyboardState.IsKeyDown(Keys.LeftControl))
                {
                    if (vxInput.IsNewKeyPress(Keys.N))
                        Event_NewFileToolbarItem_Clicked(this, new vxUIControlClickEventArgs(null));

                    if (vxInput.IsNewKeyPress(Keys.O))
                        Event_OpenFileToolbarItem_Clicked(this, new vxUIControlClickEventArgs(null));

                    if (vxInput.IsNewKeyPress(Keys.S))
                        SaveFile(true);

                    if (vxInput.IsNewKeyPress(Keys.Z))
                        CommandManager.Undo();

                    if (vxInput.IsNewKeyPress(Keys.Y))
                        CommandManager.ReDo();
                }
            }
            base.HandleInputBase();
        }

        internal void DeleteSelectedEntities()
        {
            var entitie = SelectedItems.ToArray();

            foreach (vxEntity3D entity in entitie)
            {
                if (entity.HasSandboxOption(SandboxOptions.Delete))
                {
                    CommandManager.Add(new vxCMDDeleteSandbox3DItem(this, entity.Id));
                }
            }

            SelectedItems.Clear();
        }

        /// <summary>
        /// Clears the current selection.
        /// </summary>
        protected internal virtual void ClearSelection()
        {
            // deselect all items in the selection list
            for (int ind = 0; ind < SelectedItems.Count; ind++)
                SelectedItems[ind].SelectionState = vxSelectionState.None;

            // clear the list
            SelectedItems.Clear();

            // reset the gimbal to the origin
            vxGizmo.Instance.Position = Vector3.Zero;
        }

        private void SelectEntity(vxEntity3D entity, bool isSingleSelect = true, bool toggleSelection = true)
        {
            // set entity selection state
            entity.SelectionState = vxSelectionState.Selected;

            // first check if it's a regual entity or cursor item (i.e. axis, rotator, pan etc...), 
            // if so then don't add it to theselection set
            if (entity.SandboxEntityType == vxEntityCategory.Entity)
            {
                //handle Addative Selection
                if (isSingleSelect)
                {
                    ClearSelection();
                }

                // should we toggle the selection?
                if (toggleSelection)
                {
                    if (SelectedItems.Contains(entity))
                        SelectedItems.Remove(entity);
                    else
                        SelectedItems.Add(entity);
                }
                // other wise let's only add this item if it's a new selection
                else
                {
                    if(SelectedItems.Contains(entity) == false)
                        SelectedItems.Add(entity);
                }

                RefreshEntityProptiesUI();

                m_gizmo.OnNewEntitySelection();
            }


            // Raise the 'Changed' event.
            if (ItemSelected != null)
                ItemSelected(this, new EventArgs());
        }

        internal void RefreshEntityProptiesUI()
        {
            EntityPropertiesControl.Clear();
            List<vxEntity> tempSelectionSet = new List<vxEntity>();
            foreach (var it in SelectedItems)
            {
                tempSelectionSet.Add(it);
                //Console.WriteLine("it " + it);
            }

            try
            {
                EntityPropertiesControl.GetPropertiesFromSelectionSet(tempSelectionSet);
                EntityPropertiesControl.ResetLayout();
            }
            catch { }
        }

        protected virtual bool FilterSelection(vxEntity3D entity)
        {
            if (entity.SandboxEntityType != vxEntityCategory.Entity)
                return false;

            return true;
        }

        private void SelectionBox_OnSelection(Rectangle obj)
        {
            foreach (vxEntity3D entity in Entities)
            {
                if (FilterSelection(entity) == false) continue;

                foreach(var cam in Cameras)
                {
                    var screenPos = cam.ProjectToScreenSpace(entity.BoundingShape.Center);
                    var screenPosXY = new Vector2(screenPos.X, screenPos.Y);

                    if (obj.Contains(screenPosXY))
                    {
                        // set entity selection state
                        entity.SelectionState = vxSelectionState.Selected;

                        SelectEntity(entity, false, false);
                    }
                }
            }
        }

        //public int cn = 0;
        protected internal virtual void OnLeftClick()
        {
            //Only do this if the GUI Manager isn't being used
            if (UIManager.HasFocus == false)//&& Cursor.IsMouseHovering == false)
            {
                //If it's in 'AddItem' Mode, then Add the Current Key Item
                /***********************************************************************************/
                if (SandboxEditMode == vxEnumSanboxEditMode.AddItem)
                {
                    //Set the Location of the current temp_part being added.
                    if (TempPart != null)
                    {
                        CommandManager.Add(new vxCMDAddSandbox3DItem(this, CurrentlySelectedKey, TempPart.Transform));
                    }
                }


                // If it's in 'Select Mode' then handle that.
                /***********************************************************************************/
                else if (SandboxEditMode == vxEnumSanboxEditMode.SelectItem)
                {
                    // get the encoded index of the current location under the cursor
                    var col = vxRenderPipeline.Instance.GetEncodedIndex((int)vxInput.Cursor.X, (int)vxInput.Cursor.Y);
                    var newSelectedItemHandleID = (col.R + col.G * 255 + col.B * 255 * 255);
                    //vxConsole.WriteLine("Index: " + newSelectedItemHandleID);

                    bool FoundSelection = false;
                    foreach (vxEntity3D entity in Entities)
                    {
                        if (entity.HandleID == newSelectedItemHandleID)
                        {
                            if (MouseClickState == MouseClickState.SelectItem)
                            {
                                // set entity selection state
                                entity.SelectionState = vxSelectionState.Selected;

                                FoundSelection = true;

                                SelectEntity(entity, !vxInput.IsKeyDown(Keys.LeftShift));
                            }
                            else if (MouseClickState == MouseClickState.ReturnItemToInspector)
                            {
                                if (ItemSelectedForInspector != null)
                                    ItemSelectedForInspector(this, new vxSandboxItemSelectedForInspectorEventArgs(entity));
                            }
                        }
                    }
                    // didn't find a new selection so clear it
                    if (FoundSelection == false)
                    {
                        if (vxInput.IsKeyDown(Keys.LeftShift) == false)
                        {
                            ClearSelection();
                        }
                    }
                }

                // Handle the Terrain Edit Mode
                /***********************************************************************************/
                else if (SandboxEditMode == vxEnumSanboxEditMode.TerrainEdit)
                {
                    // The individual Terrain Entities will handle the mouse downs internally
                }
            }
        }

        /// <summary>
        /// Handles mouse scroll wheel deltas
        /// </summary>
        protected internal virtual void OnMouseWheelScroll()
        {
            if (SandboxEditMode == vxEnumSanboxEditMode.AddItem && vxInput.IsKeyDown(Keys.LeftControl))
            {
                if (TempPart != null)
                {
                    TempPart.Transform.Rotate(Vector3.Up * vxInput.ScrollWheelDelta * 0.125f / 2);
                }
            }
        }

        /// <summary>
        /// The Method Called when the Mouse is Right Clicked. The Default is to rotate the part about the
        /// 'Y-Axis'. Override this method to provide your own code.
        /// </summary>
        protected internal virtual void OnRightClick()
        {
            if (SandboxEditMode == vxEnumSanboxEditMode.AddItem && vxInput.IsKeyDown(Keys.LeftControl))
            {
                if (TempPart != null)
                {
                    // TODO: Implement Euler
                    //TempPart.Yaw += MathHelper.PiOver2;
                    TempPart.Transform.Rotate(Vector3.Up * 90);
                }

            }
            else if(SandboxEditMode == vxEnumSanboxEditMode.SelectItem)
            {
                if (vxInput.IsKeyDown(Keys.LeftControl))
                {
                    ContextMenu.Show();
                }
            }
        }


        #region Picking

        /// <summary>
        /// This is the length to cast a ray during picking.
        /// </summary>
        public float RayCastLength = 1000;

        //The raycast filter limits the results retrieved from the Space.RayCast while grabbing.
        public Func<BroadPhaseEntry, bool> rayCastFilter;

        /// <summary>
        /// The Ray Cast Filter for Picking.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public virtual bool SelectionRayCastFilter(BroadPhaseEntry entry)
        {
            if (character != null)
                return entry != character.CharacterController.Body.CollisionInformation && entry.CollisionRules.Personal <= CollisionRule.Normal;
            else
                return true;
        }

        #endregion

        public virtual void HandleMouseRay(Ray ray)
        {
            //Next Find the earliest ray hit

            // Perform a Ray Cast.
            if (PhyicsSimulation.RayCast(ray, RayCastLength, SelectionRayCastFilter, out RayCastResult RayCastResult))
            {
                //var entityCollision = raycastResult.HitObject as EntityCollidable;
                OnRayHit(RayCastResult);
            }
        }

        

        protected virtual void OnRayHit(RayCastResult RayCastResult)
        {
            // Does the Tab have some info in it?
            if (RayCastResult.HitObject.Tag != null)
            {
                // Is the item hovering a SnapBox
                if (RayCastResult.HitObject.Tag is ISnapbox)
                {
                    var snap = (ISnapbox)RayCastResult.HitObject.Tag;

                    IsMouseOverSnapBox = true;
                    HoveredSnapBox = snap;
                    HoveredSnapBoxWorld = snap.Transform;
                    vxDebug.DrawBoundingBox(RayCastResult.HitObject.BoundingBox, Color.DeepPink);
                }
            }
        }
    }
}