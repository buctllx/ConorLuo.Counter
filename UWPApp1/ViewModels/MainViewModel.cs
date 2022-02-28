using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace UWPApp1.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            CurCount = 0;
            BtnText = "开始";
            OptionBases = new List<int> { 2, 8, 10, 16 };
            CurBase = OptionBases[2];
            MaxNumber = 100;
            IsShowUpper0 = false;

            AsyncCmd = new AsyncRelayCommand(OnCmdAsync);

            DownloadTextCommand = new AsyncRelayCommand(DownloadTextAsync);
        }


        #region funcs

        /// <summary>
        /// Implements the logic for <see cref="LoadDocsCommand"/>.
        /// </summary>
        /// <param name="name">The name of the docs file to load.</param>
        private async Task LoadPageAsync(string name)
        {
            CurCount = 0;
            if (name is null) return;

            // Skip if the loading has already started
            if (!(LoadPageCommand.ExecutionTask is null)) return;

            // await UpdateCounter();
        }


        private async Task UpdateCounter()
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                //代码
                while (isStart)
                {
                    CurCount += 1;
                    await Task.Delay(UpdateSpan);

                    if (CurCount == MaxNumber)
                    {
                        CurCount = 0;
                    }
                }
            });
        }


        private async Task OnCmdAsync()
        {
            if (isStart)
            {
                BtnText = "开始";
                CurCount = 1;
                isStart = false;
            }
            else if (!isStart)
            {
                BtnText = "停止";
                isStart = true;

                await UpdateCounter();
            }
        }

        private async Task AsyncStartClick()
        {
            if (isStart)
            { // 由 开始 -> 暂停, 暂停后，按钮显示 开始
                BtnText = "开始";
                isStart = false;
            }
            else if (!isStart)
            { // 由 暂停 -> 开始, 开始后，按钮显示 暂停
                isStart = true;
                BtnText = "暂停";

                await UpdateCounter();
            }
        }

        private async Task AsyncNewStartClick()
        {
            CurCount = 0;
            await Task.Delay(UpdateSpan);

            if (!isStart)
            { // 由 暂停 -> 重新开始
                isStart = true;
                await UpdateCounter();
            }
        }


        private void PauseClick()
        {
            isStart = false;
        }


        #endregion


        #region props

        private Boolean isStart = false;

        private int UpdateSpan = 1000;

        /// <summary>
        /// 当前十进制计数
        /// </summary>
        private int curCount;
        public int CurCount
        {
            get => curCount;
            // set => SetProperty(ref curCount, value);
            set
            {
                curCount = value;
                OnPropertyChanged(nameof(CurCount));

                if (CurBase == 0)
                {
                    return;
                }
                var str = Convert.ToString(CurCount, CurBase); //将十进制转化为二进制
                if (IsShowUpper0)
                {
                    var len = Convert.ToString(MaxNumber, CurBase).Length;
                    if (len > str.Length)
                    {
                        str = $"{new String('0', len - str.Length)}{str}";
                    }
                }

                CurDisplayNumber = str;
            }
        }

        private String curDisplayNumber;
        public String CurDisplayNumber
        {
            get => curDisplayNumber;
            set => SetProperty(ref curDisplayNumber, value);
        }


        private String btnText;
        public String BtnText
        {
            get => btnText;
            set => SetProperty(ref btnText, value);
        }

        public int MaxNumber { get; set; }

        public Boolean IsShowUpper0 { get; set; }
        

        public List<int> OptionBases { get; set; }

        public int CurBase { get; set; }


        public ICommand PauseCmd { get { return new RelayCommand(PauseClick); } }
        public ICommand AsyncStartCmd { get { return new AsyncRelayCommand(AsyncStartClick); } }
        public ICommand AsyncNewStartCmd { get { return new AsyncRelayCommand(AsyncNewStartClick); } }

        public IAsyncRelayCommand AsyncCmd { get; set; }


        public IAsyncRelayCommand DownloadTextCommand { get; }

        private async Task<string> DownloadTextAsync()
        {
            await Task.Delay(3000); // Simulate a web request

            return "Hello world!";
        }


        /// <summary>
        /// Gets the <see cref="IAsyncRelayCommand{T}"/> responsible for loading the source markdown docs.
        /// </summary>
        public IAsyncRelayCommand<string> LoadPageCommand { get { return new AsyncRelayCommand<string>(LoadPageAsync); } }

        #endregion
    }
}
