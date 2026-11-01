using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace VisumCRM.Client.Wpf.Internals
{
    public class WorkhandlerContainer : IDisposable
    {
        private bool disposedValue;
        private Cursor _cursor;
        private WorkerItem _item;
        public WorkhandlerContainer(string Name)
        {
            _item = new WorkerItem(Name, WorkerItemStatus.Startet);
            App.AppShare.WorkerItems.Add(_item);
            _cursor = Mouse.OverrideCursor;
            Mouse.OverrideCursor = Cursors.Wait;
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }
                disposedValue = true;
            }
            App.AppShare.WorkerItems.Add(new WorkerItem(_item.Name, WorkerItemStatus.Ended));
            Mouse.OverrideCursor = _cursor;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);

        }
    }
    public enum WorkerItemStatus
    {
        Unknown,
        Startet,
        Ended
    }

    public class WorkerItem
    {
        public string Name { get; set; } = "";
        public WorkerItemStatus Status { get; set; } = WorkerItemStatus.Unknown;
        public DateTime Timer { get; set; }
        public WorkerItem(string name, WorkerItemStatus status)
        {
            Name = name;
            Status = status;
            Timer = DateTime.Now;
        }

    }
}
