using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JWCControlLib;
using System.Reflection;

namespace JWCControlLib
{
    public static class PropItemFactory
    {
        public static APropItem GetPropItem(PropDiscribeAttribute attr,IPropGWAble jc,PropertyInfo pi)
        {
            var type = attr.ShowType;
            switch(type)
            {
                case CreatorPropType.Text:return new TextPropItem(jc,pi,attr);
                case CreatorPropType.Boolean: return new BoolPropItem(jc, pi,attr);
                case CreatorPropType.Multi: return new MultiPropItem(jc, pi, attr);
                case CreatorPropType.DialogWithText: return new DialogWithTextPropItem(jc, pi, attr);
                case CreatorPropType.EnumDropDown: return new EnumDropdownPropItem(jc, pi, attr);
                default:
                    throw new NotImplementedException("你没有实现这种CreatorPropType:" + type.ToString());
                    return null;
            }
        }
    }
}
