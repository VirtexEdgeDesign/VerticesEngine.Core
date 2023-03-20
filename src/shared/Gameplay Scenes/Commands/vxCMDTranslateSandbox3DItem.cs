using Microsoft.Xna.Framework;
using System.Collections.Generic;


namespace VerticesEngine.Commands
{
    class vxCMDEntityInfo
	{
		public string ID = "";
		public vxTransform World = vxTransform.Identity;
		public vxTransform InitialWorld = vxTransform.Identity;

		public vxCMDEntityInfo(vxEntity3D entity)
		{
			this.ID = entity.Id;
			this.World = entity.Transform;
			this.InitialWorld = entity.Transform;
		}
	}
	public class vxCMDTranslateSandbox3DItem : vxCMDBaseSandbox3DCommand
	{
		Vector3 Delta;

		List<vxCMDEntityInfo> Entities = new List<vxCMDEntityInfo>();

		/// <summary>
		/// The item identifier.
		/// </summary>
		public string Id = "";

        public vxCMDTranslateSandbox3DItem(vxGameplayScene3D Scene, List<vxEntity3D> Entities, Vector3 Delta):base(Scene)
		{
			foreach(vxEntity3D entity in Entities)
			{
                this.Entities.Add(new vxCMDEntityInfo(entity));
			}

			this.Delta = new Vector3(Delta.X, Delta.Y, Delta.Z);
            if(Entities.Count > 0)
			this.Tag = "Move: " + Entities[0].Id + "; dif: "+Delta;
		}

		public override void Do()
		{
			foreach (vxCMDEntityInfo entry in Entities)
			{
				vxEntity3D entity = GetItemFromID(entry.ID);
				entity.Position += (Delta);
			}
		}

		public override void Undo()
		{
			foreach (vxCMDEntityInfo entry in Entities)
			{
				vxEntity3D entity = GetItemFromID(entry.ID);
				entity.Position -= (Delta);
			}
		}
	}
}
