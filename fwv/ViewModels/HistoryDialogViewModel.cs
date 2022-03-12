using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using Prism.Mvvm;
using Prism.Services.Dialogs;

using fwv.Models;

namespace fwv.ViewModels
{
    public class HistoryDialogViewModel : BindableBase, IDialogAware
    {
        #region Properties

        private ObservableCollection<HistoryListItem> _histories = new ObservableCollection<HistoryListItem>();
        public ObservableCollection<HistoryListItem> Histories
        {
            get { return _histories; }
            set { SetProperty(ref _histories, value); }
        }

        #endregion

        #region Constructor

        public HistoryDialogViewModel()
        {
            Histories.AddRange(new List<HistoryListItem>
            {
                new HistoryListItem
                {
                    TimeStamp = DateTime.MinValue,
                    AuthorName = "kusa-mochi",
                    ModifiedObjects = new ObservableCollection<string>
                    {
                        "aaaaaaaaa.txt",
                        "bbbbbbbbbb.xlsx",
                        "cccc.docx",
                        "dddddddddddd.dat",
                        "ee.html"
                    }
                },
                new HistoryListItem
                {
                    TimeStamp = DateTime.MinValue,
                    AuthorName = "kusa-mochi",
                    ModifiedObjects = new ObservableCollection<string>
                    {
                        "aaaaaaaaa.txt",
                        "bbbbbbbbbb.xlsx",
                        "cccc.docx",
                        "dddddddddddd.dat",
                        "ee.html"
                    }
                },
                new HistoryListItem
                {
                    TimeStamp = DateTime.MinValue,
                    AuthorName = "kusa-mochi",
                    ModifiedObjects = new ObservableCollection<string>
                    {
                        "aaaaaaaaa.txt",
                        "bbbbbbbbbb.xlsx",
                        "cccc.docx",
                        "dddddddddddd.dat",
                        "ee.html"
                    }
                },
                new HistoryListItem
                {
                    TimeStamp = DateTime.MinValue,
                    AuthorName = "kusa-mochi",
                    ModifiedObjects = new ObservableCollection<string>
                    {
                        "aaaaaaaaa.txt",
                        "bbbbbbbbbb.xlsx",
                        "cccc.docx",
                        "dddddddddddd.dat",
                        "ee.html"
                    }
                },
                new HistoryListItem
                {
                    TimeStamp = DateTime.MinValue,
                    AuthorName = "kusa-mochi",
                    ModifiedObjects = new ObservableCollection<string>
                    {
                        "aaaaaaaaa.txt",
                        "bbbbbbbbbb.xlsx",
                        "cccc.docx",
                        "dddddddddddd.dat",
                        "ee.html"
                    }
                },
                new HistoryListItem
                {
                    TimeStamp = DateTime.MinValue,
                    AuthorName = "kusa-mochi",
                    ModifiedObjects = new ObservableCollection<string>
                    {
                        "aaaaaaaaa.txt",
                        "bbbbbbbbbb.xlsx",
                        "cccc.docx",
                        "dddddddddddd.dat",
                        "ee.html"
                    }
                },
                new HistoryListItem
                {
                    TimeStamp = DateTime.MinValue,
                    AuthorName = "kusa-mochi",
                    ModifiedObjects = new ObservableCollection<string>
                    {
                        "aaaaaaaaa.txt",
                        "bbbbbbbbbb.xlsx",
                        "cccc.docx",
                        "dddddddddddd.dat",
                        "ee.html"
                    }
                },
                new HistoryListItem
                {
                    TimeStamp = DateTime.MinValue,
                    AuthorName = "kusa-mochi",
                    ModifiedObjects = new ObservableCollection<string>
                    {
                        "aaaaaaaaa.txt",
                        "bbbbbbbbbb.xlsx",
                        "cccc.docx",
                        "dddddddddddd.dat",
                        "ee.html"
                    }
                },
                new HistoryListItem
                {
                    TimeStamp = DateTime.MinValue,
                    AuthorName = "kusa-mochi",
                    ModifiedObjects = new ObservableCollection<string>
                    {
                        "aaaaaaaaa.txt",
                        "bbbbbbbbbb.xlsx",
                        "cccc.docx",
                        "dddddddddddd.dat",
                        "ee.html"
                    }
                }
            });
        }

        #endregion

        #region Implementation of IDialogAware

        public string Title => "History";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }

        #endregion
    }
}
