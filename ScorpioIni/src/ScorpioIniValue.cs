using System;
using System.Collections.Generic;
using System.Text;

namespace Scorpio.Ini {
    /// <summary> 具体数据 </summary>
    public class ScorpioIniValue {
        /// <summary> 值 </summary>
        public string value { get; private set; }
        /// <summary> 注释 </summary>
        public string comment { get; private set; }
        /// <summary> 设置值 </summary>
        public void Set(string value, string comment) {
            if (value != null)
                this.value = value;
            if (comment != null)
                this.comment = comment;
        }
    }
}