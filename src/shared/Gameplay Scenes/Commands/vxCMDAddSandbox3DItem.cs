using Microsoft.Xna.Framework;


namespace VerticesEngine.Commands
{
    public class vxCMDAddSandbox3DItem : vxCMDBaseSandbox3DCommand
	{
		public string ItemKey;

		public vxTransform World;

		/// <summary>
		/// The item identifier.
		/// </summary>
		public string Id = "";

        public vxCMDAddSandbox3DItem(vxGameplayScene3D Scene, string ItemKey, vxTransform World):base(Scene)
		{
			this.ItemKey = ItemKey;

			this.World = World;
		}

		public override void Do()
		{
			vxEntity3D item = CurrentSandboxLevel.AddSandboxItem(ItemKey, World);

			if (Id == "")
				Id = item.Id;
			else
				item.Id = Id;
			
			this.Tag = "Added: " + Id;
			//Console.WriteLine(this.Tag);
			item.OnAdded();
		}

		public override void Undo()
		{
			vxEntity3D item = GetItemFromID(Id);
			if (item != null)
				item.Dispose();
		}
	}
}
