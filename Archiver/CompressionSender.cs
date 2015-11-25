﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archiver
{
    public interface ICompressionBlockSender
    {
        void Send(CProcessingBlock rawByteProcessingBlock);
    }
}
