using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Win32;

namespace RhonAyro.Client.Desktop.Ui.Navigation
{
    internal class MyOfd : IOpenFileDialog, IDisposable
    {
        private OpenFileDialog ofd;

        public bool AddExtension
        {
            get => ofd.AddExtension;
            set => ofd.AddExtension = value;
        }
        public bool AddToRecent
        {
            get => ofd.AddToRecent;
            set => ofd.AddToRecent = value;
        }
        public bool CheckFileExists
        {
            get => ofd.CheckFileExists;
            set => ofd.CheckFileExists = value;
        }
        public bool CheckPathExists
        {
            get => ofd.CheckPathExists;
            set => ofd.CheckPathExists = value;
        }
        public string DefaultDirectory
        {
            get => ofd.DefaultDirectory;
            set => ofd.DefaultDirectory = value;
        }
        public string DefaultExt
        {
            get => ofd.DefaultExt;
            set => ofd.DefaultExt = value;
        }
        public bool DereferenceLinks
        {
            get => ofd.DereferenceLinks;
            set => ofd.DereferenceLinks = value;
        }
        public string Filter
        {
            get => ofd.Filter;
            set => ofd.Filter = value;
        }
        public int FilterIndex
        {
            get => ofd.FilterIndex;
            set => ofd.FilterIndex = value;
        }
        public bool ForcePreviewPane
        {
            get => ofd.ForcePreviewPane;
            set => ofd.ForcePreviewPane = value;
        }
        public string InitialDirectory
        {
            get => ofd.InitialDirectory;
            set => ofd.InitialDirectory = value;
        }
        public bool Multiselect
        {
            get => ofd.Multiselect;
            set => ofd.Multiselect = value;
        }
        public bool ReadOnlyChecked
        {
            get => ofd.ReadOnlyChecked;
            set => ofd.ReadOnlyChecked = value;
        }
        public string RootDirectory
        {
            get => ofd.RootDirectory;
            set => ofd.RootDirectory = value;
        }
        public bool ShowHiddenItems { get => ofd.ShowHiddenItems; set => ofd.ShowHiddenItems = value; }
        public bool ShowReadOnly { get => ofd.ShowReadOnly; set => ofd.ShowReadOnly = value; }
        public string Title { get => ofd.Title; set => ofd.Title = value; }
        public bool ValidateNames { get => ofd.ValidateNames; set => ofd.ValidateNames = value; }

        public MyOfd()
        {
            ofd = new OpenFileDialog
            {
                AddExtension = false,
                AddToRecent = false,
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultDirectory = "",
                DefaultExt = "",
                DereferenceLinks = true,
                Filter = "",
                FilterIndex = 1,
                ForcePreviewPane = false,
                InitialDirectory = "",
                Multiselect = false,
                ReadOnlyChecked = false,
                RootDirectory = "",
                ShowHiddenItems = false,
                ShowReadOnly = false,
                Title = "",
                ValidateNames = false
            };
        }

        public void Dispose()
        {
        }
    }
}
