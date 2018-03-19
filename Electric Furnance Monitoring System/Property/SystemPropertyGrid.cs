using System;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.InteropServices;

namespace Electric_Furnance_Monitoring_System
{
    class SystemPropertyGrid
    {

        MainForm main;

        public SystemPropertyGrid(MainForm _main)
        {
            this.main = _main;
        }


        #region Variables

        // [File]
        private uint File_numberOfDataRecord = 0;
        private uint File_currentDataRecord = 0;

        // [Camera]
        private string Camera_CameraType = "";
        private string Camera_SerialNumber = "";
        private string Camera_DetectorTemperature = "";
        private string Camera_CameraTemperature = "";

        // [Document Size]
        private string DocSize_Pixel = "";

        // [Data Acquisition]
        private string DataAcq_Date = "";
        private string DataAcq_Time = "";
        private uint DataAcq_Index = 0;
        private string DataAcq_MeasurementRange = "";
        private string DataAcq_AcquisitionFrequency = "";

        // [Measurement Object]
        private float MeasurementObject_Emissivity = 0.0f;
        private float MeasurementObject_Transmission = 0.0f;
        private float MeasurementObject_AmbientTemperature = 0.0f;

        // [Scaling]
        private string Scale_ColorBar = "";
        private ushort Scale_DColorBar = 0;
        private string Scale_NumberOfColor = "";
        private ushort Scale_DNumberofColor = 0;
        //private uint Scale_NumberOfColor = 0;
        private float Scale_Maximum = 0.0f;
        private float Scale_Minimun = 0.0f;
        private float Scale_IsoTherm = 0.0f;

        // [Measuerment]
        private float Measurement_HFOV = 0.0f;
        private float Measurement_VFOV = 0.0f;
        private float Measurement_IncidnetAngle = 0.0f;
        private float Measurement_Distance = 0.0f;

        // [Data Logging]
        private string DataLog_RawDataLocation = "";
        private string DataLog_ResultDataLocation = "";

        // [For Calculate Something]
        private float fav = 0.0f;
        private ushort avg = 0;
        private float Threshold_Temperature = 0.0f;

        #endregion

        #region DefineAttribute_File

        [Category("\t\t\t\t\t\t\tFile"), DefaultValue("0"), ReadOnly(true)]

        public uint NumberOfDataRecord
        {
            get { return File_numberOfDataRecord; }
            set { File_numberOfDataRecord = value; }
        }

        [Category("\t\t\t\t\t\t\tFile"), DefaultValue("0"), ReadOnly(true)]
        public uint CurrentDataRecord
        {
            get { return File_currentDataRecord; }
            set { File_currentDataRecord = value; }
        }

        #endregion

        #region DefineAttribute_Camera

        [Category("\t\t\t\t\t\tCamera"), DefaultValue("-"), ReadOnly(true)]
        public string CameraType
        {
            get { return Camera_CameraType; }
            set { Camera_CameraType = value; }
        }

        [Category("\t\t\t\t\t\tCamera"), DefaultValue("-"), ReadOnly(true)]
        public string SerialNumber
        {
            get { return Camera_SerialNumber; }
            set { Camera_SerialNumber = value; }
        }

        [Category("\t\t\t\t\t\tCamera"), DefaultValue("-"), ReadOnly(true)]
        public string DetectorTemperature
        {
            get { return Camera_DetectorTemperature; }
            set { Camera_DetectorTemperature = value; }
        }

        [Category("\t\t\t\t\t\tCamera"), DefaultValue("-"), ReadOnly(true)]
        public string CameraTemperature
        {
            get { return Camera_CameraTemperature; }
            set { Camera_CameraTemperature = value; }
        }

        #endregion

        #region DefineAttribute_DocumentSize

        [Category("\t\t\t\t\tDocument Size"), DefaultValue("-"), ReadOnly(true)]
        public string Pixel
        {
            get { return DocSize_Pixel; }
            set { DocSize_Pixel = value; }
        }

        #endregion

        #region DefineAttribute_DataAcquisition

        [CategoryAttribute("\t\t\t\tData Acquisition"),
        ReadOnlyAttribute(true)]
        //[RefreshProperties(RefreshProperties.Repaint)]
        //[NotifyParentProperty(true)]
        public string Date
        {
            get
            {
                //string tmpStr = string.Empty;
                string tmpStr = DataAcq_Date;
                //string strDate = string.Empty;

                //if (main.device_running == true)
                //{
                //    int year = 0, month = 0, day = 0, hour = 0, min = 0, sec = 0, msec = 0;

                //    if ((DIASDAQ.DDAQ_IRDX_ACQUISITION_GET_TIMESTAMP(main.pIRDX_Array[0], ref year, ref month, ref day, ref hour, ref min, ref sec, ref msec) !=
                //        DIASDAQ.DDAQ_ERROR.NO_ERROR)) return "";

                //    strDate = "" + year + "-" + month + "-" + day;

                //    tmpStr = strDate;
                //}
                //else
                //    tmpStr = "";

                return tmpStr;
            }
            set { value = DataAcq_Date; }
        }

        [CategoryAttribute("\t\t\t\tData Acquisition"),
        ReadOnlyAttribute(true)]
        public string Time
        {
            get
            {
                string tmpStr = string.Empty;


                //s.Clear();
                //s.Append(hour + ":" + min + ":" + sec + ":" + msec);
                //DataAcq_Time = s.ToString();



                if (main.DetectedDevices != 0)
                    tmpStr = DataAcq_Time;
                else
                    tmpStr = "";

                return tmpStr;
            }
            set {   }
        }

        [CategoryAttribute("\t\t\t\tData Acquisition"),
        DefaultValueAttribute("0"),
        ReadOnlyAttribute(true)]
        public uint Index
        {
            get
            {
                uint tmpUint = 0;

                if (main.DetectedDevices != 0)
                    tmpUint = main.acq_index;
                else
                    tmpUint = 0;

                return tmpUint;
            }
            set { }
        }

        [CategoryAttribute("\t\t\t\tData Acquisition"),
        ReadOnlyAttribute(true)]
        public string MeasurementRange
        {
            get
            {
                string tmpStr = string.Empty;

                if (main.DetectedDevices != 0)
                    tmpStr = DataAcq_MeasurementRange;
                else
                    tmpStr = "";

                return tmpStr;
            }
            set { }
        }

        //// Acquisition Frequency ========================================
        //private string _cbData = "김,이,박,최,정,윤,장,비,방";

        //private void initCombo()
        //{
        //    string[] _data = "김,이,박,최,정,윤,장,비,방".Split(',');
        //    ComboData._datas = _cbData.Split(',');

        //}

        [Category("\t\t\t\tData Acquisition"), Browsable(true), TypeConverter(typeof(NameConverter))]
        public string AcquisitionFrequency
        {
            get
            {
                string tmpstr = string.Empty;

                setAcquisitionFreq(main.pIRDX_Array[0]);
                //setAcquisitionFreq(main.pIRDX_Array[1]);

                if (DataAcq_AcquisitionFrequency != "")
                    tmpstr = DataAcq_AcquisitionFrequency;
                else
                {
                    tmpstr = ComboData._datas[0];
                }
                return tmpstr;
            }
            set
            {
                DataAcq_AcquisitionFrequency = value;                           // Property 설정한 값 받아옴 (ex. # (#.#Hz))
                string Acq_Frq_Value = "";                                      // # (#.#Hz) -> # : 숫자 문자열으로 받아옴

                int Acq_Num = DataAcq_AcquisitionFrequency.IndexOf(' ');        // 공백이 들어있는 index 값 받아옴
                if (Acq_Num == -1) return;                                      // 만약 공백이 없을 경우 return;

                Acq_Frq_Value = DataAcq_AcquisitionFrequency.Remove(Acq_Num);   // Avg를 얻기 위해 첫 공백 뒤 부터 Remove
                ushort.TryParse(Acq_Frq_Value, out avg);                        // 16비트 부호 없는 정수로 변환

                DIASDAQ.DDAQ_DEVICE_DO_STOP(main.DetectedDevices);
                // err code: NOT STOP MODE
                DIASDAQ.DDAQ_IRDX_ACQUISITION_SET_AVERAGING(main.pIRDX_Array[0], avg);
                DIASDAQ.DDAQ_DEVICE_DO_START(main.DetectedDevices);

                DIASDAQ.DDAQ_DEVICE_DO_STOP(1);
                DIASDAQ.DDAQ_IRDX_ACQUISITION_SET_AVERAGING(main.pIRDX_Array[1], avg);
                DIASDAQ.DDAQ_DEVICE_DO_START(1);

                //for(uint i=0; i<main.DetectedDevices; i++)
                //{
                //    DIASDAQ.DDAQ_DEVICE_DO_STOP(i);
                //    DIASDAQ.DDAQ_IRDX_ACQUISITION_SET_AVERAGING(main.pIRDX_Array[i], avg);
                //    DIASDAQ.DDAQ_DEVICE_DO_START(i);
                //}
            }
        }
        #endregion
        float fps = 0.0f;

        private void setAcquisitionFreq(IntPtr irdxHandle)
        {
            float fav = 0.0f;
            StringBuilder s = new StringBuilder(20);            // dispose_
            string[] temp_acq = new string[10];
            string ss = "";
            DIASDAQ.DDAQ_IRDX_DEVICE_GET_INTERNALFPS(irdxHandle, ref fps);

            if (fps > 0)
            {
                avg = 1;

                for (int i = 1; i < 10; i++)
                {
                    fav = fps / avg;
                    s.Clear();
                    s.Append(avg + " (" + Math.Round(fav, 1) + " Hz)");
                    ss += s.ToString() + ",";
                    temp_acq[i] = ss;

                    avg *= 2;
                }
                ss.Remove(ss.Length - 1);

                ComboData._datas = ss.Split(',');
            }
            else
            {
                DIASDAQ.DDAQ_IRDX_ACQUISITION_GET_AVERAGING(irdxHandle, ref avg);
                s.Clear();
                s.Append("AV = " + avg);
                ss = s.ToString();
                ComboData._datas = ss.Split(',');
            }
            s = null;
        }

        #region DefineAttribute_MeasurementObject

        [CategoryAttribute("\t\t\tMeasuerment Object")]
        public float Emissivity
        {
            get
            {
                float tmpVal = 0.0f;

                if (main.currentEmissivity != 0.0f)
                    tmpVal = main.currentEmissivity;
                else
                    tmpVal = 0.0f;

                return tmpVal;
            }
            set
            {
                float fVal = value;

                if (fVal < 0.1f)       // 방사율 0.1 이하의 값 입력 시
                    main.currentEmissivity = 0.1f;
                else if (fVal > 1.0f)      // 방사율 1 이상의 값 입력 시
                    main.currentEmissivity = 1.0f;
                else                        // ( 0.1 <= x || 1 >= x )
                    main.currentEmissivity = fVal;

                for (int i = 0; i < main.DetectedDevices; i++)
                    DIASDAQ.DDAQ_IRDX_OBJECT_SET_EMISSIVITY(main.pIRDX_Array[i], main.currentEmissivity);
            }
        }

        [CategoryAttribute("\t\t\tMeasuerment Object")]
        public float Transmission
        {
            get
            {
                float tmpVal = 0.0f;

                if (main.currentTransmitance != 0.0f)
                    tmpVal = main.currentTransmitance;
                else
                    tmpVal = 0.0f;

                return tmpVal;
            }
            set
            {
                float fVal = value;

                if (fVal < 0.1f)                   // 투과율 0.1 이하의 값 입력 시
                    main.currentTransmitance = 0.1f;
                else if (fVal > 1.0f)               // 투과율 0.1 이하의 값 입력 시
                    main.currentTransmitance = 1.0f;
                else                                // 투과율 0.1 이하의 값 입력 시
                    main.currentTransmitance = fVal;

                for (int i = 0; i < main.DetectedDevices; i++)
                    DIASDAQ.DDAQ_IRDX_OBJECT_SET_TRANSMISSION(main.pIRDX_Array[i], main.currentTransmitance);
            }
        }

        [CategoryAttribute("\t\t\tMeasuerment Object")]
        public float AmbientTemperature
        {
            get
            {
                MeasurementObject_AmbientTemperature = main.currentAmbientTemp;
                return MeasurementObject_AmbientTemperature;
            }
            set
            {
                MeasurementObject_AmbientTemperature = value;
                main.currentAmbientTemp = value;

                for (int i = 0; i < main.DetectedDevices; i++)
                    DIASDAQ.DDAQ_IRDX_OBJECT_SET_AMBIENTTEMP(main.pIRDX_Array[i], MeasurementObject_AmbientTemperature);
            }
        }

        #endregion

        #region DefineAttribute_Scaling

        string colorBar_list = "SYMIICON,MULTICOLOR,SPECTRUM,THERMO,GRAY,GRAYPLUS,HOTMETAL,IRON,LIGHT"; // constant_values

        [Category("\t\tScaling"), Browsable(true), TypeConverter(typeof(ColorBarConverter))]
        public string ColorBar
        {
            get
            {
                string tmpstr = string.Empty;

                if (Scale_ColorBar != "")        // 이 조건문의 필요성 한번 더 생각
                    tmpstr = Scale_ColorBar;
                else
                {
                    if (main.DetectedDevices != 0)
                    {
                        ColorBarData._datas = colorBar_list.Split(',');
                        Scale_ColorBar = ColorBarData._datas[0];
                    }

                    tmpstr = "";
                }

                return tmpstr;
            }
            set
            {
                Scale_ColorBar = value;

                if (Scale_ColorBar == "SYMIICON") Scale_DColorBar = 0;
                else if (Scale_ColorBar == "MULTICOLOR") Scale_DColorBar = 1;
                else if (Scale_ColorBar == "SPECTRUM") Scale_DColorBar = 2;
                else if (Scale_ColorBar == "THERMO") Scale_DColorBar = 3;
                else if (Scale_ColorBar == "GRAY") Scale_DColorBar = 4;
                else if (Scale_ColorBar == "GRAYPLUS") Scale_DColorBar = 5;
                else if (Scale_ColorBar == "HOTMETAL") Scale_DColorBar = 6;
                else if (Scale_ColorBar == "IRON") Scale_DColorBar = 7;
                else if (Scale_ColorBar == "LIGHT") Scale_DColorBar = 8;

                //ushort.TryParse(NumberOfColors, )
                Scale_DNumberofColor = Convert.ToUInt16(NumberOfColors);
                for (int i = 0; i < main.DetectedDevices; i++)
                    DIASDAQ.DDAQ_IRDX_PALLET_SET_BAR(main.pIRDX_Array[i], (DIASDAQ.DDAQ_PALLET)Scale_DColorBar, Scale_DNumberofColor);
                //DIASDAQ.DDAQ_IRDX_PALLET_SET_BAR(main.pIRDX_Array[i], (DIASDAQ.DDAQ_PALLET)Scale_DColorBar, (ushort)NumberOfColors);
            }
        }

        private string NumofColor_list = "256,128,63,31,21,15,11,5";      // constant_values

        [Category("\t\tScaling"), Browsable(true), TypeConverter(typeof(NumofColorConverter))]
        public string NumberOfColors
        {
            get
            {
                string tmpstr = string.Empty;

                if (Scale_NumberOfColor != "")
                    tmpstr = Scale_NumberOfColor;
                else
                {
                    if (main.DetectedDevices != 0)
                    {
                        NumofColorData._datas = NumofColor_list.Split(',');
                        Scale_NumberOfColor = NumofColorData._datas[0];
                    }


                    tmpstr = "";
                }

                return tmpstr;
            }
            set
            {
                Scale_NumberOfColor = value;

                ushort.TryParse(Scale_NumberOfColor, out Scale_DNumberofColor);

                for (int i = 0; i < main.DetectedDevices; i++)
                    DIASDAQ.DDAQ_IRDX_PALLET_SET_BAR(main.pIRDX_Array[i], (DIASDAQ.DDAQ_PALLET)Scale_DColorBar, Scale_DNumberofColor);
            }
        }

        [CategoryAttribute("\t\tScaling")]
        public float Maximum
        {
            get
            {
                if (main.cMaxTemp != 0.0f)
                    Scale_Maximum = main.cMaxTemp;
                else
                    Scale_Maximum = 0.0f;

                return Scale_Maximum;
            }
            set
            {
                float fVal = value;

                if (fVal > main.Scale_MaxTemp)
                    main.cMaxTemp = main.Scale_MaxTemp;
                else if (fVal <= main.cMinTemp)
                    main.cMaxTemp = main.cMinTemp + 1.0f;
                else
                    main.cMaxTemp = fVal;

                //for (int i = 0; i < main.DetectedDevices; i++)
                //    DIASDAQ.DDAQ_IRDX_SCALE_SET_MINMAX(main.pIRDX_Array[i], main.cMinTemp, main.cMaxTemp);
                DIASDAQ.DDAQ_IRDX_SCALE_SET_MINMAX(main.pIRDX_Array[0], main.cMinTemp, main.cMaxTemp);      // for test
                DIASDAQ.DDAQ_IRDX_SCALE_SET_MINMAX(main.pIRDX_Array[1], main.cMinTemp, main.cMaxTemp);
            }
        }

        [CategoryAttribute("\t\tScaling")]
        public float Minimun
        {
            get
            {
                if (main.cMinTemp != 0.0f)
                    Scale_Minimun = main.cMinTemp;
                else
                    Scale_Minimun = 0.0f;

                return Scale_Minimun;
            }
            set
            {
                float fVal = value;

                if (fVal < main.Scale_MinTemp)
                    main.cMinTemp = main.Scale_MinTemp;
                else if (fVal >= main.cMaxTemp)
                    main.cMinTemp = main.cMaxTemp - 1.0f;
                else
                    main.cMinTemp = fVal;

                //for (int i = 0; i < main.DetectedDevices; i++)
                //    DIASDAQ.DDAQ_IRDX_SCALE_SET_MINMAX(main.pIRDX_Array[i],  main.cMinTemp, main.cMaxTemp);
                DIASDAQ.DDAQ_IRDX_SCALE_SET_MINMAX(main.pIRDX_Array[0], main.cMinTemp, main.cMaxTemp);      // for test
                DIASDAQ.DDAQ_IRDX_SCALE_SET_MINMAX(main.pIRDX_Array[1], main.cMinTemp, main.cMaxTemp);
            }
        }

        //[CategoryAttribute("\t\tScaling")]
        //public float Iso_Therm
        //{
        //    get
        //    {
        //        return Scale_IsoTherm;
        //    }
        //    set
        //    {
        //        Scale_IsoTherm = value;
        //    }
        //}

        #endregion

        #region DefineAttribute_Measurement

        //[CategoryAttribute("\tMeasurement")]
        //public float HFOV
        //{
        //    get { return Measurement_HFOV; }
        //    set { Measurement_HFOV = value; }
        //}

        //[CategoryAttribute("\tMeasurement")]
        //public float VFOV
        //{
        //    get { return Measurement_VFOV; }
        //    set { Measurement_VFOV = value; }
        //}

        //[CategoryAttribute("\tMeasurement")]
        //public float IncidentAngle
        //{
        //    get { return Measurement_IncidnetAngle; }
        //    set { Measurement_IncidnetAngle = value; }
        //}

        //[CategoryAttribute("\tMeasurement")]
        //public float Distance
        //{
        //    get { return Measurement_Distance; }
        //    set { Measurement_Distance = value; }
        //}

        #endregion

        #region DefineAttribute_DataLogging

        private class FolderNameEditor : UITypeEditor
        {
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                FolderBrowser2 browser = new FolderBrowser2();
                if (value != null)
                {
                    browser.DirectoryPath = string.Format("{0}", value);
                }

                if (browser.ShowDialog(null) == System.Windows.Forms.DialogResult.OK)
                    return browser.DirectoryPath;

                return value;
            }
        }

        public class FolderBrowser2
        {
            public string DirectoryPath { get; set; }

            public System.Windows.Forms.DialogResult ShowDialog(System.Windows.Forms.IWin32Window owner)
            {
                IntPtr hwndOwner = owner != null ? owner.Handle : GetActiveWindow();

                IFileOpenDialog dialog = (IFileOpenDialog)new FileOpenDialog();
                try
                {
                    IShellItem item;
                    if (!string.IsNullOrEmpty(DirectoryPath))
                    {
                        IntPtr idl;
                        uint atts = 0;
                        if (SHILCreateFromPath(DirectoryPath, out idl, ref atts) == 0)
                        {
                            if (SHCreateShellItem(IntPtr.Zero, IntPtr.Zero, idl, out item) == 0)
                            {
                                dialog.SetFolder(item);
                            }
                        }
                    }
                    dialog.SetOptions(FOS.FOS_PICKFOLDERS | FOS.FOS_FORCEFILESYSTEM);
                    uint hr = dialog.Show(hwndOwner);
                    if (hr == ERROR_CANCELLED)
                        return System.Windows.Forms.DialogResult.Cancel;

                    if (hr != 0)
                        return System.Windows.Forms.DialogResult.Abort;

                    dialog.GetResult(out item);
                    string path;
                    item.GetDisplayName(SIGDN.SIGDN_FILESYSPATH, out path);
                    DirectoryPath = path;
                    return System.Windows.Forms.DialogResult.OK;
                }
                finally
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(dialog);
                }
            }

            [DllImport("shell32.dll")]
            private static extern int SHILCreateFromPath([System.Runtime.InteropServices.MarshalAs(UnmanagedType.LPWStr)] string pszPath, out IntPtr ppIdl, ref uint rgflnOut);

            [DllImport("shell32.dll")]
            private static extern int SHCreateShellItem(IntPtr pidlParent, IntPtr psfParent, IntPtr pidl, out IShellItem ppsi);

            [DllImport("user32.dll")]
            private static extern IntPtr GetActiveWindow();

            private const uint ERROR_CANCELLED = 0x800704C7;

            [ComImport]
            [Guid("DC1C5A9C-E88A-4dde-A5A1-60F82A20AEF7")]
            private class FileOpenDialog
            {
            }

            [ComImport]
            [Guid("42f85136-db7e-439c-85f1-e4075d135fc8")]
            [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            private interface IFileOpenDialog
            {
                [PreserveSig]
                uint Show([In] IntPtr parent); // IModalWindow
                void SetFileTypes();  // not fully defined
                void SetFileTypeIndex([In] uint iFileType);
                void GetFileTypeIndex(out uint piFileType);
                void Advise(); // not fully defined
                void Unadvise();
                void SetOptions([In] FOS fos);
                void GetOptions(out FOS pfos);
                void SetDefaultFolder(IShellItem psi);
                void SetFolder(IShellItem psi);
                void GetFolder(out IShellItem ppsi);
                void GetCurrentSelection(out IShellItem ppsi);
                void SetFileName([In, MarshalAs(UnmanagedType.LPWStr)] string pszName);
                void GetFileName([MarshalAs(UnmanagedType.LPWStr)] out string pszName);
                void SetTitle([In, MarshalAs(UnmanagedType.LPWStr)] string pszTitle);
                void SetOkButtonLabel([In, MarshalAs(UnmanagedType.LPWStr)] string pszText);
                void SetFileNameLabel([In, MarshalAs(UnmanagedType.LPWStr)] string pszLabel);
                void GetResult(out IShellItem ppsi);
                void AddPlace(IShellItem psi, int alignment);
                void SetDefaultExtension([In, MarshalAs(UnmanagedType.LPWStr)] string pszDefaultExtension);
                void Close(int hr);
                void SetClientGuid();  // not fully defined
                void ClearClientData();
                void SetFilter([MarshalAs(UnmanagedType.Interface)] IntPtr pFilter);
                void GetResults([MarshalAs(UnmanagedType.Interface)] out IntPtr ppenum); // not fully defined
                void GetSelectedItems([MarshalAs(UnmanagedType.Interface)] out IntPtr ppsai); // not fully defined
            }

            [ComImport]
            [Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE")]
            [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
            private interface IShellItem
            {
                void BindToHandler(); // not fully defined
                void GetParent(); // not fully defined
                void GetDisplayName([In] SIGDN sigdnName, [MarshalAs(UnmanagedType.LPWStr)] out string ppszName);
                void GetAttributes();  // not fully defined
                void Compare();  // not fully defined
            }

            private enum SIGDN : uint
            {
                SIGDN_DESKTOPABSOLUTEEDITING = 0x8004c000,
                SIGDN_DESKTOPABSOLUTEPARSING = 0x80028000,
                SIGDN_FILESYSPATH = 0x80058000,
                SIGDN_NORMALDISPLAY = 0,
                SIGDN_PARENTRELATIVE = 0x80080001,
                SIGDN_PARENTRELATIVEEDITING = 0x80031001,
                SIGDN_PARENTRELATIVEFORADDRESSBAR = 0x8007c001,
                SIGDN_PARENTRELATIVEPARSING = 0x80018001,
                SIGDN_URL = 0x80068000
            }

            [Flags]
            private enum FOS
            {
                FOS_ALLNONSTORAGEITEMS = 0x80,
                FOS_ALLOWMULTISELECT = 0x200,
                FOS_CREATEPROMPT = 0x2000,
                FOS_DEFAULTNOMINIMODE = 0x20000000,
                FOS_DONTADDTORECENT = 0x2000000,
                FOS_FILEMUSTEXIST = 0x1000,
                FOS_FORCEFILESYSTEM = 0x40,
                FOS_FORCESHOWHIDDEN = 0x10000000,
                FOS_HIDEMRUPLACES = 0x20000,
                FOS_HIDEPINNEDPLACES = 0x40000,
                FOS_NOCHANGEDIR = 8,
                FOS_NODEREFERENCELINKS = 0x100000,
                FOS_NOREADONLYRETURN = 0x8000,
                FOS_NOTESTFILECREATE = 0x10000,
                FOS_NOVALIDATE = 0x100,
                FOS_OVERWRITEPROMPT = 2,
                FOS_PATHMUSTEXIST = 0x800,
                FOS_PICKFOLDERS = 0x20,
                FOS_SHAREAWARE = 0x4000,
                FOS_STRICTFILETYPES = 4
            }
        }

        [Category("Data Logging"), Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string RawData_Location
        {
            get { return DataLog_RawDataLocation; }
            set { DataLog_RawDataLocation = value; }
        }

        [Category("Data Logging"), Editor(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string ResultData_Location
        {
            get { return DataLog_ResultDataLocation; }
            set { DataLog_ResultDataLocation = value; }
        }

        #endregion

        #region DefineAttribute_AlarmThreshold

        //[CategoryAttribute("Threshold")]
        //public float Threshold
        //{
        //    get { return Threshold_Temperature; }
        //    set { Threshold_Temperature = value; }
        //}

        #endregion

        private delegate void RefreshPropertyGrid();
        public void SafeRefresh_PropertyGrid()
        {
            if (main.propertyGrid1.InvokeRequired)
            {
                RefreshPropertyGrid rpg = new RefreshPropertyGrid(SafeRefresh_PropertyGrid);
                //main.propertyGrid1.Invoke(rpg);
                main.propertyGrid1.Invoke(rpg);
            }
            else
            {
                GetAttributesInfo(main.pIRDX_Array[0]);
            }
        }

        private delegate void TimeRefreshOnly();
        public void SafeRefresh_Time()
        {
            if (main.propertyGrid1.InvokeRequired)
            {
                TimeRefreshOnly test = new TimeRefreshOnly(SafeRefresh_Time);
                main.propertyGrid1.Invoke(test);
            }
            else
            {
                Test_TimeRefresh(main.pIRDX_Array[0]);
            }
        }

        public void Test_TimeRefresh(IntPtr irdxHandle)
        {
            StringBuilder s = new StringBuilder(50);
            string strDate = "", strTime = "";

            int year = 0, month = 0, day = 0, hour = 0, min = 0, sec = 0, msec = 0;

            if ((DIASDAQ.DDAQ_IRDX_ACQUISITION_GET_TIMESTAMP(irdxHandle, ref year, ref month, ref day, ref hour, ref min, ref sec, ref msec) !=
                DIASDAQ.DDAQ_ERROR.NO_ERROR)) return;

            s.Clear();
            s.Append(year + "-" + month + "-" + day);
            DataAcq_Date = s.ToString();

            s.Clear();
            s.Append(hour + ":" + min + ":" + sec + ":" + msec);
            DataAcq_Time = s.ToString();

            //main.propertyGrid1.Invoke();
            main.propertyGrid1.Refresh();
        }

        public void GetAttributesInfo(IntPtr irdxHandle)
        {
            
            StringBuilder s = new StringBuilder(64);

            #region Attribute_File

            DIASDAQ.DDAQ_IRDX_FILE_GET_NUMDATASETS(irdxHandle, ref File_numberOfDataRecord);
            File_numberOfDataRecord = main.NumOfDataRecord;

            DIASDAQ.DDAQ_IRDX_FILE_GET_CURDATASET(irdxHandle, ref File_currentDataRecord);
            File_currentDataRecord = main.CurDataRecord;

            #endregion

            #region Attribute_Camera

            uint type = 0;
            byte[] deviceID = new byte[64];
            DIASDAQ.DDAQ_DEVICE_TYPE id = 0;

            DIASDAQ.DDAQ_DEVICE_GET_IDSTRING(main.DetectedDevices, deviceID, 64);
            DIASDAQ.DDAQ_DEVICE_GET_ID(main.DetectedDevices, ref id, ref type);

            string s2 = "";
            switch (type)
            {
                case (uint)DIASDAQ.DDAQ_DEVICE_TYPE.VIEW_100:
                    s2 = DIASDAQ.DDAQ_DEVICE_TYPE.VIEW_100.ToString();
                    break;
                case (uint)DIASDAQ.DDAQ_DEVICE_TYPE.VIEW_256:
                    s2 = DIASDAQ.DDAQ_DEVICE_TYPE.VIEW_256.ToString();
                    break;
                case (uint)DIASDAQ.DDAQ_DEVICE_TYPE.VIEW_320:
                    s2 = DIASDAQ.DDAQ_DEVICE_TYPE.VIEW_320.ToString();
                    break;
                case (uint)DIASDAQ.DDAQ_DEVICE_TYPE.MIDAS:
                    s2 = DIASDAQ.DDAQ_DEVICE_TYPE.MIDAS.ToString();
                    break;
                case (uint)DIASDAQ.DDAQ_DEVICE_TYPE.MODULE_128:
                    s2 = DIASDAQ.DDAQ_DEVICE_TYPE.MODULE_128.ToString();
                    break;
                case (uint)DIASDAQ.DDAQ_DEVICE_TYPE.LINE_128:
                    s2 = DIASDAQ.DDAQ_DEVICE_TYPE.LINE_128.ToString();
                    break;
                case (uint)DIASDAQ.DDAQ_DEVICE_TYPE.LINE_256:
                    s2 = DIASDAQ.DDAQ_DEVICE_TYPE.LINE_256.ToString();
                    break;
                case (uint)DIASDAQ.DDAQ_DEVICE_TYPE.HZK_160:
                    s2 = DIASDAQ.DDAQ_DEVICE_TYPE.HZK_160.ToString();
                    break;
                case (uint)DIASDAQ.DDAQ_DEVICE_TYPE.HZK_256:
                    s2 = DIASDAQ.DDAQ_DEVICE_TYPE.HZK_256.ToString();
                    break;
                case 52:
                    s2 = "VIEW_512";
                    break;
                default:
                    s2 = DIASDAQ.DDAQ_DEVICE_TYPE.NO.ToString();
                    break;
            }
            s.Clear();
            s.Append("PYRO" + s2);
            Camera_CameraType = s.ToString();

            char[] tempDevID = new char[64];
            s.Clear();
            for (int i = 0; i < 64; i++)
            {
                tempDevID[i] = (char)deviceID[i];
                string tmp = tempDevID[i].ToString();
                s.Append(tmp);
            }
            //s.Append(id.ToString());
            Camera_SerialNumber = s.ToString();

            bool ok = false;
            float temp = 0.0f;

            DIASDAQ.DDAQ_IRDX_DEVICE_GET_DETECTORTEMP(irdxHandle, ref temp, ref ok);
            s.Clear();
            s.Append(temp + " ℃");
            Camera_DetectorTemperature = s.ToString();

            DIASDAQ.DDAQ_IRDX_DEVICE_GET_CAMERATEMP(irdxHandle, ref temp, ref ok);
            s.Clear();
            s.Append(temp + " ℃");
            Camera_CameraTemperature = s.ToString();

            #endregion

            #region Attribute_DocumentSize 

            ushort x = 0, y = 0;
            DIASDAQ.DDAQ_IRDX_PIXEL_GET_SIZE(irdxHandle, ref x, ref y);
            s.Clear();
            s.Append(x + " × " + y);
            DocSize_Pixel = s.ToString();

            #endregion

            #region Attribute_DataAcquisition

            string strDate = "", strTime = "";

            int year = 0, month = 0, day = 0, hour = 0, min = 0, sec = 0, msec = 0;

            if ((DIASDAQ.DDAQ_IRDX_ACQUISITION_GET_TIMESTAMP(irdxHandle, ref year, ref month, ref day, ref hour, ref min, ref sec, ref msec) !=
                DIASDAQ.DDAQ_ERROR.NO_ERROR)) return;

            s.Clear();
            //s.Append(year + "-" + month + "-" + day);
            s.Append("1900-01-01");
            DataAcq_Date = s.ToString();

            s.Clear();
            s.Append(hour + ":" + min + ":" + sec + ":" + msec);
            DataAcq_Time = s.ToString();

            //////////////// Temperature Range ////////////////
            float pMin = 0.0f, pMax = 0.0f;
            for (int i = 0; i < main.DetectedDevices; i++)
            {
                DIASDAQ.DDAQ_IRDX_DEVICE_GET_MRANGEMINMAX(irdxHandle, ref pMin, ref pMax);
                if (i == 0)      // for test
                {
                    main.Scale_MaxTemp = pMax;
                    main.Scale_MinTemp = pMin;
                }
            }

            s.Clear();
            s.Append(pMin + " ~ " + pMax + " ℃");
            DataAcq_MeasurementRange = s.ToString();

            ///////////////////////////////////////////////////


            if (main.m_fps > 0)
            {
                for (int i = 1; i < 10; i++)
                {
                    fav = main.m_fps / avg;

                    s.Clear();
                    s.Append(avg + " (" + fav + " Hz)");
                    ComboData._datas = s.ToString().Split(',');

                    avg *= 2;
                }
                avg = 1;
                DIASDAQ.DDAQ_IRDX_ACQUISITION_GET_AVERAGING(irdxHandle, ref avg);
                main.m_avg = avg;
                fav = main.m_fps / avg;

                s.Clear();
                s.Append(avg + " (" + fav + " Hz)");
                ComboData._datas = s.ToString().Split(',');

                DataAcq_AcquisitionFrequency = ComboData._datas[3];
            }
            else
            {
                DIASDAQ.DDAQ_IRDX_ACQUISITION_GET_AVERAGING(irdxHandle, ref avg);
                main.m_avg = avg;

                s.Clear();
                s.Append("AV = " + avg);
                ComboData._datas = s.ToString().Split(',');
            }

            #endregion

            #region Attribute_MeasurementObject

            float PG_emissivity = 0.0f;
            float PG_transmission = 0.0f;
            float PG_ambient = 0.0f;
            DIASDAQ.DDAQ_IRDX_OBJECT_GET_EMISSIVITY(irdxHandle, ref PG_emissivity);
            main.currentEmissivity = PG_emissivity;
            MeasurementObject_Emissivity = main.currentEmissivity;

            DIASDAQ.DDAQ_IRDX_OBJECT_GET_TRANSMISSION(irdxHandle, ref PG_transmission);
            main.currentTransmitance = PG_transmission;
            MeasurementObject_Transmission = main.currentTransmitance;

            DIASDAQ.DDAQ_IRDX_OBJECT_GET_AMBIENTTEMP(irdxHandle, ref PG_ambient);
            main.currentAmbientTemp = PG_ambient;
            MeasurementObject_AmbientTemperature = main.currentAmbientTemp;

            #endregion

            #region Attribute_Scaling

            #endregion

            #region Attribute_Measurement

            #endregion

            #region Attribute_DataLogging

            #endregion

            //main.propertyGrid1.Refresh();
            //main.propertyGrid1.Invalidate();
        }

        public void UpdateDataSet()
        {
            //return if document is not ready now
            if (this == null) return;

            bool ok = false;
            float temp1 = 0.0f, temp2 = 0.0f;
            //float IRDX_transmisstion = 0.0f, IRDX_emissivity = 0.0f;
            //ushort Pos = 0;
            //ushort Status = 0;

            //string s1 = "", s2 = "";
            //string strDate = "", strTime = "", strPos = "";

            //uint PG_CurDataRecord = 0;

            #region File

            DIASDAQ.DDAQ_IRDX_FILE_GET_CURDATASET(main.pIRDX, ref File_currentDataRecord);
            main.CurDataRecord = File_currentDataRecord;

            #endregion

            #region Camera

            DIASDAQ.DDAQ_IRDX_DEVICE_GET_DETECTORTEMP(main.pIRDX, ref temp1, ref ok);
            DIASDAQ.DDAQ_IRDX_DEVICE_GET_CAMERATEMP(main.pIRDX, ref temp2, ref ok);

            StringBuilder s = new StringBuilder(20);
            s.Append(temp1.ToString() + " ℃");
            Camera_DetectorTemperature = s.ToString();

            s.Clear();
            s.Append(temp2.ToString() + " ℃");
            Camera_CameraTemperature = s.ToString();

            #endregion

            #region DataAcquisition

            // date, time, index만 refresh 해주면 됨


            #endregion
        }


        internal class ComboData
        {
            internal static string[] _datas;
        }
        public class NameConverter : StringConverter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                return true;
            }

            public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(ComboData._datas);
            }

        }
        internal class ColorBarData
        {
            internal static string[] _datas;
        }
        public class ColorBarConverter : StringConverter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                return true;
            }

            public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(ColorBarData._datas);
            }
        }
        internal class NumofColorData
        {
            internal static string[] _datas;
        }
        public class NumofColorConverter : StringConverter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }

            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                return true;
            }

            public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                return new StandardValuesCollection(NumofColorData._datas);
            }
        }

        //[TypeConverter (typeof(ExpandableObjectConverter))]
        //public class TextComponent : INotifyPropertyChanged
        //{
        //    public event PropertyChangedEventHandler PropertyChanged;

        //    protected void OnChanged(string propertyName)
        //    {
        //        if (PropertyChanged != null)
        //        {
        //            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //        }
        //    }
        //    private string text = string.Empty;

        //    [NotifyParentProperty(true)]
        //    public string Text
        //    {
        //        get { return text; }
        //        set
        //        {
        //            text = value;
        //            OnChanged("Text");
        //        }
        //    }
        //}

    }
}