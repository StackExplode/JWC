using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;

namespace JWC
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var lsn = new TextWriterTraceListener("./Runner.log");
            Trace.Listeners.Add(lsn);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Trace.TraceInformation("------------程序启动------------");
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.Append("[");
            sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff"));
            sb.Append("]异常类型:");
            sb.Append(ex.GetType());
            sb.Append("\t异常信息：");
            sb.AppendLine(ex.Message);
            sb.AppendLine("详细信息：");
            sb.AppendLine(ex.ToString());
            Trace.TraceError(sb.ToString());
            Trace.Flush();
            MessageBox.Show("程序执行发生错误！详细信息请参见根目录中的Creator.log，并联系开发者。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
