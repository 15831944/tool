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
import("System.Diagnostics")
--��ȡ��Form
application=getApplication()

--�����Ϣ
local tag=92
local pluginKey=92
local pluginName='NetFramework��װ���'
local description='NetFramework��װ���'
local author='�׵�˧'
local version='1.0.0.0'
--�������:��ʼ��
function init()
	return tag,pluginKey,pluginName,description,author,version
end
--������ť

tSMI=ToolStripMenuItem()

--�������:����
function load()
	debug(string.format("%d %s", pluginKey,"load"))--������־	     
	tSMI.Text=".NetFrameWork��װ���" 
	tSMI.Click:Add(tSMI_click)--����Click�¼�
	 
	getObject(43,"tsmiTool").DropDownItems:Add(tSMI)
	 
	
end
--��ť�¼�
function tSMI_click(sender,args)	 
	Process.Start("script\\NetFrameWork\\MDT.Tools.NetFrameWork.exe")
end
 

function unload()
	
end

--�������:�㲥���֮�乲�����Ϣ
function onNotify(name,o)
	 
end
