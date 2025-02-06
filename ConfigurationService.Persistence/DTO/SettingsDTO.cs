using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationService.Persistence.DTO;
public class Settings
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public string Service { get; set; }
}
