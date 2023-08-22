using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingHelper.Events;

public class UserEditedEvent
{
    public string Email { get; set; }
    public string UserName { get; set; }
    public int OriginalUserId { get; set; }
}
