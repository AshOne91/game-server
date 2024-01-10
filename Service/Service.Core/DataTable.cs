using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Core
{
    public interface ITableData
    {
        void Serialize(Dictionary<string, string> data);
    }

    public class DataTable<T, U> : Singleton<DataTable<T, U>> where U : ITableData, new()
    {
        private Dictionary<T, U> _rows;
        public Dictionary<T, U> ValuePairs { get => _rows; }
        public List<U> Values { get => _rows.Values.ToList(); }
        public int Count { get => _rows.Count; }

        public bool Init(string path)
        {
            _rows = TableLoader<T, U>.Run(path);
            return true;
        }
        public bool ContainsKey(T  key) { return _rows.ContainsKey(key); }
        public U GetData(T key) 
        { 
            if (_rows.TryGetValue(key, out U tableData) == false) 
            {
                return default(U);
            }

            return tableData;
        }
        public U GetData(Predicate<U> predicate)
        {
            return _rows.Values.ToList().Find(predicate);
        }
        public List<U> GetDataList(Predicate<U> predicate)
        {
            return _rows.Values.ToList().FindAll(predicate);
        }
        public U GetLast()
        {
            return _rows.Values.ToList()[_rows.Values.Count - 1];
        }
    }
}
