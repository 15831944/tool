﻿//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Data;
//using System.Data.Common;
//using MySql.Data.MySqlClient;
//namespace DNCCFrameWork.DataAccess.DbProvider
//{   
//    public class MySqlHelper : BaseDbHelper
//    {
//       public MySqlHelper()
//        {            
//        }
//        //重载SqlClientFactory数据工厂
//        public override DbProviderFactory Instance()
//        {
//            return MySqlClientFactory.Instance;
//        }       
//        //重载ExecuteNonQuery，连接池出现问题时，可以用SqlConnection.ClearAllPools();
//        public override int ExecuteNonQuery(string commandText)
//        {
//            int retval = 0;
//            bool flag = false;
//            int count = 0;
//            do
//            {
                
//                try
//                {
//                    PrepareCommand(CommandType.Text, commandText, null,false);
//                    Open();                    
//                    retval =Command .ExecuteNonQuery();
//                    if (Command.Transaction != null)//事务提交
//                    {
//                        Transaction.Commit();
//                    }
//                    Command.Parameters.Clear();
//                    flag = false;
//                }
//                catch (MySqlException ex)
//                {
//                    if (ex.Number != 10054)//连接池问题
//                    {
//                        if (Command.Transaction != null)//失败回滚
//                        {
//                            Transaction.Rollback();
//                        }
//                        throw ex;
//                    }                
//                    //无缓存池
//                    count++;
//                    if (count == 2) flag = false;
//                    else
//                    {
//                        flag = true;
//                    }
//                }
//                finally
//                {
//                    Close();
//                }
//            } while (flag);
//            return retval;
//        }
//        //重载 ExecuteScalar
//        public override object ExecuteScalar(string commandText)
//        {
//            object retval = null;
//            bool flag = false;
//            int count = 0;
//            do
//            {

//                try
//                {
//                    PrepareCommand(CommandType.Text, commandText, null, false);
//                    Open();
//                    retval = Command.ExecuteScalar();
//                    if (Command.Transaction != null)//事务提交
//                    {
//                        Transaction.Commit();
//                    }
//                    Command.Parameters.Clear();
//                    flag = false;
//                }
//                catch (MySqlException ex)
//                {
//                    if (ex.Number != 10054)//连接池问题
//                    {
//                        if (Command.Transaction != null)//失败回滚
//                        {
//                            Transaction.Rollback();
//                        }
//                        throw ex;
//                    }
                   
//                    count++;
//                    if (count == 2) flag = false;
//                    else
//                    {
//                        flag = true;
//                    }
//                }
//                finally
//                {
//                    Close();
//                }
//            } while (flag);
//            return retval;
//        }
//        //重载 ExecuteReader
//        public override DbDataReader ExecuteReader(string commandText)
//        {
//            DbDataReader retval = null;
//            bool flag = false;
//            int count = 0;
//            do
//            {

//                try
//                {
//                    PrepareCommand(CommandType.Text, commandText, null, false);
//                    Open();
//                    retval = Command.ExecuteReader(CommandBehavior.CloseConnection);
//                    if (Command.Transaction != null)//事务提交
//                    {
//                        Transaction.Commit();
//                    }
//                    Command.Parameters.Clear();
//                    flag = false;
//                }
//                catch (MySqlException ex)
//                {
//                    if (ex.Number != 10054)//连接池问题
//                    {
//                        if (Command.Transaction != null)//失败回滚
//                        {
//                            Transaction.Rollback();
//                        }
//                        throw ex;
//                    }
                    
//                    count++;
//                    if (count == 2) flag = false;
//                    else
//                    {
//                        flag = true;
//                    }
//                }
//                finally
//                {
//                    Close();
//                }
//            } while (flag);
//            return retval;
//        }


//        #region 数据库常用操作     
        
//        #endregion
//    }
//}
