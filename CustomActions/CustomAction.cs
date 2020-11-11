using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using Microsoft.Deployment.WindowsInstaller;
using System.Windows.Forms;

namespace WiXCustomActions
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult OpenLog(Session session)
        {
            //MessageBox.Show("OpenLog");

            var fullpath = session["INSTALLFOLDER"] + "TestFile.rtf";

            var content = "";
            using (var fileStream = new FileStream(fullpath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var textReader = new StreamReader(fileStream))
            {
                content = textReader.ReadToEnd();
            }

            var view = session.Database.OpenView("SELECT * FROM Control WHERE Dialog_ = 'LogDlg' AND Control = 'TheLog'");

            view.Execute();
            var record = view.Fetch();
            view.Delete(record);
            //record.SetString("Text", FormatAsRTF("updated dynamically"));
            record.SetString("Text", content);
            view.InsertTemporary(record);

            return ActionResult.Success;
        }

        private static string FormatAsRTF(string DirtyText)
        {
            System.Windows.Forms.RichTextBox rtf = new System.Windows.Forms.RichTextBox();
            rtf.Text = DirtyText;
            return rtf.Rtf;
        }
    }
}

//session["TEST"] = FormatAsRTF("updated dynamically");

//MessageBox.Show(session["TEST"]);
//MessageBox.Show(fullpath);