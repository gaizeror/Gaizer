using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazierConverter
{
    public class Converter
    {
        public static void Convert(string input, string filename, string output, WdSaveFormat format)
        {
            Microsoft.Office.Interop.Word._Application oWord = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word._Document oDoc = null;
            object oMissing = System.Reflection.Missing.Value;
            try
            {
                oWord.Visible = false;
                oWord.ScreenUpdating = false;

                object isVisible = true;
                object readOnly = true;
                object confirmConversions = false;
                object addToRecentFiles = false;
                object passwordDocument = "12345";

                object oInput = input;
                object oOutput = output;
                object oFormat = WdOpenFormat.wdOpenFormatAuto;
                if (filename.EndsWith(".docx") || filename.EndsWith(".doc"))
                {
                    oFormat = WdOpenFormat.wdOpenFormatAllWord;
                }

                oDoc = oWord.Documents.OpenNoRepairDialog(ref oInput, ref confirmConversions, ref readOnly, ref addToRecentFiles, ref passwordDocument, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oFormat, ref oMissing, ref isVisible, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                oDoc.Activate();

                object oSaveFormat = format;
                oDoc.SaveAs2(ref oOutput, ref oSaveFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            }

            finally
            {
                oWord.Quit(Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges, ref oMissing, ref oMissing);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oWord);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(oDoc);
                // Release all Interop objects.
                if (oDoc != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oDoc);
                if (oWord != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oWord);
                oDoc = null;
                oWord = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }
}
