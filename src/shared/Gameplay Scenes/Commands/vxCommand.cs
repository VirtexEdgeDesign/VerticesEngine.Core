namespace VerticesEngine.Commands
{
    /// <summary>
    /// The class containing Do-Redo support for handling Redo-Undo in a sandbox
    /// enviroment.
    /// </summary>
    public class vxCommand
	{
        public readonly vxGameplaySceneBase Scene;

		/// <summary>
		/// The Command Tag for Debuging
		/// </summary>
		public string Tag="Cmd: <cmd>";

        public vxCommand(vxGameplaySceneBase Scene)
		{
            this.Scene = Scene;
		}

		/// <summary>
		/// The Method which is called during 'do'
		/// </summary>
		public virtual void Do() { }

		/// <summary>
		/// The Undo method.
		/// </summary>
		public virtual void Undo() { }
	}
}
