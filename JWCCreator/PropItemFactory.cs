using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JWCControlLib;
using System.Reflection;

namespace JWCCreator
{
    static class PropItemFactory
    {
        public static IPropItem GetPropItem(CreatorPropType type,JWCControl jc,PropertyInfo pi)
        {
            switch(type)
            {
                case CreatorPropType.Text:return new TextPropItem(jc,pi);
                default: return null;
            }
        }
    }
}
