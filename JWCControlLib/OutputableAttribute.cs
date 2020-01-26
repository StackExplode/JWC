using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JWCControlLib
{
    [AttributeUsage(AttributeTargets.Property)]
    class OutputableAttribute:Attribute
    {

    }


    [AttributeUsage(AttributeTargets.Property,AllowMultiple=false)]
    class RedirectGSAttribute:Attribute
    {
        public string Fun { get; set; }
        public RedirectGSAttribute(string fun)
        {
            Fun = fun;
        }
    }


}
