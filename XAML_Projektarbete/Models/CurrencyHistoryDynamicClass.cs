using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XAML_Projektarbete.DataProvider
{
    class CurrencyHistoryDynamicClass : DynamicObject
    {
        private Dictionary<string, object> _properties;
        public CurrencyHistoryDynamicClass() => _properties = new Dictionary<string, object>();
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            string propertyName = binder.Name.ToLower();
            return _properties.TryGetValue(propertyName, out result);
        }
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _properties[binder.Name.ToLower()] = value;
            return true;
        }
    }
    class PropertSetHistory : SetMemberBinder
    {
        public PropertSetHistory(string name, bool ignoreCase) : base(name, ignoreCase)
        {
        }
        public override DynamicMetaObject FallbackSetMember(DynamicMetaObject target, DynamicMetaObject value, DynamicMetaObject errorSuggestion)
        {
            throw new NotImplementedException();
        }
    }
    class PropertyGetHistory : GetMemberBinder
    {
        public PropertyGetHistory(string name, bool ignoreCase) : base(name, ignoreCase)
        {
        }
        public override DynamicMetaObject FallbackGetMember(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
        {
            throw new NotImplementedException();
        }
    }
}
