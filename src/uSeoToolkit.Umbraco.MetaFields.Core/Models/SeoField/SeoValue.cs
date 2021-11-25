﻿using uSeoToolkit.Umbraco.MetaFields.Core.Interfaces.SeoField;

namespace uSeoToolkit.Umbraco.MetaFields.Core.Models.SeoField
{
    public class SeoValue
    {
        public ISeoField Field { get; }
        public object Value { get; set; }

        public SeoValue(ISeoField field, object value)
        {
            Field = field;
            Value = value;
        }
    }
}
