using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JWCControlLib
{
    public interface IPropGWAble
    {
        object GetProp(PropertyInfo pi);
        void SetProp(PropertyInfo pi, object val);
        JControlOutputData OutputProperty();
        void InputProperty(JControlOutputData dic);
    }
}
