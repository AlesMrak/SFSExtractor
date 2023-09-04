using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SFSExtractor
{
    public partial class MountList : Form
    {
        public MountList()
        {
            InitializeComponent();
        }

        private void MountList_Load(object sender, EventArgs e)
        {
            listMounts.Items.Clear();

            foreach (string s in ExtractManager.Global.gConfig.Mounts)
            {
                ListViewItem item = new ListViewItem(s);
                item.Tag = s;
                listMounts.Items.Add(item);
            }
        }
    }
}