using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NArchitecture.Core.Mailing.Dtos;
public class MailVerificationStorageDto
{
    public string hash { get; set; } = default!;
    public DateTime expirationDate { get; set; }

    public string operation { get; set; }
}

