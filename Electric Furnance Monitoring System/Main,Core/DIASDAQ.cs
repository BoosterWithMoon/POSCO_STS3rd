using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Electric_Furnance_Monitoring_System
{
    class DIASDAQ
    {



        private const string dllName = "DIASDAQ.dll";


        #region Global DDAQ setting functions
        // ##########################################################################################



        /// <summary>
        /// Set general locale settings for the DAQ DLL (default is "english")
        /// </summary>
        /// <param name="locale">local string (f.e. "german")</param>
        [DllImport(dllName)]
        public static extern void DDAQ_SET_LOCALE([MarshalAs(UnmanagedType.LPStr)] string locale);



        /// <summary>
        /// Set general precision for text output of temperatures (default is 1)
        /// </summary>
        /// <param name="precision">precision of text output (0, 1 or 2 characters after decimal point)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_SET_TEMPPRECISION(UInt16 precision);



        /// <summary>
        /// Set general temperature unit for all temperatur inputs and outputs (default is #DDAQ_TEMPUNIT.C)
        /// </summary>
        /// <param name="tempunit">temperature unit (refer to #DDAQ_TEMPUNIT)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_SET_TEMPUNIT(DDAQ_TEMPUNIT tempunit);

        #endregion


        #region Global DDAQ getting functions
        // ##########################################################################################



        /// <summary>
        /// Get the character string from a temperature value with global unit (#DDAQ_SET_TEMPUNIT) and global precision (#DDAQ_SET_TEMPPRECISION)
        /// </summary>
        /// <param name="temp">temperature value corresponding to #DDAQ_SET_TEMPUNIT</param>
        /// <param name="pBuf">temperature string</param>
        /// <param name="nBufSize">buffer size</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_GET_TEMPSTRING(Single temp, [MarshalAs(UnmanagedType.LPStr)] string pBuf, uint nBufSize);



        /// <summary>
        /// Get the character string from a temperature value with global unit (#DDAQ_SET_TEMPUNIT) and user precision
        /// </summary>
        /// <param name="temp">temperature value corresponding to #DDAQ_SET_TEMPUNIT</param>
        /// <param name="precision">precision of text output (0, 1 or 2 characters after decimal point)</param>
        /// <param name="pBuf">temperature string</param>
        /// <param name="nBufSize">buffer size</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_GET_TEMPPRECSTRING(Single temp, ushort precision, [MarshalAs(UnmanagedType.LPStr)] string pBuf, uint nBufSize);

        #endregion


        #region Functions DDAQ_DEVICE_DO_xxxx
        // ##########################################################################################



        /// <summary>
        /// Search for connected cameras, any open camera will be closed (refer also to \ref sec3_1)
        /// </summary>
        /// <returns>number of devices (0 if no camera was found)</returns>
        [DllImport(dllName)]
        //public static extern DDAQ_ERROR DDAQ_DEVICE_DO_DETECTION();
        public static extern uint DDAQ_DEVICE_DO_DETECTION();



        /// <summary>
        /// Open a camera for online data acquisition
        /// </summary>
        /// <param name="nDevNo">device number (1 for the first)</param>
        /// <param name="pFileName">string of filename for template (IRDX file); If pFileName empty, default settings are used</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_DEVICE_DO_OPEN(UInt32 nDevNo, [MarshalAs(UnmanagedType.LPStr)] string pFileName);



        /// <summary>
        /// Open a camera for simulation of data acquisition
        /// </summary>
        /// <param name="nDevNo">device number (1 for the first)</param>
        /// <param name="pFileName">string of filename for simulation (IRD file); To convert an IRDX file to an IRD file the delivered tool "IRDX2IRD.exe"  can be used</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_DEVICE_DO_OPENSIMULATION(UInt32 nDevNo, [MarshalAs(UnmanagedType.LPStr)] string pFileName);



        /// <summary>
        /// Close a camera after data acquisition
        /// </summary>
        /// <param name="nDevNo">device number (1 for the first)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_DEVICE_DO_CLOSE(UInt32 nDevNo);



        /// <summary>
        /// Start continous data acquisition from a camera
        /// </summary>
        /// <param name="nDevNo">device number (1 for the first)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_DEVICE_DO_START(UInt32 nDevNo);



        /// <summary>
        /// Start one float shot data acquisition from a camera
        /// </summary>
        /// <param name="nDevNo">device number (1 for the first)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_DEVICE_DO_STARTSINGLESHOT(UInt32 nDevNo);



        /// <summary>
        /// Start one float shot data acquisition from a camera and wait for the data record.
        /// </summary>
        /// <param name="nDevNo">device number (1 for the first)</param>
        /// <param name="waitms">maximum wait time (milli seconds for the data set)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_DEVICE_DO_STARTSINGLESHOT_WAIT(UInt32 nDevNo, uint waitms);



        /// <summary>
        /// Stop data acquisition from a camera
        /// </summary>
        /// <param name="nDevNo">device number (1 for the first)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_DEVICE_DO_STOP(UInt32 nDevNo);



        /// <summary>
        /// Update the new data from the device to the the internal connected IRDX memory file
        /// </summary>
        /// <param name="nDevNo">device number (1 for the first)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_DEVICE_DO_UPDATEDATA(UInt32 nDevNo);



        /// <summary>
        /// Enable sending the next message (if messaging activ)
        /// </summary>
        /// <param name="nDevNo">device number (1 for the first)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_DEVICE_DO_ENABLE_NEXTMSG(UInt32 nDevNo);



        /// <summary>
        /// Check the connection to the device (works also in stop mode)
        /// </summary>
        /// <param name="nDevNo">device number (1 for the first)</param>
        /// <param name="pConnect">pointer to connection flag; If there is no connection possible, a CLOSE and INIT should be performed</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_DEVICE_DO_CHECKCONNECT(UInt32 nDevNo, ref bool pConnect);



        /// <summary>
        /// Download one images from a MIDAS camera to an IRDX file (overwrite an existing file!)
        /// </summary>
        /// <param name="nDevNo">device number (1 for the first)</param>
        /// <param name="ImageIdx">image index (0 for the first)</param>
        /// <param name="pFileName">file name string. An existing file</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_DEVICE_DO_DOWNLOADIMAGE(UInt32 nDevNo, ushort ImageIdx, [MarshalAs(UnmanagedType.LPStr)] string pFileName);



        /// <summary>
        /// Delete all images from a MIDAS camera
        /// </summary>
        /// <param name="nDevNo">device number (1 for the first)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_DEVICE_DO_DELETEIMAGES(UInt32 nDevNo);

        #endregion


        #region Functions DDAQ_DEVICE_GET_xxxx
        // ##########################################################################################



        /// <summary>
        /// Get the device identification (number and type) from the camera (works also if the device is not open)
        /// </summary>
        /// <param name="nDevNo">device number (1 for the first)</param>
        /// <param name="pID">pointer to ID number</param>
        /// <param name="pType">pointer to ID type (#DDAQ_DEVICE_TYPE)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_DEVICE_GET_ID(UInt32 nDevNo, ref DDAQ_DEVICE_TYPE pID, ref uint pType);



        /// <summary>
        /// Get the device identification string from the camera (works also if the device is not open)
        /// </summary>
        /// <param name="nDevNo">device number (1 for the first)</param>
        /// <param name="pBuf">device string</param>
        /// <param name="nBufSize">buffer size</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        //public static extern DDAQ_ERROR DDAQ_DEVICE_GET_IDSTRING(UInt32 nDevNo, [MarshalAs(UnmanagedType.LPStr)] string pBuf, uint nBufSize);
        public static extern DDAQ_ERROR DDAQ_DEVICE_GET_IDSTRING(uint nDevNo, byte[] pBuf, int nBufSize);



        /// <summary>
        /// Get the interface string from the connection to the camera (works also if device is not open)
        /// </summary>
        /// <param name="nDevNo">device number (1 for the first)</param>
        /// <param name="pBuf">interface string</param>
        /// <param name="nBufSize">buffer size</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_DEVICE_GET_INTERFACESTRING(UInt32 nDevNo, [MarshalAs(UnmanagedType.LPStr)] string pBuf, uint nBufSize);



        /// <summary>
        /// Get the internal connected IRDX memory handle from a device
        /// </summary>
        /// <param name="nDevNo">device number (1 for the first)</param>
        /// <param name="pIRDX">pointer to IRDX memory handle</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_DEVICE_GET_IRDX(UInt32 nDevNo, ref IntPtr pIRDX);



        /// <summary>
        /// Get the new data ready state
        /// </summary>
        /// <param name="nDevNo">device number (1 for the first)</param>
        /// <param name="pBOOL">pointer to new data ready</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_DEVICE_GET_NEWDATAREADY(UInt32 nDevNo, ref bool pBOOL);



        /// <summary>
        /// Get the real data update frequency
        /// </summary>
        /// <param name="nDevNo">device number (1 for the first)</param>
        /// <param name="pFrequ">pointer to real data update frequency (Hz)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_DEVICE_GET_UPDATEFREQUENCY(UInt32 nDevNo, ref float pFrequ);



        /// <summary>
        /// Get the number of available measurement ranges (works also if the device is not open)
        /// </summary>
        /// <param name="nDevNo">device number (1 for the first)</param>
        /// <param name="pNum">pointer to number of available measurement ranges</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_DEVICE_GET_NUMMRANGES(UInt32 nDevNo, ref ushort pNum);



        /// <summary>
        /// Get the measurement range information string from the camera (works also if the device is not open)
        /// </summary>
        /// <param name="nDevNo">device number (1 for the first)</param>
        /// <param name="mr">measurement range (1 for the first)</param>
        /// <param name="pBuf">device string</param>
        /// <param name="nBufSize">buffer size</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_DEVICE_GET_MRANGESTRING(UInt32 nDevNo, ushort mr, [MarshalAs(UnmanagedType.LPStr)] string pBuf, uint nBufSize);



        /// <summary>
        /// Get the actual device errors and lost data states from the camera
        /// </summary>
        /// <param name="nDevNo">device number (1 for the first)</param>
        /// <param name="pBuf">pointer to error buffer: pBuf[0] actual error state: 0 - no error, 1 - error pBuf[1] number of errors pBuf[2] lost data in level 1 pBuf[3] lost data in level 2 pBuf[4] lost data in level 3</param>
        /// <param name="nBufSize">buffer size</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_DEVICE_GET_ERRORS(UInt32 nDevNo, ref uint pBuf, uint nBufSize);



        /// <summary>
        /// Get the actual state of the camera digital inputs and ouputs
        /// </summary>
        /// <param name="nDevNo">device number (1 for the first)</param>
        /// <param name="pDIN">actual state of the digital intputs:</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_DEVICE_GET_DINOUT(UInt32 nDevNo, ref uint pDIN);



        /// <summary>
        /// Get the camera mode
        /// </summary>
        /// <param name="nDevNo">device number (1 for the first)</param>
        /// <param name="pOK">pointer to camera mode OK ([true/false] mode is [OK/Error])</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_DEVICE_GET_CAMERAMODEOK(UInt32 nDevNo, ref bool pOK);



        /// <summary>
        /// Get the detector temperature
        /// </summary>
        /// <param name="nDevNo">device number (1 for the first)</param>
        /// <param name="pTemp">pointer to detector temperature (corresponding to #DDAQ_SET_TEMPUNIT)</param>
        /// <param name="pOK">pointer to detector temperature OK ([true/false] temperature [OK/Error])</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_DEVICE_GET_DETECTORTEMP(UInt32 nDevNo, ref float pTemp, ref bool pOK);



        /// <summary>
        /// Get the camera housing temperature
        /// </summary>
        /// <param name="nDevNo">device number (1 for the first)</param>
        /// <param name="pTemp">pointer to camera temperature (corresponding to #DDAQ_SET_TEMPUNIT)</param>
        /// <param name="pOK">pointer to camera temperature OK ([true/false] temperature [OK/Error])</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_DEVICE_GET_CAMERATEMP(UInt32 nDevNo, ref float pTemp, ref bool pOK);



        /// <summary>
        /// Get the actual device state
        /// </summary>
        /// <param name="nDevNo">device number (1 for the first)</param>
        /// <param name="pBuf">pointer to state buffer: pBuf[0]	actual acquisition state: 0 - stop, 1 - running</param>
        /// <param name="nBufSize">buffer size</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_DEVICE_GET_STATE(UInt32 nDevNo, ref uint pBuf, uint nBufSize);



        /// <summary>
        /// Get the number of images saved on a MIDAS camera
        /// </summary>
        /// <param name="nDevNo">device number (1 for the first)</param>
        /// <param name="pNumImages">pointer to image number</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern uint DDAQ_DEVICE_GET_NUMIMAGES(UInt32 nDevNo, ref ushort pNumImages);

        #endregion


        #region DDAQ_DEVICE_SET_xxxx
        // ##########################################################################################



        /// <summary>
        /// Set the actual state of the camera digital outputs
        /// </summary>
        /// <param name="nDevNo">device number (1 for the first)</param>
        /// <param name="DOUT">new state of the digital output: Bit 0: OUT 0 (error channel) - Bit 1: OUT 1 - Bit 2: OUT 2 (if supported) - Bit 3: OUT 3 (if supported) - if the Bit is "0", the output line goes to low ohmic state (no alarm) - if the Bit is "1", the output line goes to high ohmic state (alarm)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_DEVICE_SET_DOUT(UInt32 nDevNo, uint DOUT);



        /// <summary>
        /// Set a window handle for the data message. A windows message (#DDAQ_WM_DATAMSG, devive number in WPARAM) is sent after new data are ready. #DDAQ_DEVICE_DO_UPDATEDATA can be used to read the data. Only after #DDAQ_DEVICE_DO_ENABLE_NEXTMSG was called a new message is sent.
        /// </summary>
        /// <param name="nDevNo">device number (1 for the first)</param>
        /// <param name="hWnd">HANDLE of the window, which will receive the messages (0 to disable)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_DEVICE_SET_MSGHWND(UInt32 nDevNo, IntPtr hWnd);



        /// <summary>
        /// Set a thread ID for the data message. A thread message (#DDAQ_WM_DATAMSG, devive number in WPARAM) is sent after new data are ready. #DDAQ_DEVICE_DO_UPDATEDATA can be used to read the data. Only after #DDAQ_DEVICE_DO_ENABLE_NEXTMSG was called a new message is sent.
        /// </summary>
        /// <param name="nDevNo">device number (1 for the first)</param>
        /// <param name="nThreadID">ID of the thread, which will receive the messages (0 to disable).</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_DEVICE_SET_MSGTHREAD(UInt32 nDevNo, uint nThreadID);

        #endregion


        #region DDAQ_IRDX_FILE_xxxx
        // ##########################################################################################



        /// <summary>
        /// Delete an IRDX file on disk
        /// </summary>
        /// <param name="pFileName">pointer to null terminated file name string</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_FILE_DELETE([MarshalAs(UnmanagedType.LPStr)] string pFileName);



        /// <summary>
        /// Get the IRDX file size on disk
        /// </summary>
        /// <param name="pFileName">pointer to null terminated file name string</param>
        /// <param name="pFileSizeMB">pointer to file size (MB)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_FILE_GETSIZE([MarshalAs(UnmanagedType.LPStr)] string pFileName, ref Double pFileSizeMB);



        /// <summary>
        /// Rename an IRDX file on disk
        /// </summary>
        /// <param name="pOldFileName">pointer to null terminated old file name string</param>
        /// <param name="pNewFileName">pointer to null terminated new file name string</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_FILE_RENAME([MarshalAs(UnmanagedType.LPStr)] string pOldFileName, [MarshalAs(UnmanagedType.LPStr)] string pNewFileName);



        /// <summary>
        /// Open an empty IRDX memory file
        /// </summary>
        /// <param name="pIRDX">pointer to IRDX memory handle</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_FILE_OPEN_MEM(ref IntPtr pIRDX);



        /// <summary>
        /// Open an empty IRDX extended (user defined) memory file.
        /// </summary>
        /// <param name="from_IRDX">IRDX handle</param>
        /// <param name="SizeX">Size X of the IRDX file</param>
        /// <param name="SizeY">Size Y of the IRDX file</param>
        /// <param name="reserved">Reserved</param>
        /// <param name="pIRDX">pointer to IRDX memory handle</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_FILE_OPEN_MEM_EX(IntPtr from_IRDX, ushort SizeX, ushort SizeY, bool reserved, ref IntPtr pIRDX);



        /// <summary>
        /// Open an IRDX from a file for reading
        /// </summary>
        /// <param name="pFileName">pointer to null terminated file name string</param>
        /// <param name="bAll">[true/false] read [all/only the first data set]</param>
        /// <param name="pIRDX">pointer to IRDX memory handle</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_FILE_OPEN_READ([MarshalAs(UnmanagedType.LPStr)] string pFileName, bool bAll, ref IntPtr pIRDX);



        /// <summary>
        /// Open an IRDX file for writing
        /// </summary>
        /// <param name="pFileName">pointer to null terminated file name string</param>
        /// <param name="bAppend">[true/false] [append/delete] an existing file</param>
        /// <param name="pIRDX">pointer to IRDX memory handle</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_FILE_OPEN_WRITE([MarshalAs(UnmanagedType.LPStr)] string pFileName, bool bAppend, ref IntPtr pIRDX);



        /// <summary>
        /// Open an IRDX file for reading and writing
        /// </summary>
        /// <param name="pFileName">pointer to null terminated file name string</param>
        /// <param name="bAppend">[true/false] [append/delete] an existing file</param>
        /// <param name="pIRDX">pointer to IRDX memory handle</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_FILE_OPEN_READWRITE([MarshalAs(UnmanagedType.LPStr)] string pFileName, bool bAppend, ref IntPtr pIRDX);



        /// <summary>
        /// Close an open IRDX file
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_FILE_CLOSE(IntPtr hIRDX);



        /// <summary>
        /// Save an IRDX file to disk (overwrite an existing file!)
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pFileName">pointer to null terminated file name string</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_FILE_SAVE(IntPtr hIRDX, [MarshalAs(UnmanagedType.LPStr)] string pFileName);



        /// <summary>
        /// Add one IRDX file to another IRDX file
        /// </summary>
        /// <param name="hIRDX">IRDX handle (type MEM, WRITE, or READWRITE)</param>
        /// <param name="hIRDX_add">IRDX handle (will be added to hIRDX)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_FILE_ADD_IRDX(IntPtr hIRDX, IntPtr hIRDX_add);



        /// <summary>
        /// Get the number of data sets from an IRDX file
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pNumDS">pointer to number of data sets</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_FILE_GET_NUMDATASETS(IntPtr hIRDX, ref uint pNumDS);



        /// <summary>
        /// Get the current selected data set from an IRDX file
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pDSIdx">pointer to the data set index (0...)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_FILE_GET_CURDATASET(IntPtr hIRDX, ref uint pDSIdx);



        /// <summary>
        /// Set the current selected data set to an IRDX file
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="DSIdx">data set index (0...)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_FILE_SET_CURDATASET(IntPtr hIRDX, uint DSIdx);



        /// <summary>
        /// Get the modified state of IRDX file
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pModif">pointer to modified</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_FILE_GET_MODIFIED(IntPtr hIRDX, ref bool pModif);



        /// <summary>
        /// Remove a data set from an IRDX file
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="DSIdx">data set index (0...) to be removed</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_FILE_REMOVE_DATASET(IntPtr hIRDX, uint DSIdx);

        #endregion


        #region DDAQ_IRDX_DEVICE_xxxx
        // ##########################################################################################



        /// <summary>
        /// Get the device identification (number and type)
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pID">pointer to ID number</param>
        /// <param name="pType">pointer to ID type (#DDAQ_DEVICE_TYPE.NO)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_DEVICE_GET_ID(IntPtr hIRDX, ref uint pID, ref DDAQ_DEVICE_TYPE pType);



        /// <summary>
        /// Get the device identification string
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pBuf">pointer to buffer for null terminated device string</param>
        /// <param name="nBufSize">buffer size</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_DEVICE_GET_IDSTRING(IntPtr hIRDX, [MarshalAs(UnmanagedType.LPStr)] string pBuf, uint nBufSize);



        /// <summary>
        /// Get measurement temperature range min and max
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pMin">pointer to measurement minimum value (corresponding to #DDAQ_SET_TEMPUNIT)</param>
        /// <param name="pMax">pointer to measurement maximum value (corresponding to #DDAQ_SET_TEMPUNIT)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_DEVICE_GET_MRANGEMINMAX(IntPtr hIRDX, ref float pMin, ref float pMax);



        /// <summary>
        /// Get the internal frame rate of the camera without averaging
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pFR">pointer to the cameras internal frame rate (frames per second: Hz)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_DEVICE_GET_INTERNALFPS(IntPtr hIRDX, ref float pFR);



        /// <summary>
        /// Get the camera mode
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pOK">pointer to camera mode OK ([true/false] mode is [OK/Error])</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_DEVICE_GET_CAMERAMODEOK(IntPtr hIRDX, ref bool pOK);



        /// <summary>
        /// Get the detector temperature
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pTemp">pointer to detector temperature (corresponding to #DDAQ_SET_TEMPUNIT)</param>
        /// <param name="pOK">pointer to detector temperature OK ([true/false] temperature [OK/Error])</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_DEVICE_GET_DETECTORTEMP(IntPtr hIRDX, ref float pTemp, ref bool pOK);



        /// <summary>
        /// Get the camera housing temperature
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pTemp">pointer to camera temperature (corresponding to #DDAQ_SET_TEMPUNIT)</param>
        /// <param name="pOK">pointer to camera temperature OK ([true/false] temperature [OK/Error])</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_DEVICE_GET_CAMERATEMP(IntPtr hIRDX, ref float pTemp, ref bool pOK);


        /// <summary>
        /// Update status information (camera temp., detector temp, camera mode) from one IRDX to another)
        /// </summary>
        /// <param name="hIRDX_from">source IRDX handle</param>
        /// <param name="hIRDX_to">destination IRDX handle</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_DEVICE_DO_UPDATE_STATE(IntPtr hIRDX_from, IntPtr hIRDX_to);


        #endregion



        #region DDAQ_IRDX_OBJECT_xxxx
        // ##########################################################################################



        /// <summary>
        /// Get object property relative emissivity of measurement object
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pEmiss">pointer to emissivity (0.1 - 1.0)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_OBJECT_GET_EMISSIVITY(IntPtr hIRDX, ref float pEmiss);



        /// <summary>
        /// Set object property relative emissivity of measurement object
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="Emiss">new emissivity (0.1 - 1.0)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_OBJECT_SET_EMISSIVITY(IntPtr hIRDX, float Emiss);



        /// <summary>
        /// Get object property transmission of measurement distance
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pTrans">pointer to transmission (0.1 - 1.0)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_OBJECT_GET_TRANSMISSION(IntPtr hIRDX, ref float pTrans);



        /// <summary>
        /// Set object property transmission of measurement distance
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="Trans">new transmission (0.1 - 1.0)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_OBJECT_SET_TRANSMISSION(IntPtr hIRDX, float Trans);



        /// <summary>
        /// Get object property ambient temperature of measurement object
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pAmbTemp">pointer to ambient temperature (corresponding to #DDAQ_SET_TEMPUNIT)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_OBJECT_GET_AMBIENTTEMP(IntPtr hIRDX, ref float pAmbTemp);



        /// <summary>
        /// Set object property ambient temperature of measurement object
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="AmbTemp">new ambient temperature (corresponding to #DDAQ_SET_TEMPUNIT)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_OBJECT_SET_AMBIENTTEMP(IntPtr hIRDX, float AmbTemp);



        /// <summary>
        /// Get ambient temperature correction type and parameters
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pCorrMode">pointer to correction mode (refer to #DDAQ_AMBIENTTEMP_CORR.FIX)</param>
        /// <param name="pCorrOffs">pointer to offset for dynamic correction (refer to #DDAQ_AMBIENTTEMP_CORR.TCAM)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_OBJECT_GET_AMBIENTTEMP_CORR(IntPtr hIRDX, ref DDAQ_AMBIENTTEMP_CORRMODE pCorrMode, ref float pCorrOffs);



        /// <summary>
        /// Set ambient temperature correction type and parameters
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="CorrMode">correction mode (refer to #DDAQ_AMBIENTTEMP_CORRMODE.FIX)</param>
        /// <param name="CorrOffs">offset for dynamic correction (refer to #DDAQ_AMBIENTTEMP_CORRMODE.TCAM)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_OBJECT_SET_AMBIENTTEMP_CORR(IntPtr hIRDX, DDAQ_AMBIENTTEMP_CORRMODE CorrMode, float CorrOffs);



        /// <summary>
        /// Get transmission correction type and parameters
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pCorrMode">pointer to correction mode (refer to #DDAQ_TRANSMISSION_CORRMODE.FIX)</param>
        /// <param name="pRefTemp">pointer to reference temperature of black body (corresponding to #DDAQ_SET_TEMPUNIT)</param>
        /// <param name="pRefEmiss">pointer to reference emissivity</param>
        /// <param name="pLeft">pointer to left pixel coordinate (X1) of reference (1 ... sizex) in the image</param>
        /// <param name="pTop">pointer to top pixel coordinate (Y1) of reference (1 ... sizey) in the image</param>
        /// <param name="pRight">pointer to right pixel coordinate (X2) of reference (1 ... sizex) in the image</param>
        /// <param name="pBottom">pointer to bottom pixel coordinate (Y2) of reference (1 ... sizey) in the image</param>
        /// <param name="pWeight">pointer to weight for online correction (0.1 ... 1.0)</param>
        /// <param name="pMinTransm">pointer to minimum valid value for calculated transmission (0.1 ... 1.0)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_OBJECT_GET_TRANSMISSION_CORR(
            IntPtr hIRDX,
            ref DDAQ_TRANSMISSION_CORRMODE pCorrMode,
            ref float pRefTemp,
            ref float pRefEmiss,
            ref ushort pLeft,
            ref ushort pTop,
            ref ushort pRight,
            ref ushort pBottom,
            ref float pWeight,
            ref float pMinTransm);



        /// <summary>
        /// Set transmission correction type and parameters
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="CorrMode">correction mode (refer to #DDAQ_TRANSMISSION_CORRMODE.FIX)</param>
        /// <param name="RefTemp">reference temperature of black body (corresponding to #DDAQ_SET_TEMPUNIT)</param>
        /// <param name="RefEmiss">reference emissivity</param>
        /// <param name="Left">left pixel coordinate (X1) of reference (1 ... sizex) in the image</param>
        /// <param name="Top">top pixel coordinate (Y1) of reference (1 ... sizey) in the image</param>
        /// <param name="Right">right pixel coordinate (X2) of reference (1 ... sizex) in the image</param>
        /// <param name="Bottom">bottom pixel coordinate (Y2) of reference (1 ... sizey) in the image</param>
        /// <param name="Weight">weight for online correction (0.1 ... 1.0)</param>
        /// <param name="MinTransm">minimum valid value for calculated transmission (0.1 ... 1.0)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_OBJECT_SET_TRANSMISSION_CORR(
            IntPtr hIRDX,
            DDAQ_TRANSMISSION_CORRMODE CorrMode,
            float RefTemp,
            float RefEmiss,
            ushort Left,
            ushort Top,
            ushort Right,
            ushort Bottom,
            float Weight,
            float MinTransm);



        /// <summary>
        /// Proceed a transmission correction with the actual data (only if a device is connected with this irdx)
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pCalcTransm">pointer to calculated transmission</param>
        /// <param name="pValidity">pointer to validity of calculated transmission (refer to #DDAQ_TRANSMISSION_CORRRES.OFF)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_OBJECT_DO_TRANSMISSION_CORR(IntPtr hIRDX, ref float pCalcTransm, ref DDAQ_TRANSMISSION_CORRRES pValidity);



        /// <summary>
        /// Get calculated value and validity of transmission correction
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pCalcTransm">pointer to calculated transmission</param>
        /// <param name="pValidity">pointer to validity of calculated transmission (refer to #DDAQ_TRANSMISSION_CORRRES.OFF)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_OBJECT_GET_TRANSMISSION_CORR_STATE(IntPtr hIRDX, ref float pCalcTransm, ref DDAQ_TRANSMISSION_CORRRES pValidity);

        #endregion


        #region DDAQ_IRDX_ACQUISITION_xxxx
        // ##########################################################################################



        /// <summary>
        /// Get the time stamp of the actual acquisition
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pYear">pointer to time stamp year</param>
        /// <param name="pMonth">pointer to time stamp month</param>
        /// <param name="pDay">pointer to time stamp day</param>
        /// <param name="pHour">pointer to time stamp hour</param>
        /// <param name="pMin">pointer to time stamp minute</param>
        /// <param name="pSec">pointer to time stamp second</param>
        /// <param name="pMSec">pointer to time stamp milli second</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_ACQUISITION_GET_TIMESTAMP(
            IntPtr hIRDX,
            ref int pYear,
            ref int pMonth,
            ref int pDay,
            ref int pHour,
            ref int pMin,
            ref int pSec,
            ref int pMSec);



        /// <summary>
        /// Get parameter fix averaging
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pAv">pointer to averaging (1,2,4,8, ...)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_ACQUISITION_GET_AVERAGING(IntPtr hIRDX, ref ushort pAv);



        /// <summary>
        /// Set parameter fix averaging
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="Av">new averaging (1,2,4,8, ...)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_ACQUISITION_SET_AVERAGING(IntPtr hIRDX, ushort Av);



        /// <summary>
        /// Get the measurement range information string from the irdx
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pBuf">pointer to buffer for a null terminated device string</param>
        /// <param name="nBufSize">buffer size</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_ACQUISITION_GET_MRANGESTRING(IntPtr hIRDX, [MarshalAs(UnmanagedType.LPStr)] string pBuf, uint nBufSize);



        /// <summary>
        /// Get parameter measurement range
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pMR">pointer to measurement range (1,2,3 ...)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_ACQUISITION_GET_MRANGE(IntPtr hIRDX, ref ushort pMR);



        /// <summary>
        /// Set parameter measurement range
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="MR">new measurement range (1,2,3, ...)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_ACQUISITION_SET_MRANGE(IntPtr hIRDX, ushort MR);



        /// <summary>
        /// Get parameter number of frames (for line cameras)
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pNumFrames">pointer to number of frames</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_ACQUISITION_GET_NUMFRAMES(IntPtr hIRDX, ref ushort pNumFrames);



        /// <summary>
        /// et parameter number of frames (for line cameras)
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="NumFrames">number of frames</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_ACQUISITION_SET_NUMFRAMES(IntPtr hIRDX, ushort NumFrames);



        /// <summary>
        /// Get number of valid frames (for line cameras, can be less the number of frames if retriggered)
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pValidFrames">pointer to number valid of frames</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_ACQUISITION_GET_VALIDFRAMES(IntPtr hIRDX, ref ushort pValidFrames);



        /// <summary>
        /// Get parameter single trigger
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pTrig">pointer to single trigger mode (#DDAQ_SINGLETRIGGER)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_ACQUISITION_GET_SINGLETRIGGER(IntPtr hIRDX, ref DDAQ_SINGLETRIGGER pTrig);



        /// <summary>
        /// Set parameter single trigger
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="Trig">new single trigger mode (#DDAQ_SINGLETRIGGER)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_ACQUISITION_SET_SINGLETRIGGER(IntPtr hIRDX, DDAQ_SINGLETRIGGER Trig);



        /// <summary>
        /// Get parameter sequence trigger
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pTrig">pointer to sequence trigger mode (#DDAQ_SEQUENCETRIGGER.NO)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_ACQUISITION_GET_SEQUENCETRIGGER(IntPtr hIRDX, ref DDAQ_SEQUENCETRIGGER pTrig);



        /// <summary>
        /// Set parameter sequence trigger
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="Trig">new sequence trigger mode (#DDAQ_SEQUENCETRIGGER.NO)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_ACQUISITION_SET_SEQUENCETRIGGER(IntPtr hIRDX, DDAQ_SEQUENCETRIGGER Trig);



        /// <summary>
        /// Get parameters for temperature sequence trigger.
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pThreshold">pointer to threshold for temperature sequence trigger (corresponding to #DDAQ_SET_TEMPUNIT)</param>
        /// <param name="pMode">pointer to  temperature sequence trigger mode (#DDAQ_TEMPSEQUENCETRIGGER_MAX_G_TH)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_ACQUISITION_GET_TEMPSEQUENCETRIGGER(IntPtr hIRDX, ref float pThreshold, ref DDAQ_TEMPSEQUENCETRIGGER pMode);



        /// <summary>
        /// Set parameters for temperature sequence trigger
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="Threshold">new threshold for temperature sequence trigger (corresponding to #DDAQ_SET_TEMPUNIT)</param>
        /// <param name="Mode">new temperature sequence trigger mode (#DDAQ_TEMPSEQUENCETRIGGER_MAX_G_TH)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_ACQUISITION_SET_TEMPSEQUENCETRIGGER(IntPtr hIRDX, float Threshold, DDAQ_TEMPSEQUENCETRIGGER Mode);



        /// <summary>
        /// Get parameter shutter trigger (only VIEW 320/380)
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pTrig">pointer to shutter trigger mode (#DDAQ_SHUTTERTRIGGER.NO)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_ACQUISITION_GET_SHUTTERTRIGGER(IntPtr hIRDX, ref DDAQ_SHUTTERTRIGGER pTrig);



        /// <summary>
        /// Set parameter shutter trigger (only VIEW 320/380)
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="Trig">new shutter trigger mode (#DDAQ_SHUTTERTRIGGER.NO)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_ACQUISITION_SET_SHUTTERTRIGGER(IntPtr hIRDX, DDAQ_SHUTTERTRIGGER Trig);

        #endregion


        #region DDAQ_IRDX_SCALE_xxxx
        // ##########################################################################################



        /// <summary>
        /// Get actual scale minimum and maximum
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pMin">pointer to scale minimum value (corresponding to #DDAQ_SET_TEMPUNIT)</param>
        /// <param name="pMax">pointer to scale maximum value (corresponding to #DDAQ_SET_TEMPUNIT)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_SCALE_GET_MINMAX(IntPtr hIRDX, ref float pMin, ref float pMax);



        /// <summary>
        /// Set scale minimum and maximum (not for auto dynamic scaling)
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="Min">new scale minimum value (corresponding to #DDAQ_SET_TEMPUNIT)</param>
        /// <param name="Max">new scale maximum value (corresponding to #DDAQ_SET_TEMPUNIT)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_SCALE_SET_MINMAX(IntPtr hIRDX, float Min, float Max);



        /// <summary>
        /// Get auto scaling mode
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pMode">pointer to auto scaling mode (refer #DDAQ_AUTOSCALE.NO)</param>
        /// <param name="pAVPlus">pointer to AvPlus value (only for mode #DDAQ_AUTOSCALE_AVERAGE and #DDAQ_AUTOSCALE_STDDEV). For #DDAQ_AUTOSCALE_AVERAGE scaling maximum is the value of Average + AvPlus. For #DDAQ_AUTOSCALE_STDDEV scaling maximum is the value of Average + AvPlus*Standard Deviation.</param>
        /// <param name="pAVMinus">pointer to AvMinus value (only for mode #DDAQ_AUTOSCALE_AVERAGE and #DDAQ_AUTOSCALE_STDDEV). For #DDAQ_AUTOSCALE_AVERAGE scaling mainimum is the value of Average - AvMinus. For #DDAQ_AUTOSCALE_STDDEV scaling minimum is the value of Average + AvMinus*Standard Deviation.</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_SCALE_GET_AUTOMODE(IntPtr hIRDX, ref DDAQ_AUTOSCALE pMode, ref float pAVPlus, ref float pAVMinus);



        /// <summary>
        /// Set auto scaling mode
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="Mode">auto scaling mode (refer #DDAQ_AUTOSCALE.NO).</param>
        /// <param name="AVPlus">AvPlus value (corresponding to #DDAQ_SET_TEMPUNIT, only for mode #DDAQ_AUTOSCALE_AVERAGE and #DDAQ_AUTOSCALE_STDDEV). For #DDAQ_AUTOSCALE_AVERAGE scaling maximum is the value of Average + AvPlus. For #DDAQ_AUTOSCALE_STDDEV scaling maximum is the value of Average + AvPlus*Standard Deviation.</param>
        /// <param name="AVMinus">AvMinus value (corresponding to #DDAQ_SET_TEMPUNIT, only for mode #DDAQ_AUTOSCALE_AVERAGE and #DDAQ_AUTOSCALE_STDDEV). For #DDAQ_AUTOSCALE_AVERAGE scaling mainimum is the value of Average - AvMinus. For #DDAQ_AUTOSCALE_STDDEV scaling minimum is the value of Average + AvMinus*Standard Deviation.</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_SCALE_SET_AUTOMODE(IntPtr hIRDX, DDAQ_AUTOSCALE Mode, float AVPlus, float AVMinus);



        /// <summary>
        /// Get scale step from IRDX
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pStep">pointer to scale step value (corresponding to #DDAQ_SET_TEMPUNIT)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_SCALE_GET_STEP(IntPtr hIRDX, ref float pStep);



        /// <summary>
        /// Set scale step to IRDX
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="Step">new scale step value (corresponding to #DDAQ_SET_TEMPUNIT)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_SCALE_SET_STEP(IntPtr hIRDX, float Step);

        #endregion


        #region DDAQ_IRDX_PALLET_xxxx
        // ##########################################################################################



        /// <summary>
        /// Get the pallete bar number and number of colors
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pBar">pointer to bar number (definition start here #DDAQ_PALLET.SYMIICON)</param>
        /// <param name="pNumCol">pointer to number of colors (3 - 256)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_PALLET_GET_BAR(IntPtr hIRDX, ref DDAQ_PALLET pBar, ref ushort pNumCol);



        /// <summary>
        /// Set pallet bar number and number of colors
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="Bar">new bar number (definition start here #DDAQ_PALLET.SYMIICON)</param>
        /// <param name="NumCol">new number of colors (3 - 256)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_PALLET_SET_BAR(IntPtr hIRDX, DDAQ_PALLET Bar, ushort NumCol);



        /// <summary>
        /// Get the pallete bar background color
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pBkCol">pointer to background color (COLORREF)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_PALLET_GET_BACKGROUNDCOL(IntPtr hIRDX, ref COLORREF pBkCol);



        /// <summary>
        /// Set the pallete bar background color
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="BkCol">new background color (COLORREF)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_PALLET_SET_BACKGROUNDCOL(IntPtr hIRDX, COLORREF BkCol);



        /// <summary>
        /// Get the pallete bar text color
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pTxtCol">pointer to text color (COLORREF)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_PALLET_GET_TEXTCOL(IntPtr hIRDX, ref COLORREF pTxtCol);



        /// <summary>
        /// Set the pallete bar text color
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="TxtCol">new text color (COLORREF)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_PALLET_SET_TEXTCOL(IntPtr hIRDX, COLORREF TxtCol);



        /// <summary>
        /// Get the number of isotherms
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pNumIsoth">pointer to number of isotherms</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_PALLET_GET_NUMISOTHERMS(IntPtr hIRDX, ref ushort pNumIsoth);



        /// <summary>
        /// Set the number of isotherms
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="NumIsoth">number of isotherms</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_PALLET_SET_NUMISOTHERMS(IntPtr hIRDX, ushort NumIsoth);



        /// <summary>
        /// Get the settings for an isotherms
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="nNo">pointer to number of isotherm (1 - 5)</param>
        /// <param name="pMin">pointer to minimum of isotherm range</param>
        /// <param name="pMax">pointer to maximum of isotherm range</param>
        /// <param name="pCol">pointer to color of isotherm (COLORREF)</param>
        /// <param name="pSat">pointer to saturation (%) (0 - 100)</param>
        /// <param name="pTransp">pointer to transparent mode [true/false]</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_PALLET_GET_ISOTHERM(IntPtr hIRDX, ref ushort nNo, ref float pMin, ref float pMax, ref COLORREF pCol, ref ushort pSat, ref bool pTransp);



        /// <summary>
        /// Set the settings for an isotherms
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="nNo">number of isotherm (1 - 5)</param>
        /// <param name="Min">minimum of isotherm range</param>
        /// <param name="Max">maximum of isotherm range</param>
        /// <param name="Col">color of isotherm (COLORREF)</param>
        /// <param name="Sat">saturation (%) (0 - 100)</param>
        /// <param name="Transp">transparent mode [true/false]</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_PALLET_SET_ISOTHERM(IntPtr hIRDX, ushort nNo, float Min, float Max, COLORREF Col, ushort Sat, bool Transp);



        /// <summary>
        /// Get the actual pallet for drawing as bitmap
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="Width">width of the bitmap</param>
        /// <param name="Height">height of the bitmap</param>
        /// <param name="ppBits">pointer to pointer of bitmap bits</param>
        /// <param name="ppBitmapInfo">pointer to pointer of bitmap info header</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_PALLET_GET_BITMAP(IntPtr hIRDX, ushort Width, ushort Height, out IntPtr ppBits, out IntPtr ppBitmapInfo);



        /// <summary>
        /// Get the actual pallet with scale text for drawing as bitmap
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="Width">width of the bitmap</param>
        /// <param name="Height">height of the bitmap</param>
        /// <param name="ppBits">pointer to pointer of bitmap bits</param>
        /// <param name="ppBitmapInfo">pointer to pointer of bitmap info header</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_PALLET_GET_BITMAPSCALE(IntPtr hIRDX, ushort Width, ushort Height, out IntPtr ppBits, out IntPtr ppBitmapInfo);



        /// <summary>
        /// Get the actual pallet with scale text for drawing as bitmap with extended settings
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="Width">width of the bitmap</param>
        /// <param name="Height">height of the bitmap</param>
        /// <param name="ppBits">pointer to pointer of bitmap bits</param>
        /// <param name="ppBitmapInfo">pointer to pointer of bitmap info header</param>
        /// <param name="pParam">pointer to additional parameters: pParam[0] font size for text - negative values: font size in pixel - positive values: relative width of color bar: 0..100</param>
        /// <param name="size">size of additional parameters (bytes)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_PALLET_GET_BITMAPSCALEEX(
            IntPtr hIRDX,
            ushort Width,
            ushort Height,
            out IntPtr ppBits,
            out IntPtr ppBitmapInfo,
            ref int pParam,
            uint size);



        /// <summary>
        /// Get the color (RGB) of a pallet
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="nIdx">index of color in color bar (0 - number of colors-1)</param>
        /// <param name="pCol">pointer to color (COLORREF)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_PALLET_GET_BARCOL(IntPtr hIRDX, ushort nIdx, ref COLORREF pCol);

        #endregion


        #region DDAQ_IRDX_IMAGE_xxxx
        // ##########################################################################################


        /// <summary>
        /// Get image size for bitmap display of measurement data
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pWidth">pointer to width of image</param>
        /// <param name="pHeight">pointer to height of image</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_IMAGE_GET_SIZE(IntPtr hIRDX, ref ushort pWidth, ref ushort pHeight);


        /// <summary>
        /// Get image zoom mode and image zoom factor for bitmap display of measurement data
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pZMode">pointer of zoom mode (definition start here #DDAQ_ZMODE.DIRECT)</param>
        /// <param name="pZoom">pointer to zoom factor (not used for ZMode #DDAQ_ZMODE.DIRECT)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_IMAGE_GET_ZOOM(IntPtr hIRDX, ref DDAQ_ZMODE pZMode, ref float pZoom);


        /// <summary>
        /// Set image zoom mode and zoom factor for bitmap display of measurement data
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="ZMode">new zoom mode (definition start here #DDAQ_ZMODE.DIRECT)</param>
        /// <param name="Zoom">new zoom factor (not used for ZMode #DDAQ_ZMODE.DIRECT)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_IMAGE_SET_ZOOM(IntPtr hIRDX, DDAQ_ZMODE ZMode, float Zoom);


        /// <summary>
        /// Get image bitmap draw mode
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pDrawMode">pointer of draw mode (definition start here #DDAQ_DRAWMODE.R0)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_IMAGE_GET_DRAWMODE(IntPtr hIRDX, ref DDAQ_DRAWMODE pDrawMode);


        /// <summary>
        /// Set image bitmap draw mode
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="DrawMode">new draw mode (definition start here #DDAQ_DRAWMODE.R0)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_IMAGE_SET_DRAWMODE(IntPtr hIRDX, DDAQ_DRAWMODE DrawMode);


        /// <summary>
        /// Get the image of measurement data as bitmap for drawing for 2D data
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pWidth">poniter to width of the bitmap</param>
        /// <param name="pHeight">pointer to height of the bitmap</param>
        /// <param name="ppBits">pointer to pointer of bitmap bits</param>
        /// <param name="ppBitmapInfo">pointer to pointer of bitmap info header</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_IMAGE_GET_BITMAP(IntPtr hIRDX, ref ushort pWidth, ref ushort pHeight, out IntPtr ppBits, out IntPtr ppBitmapInfo);



        /// <summary>
        /// Get the image of measurement data as bitmap for drawing for 1D data
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pWidth">pointer to width of the bitmap</param>
        /// <param name="Height">height of the bitmap</param>
        /// <param name="ppBits">pointer to pointer of bitmap bits</param>
        /// <param name="ppBitmapInfo">pointer to pointer of bitmap info header</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_IMAGE_GET_BITMAP_PROFILE(IntPtr hIRDX, ref ushort pWidth, ushort Height, out IntPtr ppBits, out IntPtr ppBitmapInfo);


        #endregion


        #region IRDX_PIXEL_xxxx
        // ##########################################################################################


        /// <summary>
        /// Get the pixel data size
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pWidth">pointer to width of the pixel field</param>
        /// <param name="pHeight">pointer to height of the pixel field</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_PIXEL_GET_SIZE(IntPtr hIRDX, ref ushort pWidth, ref ushort pHeight);


        /// <summary>
        /// Get the values of minimum and maximum of all pixels
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pMin">pointer to minimum (corresponding to #DDAQ_SET_TEMPUNIT)</param>
        /// <param name="pMax">pointer to maximum (corresponding to #DDAQ_SET_TEMPUNIT)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_PIXEL_GET_MINMAX(IntPtr hIRDX, ref float pMin, ref float pMax);


        /// <summary>
        /// Get the average value of all pixels
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pAV">pointer to average (corresponding to #DDAQ_SET_TEMPUNIT)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_PIXEL_GET_AVERAGE(IntPtr hIRDX, ref float pAV);



        /// <summary>
        /// Get the average and standard deviation value of all pixels
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pAV">pointer to average (corresponding to #DDAQ_SET_TEMPUNIT)</param>
        /// <param name="pStdDev">pointer to standard deviation (corresponding to #DDAQ_SET_TEMPUNIT)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_PIXEL_GET_AVSTDDEV(IntPtr hIRDX, ref float pAV, ref float pStdDev);



        /// <summary>
        /// Get the temperature data from all pixels
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pData">pointer to data buffer for temperatures (corresponding to #DDAQ_SET_TEMPUNIT)</param>
        /// <param name="BufSize">buffer size (bytes)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_PIXEL_GET_DATA(IntPtr hIRDX, ref float pData, uint BufSize);



        /// <summary>
        /// Set the temperature data for all pixels
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="pData">pointer to data buffer for temperatures</param>
        /// <param name="NumPix">number of pixels</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_PIXEL_SET_DATA(IntPtr hIRDX, ref float pData, uint NumPix);



        /// <summary>
        /// Get the temperature data from one pixel point
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="x">x coordinate of point (1 ... sizex)</param>
        /// <param name="y">y coordinate of point (1 ... sizey)</param>
        /// <param name="pData">pointer to the data buffer for temperature value (corresponding to #DDAQ_SET_TEMPUNIT)</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_PIXEL_GET_DATA_POINT(IntPtr hIRDX, ushort x, ushort y, ref float pData);


        #endregion


        #region IRDX_USERDATA_xxxx
        // ##########################################################################################


        /// <summary>
        /// Get the size of the a user data range
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="nNo">number of user data range (1 - 32)</param>
        /// <param name="pSize">pointer to get data size</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_USERDATA_GET_SIZE(IntPtr hIRDX, ushort nNo, ref uint pSize);


        /// <summary>
        /// Get the data of a user data range
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="nNo">number of user data range (1 - 32)</param>
        /// <param name="Size">buffer size (bytes)</param>
        /// <param name="pData">pointer to buffer</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_USERDATA_GET_DATA(IntPtr hIRDX, ushort nNo, uint Size, out IntPtr pData);


        /// <summary>
        /// Set the data of a user data range
        /// </summary>
        /// <param name="hIRDX">IRDX handle</param>
        /// <param name="nNo">number of user data range (1 - 32)</param>
        /// <param name="Size">buffer size (bytes)</param>
        /// <param name="pData">pointer to buffer</param>
        /// <returns>error code, #DDAQ_ERROR.NO_ERROR for success</returns>
        [DllImport(dllName)]
        public static extern DDAQ_ERROR DDAQ_IRDX_USERDATA_SET_DATA(IntPtr hIRDX, ushort nNo, uint Size, out IntPtr pData);

        #endregion


        #region Gdi32 functions
        // ##########################################################################################


        /// <summary>
        /// sets the pixels in the specified rectangle on the device that is associated with the destination device context using color data from a DIB
        /// </summary>
        /// <param name="HDC">Handle to the device context</param>
        /// <param name="XDest">Specifies the x-coordinate, in logical units, of the upper-left corner of the destination rectangle</param>
        /// <param name="YDest">Specifies the y-coordinate, in logical units, of the upper-left corner of the destination rectangle</param>
        /// <param name="dwWidth">Specifies the width, in logical units, of the DIB</param>
        /// <param name="dwHeight">Specifies the height, in logical units, of the DIB</param>
        /// <param name="XSrc">Specifies the x-coordinate, in logical units, of the lower left corner of the DIB</param>
        /// <param name="YSrc">Specifies the y-coordinate, in logical units, of the lower left corner of the DIB</param>
        /// <param name="uStartScan">Specifies the starting scan line in the DIB</param>
        /// <param name="cScanLines">Specifies the number of DIB scan lines contained in the array pointed to by the lpvBits parameter</param>
        /// <param name="lpBits">Pointer to DIB color data stored as an array of bytes</param>
        /// <param name="lpBitsInfo">Pointer to a BITMAPINFO structure that contains information about the DIB</param>
        /// <param name="fuColorUse">Specifies whether the bmiColors member of the BITMAPINFO structure contains explicit red, green, blue (RGB) values or indexes into a palette</param>
        /// <returns></returns>
        [DllImport("gdi32.dll", EntryPoint = "SetDIBitsToDevice")]
        public static extern int GDI32_SetDIBitsToDevice(
            IntPtr HDC,
            int XDest,
            int YDest,
            uint dwWidth,
            uint dwHeight,
            int XSrc,
            int YSrc,
            uint uStartScan,
            uint cScanLines,
            IntPtr lpBits,
            IntPtr lpBitsInfo,
            uint fuColorUse);



        /// <summary>
        /// copies the color data for a rectangle of pixels in a DIB to the specified destination rectangle
        /// </summary>
        /// <param name="HDC">Handle to the destination device context</param>
        /// <param name="XDest">x-coord of destination upper-left corner</param>
        /// <param name="YDest">y-coord of destination upper-left corner</param>
        /// <param name="nDestWidth">width of destination rectangle</param>
        /// <param name="nDestHeight">height of destination rectangle</param>
        /// <param name="XSrc">x-coord of source upper-left corner</param>
        /// <param name="YSrc">y-coord of source upper-left corner</param>
        /// <param name="nSrcWidth">width of source rectangle</param>
        /// <param name="nSrcHeight">height of source rectangle</param>
        /// <param name="lpBits">bitmap bits</param>
        /// <param name="lpBitsInfo">bitmap data</param>
        /// <param name="iUsage">usage options</param>
        /// <param name="dwRop">raster operation code</param>
        /// <returns></returns>
        [DllImport("gdi32.dll", EntryPoint = "StretchDIBits")]
        public static extern int GDI32_StretchDIBits(
            IntPtr HDC,
            int XDest,
            int YDest,
            int nDestWidth,
            int nDestHeight,
            int XSrc,
            int YSrc,
            int nSrcWidth,
            int nSrcHeight,
            IntPtr lpBits,
            IntPtr lpBitsInfo,
            uint iUsage,
            int dwRop);



        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpBitsInfo">bitmap data</param>
        /// <param name="lpBits">bitmap bits</param>
        /// <param name="pBitmap">bitmap</param>
        /// <returns></returns>
        [DllImport("gdiplus.dll", EntryPoint = "GdipCreateBitmapFromGdiDib")]
        internal static extern int GDIPLUS_GdipCreateBitmapFromGdiDib(IntPtr lpBitsInfo, IntPtr lpBits, out IntPtr pBitmap);


        #endregion


        #region DDAQ defines
        // ##########################################################################################




        /// <summary>
        /// DDAQ temperatur units
        /// </summary>
        public enum DDAQ_TEMPUNIT : ushort
        {
            /// <summary>
            /// temperature unit °C
            /// </summary>
            C = 0,
            /// <summary>
            /// temperature unit °F
            /// </summary>
            F = 1,
            /// <summary>
            /// temperature unit K
            /// </summary>
            K = 2,
        }



        /// <summary>
        /// DDAQ device types
        /// </summary>
        public enum DDAQ_DEVICE_TYPE : ushort
        {
            /// <summary>
            /// invalid
            /// </summary>
            NO = 0,
            /// <summary>
            /// PYROVIEW 100
            /// </summary>
            VIEW_100 = 1,
            /// <summary>
            /// PYROVIEW 256
            /// </summary>
            VIEW_256 = 2,
            /// <summary>
            /// PYROVIEW 320
            /// </summary>
            VIEW_320 = 3,
            /// <summary>
            /// MIDAS
            /// </summary>
            MIDAS = 4,
            /// <summary>
            /// PYROVIEW 380
            /// </summary>
            VIEW_380 = 5,
            /// <summary>
            /// MODUL
            /// </summary>
            MODULE_128 = 10,
            /// <summary>
            /// PYROLINE 128
            /// </summary>
            LINE_128 = 11,
            /// <summary>
            /// PYROLINE 256
            /// </summary>
            LINE_256 = 12,
            /// <summary>
            /// PYROLINE HZK 160
            /// </summary>
            HZK_160 = 13,
            /// <summary>
            /// PYROLINE HZK 256 
            /// </summary>
            HZK_256 = 14,
        }



        /// <summary>
        /// DDAQ float trigger modes
        /// </summary>
        public enum DDAQ_SINGLETRIGGER : ushort
        {
            /// <summary>
            /// no float trigger
            /// </summary>
            NO = 0,
            /// <summary>
            /// hardware float trigger with level on camera input IN2
            /// </summary>
            LEVEL_IN2 = 1,
            /// <summary>
            /// hardware float trigger with edge on camera input IN2
            /// </summary>
            EDGE_IN2 = 2,
        }



        /// <summary>
        /// DDAQ sequence trigger modes
        /// </summary>
        public enum DDAQ_SEQUENCETRIGGER : ushort
        {
            /// <summary>
            /// no sequence trigger
            /// </summary>
            NO = 0,
            /// <summary>
            /// hardware sequence trigger with level on camera input IN3
            /// </summary>
            LEVEL_IN3 = 1,
            /// <summary>
            /// hardware sequence trigger with edge on camera input IN3
            /// </summary>
            EDGE_IN3 = 2,
            /// <summary>
            /// temperature sequence trigger with level and retrigger
            /// </summary>
            LEVEL_TEMP = 3,
            /// <summary>
            /// temperature sequence trigger with edge and retrigger
            /// </summary>
            EDGE_TEMP = 4,
            /// <summary>
            /// temperature sequence trigger with edge without retrigger
            /// </summary>
            EDGE1_TEMP = 5,
        }



        /// <summary>
        /// DDAQ temperature sequence trigger modes
        /// </summary>
        public enum DDAQ_TEMPSEQUENCETRIGGER : ushort
        {
            /// <summary>
            /// maximum greater then threshold
            /// </summary>
            MAX_G_TH = 1,
            /// <summary>
            /// average greater then threshold
            /// </summary>
            AVE_G_TH = 2,
            /// <summary>
            /// minimum greater then threshold
            /// </summary>
            MIN_G_TH = 3,
            /// <summary>
            /// maximum less then threshold
            /// </summary>
            MAX_L_TH = 4,
            /// <summary>
            /// average less then threshold
            /// </summary>
            AVE_L_TH = 5,
            /// <summary>
            /// minimum less then threshold
            /// </summary>
            MIN_L_TH = 6,
        }



        /// <summary>
        /// DDAQ shutter trigger modes
        /// </summary>
        public enum DDAQ_SHUTTERTRIGGER : ushort
        {
            /// <summary>
            /// no external shutter trigger (internal auto mode)
            /// </summary>
            NO = 0,
            /// <summary>
            /// hardware shutter trigger: disable auto shuttering with camera input IN2
            /// </summary>
            IN2 = 1,
            /// <summary>
            /// hardware shutter trigger: disable auto shuttering with camera input IN3
            /// </summary>
            IN3 = 2,
        }



        /// <summary>
        /// DDAQ pallet types for bitmap display 
        /// </summary>
        public enum DDAQ_PALLET : ushort
        {
            /// <summary>
            /// SYMIICON
            /// </summary>
            SYMIICON = 0,
            /// <summary>
            /// MULTICOLOR
            /// </summary>
            MULTICOLOR = 1,
            /// <summary>
            /// SPECTRUM
            /// </summary>
            SPECTRUM = 2,
            /// <summary>
            /// THERMO
            /// </summary>
            THERMO = 3,
            /// <summary>
            /// GRAY
            /// </summary>
            GRAY = 4,
            /// <summary>
            /// GRAYPLUS
            /// </summary>
            GRAYPLUS = 5,
            /// <summary>
            /// HOTMETAL
            /// </summary>
            HOTMETAL = 6,
            /// <summary>
            /// IRON
            /// </summary>
            IRON = 7,
            /// <summary>
            /// LIGHT
            /// </summary>
            LIGHT = 8,
        }



        /// <summary>
        /// DDAQ auto dynamic modes for scaling 
        /// </summary>
        public enum DDAQ_AUTOSCALE : ushort
        {
            /// <summary>
            /// no auto scaling (fix min and max)
            /// </summary>
            NO = 0,
            /// <summary>
            /// auto scaling for min and max
            /// </summary>
            MINMAX = 1,
            /// <summary>
            /// auto scaling for max (fix min)
            /// </summary>
            MAX = 2,
            /// <summary>
            /// auto scaling for min (fix max)
            /// </summary>
            MIN = 3,
            /// <summary>
            /// auto scaling for min and max with average plus and minus range
            /// </summary>
            AVERAGE = 4,
            /// <summary>
            /// auto scaling for min and max with average plus and minus standard deviation
            /// </summary>
            STDDEV = 5,
        }



        /// <summary>
        /// DDAQ ambient temperature correction modes
        /// </summary>
        public enum DDAQ_AMBIENTTEMP_CORRMODE : ushort
        {
            /// <summary>
            /// correction with fix value (#DDAQ_IRDX_OBJECT_GET_AMBIENTTEMP, #DDAQ_IRDX_OBJECT_SET_AMBIENTTEMP)
            /// </summary>
            CORR_FIX = 0,
            /// <summary>
            /// dynamic correction with value of camera temperature
            /// </summary>
            CORR_TCAM = 1,
        }



        /// <summary>
        /// DDAQ transmission correction modes
        /// </summary>
        public enum DDAQ_TRANSMISSION_CORRMODE : ushort
        {
            /// <summary>
            /// correction with fix value (#DDAQ_IRDX_OBJECT_GET_TRANSMISSION, #DDAQ_IRDX_OBJECT_SET_TRANSMISSION)
            /// </summary>
            FIX = 0,
            /// <summary>
            /// dynamic correction with measured maximum signal from external reference
            /// </summary>
            REF_MAX = 1,
            /// <summary>
            /// dynamic correction with measured average signal from external reference
            /// </summary>
            REF_AV = 2,
        }



        /// <summary>
        /// DDAQ transmission correction validity codes
        /// </summary>
        public enum DDAQ_TRANSMISSION_CORRRES : ushort
        {
            /// <summary>
            /// correction is off (#DDAQ_TRANSMISSION_CORRMODE.FIX)
            /// </summary>
            OFF = 0,
            /// <summary>
            /// error: irdx entry not found
            /// </summary>
            ERR = 1,
            /// <summary>
            /// calculated value is in the valid range
            /// </summary>
            OK = 2,
            /// <summary>
            /// calculated value is invalid to low but set to minimum 0.1
            /// </summary>
            INV_LOW = 3,
            /// <summary>
            /// calculated value is invalid to high but set to maximum 1.0
            /// </summary>
            INV_HIGH = 4,
            /// <summary>
            /// calculated value is lower then allowed value from tranmsission correction ()
            /// </summary>
            TOLOW = 5,
        }



        /// <summary>
        /// DDAQ data read mode
        /// </summary>
        public enum DDAQ_DATAREADMODE : ushort
        {
            /// <summary>
            /// 1:1 from detector
            /// </summary>
            DETECTOR = 0,
            /// <summary>
            /// like lens
            /// </summary>
            LENS = 1,
        }



        /// <summary>
        /// DDAQ zoom modes for bitmap display 
        /// </summary>
        public enum DDAQ_ZMODE : ushort
        {
            /// <summary>
            /// 1:1 from source
            /// </summary>
            DIRECT = 0,
            /// <summary>
            /// integer zoom without interpolation
            /// </summary>
            INTEGER = 1,
            /// <summary>
            /// real zoom with interpolation
            /// </summary>
            REAL = 2,
        }



        /// <summary>
        /// DDAQ draw modes for bitmap display
        /// </summary>
        public enum DDAQ_DRAWMODE : ushort
        {
            /// <summary>
            /// without rotation
            /// </summary>
            R0 = 0,
            /// <summary>
            /// 90?rotated
            /// </summary>
            R90 = 1,
            /// <summary>
            /// 180?rotated
            /// </summary>
            R180 = 2,
            /// <summary>
            /// 270?rotated
            /// </summary>
            R270 = 3,
            /// <summary>
            /// without rotation, horizontal inverted
            /// </summary>
            R0_IH = 4,
            /// <summary>
            /// 90?rotated, horizontal inverted
            /// </summary>
            R90_IH = 5,
            /// <summary>
            /// 180?rotated, horizontal inverted
            /// </summary>
            R180_IH = 6,
            /// <summary>
            /// 270?rotated, horizontal inverted
            /// </summary>
            R270_IH = 7,
            /// <summary>
            /// without rotation, vertical inverted
            /// </summary>
            R0_IV = 8,
            /// <summary>
            /// 90?rotated, vertical inverted
            /// </summary>
            R90_IV = 9,
            /// <summary>
            /// 180?rotated, vertical inverted
            /// </summary>
            R180_IV = 10,
            /// <summary>
            /// 270?rotated, vertical inverted
            /// </summary>
            R270_IV = 11,
            /// <summary>
            /// without rotation, horizontal and vertical inverted
            /// </summary>
            R0_IHV = 12,
            /// <summary>
            /// 90?rotated, horizontal and vertical inverted
            /// </summary>
            R90_IHV = 13,
            /// <summary>
            /// 180?rotated, horizontal and vertical inverted
            /// </summary>
            R180_IHV = 14,
            /// <summary>
            /// 270?rotated, horizontal and vertical inverted
            /// </summary>
            R270_IHV = 15,
        }



        /// <summary>
        /// DDAQ error codes
        /// </summary>
        public enum DDAQ_ERROR : ushort
        {
            NO_ERROR = 0,
            INVALID_LICENSE = 1,
            INVALID_DEVNO = 2,
            INVALID_HIRDX = 3,
            OPEN_IRDX = 4,
            ADD_MAP = 5,
            REMOVE_MAP = 6,
            READMEM = 7,
            DRAWPALLET = 8,
            BUFFERSIZE = 9,
            INVALID_USERDATANO = 10,
            INVALID_ISOTHERMNO = 11,
            INVALID_AMBIENTCORRMODE = 12,
            INVALID_AMBIENTCORROFFS = 13,
            AMBIENTCORR_ON = 14,
            TRANSMCORR_ON = 15,
            TRANSMCORR_OFF = 16,
            INVALID_TRANSMCORRMODE = 17,
            INVALID_TRANSMCORRREFTEMP = 18,
            INVALID_TRANSMCORRREFEPS = 19,
            INVALID_TRANSMCORRWEIGHT = 20,
            INVALID_TRANSMCORRMINVAL = 21,
            INVALID_XYCOORD = 22,
            INVALID_SCALE_MIN = 23,
            INVALID_SCALE_MAX = 24,
            INVALID_SCALE_STEP = 25,
            INVALID_AUTODYN = 26,
            INVALID_AUTODYN_PM = 27,
            INVALID_BAR = 28,
            INVALID_NUMCOL = 29,
            INVALID_ISOTHERMMIN = 30,
            INVALID_ISOTHERMMAX = 31,
            INVALID_ISOTHERMSAT = 32,
            INVALID_ZOOMMODE = 33,
            INVALID_ZOOM = 34,
            INVALID_READMODE = 35,
            INVALID_COLORINDEX = 36,
            INVALID_DRAWMODE = 37,
            INVALID_TEMPPRECISION = 38,
            INVALID_TEMPUNIT = 39,
            TIMEOUT_SINGLESHOT = 40,
            INVALID_IMAGE = 41,

            // from DIASCAM.dll
            INVALID_DEVICENO = 1001,
            INVALID_ITEM = 1002,
            INVALID_BUFSIZE = 1003,
            NO_DEVICE_DETECT = 1010,
            COM_NOT_FREE = 1011,
            CREATE_OBJECT = 1012,
            CREATE_THREAD = 1013,
            ALLOC_BUFFER = 1014,
            LOAD_DRIVER = 1015,
            FUNCTION_NOT_POSSIBLE = 1016,
            INVALID_CFGFILE = 1020,
            INVALID_DEVICE_TYPE = 1021,
            INVALID_DEVICE_ID = 1022,
            INIT_CORRDATA = 1023,
            TKLFILE_NOT_EXIST = 1024,
            INVALID_TKL_INDEX = 1025,
            LOAD_TKL = 1026,
            IRDFILE_NOT_EXIST = 1030,
            INVALID_HDRFILE = 1031,
            DEVICE_NOT_OPEN = 1100,
            DEVICE_ALREDAY_OPEN = 1101,
            NOT_STOP_MODE = 1102,
            ALREADY_STOP_MODE = 1103,
            SEND_COMMAND = 1104,
            START_RECORDING = 1105,
            STOP_RECORDING = 1106,
            INVALID_SOURCE_BUFFER = 1107,
            INVALID_DEST_BUFFER = 1108,
            INVALID_AVERAGING = 1200,
            INVALID_NUMFRAMES = 1201,
            INVALID_MEASUREMENTRANGE = 1202,
            INVALID_TRIGGERMODE = 1203,
            INVALID_DATAUNIT = 1204,
            INVALID_EMISSIVITY = 1205,
            INVALID_TRANSMISSION = 1206,
            INVALID_AMBIENTTEMPC = 1207,
            INVALID_THRESHOLD = 1208,
            INVALID_DATAREADMODE = 1209,
            INVALID_DISABLESHUTTERMODE = 1210,
            IN2_ALREADY_USED = 1211,
            IN3_ALREADY_USED = 1212,
            INVALID_IMAGE_NO = 1250,

            //from DIASIRDX.dll
            INVALID_IRDX_HANDLE = 2001,
            FILE_NOT_EXIST = 2002,
            NOT_ENAUGH_MEMORY = 2003,
            WRITE_BUFFER = 2004,
            WRITE_DISK = 2005,
            CLOSE_FILE = 2006,
            INVALID_IRDX_TYPE = 2007,
            DIALOG_CANCEL = 2008,
            OPEN_FILE = 2010,
            INVALID_FILE_SIZE = 2011,
            READ_FROM_FILE = 2012,
            INVALID_BUFFER_SIZE = 2013,
            DELETE_FILE = 2014,
            RENAME_FILE = 2015,
            GETFILESIZE = 2016,
            CREATE_SECTION = 2020,
            INVALID_SECTION_ID = 2021,
            INVALID_SECTION_INDEX = 2022,
            INVALID_SECTION_SIZE = 2023,
            CREATE_ENTRY = 2030,
            INVALID_ENTRY_INDEX = 2031,
            INVALID_ENTRY_SIZE = 2032,
            ENTRY_NOT_FOUND = 2033,
            INVALID_ENTRY_BUFFER = 2034,
            LOAD_HDR = 2040,
            LOAD_PXT = 2041,
            LOAD_IRD = 2042,
            INVALID_DATASETINDEX = 2050,
            ADD_IRDX_ID = 2060,
            ADD_IRDX_MR = 2061,
            ADD_IRDX_SIZE = 2062,
            ADD_IRDX_UNIT = 2063,
            ADD_IRDX_TCAL = 2064,

            // from DIASTCALC.dll
            INVALID_TCALC_HANDLE = 3001,
            OPEN_FROM_IRDX_HANDLE = 3002,
            OPEN_FROM_CAMERA_DEVNO = 3003,
            SAVE_TO_IRDX_HANDLE = 3004,
        }



        /// <summary>
        /// The BITMAPINFO structure defines the dimensions and color information for a DIB
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class BITMAPINFO
        {
            BITMAPINFOHEADER bmiHeader;
            RGBQUAD bmiColors;
        }



        /// <summary>
        /// The BITMAPINFOHEADER structure contains information about the dimensions and color format of a DIB
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private class BITMAPINFOHEADER
        {
            public uint size;
            public int width;
            public int height;
            public ushort biPlanes;
            public ushort biBitCount;
            public uint biCompression;
            public uint biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public uint biClrUsed;
            public uint biClrImportant;
        }



        /// <summary>
        /// The RGBQUAD structure describes a color consisting of relative intensities of red, green, and blue
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private class RGBQUAD
        {
            public byte rgbBlue;
            public byte rgbGreen;
            public byte rgbRed;
            public byte rgbReserved;
        }



        /// <summary>
        /// The COLORREF value is used to specify an RGB color
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class COLORREF
        {
            public byte Red;
            public byte Green;
            public byte Blue;
        }

        #endregion
    }
}
