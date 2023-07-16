using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadinProjectNotes
{
    public class Printer
    {
        private PrintDocument printDoc = new PrintDocument();

        public PrintDocument PrintDoc
        {
            get { return printDoc; }
        }

        private readonly Font projectFont = new Font("Calibri", 15.0f, FontStyle.Underline);
        private readonly Font titleFont = new Font("Calibri", 10.0f);
        private readonly Font bodyFont = new Font("Calibri", 10.0f);
        private readonly Brush projectBrush = Brushes.Black;
        private readonly Brush titleBrush = Brushes.Red;
        private readonly Brush bodyBrush = Brushes.Black;

        private ProjectFolder projectFolder;
        private Versioning.SaveStructureV1 noteDatabase;

        public int CurrentNoteIndex
        {
            get;
            set;
        }

        public bool ContinueFromLastNoteText
        {
            get;
            set;
        }

        public bool FirstPage
        {
            get;
            set;
        }

        public Printer(ProjectFolder projectFolder, Versioning.SaveStructureV1 noteDatabase)
        {
            this.projectFolder = projectFolder;
            this.noteDatabase = noteDatabase;
            this.ContinueFromLastNoteText = false;
            this.CurrentNoteIndex = 0;
            this.FirstPage = true;
            this.printDoc.DocumentName = projectFolder.projectPath + " Notes";

            printDoc.PrintPage += new PrintPageEventHandler(pd_PrintPage);
            printDoc.EndPrint += new PrintEventHandler(printDoc_PrintComplete);
        }

        public void Print()
        {
            this.printDoc.Print();
        }

        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            //new experimental print function

            int yCur = e.MarginBounds.Top;
            SizeF sz;

            //if first page, print the project title at the top
            if (this.FirstPage)
            {
                string projectTitle = this.projectFolder.projectPath;
                sz = e.Graphics.MeasureString(projectTitle, projectFont, e.MarginBounds.Width, StringFormat.GenericTypographic);
                e.Graphics.DrawString(projectTitle, projectFont, projectBrush, new PointF(e.MarginBounds.Left, yCur), StringFormat.GenericTypographic);
                yCur += (int)sz.Height*2;

                this.FirstPage = false;
            }

            //loop all notes left in the database
            for (int i = this.CurrentNoteIndex; i < noteDatabase.noteData.Count; i++)
            {
                Notes.ProjectNote note = noteDatabase.noteData[i];
                //print the comment title only if flag is false
                if (!this.ContinueFromLastNoteText)
                {
                    string commentTitle = note.CreatedByUsername + " posted on " + note.DateEditedString;
                    sz = e.Graphics.MeasureString(commentTitle, titleFont, e.MarginBounds.Width, StringFormat.GenericTypographic);
                    e.Graphics.DrawString(commentTitle, titleFont, titleBrush, new PointF(e.MarginBounds.Left, yCur), StringFormat.GenericTypographic);
                    yCur += (int)sz.Height;
                }


                //reset flag
                this.ContinueFromLastNoteText = false;
                string commentBody = note.noteText;

                //measure the text's size (width, height)
                Size printSize = e.MarginBounds.Size;
                printSize.Height = e.MarginBounds.Bottom - yCur;
                SizeF size = e.Graphics.MeasureString(commentBody, bodyFont, printSize, StringFormat.GenericTypographic, out int lineCharacters, out int linesPerPage);
                SizeF fullRectPrint = e.Graphics.MeasureString(commentBody, bodyFont, e.MarginBounds.Width, StringFormat.GenericTypographic);
                RectangleF rect = new RectangleF(e.MarginBounds.Left, yCur, printSize.Width, printSize.Height);
                e.Graphics.DrawString(commentBody, bodyFont, bodyBrush, rect, StringFormat.GenericTypographic);

                //check if the text goes beyond the bottom page margin
                if (yCur + (int)fullRectPrint.Height > e.MarginBounds.Bottom)
                {
                    //remove the portion of the text from the comment
                    note.noteText = commentBody.Substring(lineCharacters);

                    //set the flag to continue printing this note's text
                    this.ContinueFromLastNoteText = true;

                    //go to next page
                    e.HasMorePages = true;
                    return;
                }
                yCur += (int)size.Height;

                //increment the processed notes index
                this.CurrentNoteIndex++;

                yCur += titleFont.Height * 2;   //skip 2 lines between comments

                //check if we are too close to page bottom for next commment
                if (yCur >= e.MarginBounds.Bottom)
                {
                    //skip page
                    e.HasMorePages = true;
                    return;
                }
            }

            e.HasMorePages = false;

        }

        private void printDoc_PrintComplete(object sender, PrintEventArgs e)
        {
            string printFileName;
            try
            {
                printFileName = printDoc.PrinterSettings.PrintFileName;
            }
            catch
            {

            }
        }
    }
}
