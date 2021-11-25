﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Cms.Core.Composing;
using uSeoToolkit.Umbraco.MetaFields.Core.Interfaces;
using uSeoToolkit.Umbraco.MetaFields.Core.Interfaces.SeoField;

namespace uSeoToolkit.Umbraco.MetaFields.Core.Collections
{
    public class SeoFieldCollection : BuilderCollectionBase<ISeoField>, ISeoFieldCollection
    {
        public SeoFieldCollection(IEnumerable<ISeoField> items) : base(() => items)
        {
        }

        public ISeoField Get(string alias)
        {
            return this.FirstOrDefault(it => it.Alias == alias);
        }

        public IEnumerable<ISeoField> GetAll()
        {
            return this.OrderBy(it => GetWeight(it.GetType()));
        }

        private int GetWeight(Type type)
        {
            var attr = type.GetCustomAttributes(typeof(WeightAttribute), false).OfType<WeightAttribute>().SingleOrDefault();
            return attr?.Weight ?? 0;
        }
    }
}
