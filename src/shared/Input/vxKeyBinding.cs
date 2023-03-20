using Microsoft.Xna.Framework.Input;

namespace VerticesEngine.Input
{
    public class vxKeyBinding
{
	public string Name = "Key Name";

	public Keys Key;

	public vxKeyBinding(string Name, Keys Key)
	{
		this.Name = Name;
		this.Key = Key;
	}
}
}
