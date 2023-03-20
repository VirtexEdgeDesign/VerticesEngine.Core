namespace VerticesEngine.Commands
{
    public class vxCMDBaseSandbox3DCommand : vxCommand
	{
		/// <summary>
		/// Gets the current sandbox level.
		/// </summary>
		/// <value>The current sandbox level.</value>
        public vxGameplayScene3D CurrentSandboxLevel
		{
            get { return ((vxGameplayScene3D)Scene); }
		}

        public vxCMDBaseSandbox3DCommand(vxGameplayScene3D Scene):base(Scene)
		{
			
		}

		public vxEntity3D GetItemFromID(string id)
		{
			foreach (vxEntity3D item in CurrentSandboxLevel.Entities)
			{
				if (item.Id == id)
					return item;
			}
			return null;
		}
	}
}
