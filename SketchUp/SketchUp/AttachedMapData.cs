using System;
using System.Text;

namespace SketchUp
{
    public class AttachedMapData
    {
        public event EventHandler<AttachedMapChangedEventArgs> AttachedMapChangedEvent;

        public string ParentMap
        {
            get; set;
        }

        public string ChildMap
        {
            get; set;
        }

        public AttachedMapData()
        {
        }

        private void FireChangedEvent(string AttachedMapFile)
        {
            if (AttachedMapChangedEvent != null)
            {
                AttachedMapChangedEvent(this,
                    new AttachedMapChangedEventArgs()
                    {
                        AttachedMap = AttachedMapFile
                    });
            }
        }
    }

    public class AttachedMapChangedEventArgs : EventArgs
    {
        public AttachedMapChangedEventArgs()
            : base()
        {
        }

        public string AttachedMap
        {
            get; set;
        }
    }
}