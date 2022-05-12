namespace FrostweepGames.Plugins.WebGL.MicrophonePro
{
    public partial class DefineProcessing : Plugins.DefineProcessing
    {
        internal static readonly string[] _Defines = new string[] 
        {
            "FG_MPRO"
        };

        static DefineProcessing()
        {
            AddOrRemoveDefines(true, false, _Defines);
        }
    }
}