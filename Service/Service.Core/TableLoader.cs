using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Service.Core
{
    public class TableLoader<K, U> where U : ITableData, new()
    {
        public static Dictionary<K, U> Run(string path)
        {
            Dictionary<K, U> dataList = new Dictionary<K, U>();
            string line = string.Empty;
            try
            {
                if (File.Exists(path) == false) 
                {
                    Logger.Default.Log(ELogLevel.Err, $"not exist table file path : {path}");
                    return dataList;
                }

                using (var reader = new StreamReader(path)) 
                { 
                    List<string> fields = new List<string>();
                    List<int> exceptIndex = new List<int>();
                    int row = 0;
                    while (!reader.EndOfStream)
                    {
                        line = reader.ReadLine();
                        int replaceIndex = line.IndexOf("\"");
                        while (replaceIndex != -1)
                        {
                            var originText = line.Substring(replaceIndex, line.IndexOf("\"", replaceIndex + 1) - replaceIndex + ("\"").Length);
                            var replaceText = originText.Replace(",", "{$}");
                            replaceText = replaceText.Replace("\"", "");
                            line = line.Replace(originText, replaceText);
                            replaceIndex = line.IndexOf("\"");
                        }

                        List<string> column = line.Split(',').ToList();
                        if (row == 0)
                        {
                            for (int i = column.Count - 1; i >= 0; --i)
                            {
                                if (column[i].StartsWith("~") == true)
                                {
                                    exceptIndex.Add(i);
                                }
                                else
                                {
                                    fields.Add(column[i]);
                                }
                            }
                            fields.Reverse();
                        }
                        if (row++ < 2)
                        {
                            continue;
                        }

                        foreach (var index in exceptIndex)
                        {
                            column.RemoveAt(index);
                        }

                        Dictionary<string, string> colValues = new Dictionary<string, string>();
                        for (int i = 0; i < fields.Count; ++i)
                        {
                            colValues.Add(fields[i], column[i]);
                        }

                        U data = new U();
                        data.Serialize(colValues);

                        K key = (K)Convert.ChangeType(column[0], typeof(K));
                        if (dataList.ContainsKey(key) == true)
                        {
                            throw new Exception($"duplicate key!! key : {key}");
                        }

                        dataList.Add(key, data);
                    }
                }
            }
            catch(Exception e) 
            { 
                Logger.Default.Log(ELogLevel.Err, e.Message);
                Logger.Default.Log(ELogLevel.Err, $"filePath: {path}, line: {line}");
                throw;
            }

            //임시주석(FIXME)
            //Logger.Default.Log(ELogLevel.Always, $"Load data file. path: {path}");
            return dataList;
        }
    }
}
