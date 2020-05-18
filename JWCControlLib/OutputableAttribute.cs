using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace JWCControlLib
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OutputableAttribute:Attribute
    {

    }


    [AttributeUsage(AttributeTargets.Property,AllowMultiple=false)]
    public class RedirectGSAttribute:Attribute
    {
        public string Fun { get; set; }
        public RedirectGSAttribute(string fun)
        {
            Fun = fun;
        }
    }

    public enum CreatorPropType
    {
        Text,
        Boolean,
        DropDown,
        Dialog
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ProplDiscribeAttribute:Attribute
    {
        public string Describe = "";
        public CreatorPropType ShowType = CreatorPropType.Text;
        public object Param;
        public ProplDiscribeAttribute(CreatorPropType stype, string desc,object pa)
        {
            ShowType = stype;
            Describe = desc;
            Param = pa;
        }
    }


}
