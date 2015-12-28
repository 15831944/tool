using System;
using System.Data;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MDT.Tools.Core.Plugin;
using MDT.Tools.DB.Common;
using MDT;

namespace MDT.Tools.Order.Monitor.Plugin
{
    public class OrderMonitorPlugin : AbstractPlugin
    {
        protected readonly ToolStripMenuItem _tsiGen = new ToolStripMenuItem();
        protected delegate void Simple();

        #region �����Ϣ

        private int _tag = 505300;

        public override int Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public override int PluginKey
        {
            get { return 505300; }
        }

        public override string PluginName
        {
            get { return "���׼��"; }
        }

        public override string Description
        {
            get { return "���ݷ���������������Ϣ�������ɽ������ͽ��׺�ʱ"; }
        }

        public override string Author
        {
            get { return "��Ծ"; }
        }

        #endregion

        #region

        protected override void load()
        {
            AddContextMenu();
        }


        #region ���������Ĳ˵�

        protected void AddContextMenu()
        {
            if (Application.MainContextMenu.InvokeRequired)
            {
                var s = new Simple(AddContextMenu);
                Application.MainContextMenu.Invoke(s, null);
            }
            else
            {
                foreach (ToolStripMenuItem v in Application.MainMenu.Items)
                {
                    if (v.Text == "����(&T)")
                    {
                        v.DropDownItems.Add(_tsiGen);
                        _tsiGen.Text = "���׼��";
                        _tsiGen.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                        _tsiGen.Click += new EventHandler(_tsiGen_Click);
                        break;
                    }
                }
            }
        }

        private void _tsiGen_Click(object sender, EventArgs e)
        {
            var form = new Form1() { Text = _tsiGen.Text };
            form.Show(Application.Panel);
        }

        #endregion

        #endregion
    }
}