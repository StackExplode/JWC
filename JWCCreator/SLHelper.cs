using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JWCCommunicationLib;
using JWCControlLib;
using System.Windows.Media;
using System.IO;
using System.Reflection;

namespace JWCCreator
{
    class SLHelper
    {
        Stage stage;
        public JControlOutputData commu;
        public JControlOutputData adapter;
        public string ComFname, AdaFname;

        public SLHelper(Stage st, JControlOutputData com, JControlOutputData ad)
        {
            stage = st;
            commu = com;
            adapter = ad;
        }

        public void SaveFile(string fname)
        {
            List<JControlOutputData> lst = new List<JControlOutputData>();
            foreach(var c in stage.AllControls)
            {
                JWCControl jc = c as JWCControl;
                if (jc == null)
                    continue;
                lst.Add(jc.OutputProperty());
            }
           
            JWCSaveFile file = new JWCSaveFile();
            file.Width = (int)stage.MainStage.Width;
            file.Height = (int)stage.MainStage.Height;
            file.AllControls = lst;
            file.BgUsePic = stage.BgUsePic;
            file.BackGroundPic = stage.BgFilename;
            Color cl = stage.BgColor;
            file.BackColor = new byte[4] { cl.A, cl.R, cl.G, cl.B };

            file.Communicator = commu;
            file.ComAdapter = adapter;
            file.ComName = ComFname;
            file.AdaName = AdaFname;

            file.Version = Assembly.GetExecutingAssembly().GetName().Version;

            JWCSerializer<JWCSaveFile> jse = new JWCSerializer<JWCSaveFile>();
            jse.Serialize(file, fname);
        }

        public void LoadFile(string fname)
        {
            JWCSerializer<JWCSaveFile> jse = new JWCSerializer<JWCSaveFile>();
            JWCSaveFile file = jse.Deserialize(fname);

            stage.ClearAll(file.Width, file.Height);

            foreach(var s in file.AllControls)
            {
                string fullname = s["FullName"].ToString();
                JWCControl jc = JWCControlFactory.CreateInstance(fullname);
                jc.InputProperty(s);
                jc.IsEditMode = true;
                jc.Init(true);
                stage.AddControl(jc);
            }
            Color cl = Color.FromArgb(file.BackColor[0], file.BackColor[1], file.BackColor[2], file.BackColor[3]);
            stage.SetBg(file.BgUsePic, cl, file.BackGroundPic);
            commu = file.Communicator;
            adapter = file.ComAdapter;
            ComFname = file.ComName;
            AdaFname = file.AdaName;
        }
    }
}
