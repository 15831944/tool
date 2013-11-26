﻿namespace MDT.Tools.DB.Plugin.Utils
{
    internal class PluginShareHelper
    {
        public const string DBCurrentCheckTable = "DBCurrentCheckTable";
        public const string DBCurrentDBAllTable = "DBCurrentDBAllTable";
        public const string DBCurrentDBAllTablesColumns = "DBCurrentDBAllTablesColumns";
        public const string DBCurrentDBViews = "DBCurrentDBViews";
        public const string DBCurrentDBTablesPrimaryKeys = "DBCurrentDBTablesPrimaryKeys";

        //共享变量
        public const string DBCurrentDBName = "DBCurrentDBName";
        public const string DBCurrentDBConnectionString = "DBCurrentDBConnectionString";
        public const string DBCurrentDBType = "DBCurrentDBType";

        //共享常量
        public const string DBtable = "DBtable";
        public const string DBtablesColumns = "DBtablesColumns";
        public const string DBviews = "DBviews";
        public const string DBtablesPrimaryKeys = "DBtablesPrimaryKeys";

        public const string BroadCastCheckTableNumberIsGreaterThan0 = "BroadCast_CheckTableNumberIsGreaterThan0";//广播：选中table个数是否大于0


        //共享控件
        public const string TsslMessage = "tsslMessage";//状态栏label
        public const string TspbLoadDBProgress = "tspbLoadDBProgress";// 状态栏进度条


    }
}
