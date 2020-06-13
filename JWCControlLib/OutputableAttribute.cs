using System;
using System.ComponentModel;

namespace JWCControlLib
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OutputableAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RedirectGSAttribute : Attribute
    {
        public string Fun { get; set; }

        //public Func<object, bool, object,object> Func;
        public RedirectGSAttribute(string fun)
        {
            Fun = fun;
        }
    }
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class JWCControlDescAttribute : Attribute
    {
        public string FriendlyName { get; private set; }
        public string Description { get; private set; }

        //public Func<object, bool, object,object> Func;
        public JWCControlDescAttribute(string fname,string des="")
        {
            FriendlyName = fname;
            Description = des;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class JWCCommDescAttribute : Attribute
    {
        public string FriendlyName { get; private set; }
        public string Description { get; private set; }

        //public Func<object, bool, object,object> Func;
        public JWCCommDescAttribute(string fname, string des = "")
        {
            FriendlyName = fname;
            Description = des;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class JWCAdapterDescAttribute : Attribute
    {
        public string FriendlyName { get; private set; }
        public string Description { get; private set; }

        //public Func<object, bool, object,object> Func;
        public JWCAdapterDescAttribute(string fname, string des = "")
        {
            FriendlyName = fname;
            Description = des;
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
    public class PropDiscribeAttribute : Attribute
    {
        public string Describe = "";
        public CreatorPropType ShowType = CreatorPropType.Text;
        public object Param;
        public string FriendlyName = "";

        public PropDiscribeAttribute(CreatorPropType stype, string fname, string desc = "无帮助文本", object pa = null)
        {
            ShowType = stype;
            Describe = desc;
            FriendlyName = fname;
            Param = pa;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class SubPropAttribute : Attribute
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

    public enum ComuIDType
    {
        [Description("无")]
        None,

        [Description("使用ID")]
        ID,

        [Description("使用Name")]
        Name,

        [Description("自定义")]
        Custom
    }
}