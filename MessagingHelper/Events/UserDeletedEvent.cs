using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingHelper.Events;

public class UserDeletedEvent
{
    public int OriginalUserId { get; set; }
}
