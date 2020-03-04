using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WithingsECG.API.Services.Model.HeartListResponse
{
    public class HeartListResponse
    {
        public int status { get; set; }
        public Body body { get; set; }

    }


    public class Body
    {
        public Series[] series { get; set; }
        public int offset { get; set; }
        public bool more { get; set; }
    }

    public class Series
    {
        public string deviceid { get; set; }
        public int model { get; set; }
        public Ecg ecg { get; set; }
        public Bloodpressure bloodpressure { get; set; }
        public int heart_rate { get; set; }
        public int timestamp { get; set; }
    }

    public class Ecg
    {
        public int signalid { get; set; }
        public int afib { get; set; }

        public HeartGetResponse.HeartGetResponse HeartGetResponse { get; set; }
    }

    public class Bloodpressure
    {
        public int diastole { get; set; }
        public int systole { get; set; }
    }

}
