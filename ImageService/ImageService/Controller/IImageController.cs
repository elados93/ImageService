﻿using ImageService.Communication;
using Infrastracture.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    public interface IImageController
    {
        // Executing the Command Requet
        string ExecuteCommand(CommandEnum commandID, string[] args, out bool result);
    }
}
