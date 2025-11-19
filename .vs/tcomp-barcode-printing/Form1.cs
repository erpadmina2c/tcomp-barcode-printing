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
using tcomp_barcode_printing.Models;
using tcomp_barcode_printing.Services;

namespace tcomp_barcode_printing
{
    public partial class Form1 : Form
    {
        public ISerialNumber iserialnumber;
        public string printerAddress = "";

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
                var data = await iserialnumber.GetSerialNumbersAsync(
                    txt_po_number.Text?.Trim() ?? "",
                    txt_serial_no.Text?.Trim() ?? ""
                );

                serialnumbers = new BindingList<ListSerialNumber>(data);
                serialNumberGrid.DataSource = serialnumbers;

                FormatSerialNumberGrid();
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
            serialnumbers.Clear();
        }

        // PRINT
        private void btn_print_Click(object sender, EventArgs e)
        {
            try
            {
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

                MessageBox.Show("Printed Successfully");

                txt_po_number.Text = "";
                txt_serial_no.Text = "";
                serialnumbers.Clear();
            }
            catch (Exception ex)
            {
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
    }
}
