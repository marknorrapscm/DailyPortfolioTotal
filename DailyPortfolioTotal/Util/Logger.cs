using Microsoft.Azure.WebJobs.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPortfolioTotal.Util
{
    public class Logger
    {
        private TraceWriter traceWriter;

        public Logger(TraceWriter _traceWriter)
        {
            this.traceWriter = _traceWriter;
        }

        public void Write(string msg)
        {
            this.traceWriter.Info(msg);
        }
    }
}
