--��������Lua�ļ�
require("script\\CLRPackage")
--������򼯺������ռ�
import("System")
import("System.Windows.Forms")
import("System.Drawing")
import("System.Collections.Generic")
import("System.IO")
import("System.Text")
import("System.Data")
import("System.Threading")
import("MDT.Tools.Core","MDT.Tools.Core.UI")
import("MDT.Tools.Core","MDT.Tools.Core.Utils")
import("MDT.ThirdParty.Controls","WeifenLuo.WinFormsUI.Docking")
import("MDT.Tools.Core","MDT.Tools.Core.Resources")

--��ȡ��Form
application=getApplication()

--�����Ϣ
local tag=1002
local pluginKey=1002
local pluginName='������'
local description='����ϵͳ���в��'
local author='�׵�˧'
local version='1.0.0.0(build:20151120)'
--�������:��ʼ��
function init()
	return tag,pluginKey,pluginName,description,author,version
end
--������ť

pluginManagerTSMI=ToolStripMenuItem()

--�������:����
function load()
	debug(string.format("%d %s", pluginKey,"load"))--������־
	pluginManagerTSMI.Text="�������"
	 
	pluginManagerTSMI.Click:Add(pluginManagerTSMI_click)--����Click�¼�
	
	getObject(43,"tsmiHelper").DropDownItems:Insert(0,pluginManagerTSMI)	 
	
end
--��ť�¼�
function pluginManagerTSMI_click(sender,args)
	 
	tableLayoutPanel1=TableLayoutPanel()
	 
	tableLayoutPanel1.ColumnCount=1
	tableLayoutPanel1.RowCount = 2
	tableLayoutPanel1.Dock=DockStyle.Fill
	tableLayoutPanel1.ColumnStyles:Add(ColumnStyle(SizeType.Percent, 100))
	tableLayoutPanel1.RowStyles:Add(RowStyle(SizeType.Absolute, 35))
	tableLayoutPanel1.RowStyles:Add(RowStyle(SizeType.Percent, 100))
	btnSave= Button()
	btnSave.Image = Resources.start
	btnSave.Text="����"
	btnSave.Click:Add(btnSave_Click)
	 
	col1 = DataGridViewTextBoxColumn()
	col1.DataPropertyName = "PluginKey"
	col1.HeaderText = "���"
	col1.Name = "colPluginKey"
	col1.ReadOnly = true
	
	col2 = DataGridViewTextBoxColumn()
	col2.DataPropertyName = "PluginName"
	col2.HeaderText = "����"
	col2.Name = "colPluginName"
	col2.ReadOnly = true
	
	col3 = DataGridViewTextBoxColumn()
	col3.DataPropertyName = "Description"
	col3.HeaderText = "��������"
	col3.Name = "colDescription"
	col3.ReadOnly = true
	
	col4 = DataGridViewTextBoxColumn()
	col4.DataPropertyName = "Author"
	col4.HeaderText = "������Ա"
	col4.Name = "colAuthor"
	col4.ReadOnly = true
	
	col5 = DataGridViewTextBoxColumn()
	col5.DataPropertyName = "Version"
	col5.HeaderText = "�汾��"
	col5.Name = "colVersion"
	col5.ReadOnly = true
	
	col6 = DataGridViewTextBoxColumn()
	col6.DataPropertyName = "Tag"
	col6.HeaderText = "����˳���"
	col6.Name = "colTag"
	col6.ReadOnly = true
	
	col7 = DataGridViewCheckBoxColumn()
	col7.DataPropertyName = "Enabled"
	col7.HeaderText = "�Ƿ�����"
	col7.Name = "colEnable"
	
	col8 = DataGridViewTextBoxColumn()
	col8.DataPropertyName = "Dll"
	col8.HeaderText = "λ��"
	col8.Name = "colDll"
	col8.ReadOnly = true
	
	gv=DataGridView()
	gv.AutoGenerateColumns=false
	gv.Columns:Add(col1)
	gv.Columns:Add(col7)
	gv.Columns:Add(col2)
	gv.Columns:Add(col3)
	gv.Columns:Add(col4)
	gv.Columns:Add(col5)
	gv.Columns:Add(col8)
	gv.Columns:Add(col6)
	
	gv.AllowUserToAddRows=false
	gv.Dock=DockStyle.Fill
	gv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
	gv.CellValueChanged:Add(gv_CellValueChanged)
	
	tableLayoutPanel1.Controls:Add(btnSave,0,0)
	tableLayoutPanel1.Controls:Add(gv, 0, 1)	 
	
local dc=DockContent()
dc.Text=pluginManagerTSMI.Text 
dc.Controls:Add(tableLayoutPanel1)
dc:show(application.Panel)
pcall(bindGridView)
end


function bindGridView(o)
	local it="System.Int32"
	local intType=getType(it)
	dataTable=DataTable("plugins")
	dataTable.Columns:Add("PluginKey",intType)
	dataTable.Columns:Add("PluginName")
	dataTable.Columns:Add("Description")
	dataTable.Columns:Add("Author")
	dataTable.Columns:Add("Version")
	dataTable.Columns:Add("Dll")
	dataTable.Columns:Add("Tag",intType)
	dataTable.Columns:Add("Enabled",getType("System.Boolean"))
	local list=application.PluginManager.PluginList
	local count=list.Count-1
	local split=","
for i=0,count do
	local newRow=dataTable:NewRow()
	local temp=list[i]	 
		setDataRowValue(newRow,"PluginKey",temp.PluginKey)
		setDataRowValue(newRow,"PluginName",temp.PluginName)
		setDataRowValue(newRow,"Description",temp.Description)
		setDataRowValue(newRow,"Author",temp.Author)
		setDataRowValue(newRow,"Version",temp.Version)
		setDataRowValue(newRow,"Tag",temp.Tag)
		setDataRowValue(newRow,"Enabled",temp.Enabled)
		local s=temp:GetType().Assembly.FullName
		local a=string.sub(s,0, string.find(s, ",")-1)..".dll"
		local c="MDT.Tools.Lua.Plugin.dll"
		if (a==c) then		 		 
			a=temp.fileName   
		end
		setDataRowValue(newRow,"Dll",a)
		dataTable.Rows:Add(newRow)
end
	local dv=dataTable.DefaultView
	dv.Sort="PluginKey ASC"
	gv.DataSource=dv
	CallCtrlWithThreadSafety.RefreshGridViewDataSource(gv,gv)
end

 
function gv_CellValueChanged(sender, e)
 
end

function btnSave_Click(sender,args)
	local path= Application.StartupPath .."\\control\\plugins.xml"
	FileHelper.CreateDirectory(path)
	dataTable:WriteXml(path, XmlWriteMode.WriteSchema)	 
	MessageBox.Show(btnSave,"����ɹ�","��ʾ", MessageBoxButtons.OK,
	MessageBoxIcon.Information)
end

function unload()
	
end

--�������:�㲥���֮�乲�����Ϣ
function onNotify(name,o)	
end
