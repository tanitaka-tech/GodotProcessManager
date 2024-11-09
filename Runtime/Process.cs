using System;
using System.Threading;
using Fractural.Tasks;

namespace TanitakaTech.GodotProcessManager
{
    public readonly struct Process
    {
        public Func<CancellationToken, GDTask> WaitTask { get; }
        public Func<CancellationToken, GDTask<ProcessContinueType>> OnPassedTask { get; }

        private Process(Func<CancellationToken, GDTask> waitTask, Func<CancellationToken, GDTask<ProcessContinueType>> onPassedTask)
        {
            WaitTask = waitTask;
            OnPassedTask = onPassedTask;
        } 
        
        public static Process Create(Func<CancellationToken, GDTask> waitTask, Func<CancellationToken, GDTask<ProcessContinueType>> onPassedTask)
        {
            return new Process(waitTask, onPassedTask);
        }
        
        public static Process CreateWithWaitResult<TWaitResult>(Func<CancellationToken, GDTask<TWaitResult>> waitTask, Func<TWaitResult, CancellationToken, GDTask<ProcessContinueType>> onPassedTask)
        {
            TWaitResult waitResult = default;
            return new Process(
                waitTask: async (ct) => waitResult = await waitTask(ct), 
                onPassedTask: (ct) => onPassedTask(waitResult, ct)
            );
        }
        
        public GDTask ToGDTask(CancellationToken cancellationToken)
        {
            var process = this;
            return GDTask.Create(async () =>
            {
                await process.WaitTask(cancellationToken);
                await process.OnPassedTask(cancellationToken);
            });
        }
    }
}