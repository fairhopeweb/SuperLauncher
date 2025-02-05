﻿using System.IO;
using System.Windows.Controls;
using System.Windows;

namespace SuperLauncher
{
    public partial class ModernLauncherIcons : UserControl
    {
        public string rFilter;
        public string Filter
        {
            get
            {
                return rFilter;
            }
            set
            {
                rFilter = value;
                bool first = true;
                int invisibleCount = 0;
                foreach (ModernLauncherIcon icon in IconPanel.Children)
                {
                    if (icon.NameText.Text.ToLower().Contains(value.ToLower()))
                    {
                        icon.FilterFocus = first;
                        if (first) first = false;
                        icon.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        invisibleCount++;
                        icon.Visibility = Visibility.Collapsed;
                    }
                }
                if (invisibleCount == IconPanel.Children.Count)
                {
                    NoResults.Visibility = Visibility.Visible;
                }
                else
                {
                    NoResults.Visibility = Visibility.Collapsed;
                }
            }
        }
        public ModernLauncherIcons()
        {
            InitializeComponent();
            PopulateIcons();
        }
        public void PopulateIcons()
        {
            IconPanel.Children.Clear();
            foreach (string filePath in Settings.Default.FileList)
            {
                if (!File.Exists(filePath)) continue;
                ModernLauncherIcon mli = new(filePath);
                IconPanel.Children.Add(mli);
            }
        }
    }
}
