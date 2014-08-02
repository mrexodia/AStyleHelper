// AStyleInterface.cs

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

/// AStyleInterface contains methods to call the Artistic Style formatter.
public class AStyleInterface
{
    // Dll name
#if (DEBUG)
    private const String dllName = "AStyled.dll";
#else
    private const String dllName = "AStyle.dll";
#endif

    /// AStyleGetVersion DllImport.
    /// Cannot use String as a return value because Mono runtime will attempt to
    /// free the returned pointer resulting in a runtime crash.
    /// NOTE: CharSet.Unicode is NOT used here.
    [DllImport(dllName)]
    private static extern IntPtr AStyleGetVersion();

    /// AStyleMainUtf16 DllImport.
    /// Cannot use String as a return value because Mono runtime will attempt to
    /// free the returned pointer resulting in a runtime crash.
    /// NOTE: CharSet.Unicode and wide strings are used here.
    [DllImport(dllName, CharSet = CharSet.Unicode)]
    private static extern IntPtr AStyleMainUtf16(
        [MarshalAs(UnmanagedType.LPWStr)] String sIn,
        [MarshalAs(UnmanagedType.LPWStr)] String sOptions,
        AStyleErrorDelgate errorFunc,
        AStyleMemAllocDelgate memAllocFunc
    );

    /// AStyleMainUtf16 callbacks.
    /// NOTE: Wide strings are NOT used here.
    private delegate IntPtr AStyleMemAllocDelgate(int size);
    private delegate void AStyleErrorDelgate(
        int errorNum,
        [MarshalAs(UnmanagedType.LPStr)] String error
    );

    /// AStyleMainUtf16 Delegates.
    private AStyleMemAllocDelgate AStyleMemAlloc;
    private AStyleErrorDelgate AStyleError;

    /// Declare callback functions.
    public AStyleInterface()
    {
        AStyleMemAlloc = new AStyleMemAllocDelgate(OnAStyleMemAlloc);
        AStyleError = new AStyleErrorDelgate(OnAStyleError);
    }

    /// Call the AStyleMainUtf16 function in Artistic Style.
    /// An empty string is returned on error.
    public String FormatSource(String textIn, String options)
    {
        // Return the allocated string
        // Memory space is allocated by OnAStyleMemAlloc, a callback function
        String sTextOut = String.Empty;
        try
        {
            IntPtr pText = AStyleMainUtf16(textIn, options, AStyleError, AStyleMemAlloc);
            if (pText != IntPtr.Zero)
            {
                sTextOut = Marshal.PtrToStringUni(pText);
                Marshal.FreeHGlobal(pText);
            }
        }
        catch (BadImageFormatException e)
        {
            string message = e.ToString() + "\r\n\r\n" + "You may be mixing 32 and 64 bit code!";
            MessageBox.Show(message);
        }
        catch (DllNotFoundException)
        {
            MessageBox.Show("Cannot load native library: " + dllName + "\r\n\r\nAStyleWhore will be terminated!");
            Environment.Exit(1);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.ToString());
        }
        return sTextOut;
    }

    /// Get the Artistic Style version number.
    /// Does not need to terminate on error.
    /// But the exception must be handled when a function is called.
    public String GetVersion()
    {
        String sVersion = String.Empty;
        try
        {
            IntPtr pVersion = AStyleGetVersion();
            if (pVersion != IntPtr.Zero)
            {
                sVersion = Marshal.PtrToStringAnsi(pVersion);
            }
        }
        catch (BadImageFormatException e)
        {
            string message = e.ToString() + "\r\n\r\n" + "You may be mixing 32 and 64 bit code!";
            MessageBox.Show(message);
        }
        catch (DllNotFoundException)
        {
            MessageBox.Show("Cannot load native library: " + dllName + "\r\n\r\nAStyleWhore will be terminated!");
            Environment.Exit(1);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.ToString());
        }
        return sVersion;
    }

    /// Allocate the memory for the Artistic Style return string.
    private IntPtr OnAStyleMemAlloc(int size)
    {
        return Marshal.AllocHGlobal(size);
    }

    /// Display errors from Artistic Style .
    private void OnAStyleError(int errorNumber, String errorMessage)
    {
        MessageBox.Show("AStyle error " + errorNumber + "\n" + errorMessage);
    }

}   // class AStyleInterface