using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using tcomp_barcode_printing.Methods;
using tcomp_barcode_printing.Models;
using tcomp_barcode_printing.Services;

namespace tcomp_barcode_printing
{
    public partial class Form1 : Form
    {
        public ISerialNumber iserialnumber;
        public string printerAddress = "";
        public ZPLTextWidth zPLTextWidth=new ZPLTextWidth();

        private BindingList<ListSerialNumber> serialnumbers = new BindingList<ListSerialNumber>();

        public Form1(ISerialNumber _iserialNumber)
        {
            InitializeComponent();
            lbl_copy_right.Text = "© " + DateTime.Now.Year.ToString() + " T-Comp. All rights reserved.";
     
            serialNumberGrid.AllowUserToAddRows = false;
            iserialnumber = _iserialNumber;

            // Selection behavior
            serialNumberGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            serialNumberGrid.MultiSelect = true;
            serialNumberGrid.ReadOnly = true;

            // Enable copy
            serialNumberGrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;

            serialNumberGrid.ClearSelection();

            // Enable column copy
            serialNumberGrid.ColumnHeaderMouseClick += (s, eArgs) =>
            {
                serialNumberGrid.ClearSelection();

                foreach (DataGridViewRow row in serialNumberGrid.Rows)
                {
                    row.Cells[eArgs.ColumnIndex].Selected = true;
                }
            };
        }

        // SEARCH
        private async void btn_search_Click(object sender, EventArgs e)
        {
            if (txt_po_number.Text?.Trim() != "" || txt_serial_no.Text?.Trim() != "")
            {
                try
                {
                    progressBar1.Visible = true;
                    var data = await iserialnumber.GetSerialNumbersAsync(
                    txt_po_number.Text?.Trim() ?? "",
                    txt_serial_no.Text?.Trim() ?? ""
                );

                    serialnumbers = new BindingList<ListSerialNumber>(data);
                    serialNumberGrid.DataSource = serialnumbers;

                    FormatSerialNumberGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    progressBar1.Visible = false;
                }
                finally
                {
                    // Hide progress bar
                    progressBar1.Visible = false;
                }
            }
            else
            {
                MessageBox.Show("Please enter PO Number or Serial Number to search.");
            }
        }

        // CLEAR
        private void btn_clear_Click(object sender, EventArgs e)
        {
            txt_po_number.Text = "";
            txt_serial_no.Text = "";
            txt_serial_no2.Text = "";
            serialnumbers.Clear();
        }

        // PRINT
        private void btn_print_Click(object sender, EventArgs e)
        {
            try
            {
                progressBar1.Visible = true;
                string FilePath = @"C:\PRINT\";

                if (!Directory.Exists(FilePath))
                    Directory.CreateDirectory(FilePath);

                File.WriteAllLines(@"C:\PRINT\print.txt", new string[] { "" });

                foreach (ListSerialNumber serialNumber in serialnumbers)
                {
                    try
                    {
                        string[] format = File.ReadAllLines(@"C:\PRINT\BarcodeFormat.txt");

                        for (int count = 0; count < format.Length; count++)
                        {
                            string cleanedSerial = Regex.Replace(serialNumber.serial_no ?? "", @"\s+", "");

                            format[count] = format[count].Replace("BARCODEPRINT", cleanedSerial);
                            format[count] = format[count].Replace("BARCODE", cleanedSerial);
                            format[count] = format[count].Replace("QUANTITY", "1");
                        }

                        File.AppendAllLines(@"C:\PRINT\print.txt", format);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error processing serial number: " + serialNumber.serial_no +
                                        "\n" + ex.Message);
                    }
                }

                string printerAddress = File.ReadAllText(@"C:\PRINT\printerAddress.txt");
                File.Copy(@"C:\PRINT\print.txt", printerAddress, true);

               

                txt_po_number.Text = "";
                txt_serial_no.Text = "";
                serialnumbers.Clear();
                progressBar1.Visible = false;
            }
            catch (Exception ex)
            {
                progressBar1.Visible = false;
                MessageBox.Show("Printing failed!\n\n" + ex.Message);
            }
        }

        // DELETE using button
        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (serialNumberGrid.SelectedRows.Count == 0)
                return;

            var confirm = MessageBox.Show(
                "Delete selected rows?",
                "Confirm Delete",
                MessageBoxButtons.YesNo
            );

            if (confirm != DialogResult.Yes)
                return;

            var indexes = serialNumberGrid.SelectedRows
                    .Cast<DataGridViewRow>()
                    .Select(r => r.Index)
                    .OrderByDescending(i => i);

            foreach (int i in indexes)
                serialnumbers.RemoveAt(i);
        }

        // DELETE using Delete key (delete row by selected cell)
        private void serialNumberGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (serialNumberGrid.SelectedCells.Count > 0)
                {
                    int rowIndex = serialNumberGrid.SelectedCells[0].RowIndex;
                    if (rowIndex >= 0 && rowIndex < serialnumbers.Count)
                        serialnumbers.RemoveAt(rowIndex);
                }
            }
        }

        private void FormatSerialNumberGrid()
        {
            if (serialNumberGrid.Columns.Count == 0) return;

            serialNumberGrid.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            serialNumberGrid.Columns["id"].Visible = false;
            serialNumberGrid.Columns["message"].Visible = false;
            serialNumberGrid.Columns["send_to_tcomp"].Visible = false;
            serialNumberGrid.Columns["cre_date"].Visible = false;

            serialNumberGrid.Columns["serial_no"].HeaderText = "Serial Number";
            serialNumberGrid.Columns["order_no"].HeaderText = "Order No";
            serialNumberGrid.Columns["original_no"].HeaderText = "Original No";
            serialNumberGrid.Columns["make"].HeaderText = "Make";
            serialNumberGrid.Columns["model"].HeaderText = "Model";
            serialNumberGrid.Columns["processor"].HeaderText = "Processor";
            serialNumberGrid.Columns["hard_disk"].HeaderText = "Hard Disk";
            serialNumberGrid.Columns["hard_disk_size"].HeaderText = "Hard Disk Size";
            serialNumberGrid.Columns["ram_type"].HeaderText = "RAM Type";
            serialNumberGrid.Columns["ram_size"].HeaderText = "RAM Size";
            serialNumberGrid.Columns["keyboard"].HeaderText = "Key Board";
            serialNumberGrid.Columns["lcd"].HeaderText = "LCD";

            serialNumberGrid.Columns["order_no"].DisplayIndex = 0;
            serialNumberGrid.Columns["serial_no"].DisplayIndex = 1;
            serialNumberGrid.Columns["original_no"].DisplayIndex = 2;
            serialNumberGrid.Columns["make"].DisplayIndex = 3;
            serialNumberGrid.Columns["model"].DisplayIndex = 4;
            serialNumberGrid.Columns["processor"].DisplayIndex = 5;

            serialNumberGrid.Columns["serial_no"].Width = 150;
            serialNumberGrid.Columns["original_no"].Width = 150;
            serialNumberGrid.Columns["make"].Width = 120;
            serialNumberGrid.Columns["model"].Width = 120;

            serialNumberGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void label4_Click(object sender, EventArgs e)
        {
           
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = tabControl1.SelectedIndex;

            if (index == 0)
            {
                txt_po_number.Text = "";
                txt_serial_no.Text = "";
                serialnumbers.Clear();
            }
            else if (index == 1)
            {
                txt_serial_no2.Text = "";
               
                serialnumbers.Clear();
            }
        }

        private void btn_print_2_Click(object sender, EventArgs e)
        {
            try
            {
       
                progressBar1.Visible = true;
                string FilePath = @"C:\PRINT\";

                if (!Directory.Exists(FilePath))
                    Directory.CreateDirectory(FilePath);

                File.WriteAllLines(@"C:\PRINT\print.txt", new string[] { "" });

                foreach (ListSerialNumber serialNumber in serialnumbers)
                {
                    try
                    {
                        string[] format = File.ReadAllLines(@"C:\PRINT\SpecificationTagFormat.txt");

                 
                        string cleanedSerial = Regex.Replace(serialNumber.serial_no ?? "", @"\s+", "");
                        string cleanedOriginalSerial = Regex.Replace(serialNumber.original_no ?? "", @"\s+", "");

                      
                        for (int count = 0; count < format.Length; count++)
                        {
                            format[count] = format[count].Replace("MAKE", serialNumber.make);
                            format[count] = format[count].Replace("MODEL", serialNumber.model);
                            format[count] = format[count].Replace("PROCESSOR", serialNumber.processor);
                            format[count] = format[count].Replace("HARDDISK", serialNumber.hard_disk_size);
                            format[count] = format[count].Replace("RAM", serialNumber.ram_size);
                            format[count] = format[count].Replace("KEYBOARD", serialNumber.keyboard);
                            format[count] = format[count].Replace("LCD", serialNumber.lcd);

                            format[count] = format[count].Replace("OGBARCODEPRINT", cleanedOriginalSerial);
                            format[count] = format[count].Replace("OGBARCODE", cleanedOriginalSerial);
                            format[count] = format[count].Replace("BARCODEPRINT", cleanedSerial);
                            format[count] = format[count].Replace("BARCODE", cleanedSerial);
                            format[count] = format[count].Replace("QUANTITY", "1");
                           
                        }

                        File.AppendAllLines(@"C:\PRINT\print.txt", format);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error processing serial number: " + serialNumber.serial_no +
                                        "\n" + ex.Message);
                    }
                }

                string printerAddress = File.ReadAllText(@"C:\PRINT\printerAddress2.txt");
                File.Copy(@"C:\PRINT\print.txt", printerAddress, true);


                txt_po_number.Text = "";
                txt_serial_no.Text = "";
                serialnumbers.Clear();
                txt_serial_no2.Clear();
                txt_serial_no2.Focus();
                progressBar1.Visible = false;
            }
            catch (Exception ex)
            {
                progressBar1.Visible = false;
                MessageBox.Show("Printing failed!\n\n" + ex.Message);
            }
        }

        private async void btn_search2_Click(object sender, EventArgs e)
        {
           

            try
            {
                progressBar1.Visible = true;

                // Run DB call in background thread
                var data = await Task.Run(() =>
                    iserialnumber.getSerialNumberForSpecificationTag(
                        txt_serial_no2.Text.Trim()
                    )
                );

                if (data != null && data.Count > 0)
                {
                    if (serialnumbers.Count == 0)
                    {
                        serialnumbers = new BindingList<ListSerialNumber>();
                        serialNumberGrid.DataSource = serialnumbers;
                    }

                    foreach (var item in data)
                    {
                        serialnumbers.Add(item);
                    }

                    FormatSerialNumberGrid();
                    txt_serial_no2.Clear();
                    txt_serial_no2.Focus();
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                progressBar1.Visible = false;
            }
        }

    }
}



