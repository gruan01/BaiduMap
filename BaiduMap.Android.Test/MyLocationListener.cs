using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Baidu.Location;

namespace BaiduMap.Android.Test {
    public class MyLocationListener : Java.Lang.Object, IBDLocationListener {

        public event EventHandler<LocationEventArgs> OnLocated = null;

        public void OnReceiveLocation(BDLocation loc) {
            if (this.OnLocated != null)
                this.OnLocated(this, new LocationEventArgs(loc));
        }

        public class LocationEventArgs : EventArgs {

            public BDLocation Location { get; }

            public LocationEventArgs(BDLocation loc) {
                this.Location = loc;
            }
        }
    }
}