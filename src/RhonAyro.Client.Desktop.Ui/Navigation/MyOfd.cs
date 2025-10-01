using System;
using System.Windows;

using Microsoft.Win32;

namespace RhonAyro.Client.Desktop.Ui.Navigation
{
    internal class MyOfd : IOpenFileDialog
    {
        private readonly OpenFileDialog ofd;
        private readonly Window? owner;

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
        public Guid? ClientGuid
        {
            get => ofd.ClientGuid;
            set => ofd.ClientGuid = value;
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
        public string FileName
        {
            get => ofd.FileName;
            set => ofd.FileName = value;
        }
        public string[] FileNames
        {
            get => ofd.FileNames;
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
        public string[] SafeFileNames
        {
            get => ofd.SafeFileNames;
        }
        public bool ShowHiddenItems 
        {
            get => ofd.ShowHiddenItems;
            set => ofd.ShowHiddenItems = value;
        }
        public bool ShowReadOnly
        {
            get => ofd.ShowReadOnly;
            set => ofd.ShowReadOnly = value;
        }
        public string Title
        {
            get => ofd.Title;
            set => ofd.Title = value;
        }
        public bool ValidateNames
        {
            get => ofd.ValidateNames;
            set => ofd.ValidateNames = value;
        }

        public MyOfd(
            bool addExtension = false,
            bool addToRecent = false,
            bool checkFileExists = true,
            bool checkPathExists = true,
            Guid? clientGuid = null,
            string defaultDirectory = "C:\\",
            string defaultExt = "",
            bool dereferenceLinks = true,
            string fileName = "",
            string filter = "",
            int filterIndex = 0,
            bool forcePreviewPane = false,
            string InitialDirectory = "C:\\",
            bool multiselect = false,
            bool readOnlyChecked = false,
            string rootDirectory = "",
            bool showHiddenItems = false,
            bool showReadOnly = false,
            string title = "",
            bool validateNames = false,
            Window? owner = null)
        {
            ofd = new OpenFileDialog
            {
                AddExtension = addExtension,
                AddToRecent = addToRecent,
                CheckFileExists = checkFileExists,
                CheckPathExists = checkPathExists,
                ClientGuid = clientGuid,
                FileName = fileName,
                DefaultDirectory = defaultDirectory,
                DefaultExt = defaultExt,
                DereferenceLinks = dereferenceLinks,
                Filter = filter,
                FilterIndex = filterIndex,
                ForcePreviewPane = forcePreviewPane,
                InitialDirectory = InitialDirectory,
                Multiselect = multiselect,
                ReadOnlyChecked = readOnlyChecked,
                RootDirectory = rootDirectory,
                ShowHiddenItems = showHiddenItems,
                ShowReadOnly = showReadOnly,
                Title = title,
                ValidateNames = validateNames,
            };
            this.owner = owner;
        }

        public bool? ShowDialog(Window? owner = null)
        {
            return ofd.ShowDialog(owner ?? this.owner);
        }
    }
}
