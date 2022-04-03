using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Word
{
    public partial class Form1 : Form
    {
        string _path = "";
        string _theme = "default";
        public Form1()
        {
            InitializeComponent();

            richTextBox1.ContextMenuStrip = contextMenuStrip1;
        }

        /// <summary>
        /// Метод, который проверяет, не пустой ли файл(если пустой, то неактивны функции копирования и вырезания).
        /// Обработчик исключений везде одинаковый и примитивный.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // Проверка, есть ли в файле какой-либо текста(если нет, то нельзя вырезать и копировать)
                if (richTextBox1.Text.Length > 0)
                {
                    cutToolStripMenuItem.Enabled = true;
                    copyToolStripMenuItem.Enabled = true;
                }
                else
                {
                    cutToolStripMenuItem.Enabled = false;
                    copyToolStripMenuItem.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                DialogResult res = MessageBox.Show(
        "Обнаружена ошибка неизвестного происхождения",
        "Ошибка",
        MessageBoxButtons.OK,
        MessageBoxIcon.Information,
        MessageBoxDefaultButton.Button1,
        MessageBoxOptions.DefaultDesktopOnly);
                if (res == DialogResult.OK)
                {
                    return;
                }
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            _path = "";
        }

        /// <summary>
        /// Метод, который вызывается при открытии файла. Позволяет выбрать тип файла(txt или rtf).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "TextFile(.txt)|*.txt|RtfFile(.rtf)|*.rtf", ValidateNames = true, Multiselect = false })
                {
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        using (StreamReader sr = new StreamReader(openFileDialog.FileName))
                        {
                            _path = openFileDialog.FileName;
                            Task<string> text = sr.ReadToEndAsync();
                            if (_path.EndsWith(".rtf"))
                            {
                                // Следующая проверка нужна, чтобы в случае установки темной темы текст не сливался с фоном.
                                if (_theme == "night")
                                {
                                    richTextBox1.ForeColor = Color.LimeGreen;
                                }
                                richTextBox1.Rtf = text.Result;

                            }
                            else
                            {
                                if (_theme == "night")
                                {
                                    richTextBox1.ForeColor = Color.LimeGreen;
                                }
                                richTextBox1.Text = text.Result;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DialogResult res = MessageBox.Show(
        "Обнаружена ошибка неизвестного происхождения",
        "Ошибка",
        MessageBoxButtons.OK,
        MessageBoxIcon.Information,
        MessageBoxDefaultButton.Button1,
        MessageBoxOptions.DefaultDesktopOnly);
                if (res == DialogResult.OK)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Метод, вызываемый при простом сохранении(отличается от "Save as" в том случае, если изменения вносятся в уже существующий файл).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(_path))
                {
                    using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "TextFile(.txt)|*.txt|RtfFile(.rtf)|*.rtf", ValidateNames = true })
                    {
                        _path = sfd.FileName;
                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            using (StreamWriter sw = new StreamWriter(sfd.FileName))
                            {
                                sw.WriteLineAsync(richTextBox1.Rtf);
                            }
                        }
                    }
                }
                else
                {
                    using (StreamWriter sw = new StreamWriter(_path))
                    {
                        // Здесь важная проверка на формат файла(два разных аргумента передаются методу)
                        if (Path.GetExtension(_path).Equals(".rtf"))
                        {
                            sw.WriteLineAsync(richTextBox1.Rtf);
                        }
                        else
                        {
                            sw.WriteLineAsync(richTextBox1.Text);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DialogResult res = MessageBox.Show(
        "Обнаружена ошибка неизвестного происхождения",
        "Ошибка",
        MessageBoxButtons.OK,
        MessageBoxIcon.Information,
        MessageBoxDefaultButton.Button1,
        MessageBoxOptions.DefaultDesktopOnly);
                if (res == DialogResult.OK)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Метод, позволяющий сохранить файл в заданном формате(txt или rtf).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "TextFile(.txt)|*.txt|RtfFile(.rtf)|*.rtf", ValidateNames = true })
                {


                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        _path = sfd.FileName;
                        using (StreamWriter sw = new StreamWriter(sfd.FileName))
                        {
                            if (Path.GetExtension(_path).Equals(".rtf"))
                            {
                                sw.WriteLineAsync(richTextBox1.Rtf);
                            }
                            else
                            {
                                sw.WriteLineAsync(richTextBox1.Text);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DialogResult res = MessageBox.Show(
        "Обнаружена ошибка неизвестного происхождения",
        "Ошибка",
        MessageBoxButtons.OK,
        MessageBoxIcon.Information,
        MessageBoxDefaultButton.Button1,
        MessageBoxOptions.DefaultDesktopOnly);
                if (res == DialogResult.OK)
                {
                    return;
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Undo();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectedText = "";
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog();
            richTextBox1.SelectionFont = new Font(fontDialog1.Font.FontFamily, fontDialog1.Font.Size, fontDialog1.Font.Style);

        }

        private void redToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionBackColor = Color.Red;
        }

        private void blueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionBackColor = Color.Blue;
        }

        private void greenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionBackColor = Color.Green;
        }

        private void yellowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionBackColor = Color.Yellow;
        }

        private void grayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionBackColor = Color.Gray;
        }

        private void orangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionBackColor = Color.Orange;
        }

        /// <summary>
        /// Метод, устанавливающий тёмную тему.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nightThemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _theme = "night";
                BackColor = Color.DarkBlue;
                richTextBox1.BackColor = Color.Black;
                richTextBox1.ForeColor = Color.LimeGreen;
                menuStrip1.BackColor = Color.DarkBlue;
                menuStrip1.ForeColor = Color.LimeGreen;
                richTextBox1.SelectionColor = Color.LimeGreen;
            }
            catch (Exception ex)
            {
                DialogResult res = MessageBox.Show(
        "Обнаружена ошибка неизвестного происхождения",
        "Ошибка",
        MessageBoxButtons.OK,
        MessageBoxIcon.Information,
        MessageBoxDefaultButton.Button1,
        MessageBoxOptions.DefaultDesktopOnly);
                if (res == DialogResult.OK)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Метод, возвращающий светлую тему(по умолчанию).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dayThemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _theme = "default";
                BackColor = Color.White;
                richTextBox1.BackColor = Color.White;
                richTextBox1.ForeColor = Color.Black;
                menuStrip1.BackColor = Color.White;
                menuStrip1.ForeColor = Color.Black;
                richTextBox1.SelectionColor = Color.Black;
            }
            catch (Exception ex)
            {
                DialogResult res = MessageBox.Show(
        "Обнаружена ошибка неизвестного происхождения",
        "Ошибка",
        MessageBoxButtons.OK,
        MessageBoxIcon.Information,
        MessageBoxDefaultButton.Button1,
        MessageBoxOptions.DefaultDesktopOnly);
                if (res == DialogResult.OK)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Метод, вызывающий всплывающее окно при закрытии формы.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                DialogResult res = MessageBox.Show(
            "Сохранить файл перед выходом?",
            "Сообщение",
            MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Information,
            MessageBoxDefaultButton.Button1,
            MessageBoxOptions.DefaultDesktopOnly);
                if (res == DialogResult.No)
                {
                    Environment.Exit(0);
                }
                else if (res == DialogResult.Yes)
                {
                    saveAsToolStripMenuItem_Click(sender, e);
                    Environment.Exit(0);
                }
                else
                {
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                DialogResult res = MessageBox.Show(
        "Обнаружена ошибка неизвестного происхождения",
        "Ошибка",
        MessageBoxButtons.OK,
        MessageBoxIcon.Information,
        MessageBoxDefaultButton.Button1,
        MessageBoxOptions.DefaultDesktopOnly);
                if (res == DialogResult.OK)
                {
                    return;
                }
            }
        }
    }
}

