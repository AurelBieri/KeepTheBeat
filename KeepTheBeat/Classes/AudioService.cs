using System.Threading.Tasks;
using Keep_The_Beat.Classes;
using MailKit.Search;
using Microsoft.JSInterop;

namespace KeepTheBeat.Classes
{
    public class AudioService
    {
        public async Task PlayAudio(IJSRuntime jsRuntime, string audioSrc)
        {
            await jsRuntime.InvokeVoidAsync("playAudio", audioSrc);
        }

        public async Task PauseAudio(IJSRuntime jsRuntime)
        {
            await jsRuntime.InvokeVoidAsync("pauseAudio");
        }

        public async Task ResumeAudio(IJSRuntime jsRuntime)
        {
            await jsRuntime.InvokeVoidAsync("resumeAudio");
        }

        public async Task StopAudio(IJSRuntime jsRuntime)
        {
            await jsRuntime.InvokeVoidAsync("stopAudio");
        }
    }
}
