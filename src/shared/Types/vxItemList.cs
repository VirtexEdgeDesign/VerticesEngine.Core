using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine
{
    public class vxItemList : List<string>
    {
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { 
                _selectedIndex = vxMathHelper.Clamp(value, 0, this.Count-1);

                OnSelectedItemChanged?.Invoke();
            }
        }
        public int _selectedIndex = 0;

        public string SelectedItem
        {
            get {
                if (this.Count == 0)
                    return default;

                return this[_selectedIndex];
            }
        }

        /// <summary>
        /// This forces the selected item to a given value without firing off the ItemChanged event;
        /// </summary>
        /// <param name="item"></param>
        public void OverrideSelectedItem(string newItem)
        {
            for(int s = 0; s < this.Count; s++)
            {
                if(newItem == this[s])
                {
                    _selectedIndex = s;
                }
            }
        }

        public event Action OnSelectedItemChanged;
    }
}
