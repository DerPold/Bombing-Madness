using System;

class SSIStartup
{
    
    private System.Diagnostics.Process ssiProcess;

    public void StartSSI(String XMLPipeLineLocation)
    {
        System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
        info.RedirectStandardInput = true;
        ssiProcess = new System.Diagnostics.Process();
        ssiProcess.StartInfo = info;
        info.FileName = "xmlpipe.exe";
        info.Arguments = XMLPipeLineLocation;
        info.UseShellExecute = false;
        ssiProcess.Start();
    }

    public void StopSSI()
    {
        
        // works but ssi wont write logs
        //ssiProcess.CloseMainWindow();
        //ssiProcess.Close();

    }

}
