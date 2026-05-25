/*
 * ============================================
 * NET ARMOR - .NET Obfuscation Tool (Enhanced)
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
 * Version: 2.0.0 (Enhanced Protection - Optimized)
 * ============================================
 */
#nullable disable
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Cryptography;

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
        private Panel panelTop;
        private Panel panelBottom;
        private Label lblVersion;
        private MenuStrip menuStrip;
        private ToolStripMenuItem fileMenu;
        private ToolStripMenuItem exitMenuItem;
        private ToolStripMenuItem aboutMenuItem;
        private ToolStripMenuItem helpMenuItem;
        private CheckBox chkControlFlowObfuscation;
        private CheckBox chkAntiDe4dot;
        private CheckBox chkIntegrityCheck;
        private CheckBox chkAntiDnSpy;
        private CheckBox chkResourceEncryption;
        private CheckBox chkJunkCode;
        private CheckBox chkAntiMemoryDump;
        private CheckBox chkAntiVirtualMachine;
        private CheckBox chkMetadataStripping;
        private CheckBox chkProxyMixing;
        private NumericUpDown numJunkLevel;

        public MainForm()
        {
            InitializeComponent();
            SetupDragDrop();
        }

        private void InitializeComponent()
        {
            this.FormBorderStyle = FormBorderStyle.Sizable;  // تغيير ليصبح قابل للتحجيم
            this.MaximizeBox = true;  // تفعيل زر التكبير
            this.MinimumSize = new Size(950, 700);  // حجم أدنى أكبر
            this.BackColor = Color.FromArgb(45, 45, 48);
            this.Size = new Size(1100, 650);  
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            this.Text = "NET Armor - .NET Protection Tool (Enhanced)";

            menuStrip = new MenuStrip();
            fileMenu = new ToolStripMenuItem("File");
            exitMenuItem = new ToolStripMenuItem("Exit");
            exitMenuItem.Click += (s, e) => Application.Exit();
            fileMenu.DropDownItems.Add(exitMenuItem);
            
            helpMenuItem = new ToolStripMenuItem("Help");
            aboutMenuItem = new ToolStripMenuItem("About");
            aboutMenuItem.Click += (s, e) => MessageBox.Show("NET Armor v2.0.0 Enhanced\n.NET Application Protection Suite\n\nEnhanced Features:\n- Control Flow Obfuscation\n- Anti-De4dot Protection\n- Integrity Checks\n- Anti-DnSpy\n- Resource Encryption\n- Junk Code Injection\n- Anti-Memory Dump\n- Anti-VM Detection\n- Metadata Stripping\n- Proxy Mixing\n\n© 2026 RussianHarvey", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
            helpMenuItem.DropDownItems.Add(aboutMenuItem);
            
            menuStrip.Items.Add(fileMenu);
            menuStrip.Items.Add(helpMenuItem);
            menuStrip.BackColor = Color.FromArgb(60, 60, 65);
            menuStrip.ForeColor = Color.White;
            menuStrip.Renderer = new ToolStripProfessionalRenderer(new CustomProfessionalColors());

            panelTop = new Panel()
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(37, 37, 40),
                Padding = new Padding(15, 10, 15, 10)
            };

            Label lblTitle = new Label()
            {
                Text = "NET Armor",
                Font = new Font("Segoe UI", 20, FontStyle.Regular),
                ForeColor = Color.FromArgb(0, 122, 204),
                Location = new Point(15, 15),
                AutoSize = true
            };

            Label lblSubtitle = new Label()
            {
                Text = ".NET Application Protection Suite (Enhanced Edition)",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(160, 160, 165),
                Location = new Point(18, 50),
                AutoSize = true
            };

            lblVersion = new Label()
            {
                Text = "v2.0.0",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.FromArgb(110, 110, 115),
                Location = new Point(this.Width - 80, 25),
                AutoSize = true,
                TextAlign = ContentAlignment.TopRight
            };

            panelTop.Controls.AddRange(new Control[] { lblTitle, lblSubtitle, lblVersion });

            Panel mainPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                BackColor = Color.FromArgb(45, 45, 48),
                AutoScroll = true
            };

            int yOffset = 10;

            GroupBox grpInput = new GroupBox()
            {
                Text = " Input Assembly ",
                Location = new Point(0, yOffset),
                Size = new Size(1050, 90),  // زيادة العرض
                ForeColor = Color.FromArgb(210, 210, 215),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            txtFilePath = new TextBox()
            {
                Location = new Point(15, 30),
                Size = new Size(870, 27),  // زيادة العرض
                BackColor = Color.FromArgb(30, 30, 35),
                ForeColor = Color.FromArgb(210, 210, 215),
                BorderStyle = BorderStyle.FixedSingle,
                ReadOnly = true,
                Font = new Font("Consolas", 9)
            };

            btnBrowse = new Button()
            {
                Text = "Browse...",
                Location = new Point(900, 28),
                Size = new Size(130, 30),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                UseVisualStyleBackColor = false,
                Cursor = Cursors.Hand
            };
            btnBrowse.FlatAppearance.BorderSize = 0;
            btnBrowse.Click += BtnBrowse_Click;

            Label lblHint = new Label()
            {
                Text = "Supports .NET Framework / .NET Core executables and libraries | Drag & Drop supported",
                Location = new Point(15, 65),
                Size = new Size(550, 20),
                ForeColor = Color.FromArgb(110, 110, 115),
                Font = new Font("Segoe UI", 8, FontStyle.Italic)
            };

            grpInput.Controls.AddRange(new Control[] { txtFilePath, btnBrowse, lblHint });

            yOffset += 100;

            GroupBox grpOptions = new GroupBox()
            {
                Text = " Basic Protection Options ",
                Location = new Point(0, yOffset),
                Size = new Size(1050, 130),
                ForeColor = Color.FromArgb(210, 210, 215),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            chkRenameMethods = new CheckBox()
            {
                Text = "Symbol Renaming",
                Location = new Point(20, 30),
                Size = new Size(200, 25),
                ForeColor = Color.FromArgb(210, 210, 215),
                Checked = true,
                FlatStyle = FlatStyle.Flat
            };

            chkEncryptStrings = new CheckBox()
            {
                Text = "String Encryption",
                Location = new Point(20, 60),
                Size = new Size(200, 25),
                ForeColor = Color.FromArgb(210, 210, 215),
                Checked = true,
                FlatStyle = FlatStyle.Flat
            };

            chkAddAntiDebug = new CheckBox()
            {
                Text = "Anti-Debug",
                Location = new Point(20, 90),
                Size = new Size(200, 25),
                ForeColor = Color.FromArgb(210, 210, 215),
                Checked = true,
                FlatStyle = FlatStyle.Flat
            };

            chkAddAntiTamper = new CheckBox()
            {
                Text = "Anti-Tamper",
                Location = new Point(260, 30),
                Size = new Size(200, 25),
                ForeColor = Color.FromArgb(210, 210, 215),
                Checked = true,
                FlatStyle = FlatStyle.Flat
            };

            Label lblStrength = new Label()
            {
                Text = "Encryption Strength:",
                Location = new Point(260, 65),
                Size = new Size(140, 25),
                ForeColor = Color.FromArgb(210, 210, 215)
            };

            numEncryptionLevel = new NumericUpDown()
            {
                Location = new Point(410, 63),
                Size = new Size(70, 27),
                Minimum = 1,
                Maximum = 10,
                Value = 3,
                BackColor = Color.FromArgb(30, 30, 35),
                ForeColor = Color.FromArgb(210, 210, 215)
            };

            Label lblStrengthDesc = new Label()
            {
                Text = "(1-10 | Higher = Stronger)",
                Location = new Point(490, 67),
                Size = new Size(160, 20),
                ForeColor = Color.FromArgb(110, 110, 115),
                Font = new Font("Segoe UI", 8, FontStyle.Regular)
            };

            grpOptions.Controls.AddRange(new Control[] {
                chkRenameMethods, chkEncryptStrings, chkAddAntiDebug, chkAddAntiTamper,
                lblStrength, numEncryptionLevel, lblStrengthDesc
            });

            yOffset += 140;

            GroupBox grpAdvanced = new GroupBox()
            {
                Text = " Advanced Protection Options ",
                Location = new Point(0, yOffset),
                Size = new Size(1050, 200),
                ForeColor = Color.FromArgb(210, 210, 215),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            chkControlFlowObfuscation = new CheckBox()
            {
                Text = "Control Flow Obfuscation",
                Location = new Point(20, 30),
                Size = new Size(210, 25),
                ForeColor = Color.FromArgb(210, 210, 215),
                Checked = true,
                FlatStyle = FlatStyle.Flat
            };

            chkAntiDe4dot = new CheckBox()
            {
                Text = "Anti-De4dot Protection",
                Location = new Point(20, 60),
                Size = new Size(210, 25),
                ForeColor = Color.FromArgb(210, 210, 215),
                Checked = true,
                FlatStyle = FlatStyle.Flat
            };

            chkIntegrityCheck = new CheckBox()
            {
                Text = "Integrity Check (CRC32)",
                Location = new Point(20, 90),
                Size = new Size(210, 25),
                ForeColor = Color.FromArgb(210, 210, 215),
                Checked = true,
                FlatStyle = FlatStyle.Flat
            };

            chkAntiDnSpy = new CheckBox()
            {
                Text = "Anti-DnSpy Protection",
                Location = new Point(20, 120),
                Size = new Size(210, 25),
                ForeColor = Color.FromArgb(210, 210, 215),
                Checked = true,
                FlatStyle = FlatStyle.Flat
            };

            chkResourceEncryption = new CheckBox()
            {
                Text = "Resource Encryption",
                Location = new Point(270, 30),
                Size = new Size(200, 25),
                ForeColor = Color.FromArgb(210, 210, 215),
                Checked = true,
                FlatStyle = FlatStyle.Flat
            };

            chkJunkCode = new CheckBox()
            {
                Text = "Junk Code Injection",
                Location = new Point(270, 60),
                Size = new Size(200, 25),
                ForeColor = Color.FromArgb(210, 210, 215),
                Checked = true,
                FlatStyle = FlatStyle.Flat
            };

            chkAntiMemoryDump = new CheckBox()
            {
                Text = "Anti-Memory Dump",
                Location = new Point(270, 90),
                Size = new Size(200, 25),
                ForeColor = Color.FromArgb(210, 210, 215),
                Checked = true,
                FlatStyle = FlatStyle.Flat
            };

            chkAntiVirtualMachine = new CheckBox()
            {
                Text = "Anti-Virtual Machine",
                Location = new Point(270, 120),
                Size = new Size(200, 25),
                ForeColor = Color.FromArgb(210, 210, 215),
                Checked = true,
                FlatStyle = FlatStyle.Flat
            };

            chkMetadataStripping = new CheckBox()
            {
                Text = "Metadata Stripping",
                Location = new Point(520, 30),
                Size = new Size(200, 25),
                ForeColor = Color.FromArgb(210, 210, 215),
                Checked = true,
                FlatStyle = FlatStyle.Flat
            };

            chkProxyMixing = new CheckBox()
            {
                Text = "Proxy Mixing",
                Location = new Point(520, 60),
                Size = new Size(200, 25),
                ForeColor = Color.FromArgb(210, 210, 215),
                Checked = true,
                FlatStyle = FlatStyle.Flat
            };

            Label lblJunkLevel = new Label()
            {
                Text = "Junk Code Level:",
                Location = new Point(520, 95),
                Size = new Size(120, 25),
                ForeColor = Color.FromArgb(210, 210, 215)
            };

            numJunkLevel = new NumericUpDown()
            {
                Location = new Point(650, 93),
                Size = new Size(70, 27),
                Minimum = 1,
                Maximum = 5,
                Value = 2,
                BackColor = Color.FromArgb(30, 30, 35),
                ForeColor = Color.FromArgb(210, 210, 215)
            };

            Label lblJunkDesc = new Label()
            {
                Text = "(1-5 | Lower = Smaller size)",
                Location = new Point(730, 97),
                Size = new Size(160, 20),
                ForeColor = Color.FromArgb(110, 110, 115),
                Font = new Font("Segoe UI", 8, FontStyle.Regular)
            };

            grpAdvanced.Controls.AddRange(new Control[] {
                chkControlFlowObfuscation, chkAntiDe4dot, chkIntegrityCheck, chkAntiDnSpy,
                chkResourceEncryption, chkJunkCode, chkAntiMemoryDump, chkAntiVirtualMachine,
                chkMetadataStripping, chkProxyMixing, lblJunkLevel, numJunkLevel, lblJunkDesc
            });

            yOffset += 210;

            btnStart = new Button()
            {
                Text = "Protect Assembly (Enhanced)",
                Location = new Point(0, yOffset),
                Size = new Size(1050, 45),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Enabled = false,
                Cursor = Cursors.Hand
            };
            btnStart.FlatAppearance.BorderSize = 0;
            btnStart.Click += BtnStart_Click;

            yOffset += 55;

            progressBar = new ProgressBar()
            {
                Location = new Point(0, yOffset),
                Size = new Size(1050, 10),
                Style = ProgressBarStyle.Marquee,
                Visible = false,
                ForeColor = Color.FromArgb(0, 122, 204)
            };

            yOffset += 20;

            lblStatus = new Label()
            {
                Location = new Point(0, yOffset),
                Size = new Size(1050, 25),
                ForeColor = Color.FromArgb(160, 160, 165),
                Text = "Ready. Select a .NET assembly to protect (Enhanced Mode Active).",
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9, FontStyle.Regular)
            };

            yOffset += 35;

            Label lblLogTitle = new Label()
            {
                Text = "Output Log",
                Location = new Point(0, yOffset),
                Size = new Size(1050, 20),
                ForeColor = Color.FromArgb(160, 160, 165),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            yOffset += 25;

            txtLog = new RichTextBox()
            {
                Location = new Point(0, yOffset),
                Size = new Size(1050, 150),
                BackColor = Color.FromArgb(30, 30, 35),
                ForeColor = Color.FromArgb(80, 200, 120),
                ReadOnly = true,
                Font = new Font("Consolas", 8),
                BorderStyle = BorderStyle.FixedSingle
            };

            mainPanel.Controls.AddRange(new Control[] {
                grpInput, grpOptions, grpAdvanced, btnStart, progressBar, lblStatus, lblLogTitle, txtLog
            });

            panelBottom = new Panel()
            {
                Dock = DockStyle.Bottom,
                Height = 30,
                BackColor = Color.FromArgb(37, 37, 40)
            };

            Label lblFooter = new Label()
            {
                Text = "© 2026 RussianHarvey | Enhanced Protection Edition | Maximum Security",
                ForeColor = Color.FromArgb(110, 110, 115),
                Font = new Font("Segoe UI", 8, FontStyle.Regular),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            panelBottom.Controls.Add(lblFooter);

            this.Controls.Add(mainPanel);
            this.Controls.Add(panelBottom);
            this.Controls.Add(panelTop);
            this.Controls.Add(menuStrip);
        }

        private void SetupDragDrop()
        {
            this.AllowDrop = true;
            this.DragEnter += (s, e) => 
            {
                if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop)) 
                    e.Effect = DragDropEffects.Copy;
            };
            this.DragDrop += (s, e) =>
            {
                if (e.Data != null)
                {
                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    if (files != null && files.Length > 0)
                    {
                        txtFilePath.Text = files[0];
                        btnStart.Enabled = true;
                        Log($"[INFO] Loaded: {Path.GetFileName(files[0])}");
                    }
                }
            };
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select .NET Assembly";
                ofd.Filter = "Executable Files (*.exe)|*.exe|Dynamic Libraries (*.dll)|*.dll|All Files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtFilePath.Text = ofd.FileName;
                    btnStart.Enabled = true;
                    Log($"[INFO] Loaded: {Path.GetFileName(ofd.FileName)}");
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
                Log("[ERROR] File not found");
                return;
            }

            btnStart.Enabled = false;
            btnBrowse.Enabled = false;
            progressBar.Visible = true;
            lblStatus.Text = "Processing with Enhanced Protection...";

            try
            {
                await Task.Run(() => EnhancedObfuscate(txtFilePath.Text));
            }
            catch (Exception ex)
            {
                Log($"[ERROR] {ex.Message}");
            }
            finally
            {
                btnStart.Enabled = true;
                btnBrowse.Enabled = true;
                progressBar.Visible = false;
                lblStatus.Text = "Ready";
            }
        }

        private void EnhancedObfuscate(string inputFile)
        {
            string outputFile = Path.GetFileNameWithoutExtension(inputFile) + "_NET_Armor" + Path.GetExtension(inputFile);
            string outputPath = Path.Combine(Path.GetDirectoryName(inputFile) ?? "", outputFile);
            string backupPath = Path.Combine(Path.GetDirectoryName(inputFile) ?? "", Path.GetFileNameWithoutExtension(inputFile) + "_backup" + Path.GetExtension(inputFile));

            File.Copy(inputFile, backupPath, true);
            Log("[INFO] Original backup created");

            Log("[INFO] Loading assembly with enhanced protection...");
            ModuleDefMD module = ModuleDefMD.Load(inputFile);

            if (chkMetadataStripping.Checked)
            {
                Log("[INFO] Applying metadata stripping...");
                StripMetadata(module);
            }

            if (chkRenameMethods.Checked)
            {
                Log("[INFO] Applying enhanced symbol renaming...");
                EnhancedRenameSymbols(module);
            }

            if (chkControlFlowObfuscation.Checked)
            {
                Log("[INFO] Applying control flow obfuscation...");
                ObfuscateControlFlow(module);
            }

            if (chkProxyMixing.Checked)
            {
                Log("[INFO] Applying proxy mixing...");
                AddProxyMixing(module);
            }

            if (chkEncryptStrings.Checked)
            {
                Log($"[INFO] Applying enhanced string encryption (level {numEncryptionLevel.Value})...");
                EnhancedEncryptStrings(module);
            }

            if (chkResourceEncryption.Checked)
            {
                Log("[INFO] Encrypting resources...");
                EncryptResources(module);
            }

            if (chkJunkCode.Checked)
            {
                Log($"[INFO] Injecting junk code (level {numJunkLevel.Value})...");
                InjectJunkCode(module);
            }

            if (chkAddAntiDebug.Checked)
            {
                Log("[INFO] Injecting enhanced anti-debug routines...");
                EnhancedAddAntiDebug(module);
            }

            if (chkAntiMemoryDump.Checked)
            {
                Log("[INFO] Injecting anti-memory dump protection...");
                AddAntiMemoryDump(module);
            }

            if (chkAntiVirtualMachine.Checked)
            {
                Log("[INFO] Injecting anti-virtual machine detection...");
                AddAntiVirtualMachine(module);
            }

            if (chkAntiDe4dot.Checked)
            {
                Log("[INFO] Injecting anti-de4dot protection...");
                AddAntiDe4dot(module);
            }

            if (chkAntiDnSpy.Checked)
            {
                Log("[INFO] Injecting anti-DnSpy protection...");
                AddAntiDnSpy(module);
            }

            if (chkAddAntiTamper.Checked)
            {
                Log("[INFO] Injecting enhanced anti-tamper routines...");
                EnhancedAddAntiTamper(module);
            }

            if (chkIntegrityCheck.Checked)
            {
                Log("[INFO] Adding integrity checks...");
                AddIntegrityCheck(module);
            }

            Log("[INFO] Fixing branches before saving...");
            foreach (var type in module.GetTypes())
            {
                foreach (var method in type.Methods)
                {
                    if (method.HasBody && method.Body != null)
                    {
                        method.Body.SimplifyBranches();
                        method.Body.OptimizeBranches();
                    }
                }
            }

            Log("[INFO] Saving protected assembly with enhanced protection...");

            var options = new dnlib.DotNet.Writer.ModuleWriterOptions(module);
            options.MetadataOptions.Flags = dnlib.DotNet.Writer.MetadataFlags.KeepOldMaxStack;
            module.Write(outputPath, options);

            long originalSize = new FileInfo(inputFile).Length;
            long newSize = new FileInfo(outputPath).Length;
            float ratio = (float)newSize / originalSize;

            Log("[SUCCESS] Enhanced protection completed successfully!");
            Log($"[SUCCESS] Output: {outputFile}");
            Log($"[SUCCESS] Backup saved: {Path.GetFileName(backupPath)}");
            Log($"[INFO] Size: {originalSize:N0} → {newSize:N0} bytes ({(ratio * 100):F1}%)");
            Log("[WARNING] Keep your backup file safe!");
        }

        private void StripMetadata(ModuleDefMD module)
        {
            var attributesToRemove = new List<CustomAttribute>();
            
            foreach (var type in module.GetTypes())
            {
                foreach (var attr in type.CustomAttributes)
                {
                    if (attr.TypeFullName != null && (attr.TypeFullName.Contains("Debuggable") ||
                        attr.TypeFullName.Contains("AssemblyCompany") ||
                        attr.TypeFullName.Contains("AssemblyDescription") ||
                        attr.TypeFullName.Contains("AssemblyTitle") ||
                        attr.TypeFullName.Contains("AssemblyCopyright") ||
                        attr.TypeFullName.Contains("AssemblyVersion")))
                    {
                        attributesToRemove.Add(attr);
                    }
                }
                
                foreach (var attr in attributesToRemove)
                    type.CustomAttributes.Remove(attr);
                
                attributesToRemove.Clear();
            }
            
            Log("[INFO] Metadata stripped successfully");
        }

        private void EnhancedRenameSymbols(ModuleDefMD module)
        {
            int count = 0;
            var random = new Random();
            var usedNames = new HashSet<string>();

            foreach (var type in module.GetTypes())
            {
                if (type.IsGlobalModuleType || type.IsRuntimeSpecialName) continue;
                
                string newName;
                do
                {
                    newName = GenerateStrongRandomName(random);
                } while (usedNames.Contains(newName));
                
                usedNames.Add(newName);
                type.Name = newName;
                type.Namespace = GenerateStrongRandomName(random);
                count++;

                var methodsToRename = type.Methods.Where(m => m.HasBody && !m.IsConstructor && !m.IsSpecialName && !m.IsVirtual).ToList();
                foreach (var method in methodsToRename)
                {
                    do
                    {
                        newName = GenerateStrongRandomName(random);
                    } while (usedNames.Contains(newName));
                    
                    usedNames.Add(newName);
                    method.Name = newName;
                    count++;
                }
            }

            Log($"[INFO] Renamed {count} symbols with enhanced random names");
        }

        private string GenerateStrongRandomName(Random random)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string[] prefixes = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "_" };
            
            string prefix = prefixes[random.Next(prefixes.Length)];
            int length = random.Next(15, 35);
            
            char[] result = new char[length];
            result[0] = prefix[0];
            
            for (int i = 1; i < length; i++)
            {
                result[i] = chars[random.Next(chars.Length)];
            }
            
            return new string(result);
        }

        private void ObfuscateControlFlow(ModuleDefMD module)
        {
            var random = new Random();
            int obfuscatedCount = 0;

            foreach (var type in module.GetTypes())
            {
                foreach (var method in type.Methods)
                {
                    if (!method.HasBody || method.Body == null) continue;
                    
                    var instructions = method.Body.Instructions;
                    if (instructions.Count < 5) continue;
                    
                    // تقليل النسبة لتجنب التضخم
                    for (int i = 0; i < instructions.Count - 2; i++)
                    {
                        if (random.Next(0, 100) < 15)  // من 30% إلى 15%
                        {
                            var originalInstr = instructions[i];
                            var condition = Instruction.Create(OpCodes.Ldc_I4_0);
                            var branch = Instruction.Create(OpCodes.Brfalse_S, originalInstr);
                            
                            instructions.Insert(i, condition);
                            instructions.Insert(i + 1, branch);
                            i += 2;
                            obfuscatedCount++;
                        }
                    }
                    
                    method.Body.SimplifyBranches();
                    method.Body.OptimizeBranches();
                }
            }
            
            Log($"[INFO] Obfuscated control flow in {obfuscatedCount} locations");
        }

        private void EnhancedEncryptStrings(ModuleDefMD module)
        {
            int encryptedCount = 0;
            int level = (int)numEncryptionLevel.Value;
            var random = new Random();

            foreach (var type in module.GetTypes())
            {
                foreach (var method in type.Methods)
                {
                    if (!method.HasBody || method.Body == null) continue;

                    for (int i = 0; i < method.Body.Instructions.Count; i++)
                    {
                        var instr = method.Body.Instructions[i];
                        if (instr.OpCode == OpCodes.Ldstr && instr.Operand != null)
                        {
                            string original = instr.Operand.ToString();
                            if (string.IsNullOrEmpty(original) || original.Length < 2) continue;
                            
                            string encrypted = original;
                            
                            for (int layer = 0; layer < level; layer++)
                            {
                                encrypted = AESEncrypt(encrypted, GenerateKey(random));
                                encrypted = ReverseString(encrypted);
                                encrypted = Convert.ToBase64String(Encoding.UTF8.GetBytes(encrypted));
                                encrypted = XORWithKeyAdvanced(encrypted, random);
                            }
                            
                            instr.Operand = encrypted;
                            encryptedCount++;
                        }
                    }
                }
            }

            Log($"[INFO] Encrypted {encryptedCount} strings with {level}-layer encryption");
        }

        private string AESEncrypt(string plainText, string key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
                aes.IV = new byte[16];
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                
                ICryptoTransform encryptor = aes.CreateEncryptor();
                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                
                return Convert.ToBase64String(cipherBytes);
            }
        }

        private string GenerateKey(Random random)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 32).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private string XORWithKeyAdvanced(string input, Random random)
        {
            byte[] data = Encoding.UTF8.GetBytes(input);
            byte[] key = new byte[random.Next(8, 32)];
            random.NextBytes(key);
            
            for (int i = 0; i < data.Length; i++)
                data[i] ^= key[i % key.Length];
            
            return Convert.ToBase64String(data);
        }

        private string ReverseString(string input)
        {
            char[] chars = input.ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        }

        private void AddProxyMixing(ModuleDefMD module)
        {
            int proxyCount = 0;
            var random = new Random();
            
            foreach (var type in module.GetTypes())
            {
                var methods = type.Methods.Where(m => m.HasBody).ToList();
                foreach (var method in methods)
                {
                    if (method.Body == null || method.Body.Instructions.Count < 10) continue;
                    
                    int proxyCountForMethod = 0;
                    for (int i = 0; i < method.Body.Instructions.Count - 3 && proxyCountForMethod < 30; i += random.Next(3, 8))
                    {
                        if (random.Next(0, 100) < 30)  // تقليل النسبة
                        {
                            var tempVar = new Local(module.CorLibTypes.Object);
                            method.Body.Variables.Add(tempVar);
                            
                            method.Body.Instructions.Insert(i, Instruction.Create(OpCodes.Stloc, tempVar));
                            method.Body.Instructions.Insert(i + 1, Instruction.Create(OpCodes.Ldloc, tempVar));
                            proxyCount++;
                            proxyCountForMethod++;
                        }
                    }
                }
            }
            
            Log($"[INFO] Added {proxyCount} proxy mixing instructions");
        }

        private void EncryptResources(ModuleDefMD module)
        {
            int resourceCount = 0;
            var random = new Random();
            
            if (module.Resources != null)
            {
                foreach (var resource in module.Resources.ToList())
                {
                    if (resource is EmbeddedResource embeddedResource)
                    {
                        var data = embeddedResource.CreateReader().ToArray();
                        if (data != null && data.Length > 0 && data.Length < 1024 * 1024) // فقط للملفات أقل من 1 ميجابايت
                        {
                            string encrypted = AESEncrypt(Convert.ToBase64String(data), GenerateKey(random));
                            var newResource = new EmbeddedResource(resource.Name + "_enc", Encoding.UTF8.GetBytes(encrypted));
                            module.Resources.Remove(resource);
                            module.Resources.Add(newResource);
                            resourceCount++;
                        }
                    }
                }
            }
            
            Log($"[INFO] Encrypted {resourceCount} resources");
        }

        private void InjectJunkCode(ModuleDefMD module)
        {
            int junkCount = 0;
            int level = (int)numJunkLevel.Value;
            var random = new Random();
            
            string[] junkStrings = {
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ThisIsJunkCode", "UnusedVariable",
                "xXx_Protected_xXx", "Obfuscated", "AntiDebug", "AntiTamper"
            };
            
            foreach (var type in module.GetTypes())
            {
                foreach (var method in type.Methods)
                {
                    if (!method.HasBody || method.Body == null) continue;
                    
                    // تقليل الكم بناءً على مستوى الجانك
                    int injections = Math.Min(level * random.Next(1, 3), 20);
                    
                    for (int j = 0; j < injections; j++)
                    {
                        int insertPos = random.Next(0, Math.Max(1, method.Body.Instructions.Count - 1));
                        
                        var junkLocal = new Local(module.CorLibTypes.String);
                        method.Body.Variables.Add(junkLocal);
                        
                        string junkStr = junkStrings[random.Next(junkStrings.Length)] + random.Next(999999);
                        method.Body.Instructions.Insert(insertPos, Instruction.Create(OpCodes.Ldstr, junkStr));
                        method.Body.Instructions.Insert(insertPos + 1, Instruction.Create(OpCodes.Stloc, junkLocal));
                        method.Body.Instructions.Insert(insertPos + 2, Instruction.Create(OpCodes.Ldloc, junkLocal));
                        method.Body.Instructions.Insert(insertPos + 3, Instruction.Create(OpCodes.Pop));
                        
                        junkCount += 4;
                    }
                }
            }
            
            Log($"[INFO] Injected {junkCount} junk code instructions (level {level})");
        }

        private void EnhancedAddAntiDebug(ModuleDefMD module)
        {
            var entry = module.EntryPoint;
            if (entry?.HasBody != true) return;

            entry.Body.KeepOldMaxStack = true;
            
            var exitMethod = typeof(Environment).GetMethod("Exit", new Type[] { typeof(int) });
            var isAttachedMethod = typeof(System.Diagnostics.Debugger).GetMethod("get_IsAttached");
            var isLoggingMethod = typeof(System.Diagnostics.Debugger).GetMethod("get_IsLogging");
            var getProcessMethod = typeof(System.Diagnostics.Process).GetMethod("GetCurrentProcess");
            var getProcessNameMethod = typeof(System.Diagnostics.Process).GetProperty("ProcessName")?.GetGetMethod();
            var equalsMethod = typeof(string).GetMethod("Equals", new Type[] { typeof(string), typeof(string) });
            
            if (exitMethod == null || isAttachedMethod == null || isLoggingMethod == null || 
                getProcessMethod == null || getProcessNameMethod == null || equalsMethod == null) return;
            
            var code = new List<Instruction>
            {
                Instruction.Create(OpCodes.Call, module.Import(isAttachedMethod)),
                Instruction.Create(OpCodes.Brfalse_S, entry.Body.Instructions[0]),
                Instruction.Create(OpCodes.Call, module.Import(exitMethod)),
                Instruction.Create(OpCodes.Ldc_I4_0),
                Instruction.Create(OpCodes.Call, module.Import(exitMethod)),
                
                Instruction.Create(OpCodes.Call, module.Import(isLoggingMethod)),
                Instruction.Create(OpCodes.Brfalse_S, entry.Body.Instructions[0]),
                Instruction.Create(OpCodes.Call, module.Import(exitMethod)),
                
                Instruction.Create(OpCodes.Call, module.Import(getProcessMethod)),
                Instruction.Create(OpCodes.Callvirt, module.Import(getProcessNameMethod)),
                Instruction.Create(OpCodes.Ldstr, "devenv"),
                Instruction.Create(OpCodes.Call, module.Import(equalsMethod)),
                Instruction.Create(OpCodes.Brfalse_S, entry.Body.Instructions[0]),
                Instruction.Create(OpCodes.Call, module.Import(exitMethod))
            };

            foreach (var instr in code)
                entry.Body.Instructions.Insert(0, instr);

            entry.Body.SimplifyBranches();
            entry.Body.OptimizeBranches();
        }

        private void AddAntiMemoryDump(ModuleDefMD module)
        {
            var entry = module.EntryPoint;
            if (entry?.HasBody != true) return;
            
            entry.Body.KeepOldMaxStack = true;
            
            var collectMethod = typeof(GC).GetMethod("Collect", new Type[] { typeof(int) });
            var waitMethod = typeof(GC).GetMethod("WaitForPendingFinalizers");
            var getMemoryMethod = typeof(GC).GetMethod("GetTotalMemory", new Type[] { typeof(bool) });
            
            if (collectMethod == null || waitMethod == null || getMemoryMethod == null) return;
            
            var code = new List<Instruction>
            {
                Instruction.Create(OpCodes.Ldc_I4_1),
                Instruction.Create(OpCodes.Call, module.Import(collectMethod)),
                Instruction.Create(OpCodes.Ldc_I4_2),
                Instruction.Create(OpCodes.Call, module.Import(collectMethod)),
                Instruction.Create(OpCodes.Ldc_I4_1),
                Instruction.Create(OpCodes.Call, module.Import(waitMethod)),
                Instruction.Create(OpCodes.Call, module.Import(getMemoryMethod)),
                Instruction.Create(OpCodes.Pop)
            };
            
            foreach (var instr in code)
                entry.Body.Instructions.Insert(0, instr);
        }

        private void AddAntiVirtualMachine(ModuleDefMD module)
        {
            var entry = module.EntryPoint;
            if (entry?.HasBody != true) return;
            
            entry.Body.KeepOldMaxStack = true;
            
            var getEnvMethod = typeof(Environment).GetMethod("GetEnvironmentVariable", new Type[] { typeof(string) });
            var equalityMethod = typeof(string).GetMethod("op_Equality");
            var exitMethod = typeof(Environment).GetMethod("Exit", new Type[] { typeof(int) });
            
            if (getEnvMethod == null || equalityMethod == null || exitMethod == null) return;
            
            var code = new List<Instruction>
            {
                Instruction.Create(OpCodes.Ldstr, "VBOX"),
                Instruction.Create(OpCodes.Call, module.Import(getEnvMethod)),
                Instruction.Create(OpCodes.Ldstr, "VBOX"),
                Instruction.Create(OpCodes.Call, module.Import(equalityMethod)),
                Instruction.Create(OpCodes.Brfalse_S, entry.Body.Instructions[0]),
                Instruction.Create(OpCodes.Call, module.Import(exitMethod)),
                
                Instruction.Create(OpCodes.Ldstr, "VMWARE"),
                Instruction.Create(OpCodes.Call, module.Import(getEnvMethod)),
                Instruction.Create(OpCodes.Ldstr, "VMWARE"),
                Instruction.Create(OpCodes.Call, module.Import(equalityMethod)),
                Instruction.Create(OpCodes.Brfalse_S, entry.Body.Instructions[0]),
                Instruction.Create(OpCodes.Call, module.Import(exitMethod))
            };
            
            foreach (var instr in code)
                entry.Body.Instructions.Insert(0, instr);
        }

        private void AddAntiDe4dot(ModuleDefMD module)
        {
            var entry = module.EntryPoint;
            if (entry?.HasBody != true) return;
            
            entry.Body.KeepOldMaxStack = true;
            
            var getEnvMethod = typeof(Environment).GetMethod("GetEnvironmentVariable", new Type[] { typeof(string) });
            var inequalityMethod = typeof(string).GetMethod("op_Inequality");
            var exitMethod = typeof(Environment).GetMethod("Exit", new Type[] { typeof(int) });
            
            if (getEnvMethod == null || inequalityMethod == null || exitMethod == null) return;
            
            var code = new List<Instruction>
            {
                Instruction.Create(OpCodes.Ldstr, "de4dot"),
                Instruction.Create(OpCodes.Call, module.Import(getEnvMethod)),
                Instruction.Create(OpCodes.Ldnull),
                Instruction.Create(OpCodes.Call, module.Import(inequalityMethod)),
                Instruction.Create(OpCodes.Brfalse_S, entry.Body.Instructions[0]),
                Instruction.Create(OpCodes.Call, module.Import(exitMethod))
            };
            
            foreach (var instr in code)
                entry.Body.Instructions.Insert(0, instr);
        }

        private void AddAntiDnSpy(ModuleDefMD module)
        {
            var entry = module.EntryPoint;
            if (entry == null) return;
            
            var obsoleteAttrCtor = typeof(ObsoleteAttribute).GetConstructor(new Type[] { typeof(string), typeof(bool) });
            if (obsoleteAttrCtor != null)
            {
                var fakeAttr = new CustomAttribute((ICustomAttributeType)module.Import(obsoleteAttrCtor));
                fakeAttr.ConstructorArguments.Add(new CAArgument(module.CorLibTypes.String, "dnSpy Detected"));
                fakeAttr.ConstructorArguments.Add(new CAArgument(module.CorLibTypes.Boolean, true));
                entry.CustomAttributes.Add(fakeAttr);
            }
        }

        private void EnhancedAddAntiTamper(ModuleDefMD module)
        {
            var entry = module.EntryPoint;
            if (entry?.HasBody != true) return;

            entry.Body.KeepOldMaxStack = true;

            uint checksum = 0;
            foreach (var type in module.GetTypes())
            {
                foreach (var method in type.Methods)
                {
                    if (!string.IsNullOrEmpty(method.Name.String))
                    {
                        foreach (char c in method.Name.String)
                            checksum ^= (uint)c;
                        checksum = (checksum << 5) | (checksum >> 27);
                    }
                    if (method.HasBody)
                    {
                        checksum ^= (uint)method.Body.Instructions.Count;
                        checksum = (checksum << 7) | (checksum >> 25);
                    }
                }
            }

            var exitMethod = typeof(Environment).GetMethod("Exit", new Type[] { typeof(int) });
            if (exitMethod == null) return;
            
            var code = new List<Instruction>
            {
                Instruction.Create(OpCodes.Ldc_I4, (int)checksum),
                Instruction.Create(OpCodes.Ldc_I4, (int)checksum),
                Instruction.Create(OpCodes.Bne_Un_S, entry.Body.Instructions[0]),
                Instruction.Create(OpCodes.Ldc_I4_1),
                Instruction.Create(OpCodes.Call, module.Import(exitMethod)),
                
                Instruction.Create(OpCodes.Ldc_I4, (int)(checksum ^ 0xDEADBEEF)),
                Instruction.Create(OpCodes.Ldc_I4, (int)(checksum ^ 0xDEADBEEF)),
                Instruction.Create(OpCodes.Bne_Un_S, entry.Body.Instructions[0]),
                Instruction.Create(OpCodes.Call, module.Import(exitMethod))
            };

            foreach (var instr in code)
                entry.Body.Instructions.Insert(0, instr);
        }

        private void AddIntegrityCheck(ModuleDefMD module)
        {
            var entry = module.EntryPoint;
            if (entry?.HasBody != true) return;
            
            entry.Body.KeepOldMaxStack = true;
            
            uint crc = 0;
            try
            {
                var fileBytes = File.ReadAllBytes(module.Location);
                foreach (byte b in fileBytes)
                {
                    crc ^= b;
                    for (int i = 0; i < 8; i++)
                        crc = (crc & 1) != 0 ? (crc >> 1) ^ 0xEDB88320 : crc >> 1;
                }
            }
            catch { }
            
            var exitMethod = typeof(Environment).GetMethod("Exit", new Type[] { typeof(int) });
            if (exitMethod == null) return;
            
            var code = new List<Instruction>
            {
                Instruction.Create(OpCodes.Ldc_I4, (int)crc),
                Instruction.Create(OpCodes.Ldc_I4, (int)crc),
                Instruction.Create(OpCodes.Bne_Un_S, entry.Body.Instructions[0]),
                Instruction.Create(OpCodes.Call, module.Import(exitMethod))
            };
            
            foreach (var instr in code)
                entry.Body.Instructions.Insert(0, instr);
        }
    }

    public class CustomProfessionalColors : ProfessionalColorTable
    {
        public override Color MenuItemSelected => Color.FromArgb(0, 122, 204);
        public override Color MenuItemBorder => Color.FromArgb(0, 122, 204);
        public override Color ToolStripDropDownBackground => Color.FromArgb(60, 60, 65);
        public override Color ImageMarginGradientBegin => Color.FromArgb(60, 60, 65);
        public override Color ImageMarginGradientMiddle => Color.FromArgb(60, 60, 65);
        public override Color ImageMarginGradientEnd => Color.FromArgb(60, 60, 65);
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