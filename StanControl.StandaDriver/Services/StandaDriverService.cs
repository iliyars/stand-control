using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ximc;

namespace StanControl.StandaDriver.Services
{
    public class StandaDriverService
    {
        public  Result res;
        private  IntPtr device_enumeration;

        /// <summary>
        /// Имя контроллера Standa.
        /// </summary>
        public  string deviceName { get; private set; }
        /// <summary>
        /// Id контроллера Standa.
        /// </summary>
        public  int DeviceId;
        /// <summary>
        /// Структура с параметарми контроллера.
        /// </summary>
        public  status_t Status;

        #region внутренние функции и поля для логирования API.xmic
        // Probe flags, used to enable various enumeration options
        const int probe_flags = (int)(Flags.ENUMERATE_PROBE | Flags.ENUMERATE_NETWORK);
        // Enumeration hint, currently used to indicate ip address for network enumeration
        String enumerate_hints = "addr=192.168.1.1,172.16.2.3";
        // String enumerate_hints = "addr="; // this hint will use broadcast enumeration, if ENUMERATE_NETWORK flag is enabled

        public static API.LoggingCallback callback;
        public static void MyLog(API.LogLevel loglevel, string message, IntPtr user_data)
        {
            using (StreamWriter sr = new StreamWriter("log.txt", true))
            {
                sr.WriteLine("MyLog {0}: {1}", loglevel, message);
            }
            System.Console.WriteLine("MyLog {0}: {1}", loglevel, message);

        }
        #endregion
        /// <summary>
        /// Подключается к контроллеру Standa.
        /// </summary>
        public void ConnectStanda()
        {
            //  Sets bindy (network) keyfile. Must be called before any call to "enumerate_devices" or "open_device" if you
            //  wish to use network-attached controllers. Accepts both absolute and relative paths, relative paths are resolved
            //  relative to the process working directory. If you do not need network devices then "set_bindy_key" is optional.
            API.set_bindy_key("keyfile.sqlite");

            // Enumerates all devices
            device_enumeration = API.enumerate_devices(probe_flags, enumerate_hints);
            if (device_enumeration == null)
                throw new Exception("Error enumerating devices");

            // Gets found devices count
            int device_count = API.get_device_count(device_enumeration);
            // Gets device name
            String dev = API.get_device_name(device_enumeration, 0);
            deviceName = dev;
            DeviceId = API.open_device(deviceName);
            System.Console.WriteLine(DeviceId);
            res = API.get_status(DeviceId, out Status);
            if (res != Result.ok)
                throw new Exception("Error " + res.ToString());

        }



    }
}
