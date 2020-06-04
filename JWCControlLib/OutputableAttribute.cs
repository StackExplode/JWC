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
        //public Func<object, bool, object,object> Func;
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
        EnumDropDown,
        Dialog,
        DialogWithText,
        Flag,
        Multi,
        Other
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PropDiscribeAttribute:Attribute
    {
        public string Describe = "";
        public CreatorPropType ShowType = CreatorPropType.Text;
        public object Param;
        public string FriendlyName = "";
        public PropDiscribeAttribute(CreatorPropType stype,string fname, string desc="无帮助文本",object pa=null)
        {
            ShowType = stype;
            Describe = desc;
            FriendlyName = fname;
            Param = pa;
        }
    }

     [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class SubPropAttribute:Attribute
    {
         public string Describe = "";
        public CreatorPropType ShowType = CreatorPropType.Text;
        public object Param;
        public string FriendlyName = "";
        public SubPropAttribute(CreatorPropType stype, string fname, string desc = "无帮助文本", object pa = null)
        {
            ShowType = stype;
            Describe = desc;
            FriendlyName = fname;
            Param = pa;
        }
    }



}
