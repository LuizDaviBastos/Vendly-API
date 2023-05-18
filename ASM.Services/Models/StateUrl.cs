using Newtonsoft.Json;
using System.Text;

namespace ASM.Services.Models
{
    public class StateUrl
    {
        public Guid SellerId { get; set; }
        public bool Signup { get; set; }

        public static bool TryGetState(string? encodedState, out StateUrl stateOut)
        {
            stateOut = default;
            if (!string.IsNullOrEmpty(encodedState))
            {
                stateOut = JsonConvert.DeserializeObject<StateUrl>(Encoding.UTF8.GetString(Convert.FromBase64String(encodedState)));
                return true;
            }
            return false;
        }
    }
}
