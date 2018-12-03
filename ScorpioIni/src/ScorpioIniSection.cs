using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Scorpio.Ini {
    /// <summary> 单个模块数据 </summary>
    public class ScorpioIniSection {
        public string section { get; private set; }
        /// <summary> 所有数据 </summary>
        private Dictionary<string, ScorpioIniValue> m_datas = new Dictionary<string, ScorpioIniValue>();
        public ReadOnlyDictionary<string, ScorpioIniValue> datas { get { return new ReadOnlyDictionary<string, ScorpioIniValue>(m_datas); } }
        public ScorpioIniSection(string section) {
            this.section = section;
        }
        public void Set(string key, string value) {
            Set(key, value, null);
        }
        /// <summary> 设置值 </summary>
        public void Set(string key, string value, string comment) {
            if (!m_datas.ContainsKey(key))
                m_datas.Add(key, new ScorpioIniValue());
            m_datas[key].Set(value, comment);
        }
        /// <summary> 获得值 </summary>
        public ScorpioIniValue Get(string key) {
            if (m_datas.ContainsKey(key))
                return m_datas[key];
            return null;
        }
        public string GetString(string key, string def) {
            var value = Get(key);
            return value != null ? value.value : def;
        }
        /// <summary> 删除值 </summary>
        public bool Remove(string key) {
            if (m_datas.ContainsKey(key)) {
                m_datas.Remove(key);
                return true;
            }
            return false;
        }
    }
}