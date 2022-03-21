using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace EldenRingAutoBackUpService
{
    internal static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        static void Main()
        {
            //EldenRingAutoBackUpService s = new EldenRingAutoBackUpService();
            //s.OnDebug();

            //System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new EldenRingAutoBackUpService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
