using Service.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Text;

namespace Service.DB
{
    public struct RecordType
    {
        private int _index;
        private dynamic _value;

        public RecordType(int index, object value) { _index = index; _value = value; }
        public RecordType(RecordType rRecordType) { _index = rRecordType._index; _value = rRecordType._value; }

        public static implicit operator Byte(RecordType d)
        {
            return _ChangeType<Byte>(d);
        }
        public static implicit operator Int16(RecordType d)
        {
            return _ChangeType<Int16>(d);
        }
        public static implicit operator Int32(RecordType d)
        {
            return _ChangeType<Int32>(d);
        }
        public static implicit operator float(RecordType d)
        {
            return _ChangeType<float>(d);
        }
        public static implicit operator double(RecordType d)
        {
            return _ChangeType<double>(d);
        }
        public static implicit operator bool(RecordType d)
        {
            return _ChangeType<bool>(d);
        }
        public static implicit operator Int64(RecordType d)
        {
            return _ChangeType<Int64>(d);
        }
        public static implicit operator UInt64(RecordType d)
        {
            return _ChangeType<UInt64>(d);
        }
        public static implicit operator DateTime(RecordType d)
        {
            return _ChangeType<DateTime>(d);
        }
        private static T _ChangeType<T>(RecordType d)
        {
            try
            {
                //https://stackoverflow.com/questions/9741788/why-does-this-cast-from-short-to-int-fail
                //언박싱 에러
                T tempValue = (T)(d._value);
                return tempValue;
            }
            catch (Exception e)
            {
                string szError = d._index.ToString() + "(" + typeof(T).Name + ") Invalid Size!! ErrorMsg=" + e.Message;
                throw new Exception(szError);
            }
        }
    }
    public class AdoDB : DBBase
    {
        private OdbcConnection _conn;
        private OdbcDataReader _recSet;
        private bool _isFirstRecord;

        public AdoDB(Logger writeErrorLog) : base(writeErrorLog)
        {
            _conn = null;
            _recSet = null;
            _isFirstRecord = false;
        }
        ~AdoDB()
        {
            Close();
        }

        public override bool IsOpen()
        {
            long conn_state = 0;

            if (_conn != null && base.IsOpen() == true)
            {
                ConnectionState state = _conn.State;
                if (state != ConnectionState.Closed)
                {
                    return true;
                }
            }
            return false;
        }
        public override void Open(DBInfo rDBInfo, double reconnectTime)
        {
            try
            {
                base.SetDBInfo(rDBInfo);
                string strConn = "Driver={MySQL ODBC 8.0 Unicode Driver};";
                strConn += " Server=" + rDBInfo._dbIP + ";";
                strConn += " Port=" + rDBInfo._dbPort.ToString() + ";";
                strConn += " Database=" + rDBInfo._dbName + ";";
                strConn += " Uid=" + rDBInfo._id + ";";
                strConn += " Pwd=" + rDBInfo._pw + ";";
                strConn += " no_ssps=1" + ";";
                _conn = new OdbcConnection(strConn);
                //OdbcCommand.CommandTimeout = 0;
                _conn.ConnectionTimeout = 5;
                _conn.Open();
                base.Open(rDBInfo, reconnectTime);
            }
            catch (OdbcException e)
            {
                _ThrowErrorMsg(e.Message);
            }
        }
        public override void Close()
        {
            try
            {
                if (_conn != null)
                {
                    if (_conn.State != ConnectionState.Closed)
                    {
                        _conn.Close();
                    }
                    _conn = null;
                }
                if (_recSet != null)
                {
                    if (_recSet.IsClosed != true)
                    {
                        _recSet.Close();
                    }
                    _recSet = null;
                }
            }
            catch (OdbcException e)
            {
                _ThrowErrorMsg(e.Message);
            }
        }

        public void Execute(QueryBuilder rQuery, bool IsCmdText = false)
        {
            try
            {
                rQuery._cmd.Connection = _conn;
                if (IsCmdText)
                {
                    rQuery._cmd.CommandType = CommandType.Text;
                }
                else
                {
                    rQuery._cmd.CommandType = CommandType.StoredProcedure;
                }
                _recSet = rQuery._cmd.ExecuteReader();
                _isFirstRecord = true;
            }
            catch (OdbcException e)
            {
                _recSet = null;
                _ThrowErrorMsg("[Excute]" + e.Message);
            }
        }
        public void ExecuteNoRecords(QueryBuilder rQuery, bool IsCmdText = false)
        {
            try
            {
                if (IsCmdText)
                {
                    rQuery.SetCmdTextEndParam();
                }
                _recSet = null;
                rQuery._cmd.Connection = _conn;
                rQuery._cmd.ExecuteNonQuery();
            }
            catch (OdbcException e)
            {
                _ThrowErrorMsg("[ExecuteNoRecords]" + e.Message);
            }
        }

        public object ExecuteGetOutParam(QueryBuilder rQuery, string szParamName)
        {
            try
            {
                string paramName = "@" + szParamName;
                string CommandText = "select " + paramName;

                rQuery._cmd.CommandText = CommandText;
                _recSet = rQuery._cmd.ExecuteReader();
                return _RecordGetValue(_recSet.GetOrdinal(szParamName));
            }
            catch (OdbcException e)
            {
                string szMsg = "[ExecuteGetOutParam] "; szMsg += szParamName;
                _ThrowErrorMsg(szMsg + e.Message);
                return null;
            }
        }
        public string ExcuteGetStrOutParam(QueryBuilder rQuery, string szParamName)
        {
            try
            {
                object value = ExecuteGetOutParam(rQuery, szParamName);
                if (value == null)
                {
                    value = "";
                }

                return (string)value;
            }
            catch (OdbcException e)
            {
                string szMsg = "[ExecuteGetStrOutParam] "; szMsg += szParamName;
                _ThrowErrorMsg(szMsg + e.Message);
                return "";
            }
        }

        public bool RecordNotEOF()
        {
            try
            {
                if (_recSet == null)
                {
                    return false;
                }
                if (_recSet.IsClosed == true)
                {
                    return false;
                }
                if (_recSet.HasRows == false)
                {
                    return false;
                }
            }
            catch (OdbcException e)
            {
                _ThrowErrorMsg("[RecordWhileNotEOF]" + e.Message);
            }
            return true;
        }
        public bool RecordWhileNotEOF()
        {
            try
            {
                bool IsNotEOF = true;
                if (_isFirstRecord)
                {
                    //테스트 필요 !!!!
                    _isFirstRecord = false;
                    IsNotEOF = _recSet.Read();
                }
                else
                {
                    if (_recSet != null && _recSet.IsClosed != true)
                    {
                        //테스트 필요 !!!!
                        IsNotEOF = _recSet.Read();
                    }
                }
                //테스트 필요 !!!!
                if (IsNotEOF == false)
                {
                    return false;
                }
                return RecordNotEOF();
            }
            catch (OdbcException e)
            {
                _ThrowErrorMsg("[RecordWhileNotEOF]" + e.Message);
            }
            return true;
        }
        public void RecordEnd()
        {
            try
            {
                if (_recSet != null)
                {
                    if (_recSet.IsClosed != true)
                    {
                        _recSet.Close();
                    }
                    _recSet = null;
                }
            }
            catch (OdbcException e)
            {
                _ThrowErrorMsg("[RecordEnd]"+ e.Message);
            }
        }

        public bool NextRecordSet(int filedCount)
        {
            try
            {
                bool nextResult = _recSet.NextResult();
                _isFirstRecord = true;

                if (_recSet != null && _recSet.IsClosed == false && _recSet.FieldCount == filedCount)
                {
                    return true;
                }
            }
            catch (OdbcException e)
            {
                _ThrowErrorMsg("[NextRecordSet]" + e.Message);
            }
            return false;
        }
        public RecordType RecordGetValue(string szParamName)
        {
            int index = _recSet.GetOrdinal(szParamName);
            object value = _RecordGetValue(index);
            return new RecordType(index, value);
        }
        public RecordType RecordGetValue(int index)
        {
            object value = _RecordGetValue(index);
            return new RecordType(index, value);
        }
        public DateTime RecordGetTimeValue(string szParamName)
        {
            int index = _recSet.GetOrdinal(szParamName);
            return RecordGetTimeValue(index);
        }
        public DateTime RecordGetTimeValue(int index)
        {
            object value = _RecordGetValue(index);
            return new RecordType(index, value);
        }
        public string RecordGetStrValue(string szParamName)
        {
            int index = _recSet.GetOrdinal(szParamName);
            return RecordGetStrValue(index);
        }
        public string RecordGetStrValue(int index)
        {
            try
            {
                object value = _RecordGetValue(index);
                if (value == null)
                {
                    value = "";
                }

                return (string)value;
            }
            catch (OdbcException e)
            {
                string szMsg = "[RecordGetStrValue] "; szMsg += index.ToString();
                _ThrowErrorMsg(szMsg + e.Message);
                return "";
            }
        }

        public object _RecordGetValue(int index)
        {
            try
            {
                object value = _recSet.GetValue(index);
                if (DBNull.Value.Equals(value))
                {
                    return null;
                }
                else
                {
                    return value;
                }
            }
            catch (OdbcException e)
            {
                string szMsg = "[_RecordGetValue] "; szMsg += index.ToString();
                _ThrowErrorMsg(szMsg + e.Message);
                return null;
            }
        }
        protected override void _ThrowErrorMsg(string szMsg)
        {
            base._ThrowErrorMsg(szMsg);

        }
    }
}
