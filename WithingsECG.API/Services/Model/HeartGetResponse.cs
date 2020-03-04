using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WithingsECG.API.Services.Model.HeartGetResponse
{
    public class HeartGetResponse
    {
            public int status { get; set; }
            public Body body { get; set; }
    }

    public class Body
    {
        public int[] signal { get; set; }
        public int sampling_frequency { get; set; }
        public int wearposition { get; set; }
    }

}
