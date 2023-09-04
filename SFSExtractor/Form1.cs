using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Editor.Utils;

using log4net;

namespace SFSExtractor
{
    public partial class Form1 : Form
    {
        private static ILog _log = LogManager.GetLogger(typeof(Form1));

        Thread ExtractThread = null;

        public event ProcessedItem OnFinishedThread;
        public Form1()
        {
            log4net.Config.XmlConfigurator.Configure();
            InitializeComponent();

            OnFinishedThread += new ProcessedItem(EnableInput);
        }

        public void Fill()
        {
            if (ExtractManager.Global.Config.Root == null || ExtractManager.Global.Config.Root.Dirs == null) return;
            FilesTree.Nodes.Clear();

            TreeNode newNode = new TreeNode();
            newNode.Text = "..\\";
            newNode.ImageIndex = 0;
            newNode.SelectedImageIndex = 0;

            TagData t = new TagData();
            t.Data = ExtractManager.Global.Config.Root ;
            t.Type = 1;
            newNode.Tag = t;
            newNode.Checked = true;

            FillTree(ExtractManager.Global.Config.Root, newNode, null);

            FilesTree.Nodes.Add(newNode);
            /*
            foreach (sfsDir dir in ExtractManager.Global.Config.Root.Dirs)
            {
                TreeNode newNode = new TreeNode();
                newNode.Text = dir.Name;
                newNode.ImageIndex = 0;
                newNode.SelectedImageIndex = 0;

                if (dir.Enabled == true)
                    newNode.Checked = true;
                else
                    newNode.Checked = false;

                TagData t = new TagData();
                t.Data = dir;
                t.Type = 1;
                newNode.Tag = t;

                FillTree(dir, newNode, null);
                FilesTree.Nodes.Add(newNode);
            }
            /**/
        }

        public void Fill(TreeNode node)
        {
            sfsDir dir = null;
            if (node != null)
            {
                node.Nodes.Clear();
                if (node.Tag != null)
                {
                    TagData t = (TagData)node.Tag;
                    if (t.Type == 1)
                    {
                        dir = (sfsDir)t.Data;
                        FillTree(dir, node, null);
                    }
                }

            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //Category n = new Category("samples");
            //n.ParseLine("A12_Idle.wav");
            ExtractManager.Global.Init();

            textTOWME.Text = ExtractManager.Global.gConfig.sfsPath;
            textExtractLoc.Text = ExtractManager.Global.gConfig.ExtractDir;

            Fill();                     
                                    
        }

        private void FillTree(sfsDir Dir, TreeNode node,TreeView view)
        {
            if (Dir.Dirs != null)
            {
                foreach (sfsDir dir in Dir.Dirs)
                {
                    TreeNode newNode = new TreeNode();
                    newNode.Text = dir.Name;
                    newNode.ImageIndex = 0;
                    newNode.SelectedImageIndex = 0;

                    if (dir.Enabled == true)
                        newNode.Checked = true;
                    else
                        newNode.Checked = false;

                    TagData t = new TagData();
                    t.Data = dir;
                    t.Type = 1;
                    newNode.Tag = t;

                    if (node != null)
                    {
                        node.Nodes.Add(newNode);
                    }
                    else if (view != null)
                    {
                        view.Nodes.Add(newNode);
                    }

                    FillTree(dir, newNode, view);


                }
            }
            FillTreeFiles(Dir, node);
        }

        private void FillTreeFiles(sfsDir Dir, TreeNode node)
        {
            if (Dir.Files == null) return;
            foreach (sfsFile file in Dir.Files)
            {
                TreeNode newNode = new TreeNode();
                newNode.Text = file.Name;
                newNode.ImageIndex = 1;
                newNode.SelectedImageIndex = 1;

                if (file.Enabled == true)
                    newNode.Checked = true;
                else
                    newNode.Checked = false;

                TagData t = new TagData();
                t.Data = file;
                t.Type = 0;
                newNode.Tag = t;

                if (node != null)
                {
                    node.Nodes.Add(newNode);
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (ExtractThread != null)
                {
                    ExtractThread.Abort();
                }
            }
            catch (Exception ex)
            {
                _log.Fatal("Exception==>", ex);
            }
            ExtractManager.Global.DeInit();
            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void extractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int max = ExtractManager.Global.GetItemsCount();

            progressBar1.Maximum = max;
            progressBar1.Minimum = 0;
            progressBar1.Value = 0;
            DisableInput();
            CancelButton2.Enabled = true;

            ExtractManager.Global.OnProcessItem += new ProcessedItem(OnItemTraversed);
            ExtractThread = new Thread(new ThreadStart(Extracting));
            ExtractThread.Start();
        }

        
        public void OnItemTraversed()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ProcessedItem(OnItemTraversed));
            }
            else
            {
                try
                {
                    progressBar1.Value++;
                }
                catch (Exception ex)
                {
                    _log.Fatal("Exception==>", ex);
                }
            }
        }


        public void DisableInput()
        {
            menuStrip1.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            contextMenuStrip1.Enabled = false;
        }

        public void EnableInput()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ProcessedItem(EnableInput));
            }
            else
            {
                this.CancelButton2.Enabled = false;
                menuStrip1.Enabled = true;
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
                contextMenuStrip1.Enabled = true;
            }
        }


        public void Extracting()
        {
            
            try
            {
                ExtractManager.Global.Extract();
            }
            catch (Exception e)
            {
                _log.Fatal("Exception==>", e);
            }
            finally
            {
                ExtractThread = null;
                EnableInput();
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = string.Empty;
            TreeNode node = FilesTree.SelectedNode;
            sfsDir dir = null;
            if (node != null)
            {
                if (node.Tag != null)
                {
                    TagData t = (TagData)node.Tag;
                    if (t.Type == 1)
                    {
                        dir = (sfsDir)t.Data;
                        
                        dir.GetFullPath(ref path);
                        
                    }
                }

            }
            NewFile d = new NewFile();

            DialogResult res = d.ShowDialog();
            if (res == DialogResult.OK)
            {
                string file = d.textBox1.Text;
                if(path!=string.Empty)
                    ExtractManager.Global.Add(path + "\\" + file);
                else
                    ExtractManager.Global.Add(file);

                Fill(node);
            }

        }

        public sfsDir GetSelectedDir()
        {
            TreeNode node = FilesTree.SelectedNode;
            sfsDir dir = null;
            if (node != null)
            {
                if (node.Tag != null)
                {
                    TagData t = (TagData)node.Tag;
                    if (t.Type == 1)
                    {
                        dir = (sfsDir)t.Data;
                    }
                }
            }
            return dir;
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = string.Empty;
            TreeNode node = FilesTree.SelectedNode;
            sfsDir dir = null;
            if (node != null)
            {
                if (node.Tag != null)
                {
                    TagData t = (TagData)node.Tag;
                    if (t.Type == 1)
                    {
                        dir = (sfsDir)t.Data;

                        if (dir.Parent != null)
                        {
                            dir.Parent.Dirs.Remove(dir);
                            Fill(node.Parent);
                        }
                    }
                    else if (t.Type == 0)
                    {
                        sfsFile file = (sfsFile)t.Data;

                        if (file.Parent != null)
                        {
                            file.Parent.Files.Remove(file);
                            Fill(node.Parent);
                        }
                    }
                }

            }
            
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult res = openFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                ExtractManager.Global.LoadXML(openFileDialog1.FileName);
                Fill();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SelectAll(true);
        }

        public void SelectAll(bool state)
        {
            foreach (TreeNode node in FilesTree.Nodes)
            {
                SelectAllChilds(node, state);
            }
        }

        public void SelectAllChilds(TreeNode parent,bool state)
        {
            parent.Checked = state;

            if (parent.Tag != null)
            {
                TagData t = (TagData)parent.Tag;
                if (t.Type == 1)
                {
                    sfsDir dir = (sfsDir)t.Data;
                    dir.Enabled = state;
                }
                else if (t.Type == 0)
                {
                    sfsFile file = (sfsFile)t.Data;
                    file.Enabled = state;
                }
            }
            foreach (TreeNode node in parent.Nodes)
            {
                SelectAllChilds(node,state);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SelectAll(false);
        }

        private void FilesTree_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse)
            {
                bool state = e.Node.Checked;
                SelectAllChilds(e.Node, state);
            }

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult res = saveFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                ExtractManager.Global.SaveXML(saveFileDialog1.FileName);
            }
        }

        private void checkOverride_CheckedChanged(object sender, EventArgs e)
        {
            ExtractManager.Global.Override = checkOverride.Checked;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About a = new About();

            a.ShowDialog();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            ExtractManager.Global.Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult res = folderBrowserDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                ExtractManager.Global.gConfig.sfsPath =  folderBrowserDialog1.SelectedPath;
                textTOWME.Text = ExtractManager.Global.gConfig.sfsPath;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult res = folderBrowserDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                ExtractManager.Global.gConfig.ExtractDir = folderBrowserDialog1.SelectedPath;
                textExtractLoc.Text = ExtractManager.Global.gConfig.ExtractDir;
            }
        }

        private void textTOWME_TextChanged(object sender, EventArgs e)
        {
            ExtractManager.Global.gConfig.sfsPath = textTOWME.Text;
        }

        private void textExtractLoc_TextChanged(object sender, EventArgs e)
        {
            ExtractManager.Global.gConfig.ExtractDir = textExtractLoc.Text;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            sfsDir dir = GetSelectedDir();

            string name = textBox1.Text;
            if(name==null || name == string.Empty) return;

            if (dir != null)
            {
                name = name.Trim();
                sfsDir foundDir = ExtractManager.Global.SearchForFile(dir, name);
                if (foundDir != null)
                {
                    string path = string.Empty;
                    foundDir.GetFullPath(ref path);
                    ResultTex.Text = "Found at " + path;
                }
                else
                {
                    ResultTex.Text = "Could not find the file!";
                }
            }
        }

        private void CancelButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (ExtractThread != null)
                {
                    ExtractThread.Abort();
                }
            }
            catch (Exception ex)
            {
                _log.Fatal("Exception==>", ex);
            }
            progressBar1.Value = 0;
        }

        private void mountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                string sfsfile = ExtractManager.Global.GetName(openFileDialog2.FileName);

                bool found = false;
                foreach (string s in ExtractManager.Global.gConfig.Mounts)
                {
                    
                    if (s.Equals(sfsfile, StringComparison.CurrentCultureIgnoreCase) == true)
                    {
                        found = true;
                        return;
                    }
                    
                }
                if (found == false)
                {
                    ExtractManager.Global.gConfig.Mounts.Add(sfsfile);
                    if (ExtractManager.Global.MountFile(sfsfile) == true)
                    {
                        ExtractManager.Global.SaveConfig("Config.xml");
                    }
                }
                
            }
        }

        private void unMountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MountList form = new MountList();
            if (form.ShowDialog() == DialogResult.OK)
            {
                foreach(ListViewItem i in form.listMounts.SelectedItems)
                {
                    ExtractManager.Global.gConfig.Mounts.Remove(i.Text);
                    ExtractManager.Global.UnMountFile(i.Text);
                }
                ExtractManager.Global.SaveConfig("Config.xml");
            }
                
        }
    }

    public class TagData
    {
        public int Type;
        public object Data;
    }
}