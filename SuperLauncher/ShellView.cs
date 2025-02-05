﻿using Microsoft.WindowsAPICodePack.Controls;
using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.IO;

namespace SuperLauncher
{
    public partial class ShellView : Form
    {
        private string InitialPath;
        private ModernLauncherExplorerButtons MButtons = new();
        public ShellView(string InitialPath = "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}")
        {
            this.InitialPath = InitialPath;
            Init();   
        }
        public void Init()
        {
            InitializeComponent();
            ElementHost modernButtonsEH = new ElementHost();
            modernButtonsEH.Width = 90;
            modernButtonsEH.Height = 30;
            modernButtonsEH.Top = 2;
            modernButtonsEH.Left = 3;
            modernButtonsEH.Child = MButtons;
            modernButtonsEH.Parent = this;
            MButtons.Back.Click += BtnBack_Click;
            MButtons.Forward.Click += BtnForward_Click;
            MButtons.Up.Click += BtnNavUp_Click;
            Icon = Resources.logo;
            try
            {
                Browser.Navigate(ShellObject.FromParsingName(InitialPath));
            }
            catch
            {
                Browser.Navigate(ShellObject.FromParsingName("::{20D04FE0-3AEA-1069-A2D8-08002B30309D}"));
            }
        }
        private void BtnBack_Click(object sender, EventArgs e)
        {
            Browser.NavigateLogLocation(NavigationLogDirection.Backward);
        }
        private void BtnForward_Click(object sender, EventArgs e)
        {
            Browser.NavigateLogLocation(NavigationLogDirection.Forward);
        }
        private void Browser_NavigationComplete(object sender, NavigationCompleteEventArgs e)
        {
            Text = e.NewLocation.Name;
            if (e.NewLocation.IsFileSystemObject)
            {
                txtNav.Text = e.NewLocation.ParsingName;
            }
            else
            {
                txtNav.Text = e.NewLocation.GetDisplayName(DisplayNameType.Default);
            }
            MButtons.Up.IsEnabled = Directory.Exists(txtNav.Text) && Directory.GetParent(txtNav.Text) != null;
            MButtons.Back.IsEnabled = Browser.NavigationLog.CanNavigateBackward;
            MButtons.Forward.IsEnabled = Browser.NavigationLog.CanNavigateForward;
        }
        private void BtnNavUp_Click(object sender, EventArgs e)
        {
            Browser.Navigate(ShellObject.FromParsingName(Directory.GetParent(txtNav.Text).FullName));
        }
        private void ShellView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                try
                {
                    Browser.Navigate(ShellObject.FromParsingName(txtNav.Text));
                }
                catch (Exception)
                {
                    MessageBox.Show("Not a valid path / directory.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
