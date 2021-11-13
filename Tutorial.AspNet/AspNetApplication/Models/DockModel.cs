using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetApplication.Models
{
    public class DockModel
    {

        Tuple<int, int, int, int> _location;
        bool _isFree;
        public DockModel(Tuple<int, int, int, int> loc, bool isFree = false)
        {
            _location = loc;
            _isFree = isFree;
        }
    }
}