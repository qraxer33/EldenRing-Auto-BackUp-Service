using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Service.Core;

namespace EldenRingAutoBackUpService
{
    public partial class EldenRingAutoBackUpService : ServiceBase
    {
        Core core = null;

        public EldenRingAutoBackUpService()
        {
            InitializeComponent();
        }

        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            core = new Core();
        }

        protected override void OnStop()
        {

        }
    }
}
