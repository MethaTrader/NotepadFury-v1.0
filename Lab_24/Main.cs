using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace NotepadFury
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();

            NewFIleStripButton.Click += createToolStripMenuItem_Click;
            SaveFileStripButton.Click += saveAsToolStripMenuItem_Click;
            PasteStripButton.Click += pasteToolStripMenuItem_Click;
            PreviewStripButton.Click += previewToolStripMenuItem_Click;
            PrintStripButton.Click += printToolStripMenuItem_Click;
            
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Views.About about = new Views.About();
            about.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textarea.Text = File.ReadAllText(openFileDialog.FileName);
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(textarea.Text);
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(textarea.Text); textarea.Clear();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textarea.Text += Clipboard.GetText();
        }

        private void newWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Main mainwindow = new Main();
            mainwindow.Show();
        }

        private void dateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textarea.Text += DateTime.Now;
        }
        private void searchByBingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = $"https://www.bing.com/search?q={textarea.SelectedText}&ie=UTF-8";
            Process.Start(url);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // сохраняем текст в файл
                File.WriteAllText(saveFileDialog.FileName, textarea.Text);
                MessageBox.Show("Файл сохранен");
            }
        }
        
        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены что хотите создать новый файл?","Предупреждение",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
            {
                textarea.Clear();
            }
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog.ShowColor = true;
            if (fontDialog.ShowDialog() != DialogResult.Cancel)
            {
                textarea.Font = fontDialog.Font;
                textarea.ForeColor = fontDialog.Color;
            }
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {

            printDocument.DocumentName = "Печать документа";
            printDialog.Document = printDocument;
            printDialog.AllowSelection = true;
            printDialog.AllowSomePages = true;
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument.Print();
            }
            else
            {
                MessageBox.Show("Печать отменена");
            }
        }

        private void previewToolStripMenuItem_Click(object sender, EventArgs e)
        {

            printPreviewDialog.Document = printDocument;
            printPreviewDialog.ShowDialog();
        }

        private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int charactersOnPage = 0;
            int linesPerPage = 0;
            string stringToPrint = textarea.Text;

            e.Graphics.MeasureString(stringToPrint, this.Font,
                e.MarginBounds.Size, StringFormat.GenericTypographic,
                out charactersOnPage, out linesPerPage);

            e.Graphics.DrawString(stringToPrint, this.Font, Brushes.Black,
                e.MarginBounds, StringFormat.GenericTypographic);

            stringToPrint = stringToPrint.Substring(charactersOnPage);

            e.HasMorePages = (stringToPrint.Length > 0);
        }

        private void textarea_KeyPress(object sender, KeyPressEventArgs e)
        {
            countCymbol_label.Text = textarea.Text.Length.ToString();
        }
    }
}
