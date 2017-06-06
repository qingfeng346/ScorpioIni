using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
/// <summary> 单个模块数据 </summary>
public class ScorpioIniSection {
    /// <summary> 所有数据 </summary>
    public Dictionary<string, ScorpioIniValue> datas = new Dictionary<string, ScorpioIniValue>();
    /// <summary> 设置值 </summary>
    public void Set(string key, string value, string comment) {
        if (!datas.ContainsKey(key))
            datas.Add(key, new ScorpioIniValue());
        datas[key].Set(value, comment);
    }
    /// <summary> 获得值 </summary>
    public ScorpioIniValue Get(string key) {
        if (datas.ContainsKey(key))
            return datas[key];
        return null;
    }
    /// <summary> 删除值 </summary>
    public void Remove(string key) {
        if (datas.ContainsKey(key)) {
            datas.Remove(key);
        }
    }
}
/// <summary> 具体数据 </summary>
public class ScorpioIniValue {
    /// <summary> 值 </summary>
    public string value;
    /// <summary> 注释 </summary>
    public string comment;
    /// <summary> 设置值 </summary>
    public void Set(string value, string comment) {
        if (value != null)
            this.value = value;
        if (comment != null)
            this.comment = comment;
    }
}
/// <summary> 读取ini文件 </summary>
public class ScorpioIni {
    /// <summary> 所有数据 </summary>
    public Dictionary<string, ScorpioIniSection> m_ConfigData = new Dictionary<string, ScorpioIniSection>();
    /// <summary> 构造函数 </summary>
    public ScorpioIni() { }
    /// <summary> 构造函数 </summary>
    public ScorpioIni(byte[] bytes, Encoding encoding) {
        InitFormBuffer(bytes, encoding);
    }
    public ScorpioIni(string file, Encoding encoding) {
        InitFormFile(file, encoding);
    }
    /// <summary> 构造函数 </summary>
    public ScorpioIni(string data) {
        InitFormString(data);
    }
    /// <summary> 根据BYTE[]初始化数据 </summary>
    public void InitFormBuffer(byte[] buffer, Encoding encoding) {
        InitFormString(encoding.GetString(buffer, 0, buffer.Length));
    }
    public void InitFormFile(string file, Encoding encoding) {
        using (FileStream fs = new FileStream(file, FileMode.Open)) {
            long length = fs.Length;
            byte[] buffer = new byte[length];
            fs.Read(buffer, 0, (int)length);
            InitFormString(encoding.GetString(buffer));
        }
    }
    /// <summary> 根据string初始化数据 </summary>
    public void InitFormString(string buffer) {
        try {
            m_ConfigData.Clear();
            string[] datas = buffer.Split('\n');
            string section = "";
            bool startComment = false;
            StringBuilder comment = new StringBuilder();
            int count = datas.Length;
            for (int i = 0; i < count; ++i) {
                string data = datas[i].Trim();
                if (!string.IsNullOrEmpty(data)) {
                    // /* */为区域注释
                    if (data.StartsWith("/*")) {
                        comment.Append(data.Replace(@"/*", "").Replace(@"*/", ""));
                        if (data.EndsWith("*/")) {
                            startComment = false;
                            continue;
                        }
                        startComment = true;
                        continue;
                    }
                    // */ 为区域注释结尾
                    else if (data.EndsWith("*/")) {
                        comment.Append(data.Replace(@"/*", "").Replace(@"*/", ""));
                        startComment = false;
                        continue;
                    }
                    // //为行注释
                    else if (data.StartsWith("//")) {
                        comment.Append(data.Replace(@"//", ""));
                        continue;
                    }
                    if (startComment == true) {
                        continue;
                    }
                    if (data.StartsWith("[")) {
                        int indexLeft = data.IndexOf("[");
                        int indexRight = data.IndexOf("]");
                        if (indexLeft >= 0 && indexRight >= 0) {
                            section = data.Substring(indexLeft + 1, indexRight - indexLeft - 1);
                        }
                    } else {
                        int index = data.IndexOf("=");
                        if (index >= 0) {
                            string key = data.Substring(0, index).Trim();
                            string value = data.Substring(index + 1).Trim();
                            Set(section, key, value, comment.ToString());
                            comment = new StringBuilder();
                        } else {
                            throw new Exception((i + 1) + " 行填写错误, 正确格式为 key=value 不支持回车");
                        }
                    }
                }
            }
        } catch (System.Exception e) {
            throw new Exception("initialize is error : " + e.ToString());
        }
    }
    /// <summary> 返回所有数据 </summary>
    public Dictionary<string, ScorpioIniSection> GetData() {
        return m_ConfigData;
    }
    /// <summary> 返回单个模块的数据 </summary>
    public ScorpioIniSection GetSection() {
        return GetSection("");
    }
    /// <summary> 返回单个模块的数据 </summary>
    public ScorpioIniSection GetSection(string section) {
        if (!m_ConfigData.ContainsKey(section))
            return null;
        return m_ConfigData[section];
    }
    /// <summary> 获得Value </summary>
    public string Get(string key) {
        return Get("", key);
    }
    /// <summary> 设置Value </summary>
    public string Get(string section, string key) {
        if (m_ConfigData.Count <= 0)
            return "";
        if (section == null) section = "";
        if (!m_ConfigData.ContainsKey(section))
            return "";
        var configValue = m_ConfigData[section].Get(key);
        return configValue != null ? configValue.value : "";
    }
    public ScorpioIniValue GetValue(string key) {
        return GetValue("", key);
    }
    /// <summary> 设置Value </summary>
    public ScorpioIniValue GetValue(string section, string key) {
        if (m_ConfigData.Count <= 0)
            return null;
        if (section == null) section = "";
        if (!m_ConfigData.ContainsKey(section))
            return null;
        return m_ConfigData[section].Get(key);
    }
    /// <summary> 设置Value </summary>
    public void Set(string key, string value) {
        Set("", key, value);
    }
    /// <summary> 设置Value </summary>
    public void Set(string section, string key, string value) {
        Set(section, key, value, null);
    }
    /// <summary> 设置Value </summary>
    public void Set(string section, string key, string value, string comment) {
        if (!m_ConfigData.ContainsKey(section))
            m_ConfigData.Add(section, new ScorpioIniSection());
        m_ConfigData[section].Set(key, value, comment);
    }
    /// <summary> 删除Key </summary>
    public void Remove(string key) {
        Remove("", key);
    }
    /// <summary> 删除Key </summary>
    public void Remove(string section, string key) {
        if (m_ConfigData.ContainsKey(section))
            m_ConfigData[section].Remove(key);
    }
    /// <summary> 清空一个模块 </summary>
    public void ClearSection() {
        ClearSection("");
    }
    /// <summary> 清空一个模块 </summary>
    public void ClearSection(string section) {
        if (m_ConfigData.ContainsKey(section))
            m_ConfigData.Remove(section);
    }
    /// <summary> 清空所有数据 </summary>
    public void ClearData() {
        m_ConfigData.Clear();
    }
    /// <summary> 返回数据字符串 </summary>
    public string GetString() {
        SortedDictionary<string, ScorpioIniSection> data = new SortedDictionary<string, ScorpioIniSection>(m_ConfigData);
        StringBuilder builder = new StringBuilder();
        if (data.ContainsKey("")) {
            foreach (var val in data[""].datas) {
                var configValue = val.Value;
                if (!string.IsNullOrEmpty(configValue.comment))
                    builder.AppendLine(string.Format("/*{0}*/", configValue.comment));
                builder.AppendLine(string.Format("{0}={1}", val.Key, configValue.value));
            }
        }
        foreach (var pair in data) {
            if (string.IsNullOrEmpty(pair.Key))
                continue;
            builder.AppendLine(string.Format("[{0}]", pair.Key));
            foreach (var val in pair.Value.datas) {
                var configValue = val.Value;
                if (!string.IsNullOrEmpty(configValue.comment))
                    builder.AppendLine(string.Format("/*{0}*/", configValue.comment));
                builder.AppendLine(string.Format("{0}={1}", val.Key, configValue.value));
            }
        }
        return builder.ToString();
    }
}