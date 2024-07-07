using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfTest.Commands;

namespace WpfTest.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
        }


        private RelayCommandAsync testAsyncCommand;

        public ICommand TestAsyncCommand
        {
            get
            {
                if (testAsyncCommand == null)
                {
                    testAsyncCommand = new RelayCommandAsync(TestAsync);
                }

                return testAsyncCommand;
            }
        }

        private async Task TestAsync(object commandParameter)
        {
            ShowCurrentThread(1);
            Task<int> returnedTaskTResult = GetTaskOfTResultAsync();
            int intResultAsync = await returnedTaskTResult;

            int intResult = GetTaskOfTResult();
            ShowCurrentThread(2);
        }

        private async Task<int> GetTaskOfTResultAsync()
        {
            int hours = 0;
            await Task.Delay(5000);

            return hours;
        }

        private int GetTaskOfTResult()
        {
            int hours = 0;
            return hours;
        }
        private void ShowCurrentThread(int i)
        {
            Debug.WriteLine($"Операция {i} " + "выполняется в потоке ThreadID {0}",
                Thread.CurrentThread.ManagedThreadId);
        }
    }
}
