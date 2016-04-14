using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Com.Baidu.Mapapi.Map;
using Com.Baidu.Mapapi;
using Com.Baidu.Location;

[assembly: MetaData("com.baidu.lbsapi.API_KEY", Value = "yDvLTgf9PPlH7jHjlxIdO3YjT0g4IhZ8")]
[assembly: Permission(Name = "android.permission.ACCESS_NETWORK_STATE")]
[assembly: Permission(Name = "android.permission.INTERNET")]
[assembly: Permission(Name = "com.android.launcher.permission.READ_SETTINGS")]
[assembly: Permission(Name = "android.permission.WAKE_LOCK")]
[assembly: Permission(Name = "android.permission.CHANGE_WIFI_STATE")]
[assembly: Permission(Name = "android.permission.GET_TASKS")]
[assembly: Permission(Name = "android.permission.WRITE_EXTERNAL_STORAGE")]
[assembly: Permission(Name = "android.permission.WRITE_SETTINGS")]



namespace BaiduMap.Android.Test {

    [Activity(Label = "BaiduMap.Android.Test", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity {

        private MapView MapView = null;

        private LocationClient LocationClient = null;
        public MyLocationListener LocationListener = new MyLocationListener();

        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);
            SDKInitializer.Initialize(this.ApplicationContext);


            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            this.MapView = FindViewById<MapView>(Resource.Id.bmapView);
            this.MapView.Map.MyLocationEnabled = true;

            //http://lbsyun.baidu.com/index.php?title=android-locsdk/guide/getloc
            var opt = new LocationClientOption();
            opt.SetLocationMode(LocationClientOption.LocationMode.HightAccuracy);
            opt.OpenGps = true;
            opt.CoorType = "bd09ll";
            opt.ScanSpan = 1000;
            opt.SetIsNeedAddress(true);
            opt.LocationNotify = true;
            opt.SetIsNeedLocationPoiList(true);
            opt.SetIgnoreKillProcess(false);
            opt.SetIgnoreCacheException(true);
            opt.SetEnableSimulateGps(false);

            //option.setLocationMode(LocationMode.Hight_Accuracy);//可选，默认高精度，设置定位模式，高精度，低功耗，仅设备
            //option.setCoorType("bd09ll");//可选，默认gcj02，设置返回的定位结果坐标系
            //int span = 1000;
            //option.setScanSpan(span);//可选，默认0，即仅定位一次，设置发起定位请求的间隔需要大于等于1000ms才是有效的
            //option.setIsNeedAddress(true);//可选，设置是否需要地址信息，默认不需要
            //option.setOpenGps(true);//可选，默认false,设置是否使用gps
            //option.setLocationNotify(true);//可选，默认false，设置是否当gps有效时按照1S1次频率输出GPS结果
            //option.setIsNeedLocationDescribe(true);//可选，默认false，设置是否需要位置语义化结果，可以在BDLocation.getLocationDescribe里得到，结果类似于“在北京天安门附近”
            //option.setIsNeedLocationPoiList(true);//可选，默认false，设置是否需要POI结果，可以在BDLocation.getPoiList里得到
            //option.setIgnoreKillProcess(false);//可选，默认true，定位SDK内部是一个SERVICE，并放到了独立进程，设置是否在stop的时候杀死这个进程，默认不杀死  
            //option.SetIgnoreCacheException(false);//可选，默认false，设置是否收集CRASH信息，默认收集
            //option.setEnableSimulateGps(false);//可选，默认false，设置是否需要过滤gps仿真结果，默认需要
            //mLocationClient.setLocOption(option);

            this.LocationClient = new LocationClient(this.ApplicationContext, opt);
            this.LocationClient.RegisterLocationListener(this.LocationListener);
            this.LocationListener.OnLocated += LocationListener_OnLocated;

            this.LocationClient.Start();
        }

        private void LocationListener_OnLocated(object sender, MyLocationListener.LocationEventArgs e) {
            var locationData = new MyLocationData.Builder()
                .Accuracy(e.Location.Radius)
                .Direction(100)
                .Latitude(e.Location.Latitude)
                .Longitude(e.Location.Longitude)
                .Build();

            this.MapView.Map.SetMyLocationData(locationData);


            var maker = BitmapDescriptorFactory
                .FromResource(Resource.Drawable.marker);
            var config = new MyLocationConfiguration(MyLocationConfiguration.LocationMode.Normal, true, maker);
            this.MapView.Map.SetMyLocationConfigeration(config);
        }

        protected override void OnDestroy() {
            base.OnDestroy();

            this.MapView.OnDestroy();
            this.LocationClient.Dispose();
            //this.LocationListener.Dispose();
        }

        protected override void OnResume() {
            base.OnResume();
            this.MapView.OnResume();
        }
    }
}

