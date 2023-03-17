using System;
using System.Collections.Generic;
using System.Text;

namespace   Wtdl.Share
{
    public class SynchState
    {
        //同步数据已经完成
        public bool SynchDataCompleted = false;
        //同步数据的分组KEY
        public string  SynchDataKey = "";

        ///发送完成
        public static SynchState SendCompleted(string synchdatakey)
        {
            SynchState state = new SynchState();
            state.SynchDataCompleted = true;
            state.SynchDataKey = synchdatakey;
            return state;
        }
    }
}
