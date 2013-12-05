﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;
using System.Windows.Forms;
using MDT.Tools.Core.UI;
using MDT.Tools.Core.Utils;
using MDT.Tools.DB.Java_CodeGen.Plugin.Model;
using MDT.Tools.DB.Java_CodeGen.Plugin.Utils;
using WeifenLuo.WinFormsUI.Docking;
using MDT.Tools.DB.Common;
namespace MDT.Tools.DB.Java_CodeGen.Plugin.Gen
{
    /// <summary>
    /// Java SpringConfig生成器
    /// </summary>
    internal class GenJavaSpringConfig : AbstractHandler
    {
        
        public JavaCodeGenConfig cmc;
        
        private IbatisConfigHelper ibatisConfigHelper = new IbatisConfigHelper();
        public override void process(DataRow[] drTables, DataSet dsTableColumns, DataSet dsTablePrimaryKeys)
        {
            try
            {
                base.process(drTables, dsTableColumns, dsTablePrimaryKeys);
                CodeLanguage = "XML";
                OutPut = cmc.OutPut;
                setStatusBar("");
                setEnable(false);
                StringBuilder SqlMapConfig = new StringBuilder();
                StringBuilder DAOContext = new StringBuilder();
                StringBuilder WebServiceContext = new StringBuilder();
                if (drTables != null && dsTableColumns != null)
                {
                    if (cmc.CodeRule == CodeGenRuleHelper.Ibatis)
                    {
                        ibatisConfigHelper.ReadConfig(cmc.Ibatis);
                    }

                    if (!cmc.IsShowGenCode)
                    {
                        FileHelper.DeleteDirectory(cmc.OutPut);
                        setStatusBar(string.Format("正在生成Spring配置文件"));
                        setProgreesEditValue(0);
                        setProgress(0);
                        setProgressMax(drTables.Length);
                    }
                    int j = 0;
                    for (int i = 0; i < drTables.Length; i++)
                    {
                        DataRow drTable = drTables[i];
                        string className = drTable["name"] + "";
                        string[] temp = cmc.TableFilter.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries);
                        bool flag = false;
                        foreach (var str in temp)
                        {
                            if (className.StartsWith(str))//过滤
                            {
                                flag = true;
                                j++;
                                break;
                            }

                        }
                        if (!cmc.IsShowGenCode)
                        {
                            setStatusBar(string.Format("正在生成Spring配置{0}的配置,共{1}个配置，已生成了{2}个配置,过滤了{3}个配置",
                                                       cmc.CodeRule == CodeGenRuleHelper.Ibatis ? ibatisConfigHelper.GetClassName(className) : CodeGenHelper.StrFirstToUpperRemoveUnderline(className),
                                                       drTables.Length, i - j, j));
                            setProgress(1);
                        }
                        if (!flag)
                        {
                            DataRow[] drTableColumns = dsTableColumns.Tables[dbName + DBtablesColumns].Select("TABLE_NAME = '" + drTable["name"].ToString() + "'", "COLUMN_ID ASC");
                            SqlMapConfig.Append(GenSqlMapConfig(drTable, drTableColumns));
                            DAOContext.Append(GetDAOContext(drTable, drTableColumns));
                            WebServiceContext.Append(GetWebServiceContext(drTable, drTableColumns));
                        }
                    }

                }

                if (!cmc.IsShowGenCode)
                {

                    FileHelper.Write(cmc.OutPut + CodeGenRuleHelper.SqlMapConfig, new[] { SqlMapConfig.ToString() });
                    FileHelper.Write(cmc.OutPut + CodeGenRuleHelper.DAOContext, new[] { DAOContext.ToString() });
                    FileHelper.Write(cmc.OutPut + CodeGenRuleHelper.WebServiceContext, new[] { WebServiceContext.ToString() });

                    setStatusBar(string.Format("Spring配置生成成功"));
                    openDialog();
                }
                else
                {
                    CodeShow(CodeGenRuleHelper.SqlMapConfig, SqlMapConfig.ToString());
                    CodeShow(CodeGenRuleHelper.DAOContext, DAOContext.ToString());
                    CodeShow(CodeGenRuleHelper.WebServiceContext, WebServiceContext.ToString());
                }
            }
            catch (Exception ex)
            {
                setStatusBar(string.Format("Spring配置生成失败[{0}]", ex.Message));

            }
            finally
            {
                setEnable(true);
            }
        }
         
      
        public string GenSqlMapConfig(DataRow drTable, DataRow[] drTableColumns)
        {
            var sb = new StringBuilder();
            string tableName = drTable["name"] as string;
            sb.AppendFormat("<sqlMap resource=\"ats/common/model/dao/{0}_SqlMap.xml\" />", tableName).AppendFormat("\r\n");
            return sb.ToString();
        }

        public string GetDAOContext(DataRow drTable, DataRow[] drTableColumns)
        {
            var sb = new StringBuilder();
            string tableName = drTable["name"] as string;
            string className = (cmc.CodeRule == CodeGenRuleHelper.Ibatis ? ibatisConfigHelper.GetClassName(tableName) : CodeGenHelper.StrFirstToUpperRemoveUnderline(tableName));
            string dao = className + CodeGenRuleHelper.DAO;
            sb.AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("<!-- {0} -->", dao).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("<bean id=\"{0}\" class=\"ats.common.model.dao.{0}Impl\">", dao).AppendFormat("\r\n");
            sb.AppendFormat("\t\t").AppendFormat("<property name=\"sqlMapClientTemplate\">").AppendFormat("\r\n");
            sb.AppendFormat("\t\t\t").AppendFormat("<ref bean=\"sqlMapClientTemplate\" />").AppendFormat("\r\n");
            sb.AppendFormat("\t\t").AppendFormat("</property>").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("</bean>").AppendFormat("\r\n");
            sb.AppendFormat("\r\n");
            return sb.ToString();
        }

        public string GetWebServiceContext(DataRow drTable, DataRow[] drTableColumns)
        {
            var sb = new StringBuilder();
            string tableName = drTable["name"] as string;
            string model = (cmc.CodeRule == CodeGenRuleHelper.Ibatis ? ibatisConfigHelper.GetClassName(tableName) : CodeGenHelper.StrFirstToUpperRemoveUnderline(tableName));
            string bsServer = model + CodeGenRuleHelper.BSServer;
            string wsServer = model + CodeGenRuleHelper.WSService;
            string dao = model + CodeGenRuleHelper.DAO;

            sb.AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("<!-- {0} bs-->", bsServer).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("<bean id=\"I{0}_bs\" class=\"{1}.impl.{0}\">", bsServer,cmc.BSPackage).AppendFormat("\r\n");
            sb.AppendFormat("\t\t").AppendFormat("<property name=\"{0}\">", CodeGenHelper.StrFirstToLower(dao)).AppendFormat("\r\n");
            sb.AppendFormat("\t\t\t").AppendFormat("<ref bean=\"{0}\" />",dao).AppendFormat("\r\n");
            sb.AppendFormat("\t\t").AppendFormat("</property>").AppendFormat("\r\n");
            sb.AppendFormat("\t\t").AppendFormat("<property name=\"{0}\">", "dataCheckServer").AppendFormat("\r\n");
            sb.AppendFormat("\t\t\t").AppendFormat("<ref bean=\"{0}\" />", "DataCheckServer_bs").AppendFormat("\r\n");
            sb.AppendFormat("\t\t").AppendFormat("</property>").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("</bean>").AppendFormat("\r\n");
            sb.AppendFormat("\r\n");

            sb.AppendFormat("\t").AppendFormat("<!-- {0} ws-->", wsServer).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("<bean id=\"I{0}_ws\" class=\"{1}.impl.{0}\">", wsServer, cmc.WSPackage).AppendFormat("\r\n");
            sb.AppendFormat("\t\t").AppendFormat("<property name=\"i{0}\">",CodeGenHelper.StrFirstToLower(bsServer)).AppendFormat("\r\n");
            sb.AppendFormat("\t\t\t").AppendFormat("<ref bean=\"I{0}_bs\" />", bsServer).AppendFormat("\r\n");
            sb.AppendFormat("\t\t").AppendFormat("</property>").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("</bean>").AppendFormat("\r\n");
            sb.AppendFormat("\r\n");

            sb.AppendFormat("\t").AppendFormat("<!-- I{0} jaxws-->", wsServer).AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("<jaxws:server id=\"I{0}\" serviceClass=\"{1}.I{3}\" address=\"/I{2}\">", wsServer, cmc.WSPackage, wsServer, model).AppendFormat("\r\n");
            sb.AppendFormat("\t\t").AppendFormat("<jaxws:serviceBean>").AppendFormat("\r\n");
            sb.AppendFormat("\t\t\t").AppendFormat("<ref bean=\"I{0}_ws\" />", wsServer).AppendFormat("\r\n");
            sb.AppendFormat("\t\t").AppendFormat("</jaxws:serviceBean>").AppendFormat("\r\n");
            sb.AppendFormat("\t").AppendFormat("</jaxws:server>").AppendFormat("\r\n");

            return sb.ToString();
        }
        
        public GenJavaSpringConfig()
        {
            AddContextMenu();
        }
    }
}
