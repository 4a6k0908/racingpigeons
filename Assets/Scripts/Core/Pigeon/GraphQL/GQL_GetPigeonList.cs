using System;
using System.Collections.Generic;
using Core.Pigeon.Models;

namespace Core.Pigeon.GraphQL
{
    [Serializable]
    public class GQL_GetPigeonList
    {
        [Serializable]
        public class Data
        {
            [Serializable]
            public class MyPigeons
            {
                public List<PigeonStat> items;

                public string next_cursor;
            }

            public MyPigeons getPigeonList;
        }

        public Data data;
    }
}