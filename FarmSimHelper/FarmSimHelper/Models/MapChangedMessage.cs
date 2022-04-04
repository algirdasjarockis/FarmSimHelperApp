using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.Messaging.Messages;
using FarmSimHelper.ViewModels;

namespace FarmSimHelper.Models
{
    public class MapChangedMessage : AsyncRequestMessage<bool>
    {
    }
}
