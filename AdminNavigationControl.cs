using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laboratory_Management_System
{
    internal class AdminNavigationControl
    {
        private List<UserControl> _userControls;
        private Panel _containerPanel;

        public AdminNavigationControl(List<UserControl> userControls, Panel panel)
        {
            _userControls = userControls;
            _containerPanel = panel;
            InitializeControls();
        }

        private void InitializeControls()
        {
            foreach (var control in _userControls)
            {
                control.Dock = DockStyle.Fill;
                control.Visible = false; 
                _containerPanel.Controls.Add(control);
            }
        }

        public void display(int index)
        {
            if (index < 0 || index >= _userControls.Count)
            {
                MessageBox.Show("Index out of range.");
                return;
            }

            foreach (UserControl control in _userControls)
            {
                control.Visible = false;
            }

            UserControl selectedControl = _userControls[index];
            selectedControl.Visible = true;
            selectedControl.BringToFront();
        }

        public UserControl GetControl(int index)
        {
            if (index >= 0 && index < _userControls.Count)
            {
                return _userControls[index];
            }
            return null;
        }

    }
}
