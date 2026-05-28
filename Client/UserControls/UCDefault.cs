using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.UserControls
{
    public partial class UCDefault : UserControl
    {
        public UCDefault()
        {
            InitializeComponent();
            this.BackgroundImage = Client.AppResources.AutoskolaBackground;
            this.BackgroundImageLayout = ImageLayout.Zoom;
        }
    }
}
