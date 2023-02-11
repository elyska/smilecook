﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smilecook.ViewModels
{
    // code from https://www.youtube.com/watch?v=DuNLR_NJv8U
    public partial class BaseViewModel : ObservableObject
    {
        public BaseViewModel()
        {
            Title = AppInfo.Current.Name;
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        bool isBusy;

        [ObservableProperty]
        string title;

        public bool IsNotBusy => !IsBusy;

        

    }
}
