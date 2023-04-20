using System;
using System.Runtime.CompilerServices;

namespace Core.Utils.Algorithm
{
    public class Snowflake
    {
        /// <summary>
        /// 起始的時間戳:唯一時間，這是一個避免重覆的隨機量，自行設定不要大於當前時間戳。
        /// 一個計時周期表示一百納秒，即一千萬分之一秒。 1 毫秒內有 10,000 個計時周期，即 1 秒內有 1,000 萬個計時周期。
        /// </summary>
        private static long startTimeStamp = new DateTime(2023, 1, 1).Ticks / 10000;

        /*
        * 每一部分占用的位數
        * 對於移位運算符 << 和 >>，右側操作數的類型必須為 int，或具有預定義隱式數值轉換 為 int 的類型。
        */
        private const int sequenceBit   = 12; //序列號占用的位數
        private const int machingBit    = 5;  //機器標識占用的位數
        private const int dataCenterBit = 5;  //數據中心占用的位數

        /*
        * 每一部分的最大值
        */
        private static long maxSequence      = -1L ^ (-1L << sequenceBit);
        private static long maxMachingNum    = -1L ^ (-1L << machingBit);
        private static long maxDataCenterNum = -1L ^ (-1L << dataCenterBit);

        /*
        * 每一部分向左的位移
        */
        private const int machingLeft    = sequenceBit;
        private const int dataCenterLeft = sequenceBit    + machingBit;
        private const int timeStampLeft  = dataCenterLeft + dataCenterBit;

        private long dataCenterId  = 1;  //數據中心
        private long machineId     = 1;  //機器標識
        private long sequence      = 0;  //序列號
        private long lastTimeStamp = -1; //上一次時間戳

        //預設值
        public Snowflake()
        {
            this.dataCenterId = 1L; //數據中心ID
            this.machineId    = 1L; //機器標誌ID
        }


        /// <summary>
        /// 根據指定的數據中心ID和機器標誌ID生成指定的序列號
        /// </summary>
        /// <param name="dataCenterId">數據中心ID</param>
        /// <param name="machineId">機器標誌ID</param>
        public Snowflake(long dataCenterId, long machineId)
        {
            if (dataCenterId > maxDataCenterNum || dataCenterId < 0)
            {
                throw new ArgumentException("DtaCenterId can't be greater than MAX_DATA_CENTER_NUM or less than 0！");
            }

            if (machineId > maxMachingNum || machineId < 0)
            {
                throw new ArgumentException("MachineId can't be greater than MAX_MACHINE_NUM or less than 0！");
            }

            this.dataCenterId = dataCenterId;
            this.machineId    = machineId;
        }


        private long GetNextMill()
        {
            long mill = GetNewTimeStamp();
            while (mill <= lastTimeStamp)
            {
                mill = GetNewTimeStamp();
            }

            return mill;
        }

        private long GetNewTimeStamp()
        {
            return DateTime.Now.Ticks / 10000;
        }


        /// <summary>
        /// 產生一組ID
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public long GetSnowId()
        {
            long currTimeStamp = GetNewTimeStamp();
            if (currTimeStamp < lastTimeStamp)
            {
                //如果當前時間戳比上一次生成ID時時間戳還小，拋出異常，因為不能保證現在生成的ID之前沒有生成過
                throw new Exception("Clock moved backwards.  Refusing to generate id");
            }

            if (currTimeStamp == lastTimeStamp)
            {
                //相同毫秒內，序列號自增
                sequence = (sequence + 1) & maxSequence;
                //同一毫秒的序列數已經達到最大
                if (sequence == 0L)
                {
                    currTimeStamp = GetNextMill();
                }
            }
            else
            {
                //不同毫秒內，序列號置為0
                sequence = 0L;
            }

            lastTimeStamp = currTimeStamp;

            return (currTimeStamp - startTimeStamp) << timeStampLeft  //時間戳部分
                 | dataCenterId                     << dataCenterLeft //數據中心部分
                 | machineId                        << machingLeft    //機器標識部分
                 | sequence;                                          //序列號部分
        }
    }
}