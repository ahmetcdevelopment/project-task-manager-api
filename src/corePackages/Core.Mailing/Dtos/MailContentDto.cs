using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NArchitecture.Core.Mailing.Dtos;
public class MailContentDto
{
    public string? Code { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? ButtonText { get; set; }

    public string? ButtonUrl { get; set; }
}
