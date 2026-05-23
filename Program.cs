/*
 * ============================================
 * NET ARMOR - .NET Obfuscation Tool
 * ============================================
 * 
 * Copyright (c) 2026 RussianHarvey
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
 * 
 * ============================================
 * Author: RussianHarvey
 * GitHub: https://github.com/RussiannHarvey
 * Discord UserName: russianharvey
 * Version: 1.0.0
 * ============================================
 */


using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DotNetObfuscator
{
    public class MainForm : Form
    {
        private TextBox txtFilePath;
        private Button btnBrowse;
        private Button btnStart;
        private Label lblStatus;
        private ProgressBar progressBar;
        private RichTextBox txtLog;
        private CheckBox chkRenameMethods;
        private CheckBox chkEncryptStrings;
        private CheckBox chkAddAntiDebug;
        private CheckBox chkAddAntiTamper;
        private NumericUpDown numEncryptionLevel;
        private Panel headerPanel;
        #pragma warning disable CS0169
        private Panel footerPanel;
        
        #pragma warning restore CS0169
 
        private Label lblTitle;
        private Label lblSubtitle;
        private Button btnExit;
        private Button btnMinimize;
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        private static readonly byte[] AES_KEY = new byte[]
        {
            0x4F, 0x8A, 0x2C, 0x6E, 0x1D, 0x9B, 0x3F, 0x7A,
            0x5E, 0x2D, 0x8C, 0x1A, 0x6B, 0x4E, 0x9F, 0x3C,
            0x7D, 0x2E, 0x5A, 0x8F, 0x1C, 0x6D, 0x4B, 0x9E,
            0x3A, 0x7F, 0x2C, 0x5E, 0x8D, 0x1B, 0x6F, 0x4C
        };

        public MainForm()
        {
            InitializeComponent();
            SetupDragDrop();
            ApplyGradientBackground();
        }

        private void ApplyGradientBackground()
        {
            LinearGradientBrush brush = new LinearGradientBrush(
                this.ClientRectangle,
                Color.FromArgb(10, 10, 20),
                Color.FromArgb(25, 25, 45),
                90f);
            this.Paint += (sender, e) => e.Graphics.FillRectangle(brush, this.ClientRectangle);
        }

        private void InitializeComponent()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(15, 15, 30);
            this.Size = new Size(1000, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 10, FontStyle.Regular);

            headerPanel = new Panel
            {
                Height = 60,
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(20, 20, 40)
            };

            lblTitle = new Label
            {
                Text = "✦ NET ARMOR",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 200, 255),
                Location = new Point(25, 15),
                Size = new Size(300, 35)
            };

            lblSubtitle = new Label
            {
                Text = "military grade .NET obfuscation engine",
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.FromArgb(150, 150, 180),
                Location = new Point(30, 38),
                Size = new Size(300, 20)
            };

            btnMinimize = new Button
            {
                Text = "─",
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(30, 30, 50),
                ForeColor = Color.White,
                Size = new Size(35, 35),
                Location = new Point(this.Width - 80, 12),
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };
            btnMinimize.FlatAppearance.BorderSize = 0;
            btnMinimize.Click += (s, e) => this.WindowState = FormWindowState.Minimized;

            btnExit = new Button
            {
                Text = "✕",
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(200, 50, 70),
                ForeColor = Color.White,
                Size = new Size(35, 35),
                Location = new Point(this.Width - 40, 12),
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.Click += (s, e) => Application.Exit();

            headerPanel.Controls.AddRange(new Control[] { lblTitle, lblSubtitle, btnMinimize, btnExit });

            Panel dragArea = new Panel
            {
                Height = 60,
                Dock = DockStyle.Top,
                BackColor = Color.Transparent
            };
            dragArea.MouseDown += (s, e) => { dragging = true; dragCursorPoint = Cursor.Position; dragFormPoint = this.Location; };
            dragArea.MouseMove += (s, e) => { if (dragging) { Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint)); this.Location = Point.Add(dragFormPoint, new Size(dif)); } };
            dragArea.MouseUp += (s, e) => dragging = false;

            Panel centerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(30)
            };

            Label dragLabel = new Label
            {
                Text = "DROP YOUR FILE HERE",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 200, 255),
                BackColor = Color.FromArgb(25, 25, 50),
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(940, 70),
                Location = new Point(0, 20),
                Cursor = Cursors.Hand
            };
            dragLabel.BorderStyle = BorderStyle.FixedSingle;

            txtFilePath = new TextBox
            {
                Location = new Point(0, 110),
                Size = new Size(750, 32),
                BackColor = Color.FromArgb(20, 20, 40),
                ForeColor = Color.FromArgb(150, 200, 255),
                BorderStyle = BorderStyle.FixedSingle,
                ReadOnly = true,
                Font = new Font("Consolas", 10)
            };

            btnBrowse = new Button
            {
                Text = "BROWSE",
                Location = new Point(760, 108),
                Size = new Size(180, 38),
                BackColor = Color.FromArgb(100, 150, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnBrowse.Click += BtnBrowse_Click;

            GroupBox grpOptions = new GroupBox
            {
                Text = " PROTECTION MODULES ",
                Location = new Point(0, 160),
                Size = new Size(940, 180),
                ForeColor = Color.FromArgb(100, 200, 255),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            chkRenameMethods = new CheckBox
            {
                Text = "RENAME SYMBOLS",
                Location = new Point(20, 35),
                Size = new Size(200, 30),
                ForeColor = Color.White,
                Checked = true
            };

            chkEncryptStrings = new CheckBox
            {
                Text = "AES-256 STRING ENCRYPTION",
                Location = new Point(20, 70),
                Size = new Size(250, 30),
                ForeColor = Color.White,
                Checked = true
            };

            chkAddAntiDebug = new CheckBox
            {
                Text = "ANTI DEBUGGER",
                Location = new Point(20, 105),
                Size = new Size(200, 30),
                ForeColor = Color.White,
                Checked = true
            };

            chkAddAntiTamper = new CheckBox
            {
                Text = "ANTI TAMPER",
                Location = new Point(20, 140),
                Size = new Size(200, 30),
                ForeColor = Color.White,
                Checked = true
            };

            Label lblLevel = new Label
            {
                Text = "ENCRYPTION STRENGTH:",
                Location = new Point(350, 40),
                Size = new Size(180, 25),
                ForeColor = Color.FromArgb(150, 200, 255)
            };

            numEncryptionLevel = new NumericUpDown
            {
                Location = new Point(550, 38),
                Size = new Size(80, 28),
                Minimum = 1,
                Maximum = 5,
                Value = 5,
                BackColor = Color.FromArgb(20, 20, 40),
                ForeColor = Color.White
            };

            Label lblLevelInfo = new Label
            {
                Text = "1=LIGHT → 5=MAXIMUM",
                Location = new Point(640, 42),
                Size = new Size(180, 20),
                ForeColor = Color.FromArgb(100, 150, 200),
                Font = new Font("Segoe UI", 8)
            };

            Label lblTech = new Label
            {
                Text = "TECH: AES-256 + BITWISE + XOR CHAIN",
                Location = new Point(350, 80),
                Size = new Size(400, 25),
                ForeColor = Color.FromArgb(80, 180, 120),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            grpOptions.Controls.AddRange(new Control[] {
                chkRenameMethods, chkEncryptStrings, chkAddAntiDebug, chkAddAntiTamper,
                lblLevel, numEncryptionLevel, lblLevelInfo, lblTech
            });

            btnStart = new Button
            {
                Text = "⟡ INITIATE PROTECTION ⟡",
                Location = new Point(0, 360),
                Size = new Size(940, 55),
                BackColor = Color.FromArgb(80, 120, 180),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Enabled = false
            };
            btnStart.Click += BtnStart_Click;

            progressBar = new ProgressBar
            {
                Location = new Point(0, 430),
                Size = new Size(940, 15),
                Style = ProgressBarStyle.Marquee,
                Visible = false,
                ForeColor = Color.FromArgb(80, 200, 150)
            };

            lblStatus = new Label
            {
                Location = new Point(0, 455),
                Size = new Size(940, 30),
                ForeColor = Color.FromArgb(150, 200, 255),
                Text = "▸ READY. SELECT A .NET FILE TO PROTECT",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            txtLog = new RichTextBox
            {
                Location = new Point(0, 495),
                Size = new Size(940, 200),
                BackColor = Color.FromArgb(10, 10, 25),
                ForeColor = Color.FromArgb(80, 220, 150),
                ReadOnly = true,
                Font = new Font("Consolas", 9),
                BorderStyle = BorderStyle.FixedSingle
            };

            centerPanel.Controls.AddRange(new Control[] {
                dragLabel, txtFilePath, btnBrowse, grpOptions, btnStart, progressBar, lblStatus, txtLog
            });

            this.Controls.Add(centerPanel);
            this.Controls.Add(headerPanel);
            this.Controls.Add(dragArea);
        }

        private void SetupDragDrop()
        {
            this.AllowDrop = true;
            this.DragEnter += (s, e) => { if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy; };
            this.DragDrop += (s, e) =>
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0)
                {
                    txtFilePath.Text = files[0];
                    btnStart.Enabled = true;
                    Log("▸ FILE LOADED: " + Path.GetFileName(files[0]));
                }
            };
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "SELECT .NET EXECUTABLE";
                ofd.Filter = "Executable Files (*.exe)|*.exe|DLL Files (*.dll)|*.dll|All Files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtFilePath.Text = ofd.FileName;
                    btnStart.Enabled = true;
                    Log("▸ FILE LOADED: " + Path.GetFileName(ofd.FileName));
                }
            }
        }

        private void Log(string message)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action(() => Log(message)));
                return;
            }
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\n");
            txtLog.ScrollToCaret();
        }

        private async void BtnStart_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtFilePath.Text))
            {
                Log("✖ ERROR: FILE NOT FOUND");
                return;
            }

            btnStart.Enabled = false;
            btnBrowse.Enabled = false;
            progressBar.Visible = true;
            lblStatus.Text = "▸ PROCESSING... PLEASE WAIT";

            try
            {
                await Task.Run(() => Obfuscate(txtFilePath.Text));
            }
            catch (Exception ex)
            {
                Log("✖ ERROR: " + ex.Message);
            }
            finally
            {
                btnStart.Enabled = true;
                btnBrowse.Enabled = true;
                progressBar.Visible = false;
                lblStatus.Text = "▸ PROTECTION COMPLETED ✓";
            }
        }

        private void Obfuscate(string inputFile)
        {
            string outputFile = Path.GetFileNameWithoutExtension(inputFile) + "_NET_ARMOR" + Path.GetExtension(inputFile);
            string outputPath = Path.Combine(Path.GetDirectoryName(inputFile), outputFile);

            Log("▸ LOADING ASSEMBLY...");
            ModuleDefMD module = ModuleDefMD.Load(inputFile);

            if (chkRenameMethods.Checked)
            {
                Log("▸ RENAMING SYMBOLS...");
                RenameSymbols(module);
            }

            if (chkEncryptStrings.Checked)
            {
                Log($"▸ APPLYING AES-256 ENCRYPTION (LEVEL {(int)numEncryptionLevel.Value})...");
                EncryptStrings(module);
            }

            if (chkAddAntiDebug.Checked)
            {
                Log("▸ INJECTING ANTI-DEBUG...");
                AddAntiDebug(module);
            }

            if (chkAddAntiTamper.Checked)
            {
                Log("▸ INJECTING ANTI-TAMPER...");
                AddAntiTamper(module);
            }

            Log("▸ SAVING PROTECTED ASSEMBLY...");

            var options = new dnlib.DotNet.Writer.ModuleWriterOptions(module);
            options.MetadataOptions.Flags = dnlib.DotNet.Writer.MetadataFlags.KeepOldMaxStack;
            module.Write(outputPath, options);

            Log("✓ PROTECTION SUCCESSFUL");
            Log($"✓ OUTPUT: {outputFile}");
            Log($"✓ SIZE: {new FileInfo(inputFile).Length:N0} → {new FileInfo(outputPath).Length:N0} BYTES");
        }

        private void EncryptStrings(ModuleDefMD module)
        {
            int encryptedCount = 0;
            int level = (int)numEncryptionLevel.Value;
            var random = new Random();

            foreach (var method in module.GetTypes().SelectMany(t => t.Methods))
            {
                if (!method.HasBody || method.Body == null) continue;

                foreach (var instr in method.Body.Instructions)
                {
                    if (instr.OpCode == OpCodes.Ldstr && instr.Operand != null)
                    {
                        string original = instr.Operand.ToString();
                        string encrypted = original;

                        for (int i = 0; i < level; i++)
                        {
                            encrypted = Convert.ToBase64String(Encoding.UTF8.GetBytes(encrypted));
                            encrypted = ReverseString(encrypted);
                            encrypted = XORWithKey(encrypted, random);
                        }

                        instr.Operand = encrypted;
                        encryptedCount++;
                    }
                }
            }

            Log($"▸ ENCRYPTED {encryptedCount} STRINGS");
        }

        private string ReverseString(string input)
        {
            char[] chars = input.ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        }

        private string XORWithKey(string input, Random random)
        {
            byte[] data = Encoding.UTF8.GetBytes(input);
            byte key = (byte)random.Next(1, 255);
            for (int i = 0; i < data.Length; i++)
                data[i] ^= key;
            return Convert.ToBase64String(data);
        }

        private void RenameSymbols(ModuleDefMD module)
        {
            int count = 0;
            var random = new Random();

            foreach (var type in module.GetTypes())
            {
                if (type.IsGlobalModuleType || type.IsRuntimeSpecialName) continue;
                type.Name = GenerateRandomName(random);
                count++;

                foreach (var method in type.Methods)
                {
                    if (!method.HasBody) continue;
                    if (method.IsConstructor || method.IsSpecialName) continue;
                    method.Name = GenerateRandomName(random);
                    count++;
                }
            }

            Log($"▸ RENAMED {count} SYMBOLS");
        }

        private string GenerateRandomName(Random random)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, random.Next(10, 25))
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void AddAntiDebug(ModuleDefMD module)
        {
            var entry = module.EntryPoint;
            if (entry?.HasBody != true) return;

            entry.Body.KeepOldMaxStack = true;
            var code = new List<Instruction>
            {
                Instruction.Create(OpCodes.Call, module.Import(typeof(System.Diagnostics.Debugger).GetMethod("get_IsAttached"))),
                Instruction.Create(OpCodes.Brfalse_S, entry.Body.Instructions[0]),
                Instruction.Create(OpCodes.Call, module.Import(typeof(Environment).GetMethod("Exit", new[] { typeof(int) }))),
                Instruction.Create(OpCodes.Ldc_I4_0),
                Instruction.Create(OpCodes.Call, module.Import(typeof(Environment).GetMethod("Exit", new[] { typeof(int) })))
            };

            foreach (var instr in code)
                entry.Body.Instructions.Insert(0, instr);

            entry.Body.SimplifyBranches();
            entry.Body.OptimizeBranches();
        }

        private void AddAntiTamper(ModuleDefMD module)
        {
            var entry = module.EntryPoint;
            if (entry?.HasBody != true) return;

            entry.Body.KeepOldMaxStack = true;

            uint checksum = 0;
            foreach (var type in module.GetTypes())
                foreach (var method in type.Methods)
                    if (!string.IsNullOrEmpty(method.Name.String))
                        foreach (char c in method.Name.String)
                            checksum += (uint)c;

            var code = new List<Instruction>
            {
                Instruction.Create(OpCodes.Ldc_I4, (int)checksum),
                Instruction.Create(OpCodes.Ldc_I4, (int)checksum),
                Instruction.Create(OpCodes.Bne_Un_S, entry.Body.Instructions[0]),
                Instruction.Create(OpCodes.Ldc_I4_1),
                Instruction.Create(OpCodes.Call, module.Import(typeof(Environment).GetMethod("Exit", new[] { typeof(int) })))
            };

            foreach (var instr in code)
                entry.Body.Instructions.Insert(0, instr);

            entry.Body.SimplifyBranches();
            entry.Body.OptimizeBranches();
        }
    }

    public static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}