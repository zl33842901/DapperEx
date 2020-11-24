using System;
using System.Collections.Generic;
using System.Text;

namespace xLiad.DapperEx.Core.Test
{
    public class ModelBase
    {
        public int Id { get; set; }
    }

    public class TheModel : ModelBase
    {
        public string Name { get; set; }
    }
}
