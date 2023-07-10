using GameManager.UI;

namespace GameManager.GameContent.UI.States
{
	public class UISortableElement : UIElement
	{
		public int OrderIndex;

		public UISortableElement(int index)
		{
			OrderIndex = index;
		}

		public override int CompareTo(object obj)
		{
			UISortableElement uISortableElement = obj as UISortableElement;
			if (uISortableElement != null)
			{
				return OrderIndex.CompareTo(uISortableElement.OrderIndex);
			}
			return base.CompareTo(obj);
		}
	}
}
