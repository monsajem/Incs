using System.Threading.Tasks;

namespace Monsajem_Incs.DynamicAssembly
{
    public class SafeResource
    {
        private Task LastTask;

        public void Safe(Task Task)
        {
            Task LastTask;
            lock (this)
            {
                LastTask = this.LastTask;
                this.LastTask = Task;
            }
            LastTask?.Wait();
            Task.Start();
            Task.Wait();
        }
    }
}
