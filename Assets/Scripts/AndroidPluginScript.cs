
using UnityEngine;
using UnityEngine.UI;

public class AndroidPluginScript : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] Button btn;
    private AndroidJavaObject _context;
    private AndroidJavaClass javaClass = null;
    private AndroidJavaObject _instance = null;

    private const string COMPONENT_OBJECT_NAME = "AndroidPlugin";
    private const string REQUEST_PERMISSIONS = "권한을 허용해주셔야 정상적으로 앱 이용이 가능합니다.";
    private const string CAN_NOT_CHECK_NETWORK = "상용망 정보를 확인할 수 없습니다.";
    private const string ACTIVE_MOBILE_NETWORK = "모바일 네트워크를 활성화한 후 다시 시도해주세요.";
    private const string ACTIVE_LOCATION_INFORMATION = "위치 정보 설정을 활성화한 후 다시 시도해주세요.";
    private const string PLEASE_WAIT_2MINUTE = "2분뒤에 다시 시도해주세요";
    private const string PLEASE_ON_OFF_WIFI = "원활한 진행을 위해 와이파이를 껏다가 켜주시기 바랍니다.";
    private string[] REQUIRED_PERMISSION_RELEASE = new string[] {
            "android.permission.ACCESS_FINE_LOCATION",
            "android.permission.ACCESS_COARSE_LOCATION",
            "android.permission.CHANGE_WIFI_STATE",
            "android.permission.READ_PHONE_STATE",
    };

    void Awake()
    {
        //일단 아까 plugin의 context를 설정해주기 위해
        //유니티 자체의 UnityPlayerActivity를 가져옵시다.
        using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            _context = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
            
        }

        //클래스를 불러와줍니다.
        //패키지명 + 클래스명입니다.
        using (javaClass = new AndroidJavaClass("com.iansit.plugin.unityPlugin"))
        {
            if (javaClass != null)
            {
                //아까 싱글톤으로 사용하자고 만들었던 static instance를 불러와줍니다.
                _instance = javaClass.CallStatic<AndroidJavaObject>("instance");
                //Context를 설정해줍니다.
                _instance.Call("setContext", _context);
            }
        }

        btn.onClick.AddListener(() => {
            getMobileData("","","","","","");
        });
        
    }

    /**
    * Get Network Callback
    * Android 권한이 없음
    */
    public void denidedNeworkPermission(){
        _instance.Call("ShowToast",REQUEST_PERMISSIONS);
    }

    /**
    * Get Network Callback
    * GPS 비활성화 상태
    */
    public void disableGps(){
        _instance.Call("ShowToast",ACTIVE_LOCATION_INFORMATION);
    }

    /**
    * Get Network Callback
    * 인터넷 비활성화 상태
    */
    public void disableInternet(){
        _instance.Call("ShowToast",ACTIVE_MOBILE_NETWORK);
    }

    /**
    * Get Network Callback
    * WiFi 검색 횟수 초과
    * 2분뒤에 사용 가능
    */
    public void wifiSearchCountOver(){
        _instance.Call("ShowToast",PLEASE_ON_OFF_WIFI);
        userWifiSet();
    }

    /**
    * Get Network Callback
    * 모바일 네트워크 에러
    */
    public void canNotCheckMobileNetwork(){
        _instance.Call("ShowToast",CAN_NOT_CHECK_NETWORK);
    }

    /**
    * Get Network Callback
    * 모바일 네트워크 가져오기 성공
    */
    public void getNetworkSuccess(string jsonString){
        text.text = jsonString;
    }

    /**
    * Android Id 가져오기
    */
    public string getAndroidId(){
        return _instance.Call<string>("getAndroidID");
    }

    /**
    * Sim Operator 가져오기
    */
    public string getSimOperator(){
        return _instance.Call<string>("getSimOperator");
    }

    
    /**
    * Get Network Callback
    * 모바일 네트워크 가져오기 실행
        objName: String,
        homeId: String,
        measurementMode: String,
        measurementSequence: String,
        measurementId:String,
        measurementX:String,
        measurementY:String
    */
    public void getMobileData(string homeId,
    string measurementMode,
    string measurementSequence,
    string measurementId,
    string measurementX,
    string measurementY){
        _instance.Call("getNetworkInfo",
        COMPONENT_OBJECT_NAME,
        homeId,
        measurementMode,
        measurementSequence,
        measurementId,
        measurementX,
        measurementY);
    }

    /**
    * Android Wifi 설정창 띄우기
    */
    public void userWifiSet(){
        _instance.Call("userWifiSet");
    }


    void Update(){

    }

}
