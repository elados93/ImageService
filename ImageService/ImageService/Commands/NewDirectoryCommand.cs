using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class NewDirectoryCommand : ICommand
    {
        private IImageServiceModal m_modal;

        public NewDirectoryCommand(IImageServiceModal modal)
        {
            m_modal = modal;            // Storing the Modal
        }

        public string Execute(string[] args, out bool result)
        {
            throw new NotImplementedException();
        }
    }
}
