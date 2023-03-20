using Microsoft.Xna.Framework;
using System.Collections.Generic;
using VerticesEngine.Graphics;

namespace VerticesEngine.Commands
{

	public class vxCMDTransformChanged : vxCMDBaseSandbox3DCommand
	{
		List<vxCMDEntityInfo> m_entities = new List<vxCMDEntityInfo>();

		List<vxTransform> NewTransforms = new List<vxTransform>();
		List<vxTransform> OldTransforms = new List<vxTransform>();

		/// <summary>
		/// The item identifier.
		/// </summary>
		public string Id = "";

        public vxCMDTransformChanged(vxGameplayScene3D Scene, List<vxEntity3D> Entities, List<vxTransform> NewTransforms, List<vxTransform> OldTransforms):base(Scene)
		{
			foreach(vxEntity3D entity in Entities)
			{
                this.m_entities.Add(new vxCMDEntityInfo(entity));
			}

			this.NewTransforms.AddRange(NewTransforms);
			this.OldTransforms.AddRange(OldTransforms);

            if(Entities.Count > 0)
			this.Tag = "Transform: " + Entities[0].Id + "; dif: ";
		}

		public override void Do()
		{
			for(int i = 0; i < m_entities.Count; i++)
			{
				vxEntity3D entity = GetItemFromID(m_entities[i].ID);
				if (entity != null)
					entity.Transform = NewTransforms[i];
				else
					vxConsole.WriteError("Entity is null for Do");
			}
		}

		public override void Undo()
		{
			for (int i = 0; i < m_entities.Count; i++)
			{
				vxEntity3D entity = GetItemFromID(m_entities[i].ID);
				if (entity != null)
					entity.Transform = OldTransforms[i];
				else
					vxConsole.WriteError("Entity is null for UnDo");
			}
		}
	}
}
