using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Text;

namespace Service.DB
{
    public class QueryBuilder
    {
        public OdbcCommand _cmd;
        public bool _isStartParam;

        public QueryBuilder(string spName, bool IsCmdText = false)
        {
            _cmd = new OdbcCommand();
            if (IsCmdText)
            {
                _cmd.CommandType = CommandType.Text;
            }
            else
            {
                _cmd.CommandType = CommandType.StoredProcedure;
            }
            _cmd.CommandTimeout = 0; //기본값이 30이라 반드시 0으로 변경해 줘야 DB에 SET @@max_execution_timeout 호출을 안함
            _cmd.CommandText = spName;
            _isStartParam = true;
        }
        ~QueryBuilder()
        {
            _cmd.Connection = null;
            _cmd = null;
        }
        public void SetInputParam(string szParamName, UInt64 value)
        {
            _SetParam(szParamName, DbType.UInt64, ParameterDirection.Input, value, 8, value.ToString());
        }
        public void SetInputParam(string szParamName, Int64 value)
        {
            _SetParam(szParamName, DbType.Int64, ParameterDirection.Input, value, 8, value.ToString());
        }
        public void SetInputParam(string szParamName, Int32 value)
        {
            _SetParam(szParamName, DbType.Int32, ParameterDirection.Input, value, 4, value.ToString());
        }
        public void SetInputParam(string szParamName, short value)
        {
            _SetParam(szParamName, DbType.Int16, ParameterDirection.Input, value, 2, value.ToString());
        }
        public void SetInputParam(string szParamName, Byte value)
        {
            _SetParam(szParamName, DbType.Byte, ParameterDirection.Input, value, 1, value.ToString());
        }
        public void SetInputParam(string szParamName, bool value)
        {
            _SetParam(szParamName, DbType.Boolean, ParameterDirection.Input, value ? 1 : 0, 1, value.ToString());
        }
        public void SetInputParam(string szParamName, float value)
        {
            _SetParam(szParamName, DbType.Single, ParameterDirection.Input, value, 4, value.ToString());
        }
        public void SetInputParam(string szParamName, double value)
        {
            _SetParam(szParamName, DbType.Single, ParameterDirection.Input, value, 8, value.ToString());
        }
        public void SetInputParam(string szParamName, string value)
        {
            _SetParam(szParamName, DbType.String, ParameterDirection.Input, value, value.Length > 0 ? value.Length : 1, value);
        }
        public void SetInputParam(string szParamName, DateTime value)
        {
            _SetParam(szParamName, DbType.DateTime, ParameterDirection.Input, value, 8, "");
        }
        public void SetOutputParam(string szParamName, Int32 value)
        {
            _SetParam(szParamName, DbType.Int32, ParameterDirection.Output, value, 4, value.ToString());
        }
        public void SetOutputParam(string szParamName, Byte value)
        {
            _SetParam(szParamName, DbType.Byte, ParameterDirection.Output, value, 1, value.ToString());
        }
        public void SetOutputParam(string szParamName, string value, long size)
        {
            _SetParam(szParamName, DbType.String, ParameterDirection.Output, value, value.Length, value.ToString());
        }


        public object GetParam(string szParamName)
        {
            try
            {
                return _cmd.Parameters[szParamName].Value;
            }
            catch (OdbcException e)
            {
                string ErrorMsg = "[GetParam] " + e.Message;
                throw new Exception(ErrorMsg);
            }
        }
        public string GetStrParam(string szParamName)
        {
            object value = GetParam(szParamName);
            if (value == null)
            {
                value = "";
            }
            return (string)value;
        }

        public string GetSpName()
        {
            return _cmd != null ? _cmd.CommandText : "";
        }

        private void _SetParam(string szParamName, DbType type, ParameterDirection direction, object value, int size, string strValue)
        {
            try
            {
                OdbcParameter parameter = _cmd.CreateParameter();
                parameter.ParameterName = szParamName;
                parameter.DbType = type;
                //parameter.OdbcType = type;
                parameter.Direction = direction;
                parameter.Value = value;
                parameter.Size = size;
                _cmd.Parameters.Add(parameter);

                if (_cmd.CommandType == CommandType.Text)
                {
                    string commandText = _cmd.CommandText;
                    if (_isStartParam)
                    {
                        commandText += "(" + (direction == ParameterDirection.Input ? strValue : "@" + szParamName);
                    }
                    else
                    {
                        commandText += ", " + (direction == ParameterDirection.Input ? strValue : "@" + szParamName);
                    }
                    _cmd.CommandText = commandText;
                }
            }
            catch (OdbcException e)
            {
                string ErrorMsg = "[_SetParam] " + szParamName;
                ErrorMsg += e.Source + ", Description=" + e.HResult.ToString() + ", ErrorMsg=" + e.Message;
                throw new Exception(ErrorMsg);
            }
        }
        public void SetCmdTextEndParam()
        {
            string commandText = _cmd.CommandText;
            commandText += ")";
            _cmd.CommandText = commandText;
        }
    }
}
